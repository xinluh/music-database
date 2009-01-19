using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SqliteClient
{
    partial class SQLiteClient
    {
        private string[] column_names;
        private object param;
        private ResultCallBack callbackfunc;

        /// <summary>
        /// The callback function for returning result from query
        /// </summary>
        /// <param name="Param">The object passed on by the user</param>
        /// <param name="ColumnNames">Column names</param>
        /// <param name="Data">Data</param>
        /// <returns></returns>
        public delegate int ResultCallBack(object Param, string[] ColumnNames, string[] Data);

        /// <summary>
        /// Executes the supplied query and calls the callback function for each row of result
        /// </summary>
        /// <remarks>
        /// <p>
        /// If the database is busy (ie with another process/thread using it) then
        /// this method will call Thread.Sleep(), and then retry the query. Number of
        /// retries and retry delay are configurable using the appropriate properties.</p>
        /// <p>The result set object may be empty if there are no results, or if the
        /// query does not return results (eg. UPDATE, INSERT, DELETE etc)</p>
        /// </remarks>
        /// <param name="query">The SQL query to execute</param>
        /// <param name="callback">The callback function to call 
        /// (object Param, string[] ColumnNames, ArrayList Data)</param>
        /// <param name="Param">A object to pass to the callback function</param>
        /// <exception cref="SQLiteException">
        /// Thrown if an error occurs or if the database is busy and the retries
        /// are exhausted.
        /// </exception>
        public unsafe void Execute(string query,ResultCallBack callback,object Param)
        {
            column_names = null; //reset for each new query
            param = Param;
            callbackfunc = callback;

            ResultCode errorCode;
            string errorMsg;
            int retries = 0;

            using (SqliteString sql = new SqliteString(query))
            {
                while ((errorCode = SQLiteClient.sqlite3_exec(
                    this.dbHandle, 
                    sql.ToPointer(),
                    (callbackfunc == null) ? null : new SQLiteCallback(CallBack), 
                    IntPtr.Zero, out errorMsg))
                            == ResultCode.BUSY && retries < this.busyRetries)
                {
                    Thread.Sleep(this.busyRetryDelay);
                    ++retries;
                    continue;
                }

                if (errorCode != ResultCode.OK)
                {
                    throw new SQLiteException(SQLiteClient.GetMessageForError(errorCode) + ":\n"
                        + errorMsg + "\n the query string is :\n" + query, errorCode);
                } 
            }
        }

        /// <summary>
        /// Relay the returned result from sqlite3 callback to the callback func specified 
        /// when Execute() is called
        /// </summary>
        /// <param name="pArg">Not used</param>
        /// <param name="argc">Number of columns in argv</param>
        /// <param name="argv">The column data in this row</param>
        /// <param name="columnNames">The column names</param>
        internal unsafe int CallBack(IntPtr pArg, int argc, sbyte** argv, sbyte** columnNames)
        {
            List<string> ar = new List<string>(argc);
            
            // First time in? Add column names
            if (this.column_names == null)
            {
                for (int i = 0; i < argc; ++i)
                    ar.Add(SqliteString.PointerToString(columnNames[i]));
            }
            column_names = ar.ToArray();

            // Go through this rows data

            ar.Clear();

            for (int i = 0; i < argc; ++i)
            {
                if (argv[i] == ((sbyte*)0))
                    ar.Add(null);
                //ar[i] = null;
                else
                    ar.Add(SqliteString.PointerToString(argv[i]));
                    //ar[i] = SqliteString.PointerToString(argv[i]);
            }

            return callbackfunc(param, column_names, ar.ToArray());
        }

    }
    }
