using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MusicLib
{
    public static class Utils
    {
        public static int FindFirstRow(this System.Windows.Forms.DataGridView dg,string TextToFind,int ColumnIndex)
        {
            if (ColumnIndex >= dg.Columns.Count) throw new ArgumentOutOfRangeException();

            //System.Collections.Generic.List<DataGridViewRow> list = new List<DataGridViewRow>(dg.Rows.Count);

            System.Collections.ArrayList ls = new System.Collections.ArrayList(dg.Rows.Count);

            for (int i = 0; i < dg.Rows.Count; i++)
            {
                if (!dg.Rows[i].IsNewRow)
                    ls.Add(dg.Rows[i]);
            }

            return ls.BinarySearch(TextToFind,new CompareDataGridRowAndString(ColumnIndex));
        }

        public class CompareDataGridRowAndString : IComparer
        {
            int columnIndex = -1;

            public CompareDataGridRowAndString(int ColumnIndex)
            {
                columnIndex = ColumnIndex;
            }

            #region IComparer Members

            public int Compare(object x, object y)
            {
                DataGridViewRow dr = (DataGridViewRow) x; 
                string s2 = y as string;

                if (dr == null || s2 == null)
                    throw new InvalidOperationException("This class only compares string and datagridviewrow!");

                if (columnIndex >= dr.Cells.Count)
                    throw new IndexOutOfRangeException();
                
                string s1;
                if (!dr.IsNewRow)
                    s1 = dr.Cells[columnIndex].Value.ToString();
                else
                    return 1;

               if (s1.Trim().ToUpper() == s2.Trim().ToUpper())
                    return 0;
                else return libdb.NaturalSortComparer.Default.Compare(s1,s2);
            }

            #endregion
        }
    }
}
