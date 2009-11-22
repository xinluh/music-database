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
    public partial class SelectMultipleArtistsDialog : Form
    {
        public SelectMultipleArtistsDialog()
        {
            InitializeComponent();

            btnOk.Click += (sender, e) => DialogResult = DialogResult.OK;
            btnCancel.Click += (sender, e) => DialogResult = DialogResult.Cancel;
            multipleArtistSelector1.UserSaysOk += (sender, e) => btnOk.PerformClick();

            multipleArtistSelector1.startNewSearch();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                DialogResult = DialogResult.Cancel;
            base.OnFormClosed(e);
        }
        public static List<Artist> GetResult(List<Artist> original_list)
        {
            SelectMultipleArtistsDialog d = new SelectMultipleArtistsDialog();

            if (original_list != null && original_list.Count > 0)
                d.multipleArtistSelector1.populateTable(original_list);

            if (d.ShowDialog() == DialogResult.OK)
                return d.multipleArtistSelector1.GetResults();
            else
                return null;
        }
    }
   
}
