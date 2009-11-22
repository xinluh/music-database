using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using libdb;

namespace MusicLib
{
    public partial class AddAlbum : Form
    {
        Album album;

        public AddAlbum()
        {
            InitializeComponent();

            txbAlbumArtist.SetFieldToSearch(new ArtistSearch(),
                ArtistSearch.Fields.MatchName, 
                ArtistSearch.Fields.FullName, 
                ArtistSearch.Fields.ID);
            txbAlbumArtist.PopupWidth = -1;

            txbLabel.SetFieldToSearch(new LabelSearch());
            txbLabel.PopupWidth = -1;

            cmbAlbumType.DataSource = Enum.GetValues(typeof(AlbumType));

            btnAddSelected.Click += (sender, e) => dg.addTrackInfo(browseControl.GetSelectedAsTracks());
            btnChoosePiece.Click += (sender, e) => browseControl.StartNewSearch();
            btnBrowse.Click += (sender, e) => openFolder("");
            btnOpen.Click += (sender, e) => System.Diagnostics.Process.Start(txbPath.Text);

            importFromAlbum(new Album(78));
            //openFolder(@"D:\temp\rips\levin mozart");
        }
        
        public void importFromAlbum(Album al)
        {
            album = al;

            txbLabel.SetTextAndSelect(al.Label);
            txbAlbumName.Text = al.Name;
            txbAlbumArtist.SetTextAndSelect(al.AlbumArtist.GetName(Artist.NameFormats.Last_First));
            nudTotalDiscs.Value = al.TotalDisc == 0? 1: al.TotalDisc;
            txbTotalTracks.Text = al.TotalTracks == null? al.Tracks.Count.ToString() : 
                string.Join(",", al.TotalTracks.Select(x => x.ToString()).ToArray());
            txbPath.Text = al.Path;
            cmbAlbumType.SelectedItem = al.AlbumType;

            dg.populateTracks(al.Tracks);

        }

        public void openFolder(string path)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = path == ""? Environment.GetFolderPath(Environment.SpecialFolder.Desktop) : path;
               
            if (dialog.ShowDialog() == DialogResult.Cancel || dialog.SelectedPath == "") return;

            importFromAlbum(Album.ReadFromFolder(dialog.SelectedPath));

            txbAlbumName.Focus();
        }
        public string checkInfoCompleteness()
        {
            string error = "";

            if (!txbAlbumArtist.HasValidSelection)
                error += "Album artist choice is invalid\n";
            if (string.IsNullOrEmpty(txbAlbumName.Text))
                error += "Album name is empty\n";
            if (!dg.checkInfoCompleteness())
                error += "Some tracks are missing information\n";
            if (txbTotalTracks.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x=>int.Parse(x)).Count() != (int) nudTotalDiscs.Value)
                error += "The number of total tracks given does not match the number of discs";

            return error;

        }
        public Album exportToAlbum()
        {
            album.AlbumArtist = new Artist(int.Parse(txbAlbumArtist.SelectedValue.ToString()));
            album.AlbumType = (AlbumType)cmbAlbumType.SelectedValue;
            album.Label = txbLabel.Text;
            album.Name = txbAlbumName.Text;
            album.Path = txbPath.Text;
            album.TotalDisc = (int)nudTotalDiscs.Value;
            album.TotalTracks = txbTotalTracks.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x=>int.Parse(x)).ToArray();
            album.Tracks = dg.exportInfo();

            return album;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(dg.checkInfoCompleteness().ToString());
            //exportToAlbum().SaveToFile("test.xml");

            Album test = Album.ReadFromFile("album.xml");
            importFromAlbum(test);

        }

        private void btnAddAlbum_Click(object sender, EventArgs e)
        {
            Album a = exportToAlbum();
            a.Insert();

            // refresh the values from database
            a = new Album(a.ID);
            a.WriteTag();

            a.RenameFiles();
        }

    }
}
