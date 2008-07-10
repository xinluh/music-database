using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using System.Linq;
using libdb;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.Open();

            //DataTable t = (DataTable)null;

            Album o = new Album();
            o.Fill(77);
            o.ID = 0;
            o.Name = "new album";
            //o.Insert();

            Artist art = new Artist();
            art.Fill(53);

            Piece p = new Piece(89);

            AlbumType b = AlbumType.ArtistAlbum;
            Console.WriteLine(b.GetType().Name);

            SqliteClient.SQLiteClient sql = new SqliteClient.SQLiteClient("test.db");
//            sql.Execute(@"CREATE TABLE customer
//(First_Name char(50),
//Last_Name char(50),
//Address char(50),
//City char(50),
//Country char(25),
//Birth_Date date) ");
           // sql.Execute("select * from sqlite_master", ResultCallBack, null);


            //string a =
            //    sql.GetOne("select b from 'temp' where a = 1");

            //Console.WriteLine(typeof(string).Name);

            //string[] X = System.Text.RegularExpressions.Regex.Split("glad22.33butgreat.3251z", "([0-9]+)");
            //foreach (string s in X)
            //{
            //    Console.WriteLine(s);
            //}
            
            Console.ReadLine();
        }

        //static int ResultCallBack(object Param, string[] ColumnNames, System.Collections.ArrayList Data)
        //{
        //    foreach (string s in ColumnNames)
        //    {
        //        Console.WriteLine(s);
        //    }
        //    return 0;
        //}
    }
}