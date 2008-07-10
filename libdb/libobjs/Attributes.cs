using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Reflection;
using libdb;

namespace libdb
{
    [global::System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    internal sealed class AutoUpdatePropAttribute : Attribute
    {
        public AutoUpdatePropAttribute(string db_field, data_type data_type, bool nullable)
        {
            DBField = db_field;
            DataType = data_type;
            Nullable = nullable;
            ReadOnly = false;
        }

        public string DBField { get; set; }
        public libdb.data_type DataType { get; set; }
        public bool Nullable { get; set; }
        public string ConversionHandler { get; set; }
        public bool ReadOnly { get; set; }
    }
    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    internal sealed class AutoUpdateClassAttribute : Attribute
    {
        private Tables _associated_table;

        public Tables AssociatedTable
        {
            get { return _associated_table; }
            set { _associated_table = value; }
        }

        public AutoUpdateClassAttribute(Tables associate_table)
        {
            AssociatedTable = associate_table;
        }
    }
}