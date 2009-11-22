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
            public _ArtistName(string name)
            {
                Fill(name);
            }
            
            internal void parse_name(string name)
            {
                if ((name.StartsWith("\"") && name.EndsWith("\"")) || // string entirely double quoted
                    (!name.Contains(",") && !name.Trim().Contains(" "))) // name is a single word
                {
                    LastName = name.Trim('"');
                    FirstName = null;
                }
                else if (name.Contains(",")) // probably "lastname, firstname" format 
                {
                    string[] s = name.Split(new char[] {','}, System.StringSplitOptions.RemoveEmptyEntries);
                    LastName = s[0].Trim(' ','"');
                    FirstName = s[1].Trim(' ','"');
                }
                else 
                {
                    string[] s = name.Split(new char[] {' '}, System.StringSplitOptions.RemoveEmptyEntries);
                    LastName = s[s.Length - 1].Trim(' ','"');
                    FirstName = string.Join(" ", s, 0, s.Length - 1);
                }
            }
            
            public void Fill(string name)
            {
                parse_name(name);
                ID = GuessID();
                if (ID > 0) Fill(ID);
            }

            public int GuessID()
            {
                string ret = (string)Database.GetScalar(string.Format(
                    "SELECT id FROM {0} WHERE (last_name LIKE '{1}' OR alternate_last LIKE '%{1}|%') {2} LIMIT 1",
                    "tblartistname", 
                    Database.Quote(LastName.ToLower().Trim(),false), 
                    string.IsNullOrEmpty(FirstName)? "": string.Format(
                        "AND (first_name LIKE '{0}' OR alternate_first LIKE '%{0}|%')", 
                        Database.Quote(FirstName.ToLower().Trim(),false))));
                return string.IsNullOrEmpty(ret) ? 0 : int.Parse(ret);
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
            public _ArtistType (string name)
	        {
                this.Fill(name);   
	        }

            [AutoUpdateProp("name", data_type.text, true)]
            public string Name { get; set; }

            public void Fill(string name)
            {
 	            ID = GuessID(name);
                if (ID != 0)
                    this.Fill(ID);
                else
                    this.Name = name.ToLower();
            }

            internal static int GuessID(string t)
            {  //todo:  maybe the "name" in the sql string should be gotten directly from the AutoUpdateProp of property Name?
                string ret = (string)Database.GetScalar(string.Format(
                    "SELECT id FROM {0} WHERE name = {1} LIMIT 1",
                    "tblartisttype", Database.Quote(t.ToLower().Trim())));
                return string.IsNullOrEmpty(ret) ? 0 : int.Parse(ret);
            }
        }

        _ArtistName name = new _ArtistName();
        _ArtistType type = new _ArtistType();

        public Artist() { }
        public Artist(int id)
        {
            this.Fill(id);
        }

        /// <summary>
        /// Fill with information from database if an artist matching name and type is found (not case sensitive); otherwise 
        /// fill the necessary fields so that the newly created object is ready for Insert() into database. Whether the artist 
        /// given matchs a record in database can be checked by checking whether the ID is non-zero.
        /// </summary>
        /// <param name="name">See specification of property name for the valid formatting accepted</param>
        /// <param name="type">i.e. composer, pianist etc.</param>
        /// <param param name="autoInsertNewType">sets the property WillAutoInsertNewType</param>
        public Artist(string name, string type)
        {
            Type = type;
            this.name.Fill(name);
            ID = 0;

            if (this.type.ID != 0 && this.name.ID != 0)
            {
                string ret = (string)Database.GetScalar(string.Format(
                    "SELECT id FROM {0} WHERE name_id = {1} AND type_id = {2} LIMIT 1",
                    "tblartist", name_id, type_id));
                ID = string.IsNullOrEmpty(ret) ? 0 : int.Parse(ret);
            }            
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

        /// <summary>
        /// Use GetName() to get the value in different formats.
        /// Accept string formatted either as "firstname middlenames lastname" or "lastname, firstname middlenames"
        /// if a string is given as double quoted, it will be treated as only lastname; i.e. "\"New York Philharmonic\"" 
        /// will NOT be treated as a person with lastname "Philharmonic"
        /// </summary>
        public string Name
        {
            get { return GetName(NameFormats.Last_First); }
            set { name.parse_name(value); }
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
                    return (!(string.IsNullOrEmpty(name.FirstName)) ? name.FirstName + " " : "") + name.LastName;
                case NameFormats.Last_First:
                    return name.LastName + ((!string.IsNullOrEmpty(name.FirstName)) ? (", " + name.FirstName) : "");
                case NameFormats.First_Last_Type:
                    if (this.type_id == 3 || type_id ==4 || type_id == 5)
                        return GetName(NameFormats.First_Last);
                    else
                        return GetName(NameFormats.First_Last) + ", " + type.Name;
                case NameFormats.Last_First_Type:
                    if (this.type_id == 3 || type_id == 4 || type_id == 5)
                        return GetName(NameFormats.Last_First);
                    else
                        return GetName(NameFormats.Last_First) + " (" + type.Name + ")";
                default:
                    return "";
            }
        }

        public string Type 
        {
            get { return GetName(NameFormats.Type); }
            set
            {
                type.Fill(value);
                if (type.ID == 0)
                    type.Insert();
            }
        }

        public static bool TypeExists(string t)
        {
            return _ArtistType.GuessID(t) != 0;
        }

        public override void Insert()
        {
            name.Commit();
            type.Commit();
            base.Insert();
        }

        public override void Update()
        {
            name.Commit();
            type.Commit();
            base.Update();
        }

        public override string ToString()
        {
            return GetName(NameFormats.Last_First);
        }
    }
}
