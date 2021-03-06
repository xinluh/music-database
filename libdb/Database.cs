﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using SqliteClient;

namespace libdb
{
    public class Database
    {
        private static SQLiteClient sqlclient;
        private static bool transaction = false;
        private static string sqls = "";
        private static string currentDBLocation = "";
        
        public enum OutputFormats
        {
            Nothing = 0,
            IEnumerableOfList,
            DataTable,
            DataGridView,
        }

        public static int Open(string db_location)
        {
            try
            {
                if (!System.IO.File.Exists(db_location))
                    throw new System.IO.FileNotFoundException("Cannot open non-existent database!", db_location);
                if (sqlclient == null || (sqlclient != null && 
                    System.IO.Path.GetFullPath(db_location) != System.IO.Path.GetFullPath(currentDBLocation)))
                {
                    sqlclient = new SQLiteClient(db_location);
                    currentDBLocation = db_location;
                }
            }
            catch (SQLiteException e)
            {
                Debug.HandleException(new Exception("Opening database fail!", e));
                return 1;
            }
            return 0;
        }

        public static int Close()
        {
            sqlclient.Close();
            return 0; //nothing to do for sqlite
        }

        public static bool IsOpen { get { return (sqlclient != null); } }

        public static void StartTransaction()
        {
            transaction = true;
        }

        public static void CommitTransaction()
        {
            string str = "BEGIN TRANSACTION; \n" + sqls + "\nCOMMIT;";
            try
            {
                transaction = false;
                sqlclient.Execute(str);
            }
            finally
            {
                sqls = "";
            }
        }

        public static void WriteLog(string sql, string undo_sql)
        {
            ExecuteNonQuery(string.Format("INSERT INTO log (sql, undo_sql) VALUES ({0},{1})", Quote(sql), Quote(undo_sql)));
        }

        public static object GetScalar(string strsql)
        {
            check_connection();
            return sqlclient.GetOne(strsql);
        }

        public static ArrayList GetFirstRow(string strsql)
        {
            check_connection();
            ArrayList list = new ArrayList();
            sqlclient.Execute(strsql, (obj, columns, data) =>
            {
                ArrayList a = (ArrayList)obj;
                if (a.Count == 0) //only the first row is needed
                    a.AddRange(data);
                return 0;
            }, list);
            
            return list;
        }

        public static ArrayList GetFirstColumn(string strsql)
        {
            check_connection();
            ArrayList list = new ArrayList();
            sqlclient.Execute(strsql, (obj, columns, data) =>
            {
                ArrayList a = (ArrayList)obj;
                a.Add(data[0]);
                return 0;
            }, list);

            return list;
        }

        public static void ExecuteNonQuery(string strsql)
        {
            check_connection();
            sqlclient.Execute(strsql, null, null);
        }

        public static object Execute(string strsql,OutputFormats output,object InputObject)
        {
            if (transaction)
            {
                sqls += strsql + "\n";
                return null;
            }
            else
            {
                return _execute(strsql,output,InputObject);
            }
        }

        public static void Execute(string strsql)
        {
            Execute(strsql, OutputFormats.Nothing, null);
        }


        public static int LastInsertRowID()
        {
            return sqlclient.LastInsertID();
        }

        /// <summary>
        /// Return string quoted in ' ' and escape ' in the string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Quote(string str)
        {
            return Quote(str, true);
        }

        /// <summary>
        /// Return string escaped ' in the string; also quoted in '' if outerQuote is true.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Quote(string str, bool outerQuote)
        {
            return string.Format("{0}{1}{0}", outerQuote ? "'" : "", str.Replace("'", "''"));
        }

        private static object _execute(string sql, OutputFormats output, object InputObject)
        {
            check_connection();
            switch (output)
            {
                case OutputFormats.Nothing:
                    {
                        sqlclient.Execute(sql, null, null);
                        return null;
                    }
                case OutputFormats.IEnumerableOfList:
                    {
                        ArrayList data = new ArrayList();
                        sqlclient.Execute(sql, (obj, ColumnNames, RowData) =>
                        {
                            IList _data = obj as IList;
                            _data.Add(RowData);
                            return 0;
                        }, data);

                        return data.Cast<IList>();
                    }
                case OutputFormats.DataTable:
                    throw new NotImplementedException();
                case OutputFormats.DataGridView:
                    {
                        sqlclient.Execute(sql, (obj, ColumnNames, RowData) =>
                        {
                            System.Windows.Forms.DataGridView dg = (System.Windows.Forms.DataGridView)obj;

                            if (dg == null) //make a new one!
                                dg = new System.Windows.Forms.DataGridView();
                            if (dg.Columns.Count == 0) // add columns as necessary
                                System.Array.ForEach(ColumnNames, (s) => { dg.Columns.Add(s, s); });

                            dg.Rows.Add(RowData);
                            return 0;
                        }, InputObject);
                        return InputObject;
                    }
                default:
                    throw new NotImplementedException();
            }
            
        }

        private static void check_connection()
        {
            if (!IsOpen) { throw new Exception("Database not opened!"); }
        }
    }
}
