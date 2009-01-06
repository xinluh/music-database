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
        public const string UnknownPath = "?";
        private Album album;
        private Piece piece;
        

        public Track()
        {
            Performers = new List<Artist>();
        }
        public Track(int id) : this()
        {
            this.Fill(id);
            this.album = new Album(album_id);
        }
        public Track(int id,Album album) : this()
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
            set { piece = new Piece(value); }
        }

        [AutoUpdateProp("performer", data_type.number_array, true)]
        private int[] performer_ids
        {
            get
            {
                if (Performers.Count == 0) return null;

                List<int> ints = new List<int>(Performers.Count);
                Performers.ForEach((artist) => ints.Add(artist.ID));
                return ints.ToArray();
            }
            set
            {
                if (value == null) return;
				// create new Artists objects based on the string of IDs
                Performers.Clear();
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






        public void ReadFromFile(string filename)
        {
            FileInfo finfo = new FileInfo(filename);
            ReadFromFile(finfo);
        }

        public void ReadFromFile(FileInfo finfo)
        {
            if (finfo == null || !finfo.Exists ) 
            {
                this.FileName = "?";
                this.Size = 0;
                this.Length = 0;
				return;
            }
			
			// TODO: Check whether is valid mp3 file

			Tag tag = new Tag(finfo.FullName);

            Size = (int) finfo.Length;
			Length = (int) System.Math.Round(tag.UltraID3.Duration.TotalSeconds, 0);
			FileName = finfo.FullName;
			ID = tag.TrackID ?? 0;
			TrackNumber = tag.TrackNum ?? 0;
			Year = tag.Year ?? 0;
			DiscNumber = tag.DiscNum ??  1;
            Name = tag.Title;
            //TODO: Performers = ???

            if (Piece == null) Piece = new Piece();
            if (Album == null) Album = new Album();

            //Piece.ReadFromTag(tag);
            //Album.ReadFromTag(tag);
        }

        public void WriteToFile()
        {
            if (string.IsNullOrEmpty(FileName) || !File.Exists(FileName))
                return;

            if (Album == null)
                throw new NullReferenceException("This track's parent album is null; cannot write to file!");

            if (Piece == null)
                throw new NullReferenceException("This track's piece infomation is null; cannot write to file!");

            // TODO: Check whether is valid mp3 file

            Tag tag = new Tag(FileName);
            tag.Clear();

            tag.Year = Year; 
            tag.SetTrack(TrackNumber, Album.TotalTracks[DiscNumber - 1]);
            tag.SetDiscNum(DiscNumber, Album.TotalDisc);
            tag.TrackID = ID;
            tag.Title = Name;

            List<string> s = new List<string>();
            Performers.ForEach((a) => s.Add(a.GetName(Artist.NameFormats.Last_First_Type)));
            tag.Performers = s.ToArray();

            Piece.WriteToTag(tag);
            Album.WriteToTag(tag);

            tag.Write();

            //update the file size after adding/changing tag
            FileInfo f = new FileInfo(FileName);
            Size = (int)f.Length;
            Length = (int)System.Math.Round(tag.UltraID3.Duration.TotalSeconds, 0);
            Update();
        }
    }

}
