using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SqliteClient
{
        //all these unmanaged stuff taken heavily from Finisar.SQLite.Net
        internal class SqliteString : IDisposable
        {
            internal readonly static Encoding SqliteEncoding = Encoding.UTF8;
            private string str;
            IntPtr ptr;

            // imports system functions for work with pointers
            [DllImport("kernel32")]
            internal extern static IntPtr HeapAlloc(IntPtr heap, UInt32 flags, UInt32 bytes);
            [DllImport("kernel32")]
            internal extern static IntPtr GetProcessHeap();
            [DllImport("kernel32")]
     		private extern static bool HeapFree(IntPtr heap, UInt32 flags, IntPtr lpMem);
            [DllImport("kernel32")]
            internal extern static int lstrlen(IntPtr str);

            public SqliteString(string str)
            {
                this.str = str;
                to_pointer();
            }

            private void to_pointer()
            {
                // if string is null, pointer is 0
                if (str == null)
                {
                    ptr =  IntPtr.Zero;
                }
                else
                {
                    // else, convert it to pointer
                    Byte[] bytes = SqliteEncoding.GetBytes(str);
                    int length = bytes.Length + 1;
                    ptr = HeapAlloc(GetProcessHeap(), 0, (UInt32)length);
                    Marshal.Copy(bytes, 0, ptr, bytes.Length);
                    Marshal.WriteByte(ptr, bytes.Length, 0);
                }
            }

            public IntPtr ToPointer()
            {
                return ptr;
            }

            public override string ToString()
            {
                return str;
            }
            
            internal unsafe static string PointerToString(sbyte* str)
            {
                return new String(str, 0, lstrlen(new IntPtr(str)), SqliteEncoding);
            }



            #region IDisposable Members

            public void Dispose()
            {
                HeapFree(GetProcessHeap(), 0, ptr);
            }

            #endregion
    }
}
