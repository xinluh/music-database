using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Reflection;
using libdb;


namespace libdb
{
    [AutoUpdateClass(Tables.tblPiece)]
    public class Piece : libobj
    {
        [AutoUpdateClass(Tables.tblGenre)]
        class _Genre : libobj
        {
            public _Genre() { }
            public _Genre(int id)
            {
                this.Fill(id);
            }
            public _Genre(string name)
            {
                ArrayList data;
                if ((data = Database.GetFirstRow("SELECT id, name FROM tblGenre WHERE name = '"
                        + name + "'")).Count != 0)
                {
                    ID = int.Parse(data[0].ToString());
                    Name = data[1].ToString();
                }
                else
                {
                    // TODO: add a new genre entry here...
                    throw new NotImplementedException();
                }
            }
            [AutoUpdateProp("name", data_type.text, false, ReadOnly = true)]
            public string Name { get; set; }
        }
        private _Genre genre = new _Genre();

        private string[] detail_lines;

        public Piece() 
        { 
            Composer = new Artist(); 
        }
        public Piece(int id) : this()
        {
            this.Fill(id);

            detail_lines = string.IsNullOrEmpty(details) ? null :
                details.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
        }

        [AutoUpdateProp("name", data_type.text, false)]
        public string Name { get; set; }

        [AutoUpdateProp("parent_piece_id", data_type.number_array, true)]
        public int[] ParentPieceID { get; set; }

        [AutoUpdateProp("old_name", data_type.text, true)]
        public string OtherName { get; set; }

        [AutoUpdateProp("connector", data_type.text, true)]
        public string Connector { get; set; }

        [AutoUpdateProp("composer_id", data_type.number, false)]
        private int composer_id
        {
            get { return Composer.ID; }
            set { Composer.Fill(value); }
        }

        [AutoUpdateProp("genre_id", data_type.number, false)]
        private int genre_id
        {
            get { return genre.ID; }
            set { genre.Fill(value); }
        }

        [AutoUpdateProp("detail", data_type.text, true)]
        private string details { get; set; }

        [AutoUpdateProp("text", data_type.text, true)]
        public string Text { get; set; }

        public string[] Details
        {
            get { return detail_lines; }
            set { details = (value == null)? "" : string.Join("||", value); }
        }

        public string Genre 
        { 
            get { return (genre != null) ? genre.Name : ""; }
            set { genre = new _Genre(value); }
        }

        public Artist Composer { get; set; }


        public override int Update()
        {
            // urgent TODO: don't update the unnecessary!
            Composer.Update();
            genre.Update();

            return base.Update();
        }

        public override int Insert()
        {
            if (Composer.ID == 0)
                Composer.Insert();

            return base.Insert();
        }

		internal void ReadFromFile(Tag tag)
		{
		  // TODO
		}

    }
}
