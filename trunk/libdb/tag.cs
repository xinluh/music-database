using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using HundredMilesSoftware.UltraID3Lib;
using System.Text.RegularExpressions;

namespace libdb
{
    class Tag
    {
        private UltraID3 u = new UltraID3();
        private System.Collections.Specialized.NameValueCollection UserText =
            new System.Collections.Specialized.NameValueCollection();
        private string _Comments = "";
        private string _DBInfo = "";

        public Tag() { }
        public Tag(string Filepath) : this()
        {
            this.Read(Filepath);
        }

        public void Read(string Filepath)
        {
            u.Read(Filepath);

            this.UserText.Clear();

            foreach (ID3v23UserDefinedTextFrame fr in u.ID3v2Tag.Frames.GetFrames(MultipleInstanceID3v2FrameTypes.ID3v23UserDefinedText))
                this.UserText.Add(fr.Description, fr.UserDefinedText);

            this._Comments = "";
            this._DBInfo = "";

            foreach (ID3v23CommentsFrame c in u.ID3v2Tag.Frames.GetFrames(MultipleInstanceID3v2FrameTypes.ID3v23Comments))
            {
                // TODO: exclude some other comments such as itune stuff
                if (c.Description != "DBInfo" & !string.IsNullOrEmpty(c.Comments.Trim()))
                    this._Comments += (string.IsNullOrEmpty(this._Comments) ? "" : "\n") + c.Comments;
                else if (c.Description == "DBInfo")
                    this._DBInfo = c.Comments;
            }

            u.ID3v2Tag.WillWrite = true;
            u.ID3v1Tag.WillWrite = false;
            u.ID3v2Tag.Frames.AddNewFrameTextEncodingType = TextEncodingTypes.Unicode;
        }

        public void Write()
        {
            u.ID3v2Tag.Frames.Remove(ID3v2FrameTypes.ID3v23UserDefinedText);
            ID3v23UserDefinedTextFrame n = default(ID3v23UserDefinedTextFrame);
            System.Text.StringBuilder s = new System.Text.StringBuilder();

            for (int i = 0; i < this.UserText.Count; i++)
            {
                n = new ID3v23UserDefinedTextFrame(this.UserText.GetValues(i)[0]);
                n.Description = this.UserText.GetKey(i);
                //n.UserDefinedText = this.UserText.GetValues(i)[0];

                u.ID3v2Tag.Frames.Add(n);

                s.Append(n.Description + ":" + n.UserDefinedText);
            }

            u.ID3v2Tag.Frames.SetComments("DBInfo", s.ToString());

            u.Write();
        }

        public void Clear()
        {
            u.ID3v1Tag.Clear();
            u.ID3v2Tag.Clear();
            u.Clear();
            u.ID3v2Tag.WillWrite = false;
            u.ID3v1Tag.WillWrite = false;
            u.Write(); // okay dirty fix since the lib does not seem to actually Clear() the tag.
            u.ID3v2Tag.WillWrite = true;
        }

        public UltraID3 UltraID3
        {
            get { return u; }
        }

        public string Album
        {
            get
            {
                if (!string.IsNullOrEmpty(u.ID3v2Tag.Album))
                    return u.ID3v2Tag.Album;
                else if (!string.IsNullOrEmpty(u.ID3v1Tag.Album))
                    return u.ID3v1Tag.Album;
                else
                    return null;
            }
            set
            {
                u.ID3v2Tag.Frames.Remove(ID3v2FrameTypes.ID3v23Album);
                if (!string.IsNullOrEmpty(value))
                    u.ID3v2Tag.Frames.Add(new ID3v23AlbumFrame(value));
            }
        }
        public string[] Composer
        {
            get
            {
                if (!string.IsNullOrEmpty(u.ID3v2Tag.Artist))
                    return u.ID3v2Tag.Artist.Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);
                else if (!string.IsNullOrEmpty(u.ID3v1Tag.Artist))
                    return u.ID3v1Tag.Artist.Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);
                else
                    return null;
            }
            set
            {
                u.ID3v2Tag.Frames.Remove(ID3v2FrameTypes.ID3v23Artist);
                if (value != null && value.Length > 0)
                    u.ID3v2Tag.Frames.Add(new ID3v23ArtistFrame(string.Join("/", value)));
            }
        }
        public string Title
        {
            get
            {
                if (!string.IsNullOrEmpty(u.ID3v2Tag.Title))
                    return u.ID3v2Tag.Title;
                else if (!string.IsNullOrEmpty(u.ID3v1Tag.Title))
                    return u.ID3v1Tag.Title;
                else
                    return null;
            }
            set
            {
                u.ID3v2Tag.Frames.Remove(ID3v2FrameTypes.ID3v23Title);
                if (!string.IsNullOrEmpty(value))
                    u.ID3v2Tag.Frames.Add(new ID3v23TitleFrame(value));
            }
        }
        public string Genre
        {
            get
            {
                if (!string.IsNullOrEmpty(u.ID3v2Tag.Genre))
                    return u.ID3v2Tag.Genre;
                else if (!string.IsNullOrEmpty(u.ID3v1Tag.GenreName))
                    return u.ID3v1Tag.GenreName;
                else
                    return null;
            }
            set
            {
                u.ID3v2Tag.Frames.Remove(ID3v2FrameTypes.ID3v23Genre);
                if (!string.IsNullOrEmpty(value))
                    u.ID3v2Tag.Frames.Add(new ID3v23GenreFrame(value));
            }
        }
        // public int? GenreID {
        //     get {
        //         if (this.Genre == null) return null;
        //         int i = MusicDataBase.Genre.GetGenreID(this.Genre);
        //         return (i != 0) ? i : null;
        //     }
        //     set {
        //         u.ID3v2Tag.Frames.Remove(ID3v2FrameTypes.ID3v23Genre);
        //         if ((int)value != 0) u.ID3v2Tag.Frames.Add(new ID3v23GenreFrame(MusicDataBase.Genre.GetGenreName(value)));
        //     }
        // }
        public string AlbumArtist
        {
            get
            {
                return (u.ID3v2Tag.Frames.GetFrame(SingleInstanceID3v2FrameTypes.ID3v23Band) != null) ?
                                                   ((ID3v23BandFrame)u.ID3v2Tag.Frames.GetFrame(SingleInstanceID3v2FrameTypes.ID3v23Band)).Band : null;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    u.ID3v2Tag.Frames.Remove(ID3v2FrameTypes.ID3v23Band);
                    u.ID3v2Tag.Frames.Add(new ID3v23BandFrame(value));
                }
            }
        }
        public string Label
        {
            get
            {
                return u.ID3v2Tag.Frames.GetFrame(SingleInstanceID3v2FrameTypes.ID3v23Publisher) != null ?
                      ((ID3v23PublisherFrame)u.ID3v2Tag.Frames.GetFrame(SingleInstanceID3v2FrameTypes.ID3v23Publisher)).Publisher : null;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    u.ID3v2Tag.Frames.Remove(ID3v2FrameTypes.ID3v23Publisher);
                    u.ID3v2Tag.Frames.Add(new ID3v23PublisherFrame(value));
                }
            }
        }
        public string ContentGroup
        {
            get
            {
                return u.ID3v2Tag.Frames.GetFrame(SingleInstanceID3v2FrameTypes.ID3v23ContentGroupDescription) != null ?
                  ((ID3v23ContentGroupDescriptionFrame)
                   u.ID3v2Tag.Frames.GetFrame(SingleInstanceID3v2FrameTypes.ID3v23ContentGroupDescription)).ContentGroupDescription : null;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    u.ID3v2Tag.Frames.Remove(ID3v2FrameTypes.ID3v23ContentGroupDescription);
                    u.ID3v2Tag.Frames.Add(new ID3v23ContentGroupDescriptionFrame(value));
                }
            }
        }
        public int? TrackNum
        {
            get
            {
                return u.ID3v2Tag.Frames.GetFrame(SingleInstanceID3v2FrameTypes.ID3v23TrackNum) != null ?
                    ((ID3v23TrackNumFrame)u.ID3v2Tag.Frames.GetFrame(SingleInstanceID3v2FrameTypes.ID3v23TrackNum)).TrackNum : null;
            }
        }
        public int? TotalTracks
        {
            get
            {
                return u.ID3v2Tag.Frames.GetFrame(SingleInstanceID3v2FrameTypes.ID3v23TrackNum) != null ?
                 ((ID3v23TrackNumFrame)u.ID3v2Tag.Frames.GetFrame(SingleInstanceID3v2FrameTypes.ID3v23TrackNum)).TrackCount : null;
            }
        }
        public void SetTrack(int TrackNum, int TotalTrack)
        {
            u.ID3v2Tag.Frames.Remove(ID3v2FrameTypes.ID3v23TrackNum);
            if (TrackNum == 0 & TotalTrack == 0) return;

            if (TotalTrack != 0)
                u.ID3v2Tag.Frames.Add(new ID3v23TrackNumFrame((byte)TrackNum, (byte)TotalTrack));
            else
                u.ID3v2Tag.Frames.Add(new ID3v23TrackNumFrame((byte)TrackNum));
        }
        public int? DiscNum
        {
            get
            {
                return u.ID3v2Tag.Frames.GetFrame(SingleInstanceID3v2FrameTypes.ID3v23PartOfSet) != null ?
                  (int?)((ID3v23PartOfSetFrame)u.ID3v2Tag.Frames.GetFrame(SingleInstanceID3v2FrameTypes.ID3v23PartOfSet)).PartNum : null;
            }
        }
        public int? DiscCount
        {
            get
            {
                return u.ID3v2Tag.Frames.GetFrame(SingleInstanceID3v2FrameTypes.ID3v23PartOfSet) != null ?
                   (int?)((ID3v23PartOfSetFrame)u.ID3v2Tag.Frames.GetFrame(SingleInstanceID3v2FrameTypes.ID3v23PartOfSet)).PartCount : null;
            }
        }
        public void SetDiscNum(int DiscNum, int TotalDisc)
        {
            u.ID3v2Tag.Frames.Remove(ID3v2FrameTypes.ID3v23PartOfSet);
            if (DiscNum == 0 & TotalDisc == 0) return;

            if (TotalDisc != 0)
            {
                u.ID3v2Tag.Frames.Add(new ID3v23PartOfSetFrame((byte)DiscNum, (byte)TotalDisc));
            }
            else
            {
                u.ID3v2Tag.Frames.Add(new ID3v23PartOfSetFrame((byte)DiscNum));
            }
        }
        public int? Year
        {
            get { return u.ID3v2Tag.Year; }
            set
            {
                u.ID3v2Tag.Frames.Remove(ID3v2FrameTypes.ID3v23Year);
                if (value.HasValue && value.Value != 0)
                    u.ID3v2Tag.Frames.Add(new ID3v23YearFrame((short)value.Value));
            }
        }
        public string[] Performers
        {
            get
            {
                string[] a = new string[((ID3v23ComposersFrame)u.ID3v2Tag.Frames.GetFrame(SingleInstanceID3v2FrameTypes.ID3v23Composers)).Composers.Count];
                for (int i = 0; i <= a.GetUpperBound(0); i++)
                    a[i] = ((ID3v23ComposersFrame)u.ID3v2Tag.Frames.GetFrame(SingleInstanceID3v2FrameTypes.ID3v23Composers)).Composers[i];
                return a;
            }
            set
            {
                ID3v23ComposersFrame c = new ID3v23ComposersFrame();
                for (int i = 0; i <= value.Length - 1; i++)
                    c.Composers.Add(value[i]);
                u.ID3v2Tag.Frames.Add(c);
            }
        }
        public Bitmap AlbumPicture
        {
            get
            {
                ID3v23PictureFrame p = (ID3v23PictureFrame)u.ID3v2Tag.Frames.GetFrames(MultipleInstanceID3v2FrameTypes.ID3v23Picture)[0];
                return (p != null) ? p.Picture : null;
            }
            set
            {
                u.ID3v2Tag.Frames.Remove(ID3v2FrameTypes.ID3v23Picture);
                if (value != null)
                    u.ID3v2Tag.Frames.Add(new ID3v23PictureFrame(value, PictureTypes.CoverFront, "", TextEncodingTypes.Unicode));
            }
        }
        public string UnsynchedLyric
        {
            get { return ((ID3v23UnsynchedLyricsFrame)u.ID3v2Tag.Frames.GetFrames(MultipleInstanceID3v2FrameTypes.ID3v23UnsyncedLyrics)[0]).UnsynchedLyrics; }
            set
            {
                u.ID3v2Tag.Frames.Remove(ID3v2FrameTypes.ID3v23UnsyncedLyrics);
                if (value != null & !string.IsNullOrEmpty(value))
                    u.ID3v2Tag.Frames.Add(new ID3v23UnsynchedLyricsFrame(value));
            }
        }
        // public string UserDefinedText (string Description) {
        //     get { return this.UserText.Get(Description); }
        //     set {
        //         this.UserText.Remove(Description);
        //         if (!string.IsNullOrEmpty(value)) {           
        //             this.UserText.Add(Description, value);
        //     }
        // }
        public string Comment
        {
            get { return this._Comments; }
            set
            {
                value = value.Trim();
                if (!string.IsNullOrEmpty(value))
                {
                    this._Comments = value;
                    u.ID3v2Tag.Frames.Remove(ID3v2FrameTypes.ID3v23Comments);
                    u.ID3v2Tag.Frames.Add(new ID3v23CommentsFrame(value, ""));
                }
            }
        }
        private string DBInfoFromComment(string key)
        {
            if (this._DBInfo.ToUpper().Contains(key.ToUpper()))
            {
                int i = this._DBInfo.ToUpper().IndexOf(":", _DBInfo.ToUpper().IndexOf(key.ToUpper(), 0)) + 1;
                int j = this._DBInfo.ToUpper().IndexOf("X", i);
                if (j == -1) j = this._DBInfo.Length;
                return this._DBInfo.Substring(i, j - i);
            }
            else
            {
                return null;
            }
        }
        private int? DBInfo(string key)
        {
            if (!string.IsNullOrEmpty(this.UserText.Get(key)))
                return int.Parse(UserText.Get(key));
            else if (!string.IsNullOrEmpty(this.DBInfoFromComment(key)))
                return int.Parse(this.DBInfoFromComment(key));
            else
                return null;
        }
        public int? AlbumID
        {
            get
            {
                if (this.DBInfo("xAlbumID").HasValue)
                    return (int)this.DBInfo("xAlbumID");
                else
                    return null;
            }
            set
            {
                this.UserText.Remove("xAlbumID");
                if (value.HasValue && value.Value != 0)
                    this.UserText.Add("xAlbumID", value.Value.ToString());
            }
        }


        public libdb.AlbumType? AlbumType
        {
            get
            {
                if (this.DBInfo("xAlbumType").HasValue)
                    return (AlbumType)this.DBInfo("xAlbumType").Value;
                else if (this.DBInfo("!AlbumType").HasValue)
                    return (AlbumType)this.DBInfo("!AlbumType").Value;
                else if (u.FileName.Contains("Various Artists"))
                    return libdb.AlbumType.VariousArtistAlbum;
                else if (u.FileName.Contains("Collection I"))
                    return libdb.AlbumType.ComposerAlbum;
                else if (u.FileName.Contains("Collection II"))
                    return libdb.AlbumType.ArtistAlbum;
                else
                    return null;
            }
            set
            {
                this.UserText.Remove("xAlbumType");
                this.UserText.Add("xAlbumType", ((int)value).ToString());
            }
        }
        public int? TrackID
        {
            get
            {
                if (this.DBInfo("xTrackID").HasValue)
                    return (int)this.DBInfo("xTrackID");
                else
                    return null;
            }
            set
            {
                this.UserText.Remove("xTrackID");
                if (value.HasValue && value.Value != 0)
                    this.UserText.Add("xTrackID", value.Value.ToString());
            }
        }
        public bool? IsComplete
        {
            get
            {
                if (this.DBInfo("xCompleteAlbum").HasValue)
                    return (this.DBInfo("xCompleteAlbum").Value == 0) ? false : true;
                else
                    return null;
            }
            set
            {
                this.UserText.Remove("xCompleteAlbum");
                if (value.HasValue)
                    this.UserText.Add("xCompleteAlbum", value.Value ? "1" : "0");
            }
        }
        public bool? IsInIpod
        {
            get
            {
                if (this.DBInfo("xiPod").HasValue)
                    return (DBInfo("xiPod").Value == 0) ? true : false;
                else
                    return null;
            }
            set
            {
                this.UserText.Remove("xiPod");
                if (value.HasValue)
                    this.UserText.Add("xiPod", value.Value ? "1" : "0");
            }
        }
    }
}

