using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MusicLib
{
    public partial class BrowseMusic : Form
    {
        public BrowseMusic()
        {
            InitializeComponent();
            browse1.StatusChanged += (sender, e) => { this.StatusLabel.Text = browse1.Status; };

//            MessageBox.Show("Test");
        }
    }
}
