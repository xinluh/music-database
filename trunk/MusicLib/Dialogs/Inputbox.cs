using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MusicLib.Dialogs
{
    public partial class Inputbox : Form
    {
        public Inputbox()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Inputbox_FormClosing);
        }


        public Inputbox(string Prompt,string Default,bool acceptEmptyString) : this()
        {
            label.Text = Prompt;
            txb.Text = Default;
            AcceptEmptyString = acceptEmptyString;
            txb.SelectAll();
        }

        public bool AcceptEmptyString { get; set; }

        public string InputText {
            get {return txb.Text;}
            set { txb.Text = value; }
        }


        void Inputbox_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txb.Text == "" && !AcceptEmptyString) DialogResult = DialogResult.Cancel;
            else DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        
    }
}
