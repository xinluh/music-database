using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using libdb;

namespace MusicLib.Dialogs
{
    public partial class Inputbox : Form
    {
        public enum InputModes
        {
            Text,
            Combo
        }

        public static Artist GetComposer (Artist defaultComposer)
        {
            ArtistSearch s = new ArtistSearch(ArtistSearch.Fields.FullName);
            s.AddTypeToSearch(ArtistSearch.TypeCategory.Composers);

            Inputbox ib = new Inputbox("Choose a composer: ", s.PerformSearchToScalar().ToArray(),
                defaultComposer != null ? defaultComposer.GetName(Artist.NameFormats.Last_First) : "",
                false);

            if (ib.ShowDialog() == DialogResult.Cancel)
                return null;
            else
            {
                Artist a = new Artist(ib.SelectedItem.ToString(), "composer", false);
                return a;
            }
        }

        public static string GetGenre(string defaultGenre)
        {
            GenreSearch s = new GenreSearch(GenreSearch.Fields.Name);

            Inputbox ib = new Inputbox("Choose a genre: ", s.PerformSearchToScalar().ToArray(),
                defaultGenre, false);

            if (ib.ShowDialog() == DialogResult.Cancel)
                return null;
            else
                return ib.SelectedItem.ToString();
        }

        public Inputbox()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Inputbox_FormClosing);
        }

        public Inputbox(string Prompt,string Default, bool acceptEmptyString) : this()
        {
            InputMode = InputModes.Text;

            label.Text = Prompt;
            txb.Text = Default;
            AcceptEmptyString = acceptEmptyString;
            txb.SelectAll();
        }


        public Inputbox(string Prompt, object[] Choices, string Default, bool acceptNewChoice) : this()
        {
            InputMode = InputModes.Combo;

            AcceptNewChoice = acceptNewChoice;
            SetChoices(Choices);
            label.Text = Prompt;
            cmb.Text = Default;
        }

        public bool AcceptEmptyString { get; set; }
        
        public bool AcceptNewChoice {
            get { return cmb.DropDownStyle == ComboBoxStyle.DropDown; }
            set { cmb.DropDownStyle = value ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList; }
        }
        
        public InputModes InputMode 
        { 
            get { return txb.Visible? InputModes.Text : InputModes.Combo; }
            set
            {
                txb.Visible = (value == InputModes.Text);
                cmb.Visible = (value == InputModes.Combo);
            }
        }

        public string InputText {
            get { return InputMode == InputModes.Text ? txb.Text : cmb.Text;}
            set 
            { 
                txb.Text = value;
                cmb.Text = value;
            }
        }

        public object SelectedItem
        {
            get { return cmb.SelectedItem; }
        }

        //public void SetAutoCompleteItems(IEnumerable<IList> items, int MatchMember,
        //    int DisplayMember, int ValueMember)
        //{
        //    txb.Items.AddRange(items, MatchMember, DisplayMember, ValueMember);
        //}

        public void SetChoices(object[] choices)
        {
            if (InputMode == InputModes.Combo)
            {
                cmb.Items.Clear();
                cmb.Items.AddRange(choices);
            }
        }


        void Inputbox_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if ((InputMode == InputModes.Text && txb.Text == "" && !AcceptEmptyString) ||
                 (InputMode == InputModes.Combo && !AcceptNewChoice && cmb.SelectedItem == null)) 
                DialogResult = DialogResult.Cancel;
            
            else DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

    }
}
