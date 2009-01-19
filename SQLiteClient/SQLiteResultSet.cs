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
using System.Collections;
using System.Runtime.InteropServices;
using SqliteClient;

namespace SqliteClient
{
	#region Delegates
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal unsafe delegate int SQLiteCallback(IntPtr pArg, int argc, sbyte** argv, sbyte** columnNames);
	#endregion

	/// <summary>
	/// ResultSet class to complement the SQLiteClient class and represent a single
	/// result set from a select query.
	/// </summary>
	public class SQLiteResultSet
	{
		#region Fields
		private Hashtable columnIndexes;
		internal ArrayList columnNames;
		internal ArrayList rowData;
		private int internalRowPointer;
		#endregion

		#region Properties
		/// <summary>
		/// Returns the row data as an ArrayList.
		/// </summary>
		/// <remarks>
		/// Each entry in the ArrayList will itself be an ArrayList of
		/// the fields.
		/// </remarks>
		public ArrayList Rows
		{
			get {
				return this.rowData;
			}
		}

		/// <summary>
		/// Returns an ArrayList of the column names associated with
		/// this result set.
		/// </summary>
		public ArrayList ColumnNames
		{
			get {
				return this.columnNames;
			}
		}

		/// <summary>
		/// Use in conjunction with GetRow() or GetRowHash() to determine
		///  if there is more data left to be retrieved.
		/// </summary>
		/// <remarks>
		/// Eg.
		/// <code>
		/// while (results.IsMoreData()) {
		///		ArrayList row = results.GetRow();
		///		
		///		// Do something with the row data...
		/// }
		/// </code>
		/// </remarks>
		public bool IsMoreData
		{
			get {
				return this.internalRowPointer < this.rowData.Count;
			}
		}
		#endregion

		#region Constructor(s)
		/// <summary>
		/// Initialises a new instance of an SQLiteResultSet object
		/// </summary>
		public SQLiteResultSet()
		{
			this.columnIndexes = new Hashtable();
			this.columnNames   = new ArrayList();
			this.rowData       = new ArrayList();
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Moves the internal row pointer to the given
		/// index. Returns true/false as to whether it 
		/// succeeded. (Fails if there aren't enough rows).
		/// </summary>
		/// <param name="index">Index to seek to (zero based)</param>
		public bool Seek(int index)
		{
			if (index < this.rowData.Count) {
				this.internalRowPointer = index;
				return true;
			}

			return false;
		}

		/// <summary>
		/// Resets the internal row pointer to zero (start of result set).
		/// </summary>
		public void Reset()
		{
			this.internalRowPointer = 0;
		}

		/// <summary>
		/// Returns the next row from the result set based on the
		/// internal row pointer. 
		/// </summary>
		/// <returns>ArrayList of the next row data</returns>
		public ArrayList GetRow()
		{
			return this.GetRow(this.internalRowPointer++);
		}

		/// <summary>
		/// Gets the row with the supplied index.
		/// </summary>
		/// <remarks>
		/// The row index is zero based.
		/// </remarks>
		/// <param name="rowIndex">The row to retrieve</param>
		/// <returns>An ArrayList of the fields</returns>
		public ArrayList GetRow(int rowIndex)
		{
			if (this.rowData.Count >= (rowIndex + 1)) {
				return (ArrayList)this.rowData[rowIndex];
			} else {
				return null;
			}
		}

		/// <summary>
		/// Returns a Hashtable of the next row
		/// </summary>
		/// <remarks>
		/// Similar to GetRow() but returns a Hashtable with the column
		/// names as the key and column values as the value.
		/// </remarks>
		/// <returns>Hashtable of the row data</returns>
		public Hashtable GetRowHash()
		{
			return this.GetRowHash(this.internalRowPointer++);
		}

		/// <summary>
		/// Returns a Hashtable of the given row index
		/// </summary>
		/// <remarks>
		/// Similar to GetRow(idx) but returns a Hashtable with the column
		/// names as the key and column values as the value.
		/// </remarks>
		/// <param name="rowIndex">Index of row to return</param>
		/// <returns>Hashtable of the row data</returns>
		public Hashtable GetRowHash(int rowIndex) 
		{
			Hashtable ht = new Hashtable(this.columnNames.Count);
			ArrayList rowData = GetRow(rowIndex);

			for (int i=0; i<rowData.Count; ++i) {
				if (rowData[i] != null) {
					ht[(string)this.columnNames[i]] = rowData[i];
				} else {
					ht[(string)this.columnNames[i]] = null;
				}
			}

			return ht;
		}

		/// <summary>
		/// Returns an ArrayList of a column of data in the result set.
		/// </summary>
		/// <remarks>
		/// The column index is zero based. A quicker way of getting a
		/// single column of data is the <see cref="SQLiteClient.GetColumn">
		/// GetColumn</see> method. Returns an empty ArrayList if the column
		/// index is not valid.
		/// </remarks>
		/// <param name="columnIndex">The column to retrieve</param>
		/// <returns>An ArrayList of the column data</returns>
		public ArrayList GetColumn(int columnIndex)
		{
			ArrayList retval = new ArrayList();

			if (this.columnNames.Count >= (columnIndex + 1)) {
				foreach (ArrayList row in this.rowData) {
					retval.Add(row[columnIndex]);
				}
			}

			return retval;
		}

		/// <summary>
		/// Returns an ArrayList of a column of data in the result set
		/// using the coumn name.
		/// </summary>
		/// <remarks>
		/// Returns an empty ArrayList if the given column name is not defined.
		/// </remarks>
		/// <param name="columnName">The name of the column to retrieve</param>
		/// <returns>An ArrayList of the column data</returns>
		public ArrayList GetColumn(string columnName)
		{
			if (this.columnIndexes.ContainsKey(columnName)) {
				return this.GetColumn((int)this.columnIndexes[columnName]);
			} else {
				return new ArrayList();
			}
		}

		/// <summary>
		/// Gets a particular field of a particular row from the result set.
		/// </summary>
		/// <remarks>
		/// Both the row and column indexes are zero based.
		/// </remarks>
		/// <param name="rowIndex">The row index of the field</param>
		/// <param name="columnIndex">The column index of the field</param>
		/// <returns>A string of the field data</returns>
		public string GetField(int rowIndex, int columnIndex)
		{
			if (this.rowData.Count >= (rowIndex + 1) && this.ColumnNames.Count >= (columnIndex + 1)) {
				ArrayList row = this.GetRow(rowIndex);

				return row[columnIndex].ToString();
			} else {
				return "";
			}
		}
		#endregion

		#region Private methods
		/// <summary>
		/// Callback which populates the result set from the queries results
		/// </summary>
		/// <param name="pArg">Not used</param>
		/// <param name="argc">Number of columns in argv</param>
		/// <param name="argv">The column data in this row</param>
		/// <param name="columnNames">The column names</param>
		internal unsafe int CallBack(IntPtr pArg, int argc, sbyte ** argv, sbyte ** columnNames)
		{
			// First time in? Add column names
			if (this.columnNames.Count == 0) {
				for (int i=0; i<argc; ++i) {
					this.columnNames.Add(new String(columnNames[i]));
				}
			}

			ArrayList row = new ArrayList();

			// Go through this rows data
			for (int i=0; i<argc; ++i) {
				if (argv[i] == ((sbyte *)0) ) {
					row.Add(null);
				} else {
					row.Add(new String(argv[i]));
				}
			}

			this.rowData.Add(row);

			return 0;
		}
		#endregion
	}
}
