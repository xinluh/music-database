using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Reflection;
using libdb;

namespace libdb
{
    //dynamically generated db schema based on the attributes set on the varous classes
    //generation happens only when requested - must call IsAutoupdate() first
    //would be perfect for a preprocessor, but alas c# does not have one...
    internal static class db_structure
    {
        private class table_info
        {
            private PropertyInfo[] _props;
            
            #region ctors
            public table_info(AutoUpdateClassAttribute class_attr, PropertyInfo[] props)
            {
                ClassAttr = class_attr;
                _props = props;
            }

            public table_info() { }

            #endregion

            public AutoUpdateClassAttribute ClassAttr { get; set; }
            
            public PropertyInfo[] PropertyInfos
            {
                get { return _props; }
                set
                {
                    _props = value;
                    if (value != null) //then parse the propinfo once for all...
                    {
                        AutoUpdatePropAttribute attr;
                        ArrayList names = new ArrayList();
                        ArrayList datatype = new ArrayList();
                        ArrayList nullable = new ArrayList();
                        ArrayList chandler = new ArrayList();
                        ArrayList writable = new ArrayList();

                        foreach (PropertyInfo p in value)
                        {
                            attr = (AutoUpdatePropAttribute)
                               p.GetCustomAttributes(typeof(AutoUpdatePropAttribute), false)[0];
                            names.Add(attr.DBField);
                            datatype.Add(attr.DataType);
                            nullable.Add(attr.Nullable);
                            writable.Add(!attr.ReadOnly);
                            if (!string.IsNullOrEmpty(attr.ConversionHandler))
                            {
                                Type a = p.DeclaringType;
                                MethodInfo mi = a.GetMethod(attr.ConversionHandler, BindingFlags.Static | BindingFlags.NonPublic);

                                chandler.Add(Delegate.CreateDelegate(typeof(ConversionHandler), mi));
                            }
                            else
                            {
                                chandler.Add(null);
                            }
                        }

                        FieldNames = (string[])names.ToArray(typeof(string));
                        DataTypes = (data_type[])datatype.ToArray(typeof(data_type));
                        IsNullable = (bool[])nullable.ToArray(typeof(bool));
                        ConversioHandlers = (ConversionHandler[])chandler.ToArray(typeof(ConversionHandler));
                        Writable = (bool[])writable.ToArray(typeof(bool));
                    }
                }
            }
            public string[] FieldNames { get; private set; }
            public data_type[] DataTypes { get; private set; }
            public bool[] IsNullable { get; private set; }
            public ConversionHandler[] ConversioHandlers { get; private set; }
            public bool[] Writable { get; private set; }
        }
        static Dictionary<string, table_info> _tables = new Dictionary<string, table_info>();

        public static bool IsAutoupdate(libobj obj)
        {
            Type type = obj.GetType();
            string name = type.Name;
            table_info t;

            if (!_tables.ContainsKey(name))
            {
                AutoUpdateClassAttribute[] attr = (AutoUpdateClassAttribute[])
                    type.GetCustomAttributes( typeof(AutoUpdateClassAttribute), false);
                t = new table_info();

                if (attr.Length > 0) //class labeled as autoupde-able
                {
                    PropertyInfo[] props = type.GetProperties(BindingFlags.NonPublic |
                        BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
                    AutoUpdatePropAttribute[] attrs;
                    List<PropertyInfo> updateable_props = new List<PropertyInfo>();

                    System.Array.ForEach<PropertyInfo>(props,(p) =>
                    {
                        attrs = (AutoUpdatePropAttribute[])p.GetCustomAttributes(
                            typeof(AutoUpdatePropAttribute), false);

                        if (attrs.Length > 0)
                            updateable_props.Add(p);
                    });

                    // if no property is labeled to be updated to a specific field (i.e. updateable_prop.count == 0)
                    // then there is nothing to update; thus only populate table_info if necc.
                    if (!(updateable_props.Count == 0))
                    {
                        t.ClassAttr = attr[0];
                        t.PropertyInfos = updateable_props.ToArray();
                    }
                } //end if (attr.Length > 0) //class labeled as autoupde-able

                _tables.Add(name, t);

            }
            else //if (!_tables.ContainsKey(name))
            {
                _tables.TryGetValue(name, out t);
            } //end if (!_tables.ContainsKey(name))

            return (t.ClassAttr != null);
        }
        public static Tables GetTable(libobj obj)
        {
            return get_table_info(obj).ClassAttr.AssociatedTable;
        }
        public static string GetTableName(libobj obj)
        {
            return System.Enum.GetName(typeof(Tables), GetTable(obj));
        }
        public static PropertyInfo[] PropertiesToBeUpdated(libobj obj)
        {
            return get_table_info(obj).PropertyInfos;
        }
        public static string[] GetFieldNames(libobj obj) { return get_table_info(obj).FieldNames; }
        public static data_type[] GetDataTypes(libobj obj) { return get_table_info(obj).DataTypes; }
        public static bool[] GetIsNullable(libobj obj) { return get_table_info(obj).IsNullable; }
        public static bool[] GetWritable(libobj obj) { return get_table_info(obj).Writable; }
        public static ConversionHandler[] GetConversionHandlers(libobj obj)
        { return get_table_info(obj).ConversioHandlers; }
        
        /// <summary>
        /// Remove the readonly member of the list
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string[] CleanUpForWrite(libobj obj, string[] list)
        {
            List<string> newlist = new List<string>(list.Length);
            
            for (int i = 0; i < list.Length; i++)
                if (GetWritable(obj)[i]) { newlist.Add(list[i]); }
            
            return newlist.ToArray();
        }

        private static table_info get_table_info(libobj obj)
        {
            table_info t;
            _tables.TryGetValue(obj.GetType().Name, out t);
            return t;
        }
    }
}