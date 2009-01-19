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

namespace SqliteClient
{
	/// <summary>
	/// Exception class to support SQLiteClient class
	/// </summary>
	public class SQLiteException : Exception
	{
		#region Fields
		private SQLiteClient.ResultCode errorCode;
		#endregion

		#region Properties
		/// <summary>
		/// Returns any error code associated with this exception
		/// </summary>
		public SQLiteClient.ResultCode ErrorCode
		{
			get {
				return this.errorCode;
			}
		}
		#endregion

		#region Constructor(s)
		/// <summary>
		/// Instanciates a new copy of the SQLiteException class with the given
		/// error message and errorcode
		/// </summary>
		/// <param name="message">The error message</param>
		/// <param name="code">The errorcode (see <see cref="SQLiteClient.ResultCode">ResultCode</see>)</param>
		public SQLiteException(string message, SQLiteClient.ResultCode code) : base(message)
		{
			this.errorCode = code;
		}

		/// <summary>
		/// Instanciates a new copy of the SQLiteException class with the given
		/// error message
		/// </summary>
		/// <param name="message">The error message</param>
		public SQLiteException(string message) : base(message)
		{
		}
		#endregion
	}
}
