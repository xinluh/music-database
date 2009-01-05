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

            this.Size = (int) finfo.Length;
			this.Length = (int) System.Math.Round(tag.UltraID3.Duration.TotalSeconds, 0);
			this.FileName = finfo.FullName;
			this.ID = tag.TrackID ?? 0;
			this.TrackNumber = tag.TrackNum ?? 0;
			this.Year = tag.Year ?? 0;
			this.DiscNumber = tag.DiscNum ??  1;
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

            tag.Album = Album.Name;
            tag.Title = Piece.Name;
            tag.SetTrack(TrackNumber, Album.TotalTracks[DiscNumber - 1]);
            tag.SetDiscNum(DiscNumber, Album.TotalDisc);

            //is not a transcription
            if (Piece.ParentPieceID == 0 || Piece.Composer.ID == Piece.ParentPieceID) //oops TODO!
                tag.Artist = new string[] { Piece.Composer.GetName(Artist.NameFormats.Last_First) };
            //is a transcritption
            else
            {
                //todo!
                tag.Artist = new string[] { Piece.ComposerNameWithTranscriptor, Piece.Composer.ArtistFullName, Piece.PrimaryPiece.Composer.ArtistFullName };
            }

            tag.Genre = Piece.Genre;
            tag.Year = Year;
            tag.AlbumArtist = "a"; //todo!
            tag.Composers = Artist.GetNames(t.Performer, Artist.NameType.LastNameFirstName);

            tag.ContentGroup = Piece.PieceName;
            tag.Label = Label.GetLabelName(al.LabelID);
            tag.UnsynchedLyric = Artist.GetName(t.Performer, Artist.NameType.FirstNameLastNameWithBriefArtistType, Constants.vbCrLf).Trim;
            //tag.Comment = String.Join(vbCrLf, New String() {al.CDComment, t.TrackComment, Piece.ExtraInfo, Piece.Text})

            tag.AlbumID = al.PrimaryKeyValue;
            tag.TrackID = t.PrimaryKeyValue;
            tag.AlbumType = al._AlbumType;
            tag.IsComplete = al._Complete;
            tag.IsInIpod = al._InIPOD;

            if (IO.File.Exists(t.AssociatedAlbum.GetFilePath(Album.FilePaths.AlbumCoverPath_Auto)))
            {
                tag.AlbumPicture = new Bitmap(t.AssociatedAlbum.GetFilePath(Album.FilePaths.AlbumCoverPath_Auto));
            }

            tag.Write();

            //update the file size after adding/changing tag
            t.FileSize = new IO.FileInfo(path).Length;
            t.SongLength = tag.UltraID3.Duration.TotalSeconds;
            t.Update();

            this.ID = tag.TrackID ?? 0;
            this.TrackNumber = tag.TrackNum ?? 0;
            this.Year = tag.Year ?? 0;
            this.DiscNumber = tag.DiscNum ?? 1;
        }
    }

}
