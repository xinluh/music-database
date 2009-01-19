//
// This file is part of SqliteClient
//
// SqliteClient is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// SqliteClient is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with SqliteClient; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// © Copyright 2004 Richard Heyes
//
using System;
using System.Text;
using System.Threading;
using System.Collections;
using System.Runtime.InteropServices;

namespace SqliteClient
{
	/// <summary>
	/// A .NET wrapper around the SQLite library
	/// </summary>
	public partial class SQLiteClient
	{
		#region Enumerations
		/// <summary>
		/// Represents possible error codes that may be
		/// given to the SQLiteException class as the
		/// errorcode.
		/// </summary>
		public enum ResultCode : int
		{
			/// <summary>
			/// Successful result
			/// </summary>
			OK,

			/// <summary>
			/// SQL error or missing database
			/// </summary>
			ERROR,

			/// <summary>
			/// An internal logic error in SQLite
			/// </summary>
			INTERNAL,

			/// <summary>
			/// Access permission denied
			/// </summary>
			PERM,

			/// <summary>
			/// Callback routine requested an abort
			/// </summary>
			ABORT,

			/// <summary>
			/// The database file is locked
			/// </summary>
			BUSY,

			/// <summary>
			/// A table in the database is locked
			/// </summary>
			LOCKED,

			/// <summary>
			/// A malloc() failed
			/// </summary>
			NOMEM,

			/// <summary>
			/// Attempt to write a readonly database
			/// </summary>
			READONLY,

			/// <summary>
			/// Operation terminated by sqlite_interrupt()
			/// </summary>
			INTERRUPT,

			/// <summary>
			/// Some kind of disk I/O error occurred
			/// </summary>
			IOERR,

			/// <summary>
			/// The database disk image is malformed
			/// </summary>
			CORRUPT,

			/// <summary>
			/// (Internal Only) Table or record not found
			/// </summary>
			NOTFOUND,

			/// <summary>
			/// Insertion failed because database is full
			/// </summary>
			FULL,

			/// <summary>
			/// Unable to open the database file
			/// </summary>
			CANTOPEN,

			/// <summary>
			/// Database lock protocol error
			/// </summary>
			PROTOCOL,

			/// <summary>
			/// (Internal Only) Database table is empty
			/// </summary>
			EMPTY,

			/// <summary>
			/// The database schema changed
			/// </summary>
			SCHEMA,

			/// <summary>
			/// Too much data for one row of a table
			/// </summary>
			TOOBIG,

			/// <summary>
			/// Abort due to contraint violation
			/// </summary>
			CONSTRAINT,

			/// <summary>
			/// Data type mismatch
			/// </summary>
			MISMATCH,

			/// <summary>
			/// Library used incorrectly
			/// </summary>
			MISUSE,

			/// <summary>
			/// Uses OS features not supported on host
			/// </summary>
			NOLFS,

			/// <summary>
			/// Authorization denied
			/// </summary>
			AUTH
		} 
		#endregion

		#region Fields
		private int busyRetries;
		private int busyRetryDelay;
		//private SQLiteProxy proxy;
		private IntPtr dbHandle;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the number of retries that occur if the
		/// database is busy
		/// </summary>
		/// <remarks>
		/// Defaults to 5 retries (not including the initial attempt).
		/// </remarks>
		/// <example>
		/// <code>
		/// db = new SQLite("testdb");
		/// db.BusyRetries = 10;
		/// </code>
		/// </example>
		public int BusyRetries
		{
			get {
				return this.busyRetries;
			}

			set {
				this.busyRetries = value;
			}
		}

		/// <summary>
		/// Gets or sets the retry delay in milliseconds.
		/// </summary>
		/// <remarks>
		/// Defaults to 10 milliseconds.
		/// </remarks>
		/// <example>
		/// <code>
		/// db = new SQLite("testdb");
		/// db.BusyRetryDelay = 15;
		/// </code>
		/// </example>
		public int BusyRetryDelay
		{
			get {
				return this.busyRetryDelay;
			}

			set {
				this.busyRetryDelay = value;
			}
		}
		#endregion

		#region Constructor(s)
		/// <summary>
		/// Instanciates a new copy of a SQLiteClient object and attempts to
		/// open the database with the supplied name.
		/// </summary>
		/// <remarks>
		/// <p>The name of an SQLite database is the name of a file that
		/// will contain the database. If the file does not exist,
		/// SQLite attempts to create and initialize it. If the file
		/// is read-only (due to permission bits or because it is located
		/// on read-only media like a CD-ROM) then SQLite opens the
		/// database for reading only.</p>
		/// <p>The entire SQL database is stored
		/// in a single file on the disk. But additional temporary files
		/// may be created during the execution of an SQL command in order
		/// to store the database rollback journal or temporary and
		/// intermediate results of a query.</p>
		/// </remarks>
		/// <example>
		/// <code>
		/// db = new SQLite("testdb");
		/// </code>
		/// </example>
		/// <param name="dbName">Name of the database to open</param>
		/// <exception cref="SQLiteException">Thrown if an error occurs opening the database</exception>
		public SQLiteClient(string dbName)
		{
			this.busyRetries = 5;		// 5 retries
			this.busyRetryDelay = 5;	// 10ms delay
			
			//string errorMsg;
            ResultCode Ret = sqlite3_open(dbName, out this.dbHandle);
			//this.dbHandle = SQLiteClient.sqlite_open(dbName, 0, out errorMsg);

            if (Ret != ResultCode.OK){
			//if (this.dbHandle == IntPtr.Zero) {
				throw new SQLiteException(String.Format("Failed to open database, SQLite said: {0}",sqlite3_errmsg16()));
			}
		}
		#endregion

		#region Destructor
		/// <summary>
		/// Closes the the database if it is still open.
		/// </summary>
		/// <remarks>
		/// This is done by calling the <see cref="Close">Close()</see> method,
		/// so you don't have to call Close() if you don't absolutely
		/// need to close the database.
		/// </remarks>
		~ SQLiteClient()
		{
			this.Close();
		}
		#endregion

		#region External Methods		
        [DllImport("sqlite3.dll")]
		private static extern string sqlite3_libversion();

        [DllImport("sqlite3.dll")]
		private static extern int sqlite3_last_insert_rowid(IntPtr db);

        [DllImport("sqlite3.dll")]
		private static extern int sqlite3_changes(IntPtr db);

        [DllImport("sqlite3.dll")]
		private static extern void sqlite3_interrupt(IntPtr db);

        [DllImport("sqlite3.dll")]
		private static extern ResultCode sqlite3_open(string filename, out IntPtr db);

        //[DllImport("sqlite3.dll")]
        //private static extern IntPtr sqlite_open(string filename, int mode, out string errmsg);

        [DllImport("sqlite3.dll")]
		private static extern void sqlite3_close(IntPtr handle);

        [DllImport("sqlite3.dll")]
		private static extern ResultCode sqlite3_exec(IntPtr handle, IntPtr query, SQLiteCallback callBack, 
            IntPtr pArg, out string errMsg);

        [DllImport("sqlite3.dll")]
        private static extern string sqlite3_errmsg16();

		#endregion

		#region Public static methods
		/// <summary>
		/// Returns the underlying library version
		/// </summary>
		/// <example>
		/// <code>
		/// string libVersion = SQLite.GetLibVersion();
		/// </code>
		/// </example>
		/// <returns>String of the version number</returns>
		public static string GetLibVersion()
		{
			return sqlite3_libversion();
		}


		/// <summary>
		/// Escapes a string to allow it to be safely used in an SQL
		/// query. It will double up single quotes, and return the supplied
		/// string wrapped in single quotes. Eg the string "Steve's a guy"
		/// will be returned as "'Steve''s a guy'". Binary characters are
		/// not handled.
		/// </summary>
		/// <returns>Resulting string</returns>
		public static string Quote(string input)
		{
			return String.Format("'{0}'", input.Replace("'", "''"));
		}

		/// <summary>
		/// Returns an appropriate error message for the given error code
		/// </summary>
		/// <param name="errorCode">Result code from query</param>
		/// <returns>Appropriate error message</returns>
		public static string GetMessageForError(ResultCode errorCode)
		{
			string message;
			
			switch (errorCode) {
				case ResultCode.ABORT:		message = "Callback routine requested an abort";		break;
				case ResultCode.AUTH:		message = "Authorization denied";						break;
				case ResultCode.BUSY:		message = "The database file is locked";				break;
				case ResultCode.CANTOPEN:	message = "Unable to open the database file";			break;
				case ResultCode.CONSTRAINT:	message = "Abort due to contraint violation";			break;
				case ResultCode.CORRUPT:	message = "The database disk image is malformed";		break;
				case ResultCode.EMPTY:		message = "(Internal Only) Database table is empty";	break;
				case ResultCode.ERROR:		message = "SQL error or missing database";				break;
				case ResultCode.FULL:		message = "Insertion failed because database is full";	break;
				case ResultCode.INTERNAL:	message = "An internal logic error in SQLite";			break;
				case ResultCode.INTERRUPT:	message = "Operation terminated by sqlite_interrupt()";	break;
				case ResultCode.IOERR:		message = "Some kind of disk I/O error occurred";		break;
				case ResultCode.LOCKED:		message = "A table in the database is locked";			break;
				case ResultCode.MISMATCH:	message = "Data type mismatch";							break;
				case ResultCode.MISUSE:		message = "Library used incorrectly";					break;
				case ResultCode.NOLFS:		message = "Uses OS features not supported on host";		break;
				case ResultCode.NOMEM:		message = "A malloc() failed";							break;
				case ResultCode.NOTFOUND:	message = "(Internal Only) Table or record not found";	break;
				case ResultCode.OK:			message = "Successful result";							break;
				case ResultCode.PERM:		message = "Access permission denied";					break;
				case ResultCode.PROTOCOL:	message = "Database lock protocol error";				break;
				case ResultCode.READONLY:	message = "Attempt to write a readonly database";		break;
				case ResultCode.SCHEMA:		message = "The database schema changed";				break;
				case ResultCode.TOOBIG:		message = "Too much data for one row of a table";		break;
				default:					message = "";											break;
			}

			return message;
		}
		#endregion

		#region Public instance methods
		/// <summary>
		/// Executes the supplied query and returns an SQLiteResultSet object
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
		/// <exception cref="SQLiteException">
		/// Thrown if an error occurs or if the database is busy and the retries
		/// are exhausted.
		/// </exception>
		/// <returns>SQLiteResultSet of results</returns>
		public unsafe SQLiteResultSet Execute(string query)
		{
			SQLiteResultSet resultSet = new SQLiteResultSet();

            this.Execute(query, (obj, columnNames, data) =>
            {
                SQLiteResultSet result_set = obj as SQLiteResultSet;
                if (result_set.columnNames.Count == 0) 
                    result_set.columnNames.AddRange(columnNames);
                result_set.rowData.Add(new ArrayList(data));
                return 0;
            }, resultSet);
            			
			return resultSet;
		}

		/// <summary>
		/// Returns the last insert id for this database
		/// </summary>
		/// <remarks>
		/// <p>Every row of an SQLite table has a unique integer key. If the
		/// table has a column labeled INTEGER PRIMARY KEY, then that column
		/// serves as the key. If there is no INTEGER PRIMARY KEY column then
		/// the key is a unique integer. The key for a row can be accessed in a
		/// SELECT statement or used in a WHERE or ORDER BY clause using any of
		/// the names "ROWID", "OID", or "_ROWID_".</p>
		/// 
		/// <p>When you do an insert into a table that does not have an
		/// INTEGER PRIMARY KEY column, or if the table does have an
		/// INTEGER PRIMARY KEY but the value for that column is not specified
		/// in the VALUES clause of the insert, then the key is automatically
		/// generated. You can find the value of the key for the most recent
		/// INSERT statement using the this method.</p>
		/// </remarks>
		/// <returns>Integer of the id</returns>
		public int LastInsertID()
		{
			return SQLiteClient.sqlite3_last_insert_rowid(this.dbHandle);
		}

		/// <summary>
		/// Returns the number of rows changed since the database was last 
		/// "quiescent".
		/// </summary>
		/// <remarks>
		/// <p>The ChangedRows() method returns the number of rows that
		/// have been inserted, deleted, or modified since the database was
		/// last quiescent. A "quiescent" database is one in which there are
		/// no outstanding calls to <see cref="SQLiteClient.Execute(string)">Execute</see>.
		/// In common usage,
		/// ChangedRows() returns the number of rows inserted, deleted, or
		/// modified by the most recent <see cref="SQLiteClient.Execute(string)">Execute</see> call.
		/// The number reported includes any
		/// changes that were later undone by a ROLLBACK or ABORT. But rows that
		/// are deleted because of a DROP TABLE are not counted.</p>
		/// 
		/// <p>SQLite implements the command "DELETE FROM table" (without a WHERE
		/// clause) by dropping the table then recreating it. This is much faster
		/// than deleting the elements of the table individually. But it also means
		/// that the value returned from ChangedRows() will be zero regardless
		/// of the number of elements that were originally in the table. If an
		/// accurate count of the number of elements deleted is necessary, use
		/// "DELETE FROM table WHERE 1" instead.</p>
		/// </remarks>
		/// <returns>Integer of the number of changed rows</returns>
		public int ChangedRows()
		{
			return SQLiteClient.sqlite3_changes(this.dbHandle);
		}

		/// <summary>
		/// Interrupts the current database operation asap
		/// </summary>
		/// <remarks>
		/// The Interrupt() method can be called from a different thread
		/// or from a signal handler to cause the current database operation to
		/// exit at its first opportunity. When this happens, the Execute()
		/// method (or the equivalent) that started the database operation
		/// will throw an <see cref="SQLiteException">SQLiteException</see>
		/// with the <see cref="SQLiteException.ErrorCode">errorcode</see> set to INTERRUPT
		/// </remarks>
		public void Interrupt()
		{
			SQLiteClient.sqlite3_interrupt(this.dbHandle);
		}

		/// <summary>
		/// Closes the database
		/// </summary>
		/// <remarks>
		/// If a transaction is active when the database is closed, the
		/// transaction is rolled back.
		/// </remarks>
		public void Close()
		{
			SQLiteClient.sqlite3_close(this.dbHandle);
            dbHandle = IntPtr.Zero;            
		}

		/// <summary>
		/// Returns an ArrayList of the rows of the results of the
		/// given query.
		/// </summary>
		/// <param name="query">The SQL to execute</param>
		/// <returns>ArrayList of rows</returns>
		public ArrayList GetAll(string query)
		{
			SQLiteResultSet results = this.Execute(query);

			return results.Rows;
		}

		/// <summary>
		/// Returns an ArrayList of the rows of the results of the
		/// given query.
		/// </summary>
		/// <param name="query">The SQL to execute</param>
		/// <returns>ArrayList of hashtables of row data</returns>
		public ArrayList GetAllHash(string query)
		{
			SQLiteResultSet results = this.Execute(query);

			ArrayList rows = new ArrayList();

			// Loop through rows calling GetRowHash()
			while (results.IsMoreData) {
				rows.Add(results.GetRowHash());
			}

			return rows;
		}

		/// <summary>
		/// Returns the specified column of a queries result set
		/// </summary>
		/// <remarks>
		/// Use this method to get an ArrayList of a particular column of a
		/// query. The column is a zero based index.
		/// </remarks>
		/// <param name="query">The query to execute</param>
		/// <param name="column">The column to retrieve</param>
		/// <returns>An ArrayList of the results</returns>
		public ArrayList GetColumn (string query, int column)
		{
			SQLiteResultSet resultSet = this.Execute(query);

			return resultSet.GetColumn(column);
		}

		/// <summary>
		/// Returns the first column of a queries result set
		/// </summary>
		/// <remarks>
		/// Use this method to get an ArrayList of a the first column of a
		/// query.
		/// </remarks>
		/// <param name="query">The query to execute</param>
		/// <returns>An ArrayList of the results</returns>
		public ArrayList GetColumn(string query)
		{
			return this.GetColumn(query, 0);
		}

		/// <summary>
		/// Returns the specified row of a queries result set
		/// </summary>
		/// <remarks>
		/// Use this method to get an ArrayList of a particular row of a
		/// query. The row is a zero based index.
		/// </remarks>
		/// <param name="query">The query to execute</param>
		/// <param name="row">The row to retrieve</param>
		/// <returns>An ArrayList of the results</returns>
		public ArrayList GetRow(string query, int row)
		{
			SQLiteResultSet resultSet = this.Execute(query);

			return resultSet.GetRow(row);
		}

		/// <summary>
		/// Returns the first row of a queries result set
		/// </summary>
		/// <remarks>
		/// Use this method to get an ArrayList of a the first row of a
		/// query.
		/// </remarks>
		/// <param name="query">The query to execute</param>
		/// <returns>An ArrayList of the results</returns>
		public ArrayList GetRow(string query)
		{
			return this.GetRow(query, 0);
		}

		/// <summary>
		/// Returns a hashtable row of resultset with given row index
		/// </summary>
		/// <param name="query">The query to perform</param>
		/// <param name="row">The row index to return</param>
		/// <returns>Hashtable of row data</returns>
		public Hashtable GetRowHash(string query, int row)
		{
			SQLiteResultSet resultSet = this.Execute(query);

			return resultSet.GetRowHash(row);
		}

		/// <summary>
		/// Returns a hashtable of first row of resultset
		/// </summary>
		/// <param name="query">The query to perform</param>
		/// <returns>Hashtable of row data</returns>
		public Hashtable GetRowHash(string query)
		{
			return this.GetRowHash(query, 0);
		}

		/// <summary>
		/// Returns the first column of the first row of a queries result set
		/// </summary>
		/// <remarks>
		/// Useful for such queries as: SELECT COUNT(*) FROM myTable
		/// </remarks>
		/// <example>
		/// <code>
		/// db = new SQLite("testdb");
		/// string strCount = db.GetOne("SELECT COUNT(*) FROM myTable");
		/// int intCount = Int32.Parse(strCount);
		/// </code>
		/// </example>
		/// <param name="query">The query to execute</param>
		/// <returns>A string of the result</returns>
		public string GetOne(string query)
		{
			SQLiteResultSet resultSet = this.Execute(query);

			return resultSet.GetField(0, 0);
		}
		#endregion

        
	}
}
