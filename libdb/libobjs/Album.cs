using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Reflection;
using libdb;
using System.Xml;
using System.Xml.Serialization;


namespace libdb
{
    [AutoUpdateClass(Tables.tblAlbum)]
    public partial class Album : libobj
    {
        [AutoUpdateClass(Tables.tblLabel)]
        class _Label : libobj
        {
            public override int ID { get; set; }

            [AutoUpdateProp("name", data_type.text, false)]
            public string Name { get; set; }

            public _Label() { }
            public _Label(int id)
            {
                this.Fill(id);
            }

            public override void Update()
            {
                if (ID == 0)
                    ID = int.Parse(Database.GetScalar(String.Format(
                        "SELECT id from tblLabel WHERE name = {0}",
                        Database.Quote(Name))).ToString());

                if (ID == 0)
                    base.Insert();
            }
        }
        private _Label label = new _Label();
        private List<Track> tracks = new List<Track>();
        private readonly string base_path = @"D:\Users\xinlu\Music\Music\";
        private readonly string cover_path = @"D:\Users\xinlu\Music\db-files\Cover";
        private readonly string backcover_path = @"D:\Users\xinlu\Music\db-files\Back";
        private readonly string booklet_path = @"D:\Users\xinlu\Music\db-files\Booklet";
        
        #region ctor

        public Album() 
        {
            AlbumArtist = new Artist();
            
            IsComplete = true;
            IsInIpod = false;
        }
        public Album(int id):this()
        {
            this.Fill(id);
        }
        #endregion

        #region   properties
        [AutoUpdateProp("name", data_type.text, false)]
        public string Name { get; set; }

        [AutoUpdateProp("type", data_type.number, false)]
        public AlbumType AlbumType { get; set; }

        [AutoUpdateProp("complete", data_type.boolean, false)]
        public bool IsComplete { get; set; }

        [AutoUpdateProp("total_disc", data_type.number, false)]
        public int TotalDisc { get; set; }

        [AutoUpdateProp("in_ipod", data_type.boolean, false)]
        public bool IsInIpod { get; set; }

        [AutoUpdateProp("comment", data_type.text, true)]
        public string Comment { get; set; }

        [AutoUpdateProp("need_update", data_type.boolean, false)]
        public bool NeedUpdate { get; set; }

        [AutoUpdateProp("total_track", data_type.number_array, false)]
        public int[] TotalTracks { get; set; }

        [AutoUpdateProp("label_id", data_type.number, true)]
        private int? label_id
        {
            get { return this.label.ID; }
            set
            {
                label = new _Label();
                if (value.HasValue && value.Value != 0)
                {
                    label.Fill(value.Value);
                }
            }
        }

        [AutoUpdateProp("albumartist_id", data_type.number, false)]
        private int albumartist_id
        {
            get { return AlbumArtist.ID; }
            set { AlbumArtist.Fill(value); }
        }

        [AutoUpdateProp("location", data_type.text, false)]
        private string location { get; set; }

        public string Label
        {
            get { return label.Name; }
            set { label.Name = value; }
        }

        public string Location 
        {
            get
            {
                if (string.IsNullOrEmpty(location)) return "";
                if (Path.IsPathRooted(location))
                    return location;
                else
                    return Path.Combine(base_path, location);
            }
            set 
            {
                location = Path.GetFullPath(value);
                if (location.StartsWith(base_path))
                    location = location.Replace(base_path,""); 
            }
        }

        public Artist AlbumArtist { get; set; }

        public List<Track> Tracks
        {
            get 
            {
                if (tracks.Count == 0 && ID != 0)
                {
                    ArrayList trackIds = Database.GetFirstColumn(string.Format(
                        "SELECT id FROM {0} WHERE {1} = {2}",
                        Enum.GetName(typeof(Tables),Tables.tblTrack),
                        "album_id",
                        this.ID));

                    foreach (var item in trackIds)
                        tracks.Add(new Track(int.Parse(item.ToString())));                      
                }
                return tracks; 
            }
            set
            {
                if (value == null) throw new NullReferenceException();
                tracks = value.Where(x => x != null).ToList();
                foreach (var t in tracks)
                    t.Album = this;
            }
        }

        #endregion

        internal enum ExternalFile
        {
            AlbumCover,
            AlbumBack,
            Booklet,
        }
        internal string GetExternalFile(ExternalFile type)
        {
            string file = "";
            switch (type)
            {
                case ExternalFile.AlbumCover:
                    file = "Folder.jpg";
                    break;
                case ExternalFile.AlbumBack:
                    file = "back.jpg";
                    break;
                case ExternalFile.Booklet:
                    file = "booklet.pdf";
                    break;
                default:
                    break;
            }

            file = file == "" ? "" : Path.Combine(Location, file);
            return File.Exists(file) ? file : "";
        }
        public System.Drawing.Bitmap GetAlbumCover()
        {
            String s = GetExternalFile(ExternalFile.AlbumCover);
            return s == "" ? null : new System.Drawing.Bitmap(s);
        }
        public override void Insert()
        {
            this.label.Update();
            base.Insert();

            foreach (var track in Tracks)
            {
                track.InsertOrUpdate();
            }
        }

        public override void Update()
        {
            this.label.Update();
            foreach (var track in Tracks)
                track.InsertOrUpdate();

            base.Update();
        }
        public static Album ReadFromFolder(string path)
        {
            DirectoryInfo dir = Directory.CreateDirectory(path);
            FileInfo[] files = dir.GetFiles("*.mp3", SearchOption.TopDirectoryOnly);
            if (files.Length == 0) throw new System.Exception("The specified path does not contain any mp3 files");

            Album al = new Album();
            al.Location = path;

            al.Tracks.AddRange(files.Select<FileInfo,Track>(x => new Track().ReadFromFile(x)).ToList<Track>());
            //al.ReadFromTag(al.Tracks[0].tag);

            return al;
        }

        public void SaveToFile(string path)
        {
            TextWriter writer = new StreamWriter(path);
            XmlSerializer serializer = new XmlSerializer(typeof(Album));
            serializer.Serialize(writer, this);
            writer.Close();
        }
        public static Album ReadFromFile(string path)
        {
            Album al;
            TextReader r = new StreamReader(path);
            XmlSerializer serializer = new XmlSerializer(typeof(Album));
            al = (Album)serializer.Deserialize(r);
            r.Close();

            // quirks in the XML deserializer to duplicate lists?
            // al.Tracks.RemoveRange(al.Tracks.Count / 2, al.Tracks.Count / 2);
            return al;
        }
        public void WriteTag()
        {
            foreach (var track in Tracks)
                track.WriteTag();
        }
        public void RenameFiles()
        {
            foreach (Track t in Tracks)
                t.RenameFile();
            
            string new_path = "";
            string artist_part = PathUtils.FixPathString(AlbumArtist.GetName(Artist.NameFormats.Last_First));
            string albumname_part = PathUtils.FixPathString(this.Name);
            string artist_albumname_part = Path.Combine(artist_part,albumname_part);

            if (AlbumType == AlbumType.ArtistAlbum)
                new_path = Path.Combine("Collection II", artist_albumname_part);
            else if (AlbumType == AlbumType.ComposerAlbum)
            {
                albumname_part = PathUtils.FixPathString(
                    System.Text.RegularExpressions.Regex.Replace(this.Name,@"^(\w)+:\s", ""));
                artist_albumname_part = Path.Combine(artist_part, albumname_part);
                new_path = Path.Combine("Collection I", artist_albumname_part);
            }
            else if (AlbumType == AlbumType.VariousArtistAlbum)
                new_path = Path.Combine("Collection III", albumname_part);

            new_path = Path.Combine(Path.GetDirectoryName(Location), new_path);

            Directory.CreateDirectory(Path.GetDirectoryName(new_path));
            Directory.Move(Location, new_path);

            Location = new_path;
            Update();


        }
        internal void WriteToTag(Tag tag)
        {
            tag.Label = Label;
            tag.Album = Name;
            tag.AlbumArtist = AlbumArtist.GetName(Artist.NameFormats.Last_First);
            tag.AlbumID = ID;
            tag.AlbumType = AlbumType;
            tag.IsComplete = IsComplete;
            tag.IsInIpod = IsInIpod;

            tag.AlbumPicture = GetAlbumCover();
            //if (IO.File.Exists(t.AssociatedAlbum.GetFilePath(Album.FilePaths.AlbumCoverPath_Auto)))
            //{
            //    tag.AlbumPicture = new Bitmap(t.AssociatedAlbum.GetFilePath(Album.FilePaths.AlbumCoverPath_Auto));
            //}

        }
        internal void ReadFromTag(Tag tag)
        {
            throw new NotImplementedException();
        }

    }
}
