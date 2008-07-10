using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Reflection;
using libdb;


namespace libdb
{

    [AutoUpdateClass(Tables.tblArtist)]
    public class Artist : libobj
    {
        [AutoUpdateClass(Tables.tblArtistName)]
        class _ArtistName : libobj
        {
            public _ArtistName() { }
            public _ArtistName(int id)
            {
                this.Fill(id);
            }

            public override int ID { get; set; }

            [AutoUpdateProp("last_name", data_type.text, false)]
            public string LastName { get; set; }

            [AutoUpdateProp("first_name", data_type.text, true)]
            public string FirstName { get; set; }

            [AutoUpdateProp("alternate_last", data_type.text, true)]
            public string AlternateLastName { get; set; }

            [AutoUpdateProp("alternate_first", data_type.text, true)]
            public string AlternateFirstName { get; set; }
        }

        [AutoUpdateClass(Tables.tblArtistType)]
        class _ArtistType : libobj
        {
            public override int ID { get; set; }
            public _ArtistType() { }
            public _ArtistType(int id)
            {
                this.Fill(id);
            }

            [AutoUpdateProp("name", data_type.text, true)]
            public string Name { get; set; }
        }

        _ArtistName name = new _ArtistName();
        _ArtistType type = new _ArtistType();

        public Artist() { }
        public Artist(int id)
        {
            this.Fill(id);
        }

        [AutoUpdateProp("name_id", data_type.number, false)]
        private int name_id
        {
            get { return this.name.ID; }
            set { name.Fill(value); }
        }

        [AutoUpdateProp("type_id", data_type.number, false)]
        private int type_id
        {
            get { return this.type.ID; }
            set { type.Fill(value); }
        }

        public enum NameFormats
        {
            Last,
            First,
            Type,
            First_Last,
            Last_First,
            First_Last_Type,
            Last_First_Type,
        }

        public string GetName(NameFormats format)
        {
            switch (format)
            {
                case NameFormats.Last:
                    return name.LastName;
                case NameFormats.First:
                    return (name.FirstName) ?? "";
                case NameFormats.Type:
                    return type.Name;
                case NameFormats.First_Last:
                    return ((string.IsNullOrEmpty(name.FirstName)) ? name.FirstName + " " : "") + name.LastName;
                case NameFormats.Last_First:
                    return name.LastName + ((string.IsNullOrEmpty(name.FirstName)) ? "," + name.FirstName : "");
                case NameFormats.First_Last_Type:
                    return GetName(NameFormats.First_Last) + ", " + type.Name;
                case NameFormats.Last_First_Type:
                    return GetName(NameFormats.Last_First) + "(" + type.Name + ")";
                default:
                    return "";
            }
        }
    }
}
