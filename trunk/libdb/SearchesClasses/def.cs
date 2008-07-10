using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using SqliteClient;


namespace libdb
{
    [global::System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    sealed class DesAttribute : Attribute
    {
        // This is a positional argument
        public DesAttribute(params string[] s)
        {
            DescriptionStrings = s;
        }

        public string[] DescriptionStrings { get; private set; }

    }

    public abstract class SearchBase
    {
        protected Utils.MultiValueDictionary<Enum, string> filters = new Utils.MultiValueDictionary<Enum, string>();
        IEnumerable<object> fields;
        protected string columns = "";
        
        abstract protected string table { get; }
        abstract protected string orderby { get; }

        //public System.Data.DataTable PerformSearchToTable();

        protected SearchBase()
        {
            SelectUnique = true;
        }
        
        public bool SelectUnique { get; set; }
        
        /// <summary>
        /// Clear all the filters.
        /// </summary>
        public void ClearFilters()
        {
            filters.Clear();
        }
        public IEnumerable<IList> PerformSearchToList()
        {
            return (IEnumerable<IList>) Database.Execute(get_sql(),Database.OutputFormats.IEnumerableOfList,null);
        }

        public System.Windows.Forms.DataGridView PerformSearchToDataGridView(System.Windows.Forms.DataGridView dg)
        {
            bool allow_add_row = true;
            if (dg != null) 
            { 
                dg.Rows.Clear();
                allow_add_row = dg.AllowUserToAddRows;
                dg.AllowUserToAddRows = false;
            }

            dg = (System.Windows.Forms.DataGridView) Database.Execute(get_sql(),Database.OutputFormats.DataGridView,dg);           
            dg.AllowUserToAddRows = allow_add_row;
            return dg;
        }

        protected string get_sql()
        {
            string conditions = get_conditions();
            return string.Format("SELECT {0} FROM {1} {2} {3}", columns, table,
                (string.IsNullOrEmpty(conditions) ? "" : ("WHERE " + conditions)),orderby);
        }

        /// <summary>
        /// Get the Des attribute associated with an enum item
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static protected string[] enum_to_des(Enum value)
        {
            return ((DesAttribute)
                value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DesAttribute), 
                false)[0]).DescriptionStrings;
        }

        protected void set_field_to_search(IEnumerable<object> fields)
        {
            this.fields = fields;
            StringBuilder s = new StringBuilder();
            
            foreach (object f in fields)
            {
                s.Append(enum_to_des((Enum) f)[0] + ",");
            }
            if (s.Length > 0)
            {
                s.Remove(s.Length - 1, 1); //removing the ending "," 
            }

            this.columns = s.ToString();
        }

        protected void add_filter(Enum value, string filterstring)
        {
            if (!filterstring.Contains("{0}"))
            { //append column name in front if a string.format placeholder is not found
                filterstring = "{0} " + filterstring;
            }

            filterstring = string.Format(filterstring, enum_to_des(value));
            filters.Add(value,"(" + filterstring + ")");
        }

        protected void add_words_filter(Enum value, string filter)
        {
            if (string.IsNullOrEmpty(filter)) return;

            string[] words = filter.Split(new string[]{" "}, StringSplitOptions.RemoveEmptyEntries);
            string field = enum_to_des(value)[0];
            StringBuilder str = new StringBuilder();
            foreach (string s in words)
            {
                str.AppendFormat("({0} LIKE '%{1}%') AND ", field, s);
            }

            if (str.Length > 0)
            {
                str.Remove(str.Length - 5, 5); //removing the ending " AND " 
            }
            filters.Add(value, str.ToString());
        }

        protected string[] concat_conditions()
        {
            System.Collections.ArrayList list = new System.Collections.ArrayList();
            foreach (List<string> s in filters.Values)
            {
                list.Add("(" + string.Join(" OR ", (string[]) s.ToArray()) + ")");
            }
            return (string[]) list.ToArray(typeof(String));
        }

        protected virtual string get_conditions()
        {
            return string.Join(" AND ", concat_conditions());
        }
    }

}

namespace libdb.Utils
{
    public class MultiValueDictionary<TKey, TValue> : Dictionary<TKey, List<TValue>>
    {
        public void Add(TKey key, TValue value)
        {
            List<TValue> lst;
            if (!base.ContainsKey(key))
            {
                lst = new List<TValue>();
                lst.Add(value);
            }
            else
            {
                base.TryGetValue(key, out lst);
                lst.Add(value);
                base.Remove(key);
            }
            base.Add(key, lst);
        }
    }
}