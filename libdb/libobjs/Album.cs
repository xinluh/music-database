using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Reflection;
using libdb;


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
        }
        private _Label label;
        
        #region ctor

        public Album() 
        {
            AlbumArtist = new Artist(); 
        }
        public Album(int id):this()
        {
            this.Fill(id);
        }
        #endregion

        #region   properties
        [AutoUpdateProp("name", data_type.text, false)]
        public string Name { get; set; }

        [AutoUpdateProp("location", data_type.text, false)]
        public string Path { get; set; }

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

        public string Label
        {
            get { return label.Name; }
            set { label.Name = value; }
        }

        public Artist AlbumArtist { get; set; }

        #endregion

        public override void Insert()
        {
            this.label.Update();
            base.Insert();
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

            //if (IO.File.Exists(t.AssociatedAlbum.GetFilePath(Album.FilePaths.AlbumCoverPath_Auto)))
            //{
            //    tag.AlbumPicture = new Bitmap(t.AssociatedAlbum.GetFilePath(Album.FilePaths.AlbumCoverPath_Auto));
            //}

        }
        internal static void ReadFromTag(Tag tag)
        {
            throw new NotImplementedException();
        }

    }
}
