using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using libdb;

namespace MusicLib.UIControls
{
    public partial class MultipleArtistSelector : UserControl
    {
        public event EventHandler UserSaysOk;

        public MultipleArtistSelector()
        {
            InitializeComponent();
            if (System.ComponentModel.LicenseManager.UsageMode == LicenseUsageMode.Runtime)
                txbType.SetFieldToSearch(new ArtistTypeSearch());
            txbType.ItemSelected += (sender, e) => txbName.ArtistType = txbType.Text;

            txbName.ItemSelected += (sender, e) => {
                if (txbName.SelectedItem == null) return;
                populateTable(new Artist(int.Parse(txbName.SelectedValue.ToString())));
                txbName.Clear();
            };

            txbName.AllowAddingOnEmptyString = false;
            txbName.KeyDown += (sender, e) =>
            {
                if (e.KeyData == Keys.Enter && txbName.Text == "")
                    if (UserSaysOk != null) UserSaysOk(this,new EventArgs());
            };

            dg.LostFocus += (sender, e) => dg.ClearSelection();

            txbName.Focus();
        }

        public void startNewSearch()
        {
            Focus();
            txbType.Text = "";
            txbName.SelectAll();
            txbName.Focus();
        }
        public void populateTable(int[] artistIds)
        {
            populateTable(artistIds.Select(x => new Artist(x)).ToList());
        }
        public void populateTable(List<Artist> artists)
        {
            foreach (var artist in artists)
                populateTable(artist);
        }
        public void populateTable(Artist artist)
        {
            dg.Rows.Add(artist, artist.Type, 
                artist.GetName(Artist.NameFormats.Last_First));
            dg.ClearSelection();
        }

        public List<Artist> GetResults()
        {
            return dg.Rows.Cast<DataGridViewRow>().Select(
                x => (Artist)x.Cells["id"].Value).Where(x=>x!=null).ToList<Artist>();
        }


    }
}
