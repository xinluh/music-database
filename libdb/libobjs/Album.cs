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
        private Artist artist;

        #region ctor

        public Album() { }
        public Album(int id)
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
        public bool InIpod { get; set; }

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
            get { return artist.ID; }
            set
            {
                artist = new Artist(value);
            }
        }

        public string Label
        {
            get { return label.Name; }
            set { label.Name = value; }
        }


        #endregion

        public override int Insert()
        {
            int retval;
            retval = this.label.Update();

            return base.Insert();
        }
    }
}
