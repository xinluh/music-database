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
            ParentPieces = new List<Piece>();
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
        private int[] parent_piece_id 
        {
            get
            {
                List<int> l = new List<int>();
                ParentPieces.ForEach((i) => l.Add(i.ID));
                return l.ToArray();
            }
            set
            {
                ParentPieces.Clear();
                if (value == null) return;
                Array.ForEach<int>(value, (i) => ParentPieces.Add(new Piece(i)));
            }
        }

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
        public List<Piece> ParentPieces { get; private set; }

        public override void Update()
        {
            Composer.Update();
            genre.Update();
            base.Update();
        }

        public override void Insert()
        {
            Composer.Commit();
            base.Insert();
        }

		internal void ReadFromTag(Tag tag)
		{
		  // TODO
		}


        internal void WriteToTag(Tag tag)
        {
            // if there is no piece from which this piece derived from or the composer himself transcripted the piece,
            // then no need to put composer name as e.g. Bach-Bach
            if (ParentPieces.Count == 0 || ParentPieces.TrueForAll((Piece p) => {return p.Composer.ID == Composer.ID;})) 
                tag.Composer = new string[] { Composer.GetName(Artist.NameFormats.Last_First) };
            else // make the first artist name as e.g. Bach-Busoni, then append the full names of all involved composers
            {
                List<Artist> l = new List<Artist>();
                l.Add(Composer);

                // TODO: for simplicity only the first parent piece (and it's parentage) are included in the list of composers
                Piece p = ParentPieces[0];
                while (p != null)
                {
                    l.Insert(0, p.Composer);
                    p = p.ParentPieces[0];
                }

                List<string> s1 = new List<string>();
                List<string> s2 = new List<string>();
                l.ForEach((a) => {s1.Add(a.GetName(Artist.NameFormats.Last_First)); s2.Add(a.GetName(Artist.NameFormats.Last));});
                s1.Insert(0, string.Join("-", s2.ToArray()));
                tag.Composer = s1.ToArray();
            }

            tag.Genre = Genre;
            tag.ContentGroup = Name;
            //tag.UnsynchedLyric = Artist.GetName(t.Performer, Artist.NameType.FirstNameLastNameWithBriefArtistType, Constants.vbCrLf).Trim;
            //tag.Comment = String.Join(vbCrLf, New String() {al.CDComment, t.TrackComment, Piece.ExtraInfo, Piece.Text})

        }
    }
}
