using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Reflection;
using HundredMilesSoftware.UltraID3Lib;
using System.IO;
using libdb;


namespace libdb
{
   
    [AutoUpdateClass(Tables.tblTrack)]
    public class Track : libobj
    {
        private Album album;
        private Piece piece;
        

        public Track() { }
        public Track(int id)
        {
            this.Fill(id);
        }
        public Track(int id,Album album)
        {
            this.Fill(id);

            if (album == null)
                this.album = new Album(album_id);
            else    
                this.album = album;
        }

        [AutoUpdateProp("name",data_type.text,false)]
        public string Name { get; set; }

        [AutoUpdateProp("disc_num",data_type.number,false)]
        public int DiscNumber { get; set; }

        [AutoUpdateProp("track_num",data_type.number,false)]
        public int TrackNumber { get; set; }

        [AutoUpdateProp("filename",data_type.text,false)]
        public string FileName { get; set; }

        [AutoUpdateProp("length",data_type.number,false)]
        public int Length { get; set; }

        [AutoUpdateProp("size",data_type.number,false)]
        public int Size { get; set; }

        [AutoUpdateProp("year",data_type.number,true)]
        public int? Year { get; set; }

        [AutoUpdateProp("comment",data_type.text,true)]
        public string Comment { get; set; }

        [AutoUpdateProp("need_update", data_type.boolean, false)]
        public bool NeedUpdate { get; set; }

        [AutoUpdateProp("album_id", data_type.number, false)]
        private int album_id { get; set; }

        [AutoUpdateProp("piece_id", data_type.number, false)]
        private int piece_id
        {
            get { return this.piece.ID; }
            set { piece.Fill(value); }
        }

        [AutoUpdateProp("performer", data_type.number_array, true)]
        private int[] performer_ids
        {
            get
            {
                List<int> ints = new List<int>(Performers.Count);
                Performers.ForEach((artist) => ints.Add(artist.ID));
                return ints.ToArray();
            }
            set
            {
				// create new Artists objects based on the string of IDs
                Performers = new List<Artist>(value.Length);
                System.Array.ForEach<int>(value, (i) => Performers.Add(new Artist(i)));
            }
        }
        
        public Album Album { 
            get {return album;}
            set {
                if (value == null) throw new NullReferenceException();
                this.album = value;
                this.album_id = (this.Album == null) ? 0 : this.Album.ID;
            }
        }
        public Piece Piece
        {
            get { return this.piece; }
            set
            {
                if (value == null)
                    throw new NullReferenceException();
                else
                    this.piece = value;
            }
        }
        public List<Artist> Performers { get; set; }






        public void GetInfoFromFile(string filename)
        {
            FileInfo finfo = new FileInfo(filename);
            GetInfoFromFile(finfo);
        }

        public void GetInfoFromFile(FileInfo finfo)
        {
            if (finfo == null || !finfo.Exists ) 
            {
                this.FileName = "?";
                this.Size = 0;
                this.Length = 0;
            }

            this.Size = (int) finfo.Length;

            UltraID3 u = new UltraID3();
            try
            { 
                u.Read(finfo.FullName);

                this.Length = (int) System.Math.Round(u.Duration.TotalSeconds, 0);
            }
            catch (UltraID3Exception e)
            {
                Debug.HandleException(e); 
            }


        }

    }

}
