using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libdb;
using System.Windows.Forms;
using System.ComponentModel;

namespace MusicLib
{
    class ArtistSearchTextBox : AutoSearchCompleteTextBox
    {
        string artistType = "";
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public string ArtistType
        {
            get { return artistType; }
            set 
            { 
                artistType = value;
                ((ArtistSearch)Search).ClearFilters(ArtistSearch.Fields.Type);
                if (!string.IsNullOrEmpty(artistType))
                    ((ArtistSearch)Search).AddFilter(ArtistSearch.Fields.Type,
                        " = " + libdb.Database.Quote(artistType));
                UpdateCompletionSource(); 
            }
        }
        public ArtistSearchTextBox() : base()
        {
            if (System.ComponentModel.LicenseManager.UsageMode ==
                System.ComponentModel.LicenseUsageMode.Runtime)
            {
                Search = new ArtistSearch(ArtistSearch.Fields.MatchName,
                    ArtistSearch.Fields.FullNameType,
                    ArtistSearch.Fields.FullName,
                    ArtistSearch.Fields.ID);
                UpdateCompletionSource();
            }

            UserAttemptedAddItem += (sender, e) => AddArtist(Text);
            UserAttemptedDeleteItem += (sender, e) => DeleteArtist(e);
            UserAttemptedRenameItem += (sender, e) => RenameArtist(e, "");
        }

        public override void UpdateCompletionSource()
        {
            if (Search == null) throw new NullReferenceException();

            this.Items.Clear();
            if (string.IsNullOrEmpty(ArtistType))
                Items.AddRange(Search.PerformSearchToList(), 0, 1, 3);
            else
                Items.AddRange(Search.PerformSearchToList(), 0, 2, 3);
            ForceUpdateList();
        }

        private void RenameArtist(CustomForm.AutoCompleteDataEntry entry, string newName)
        {
            int id = int.Parse(entry.ValueMember.ToString());
            Artist a;
            if (!(id > 0 && (a = new Artist(id)).ID > 0)) return;
            string type = string.IsNullOrEmpty(ArtistType) ? "artist" : ArtistType;
            if (newName == "")
            {   // ask for a new name
                Dialogs.Inputbox input = new Dialogs.Inputbox(
                    "Enter a new name for " + type + " "
                    + a.GetName(Artist.NameFormats.First_Last),
                    a.GetName(Artist.NameFormats.First_Last), false);

                if (input.ShowDialog() == DialogResult.OK) newName = input.InputText;
                else return;
            }

           a.Name = newName;
           a.Update();

           UpdateCompletionSource();
           SetTextAndSelect(a.GetName(Artist.NameFormats.Last_First));
           SelectAll();
        }

        private bool AddArtist(string name)
        {
            string type = string.IsNullOrEmpty(ArtistType) ? "artist" : ArtistType;
            if (name == "")
            {   // ask for a new name
                Dialogs.Inputbox input = new Dialogs.Inputbox(
                    "Enter a new name for the new "+ type,
                    "", false);

                if (input.ShowDialog() == DialogResult.OK) name = input.InputText;
                else return false;
            }
            else if (type == "artist")
            {
                Dialogs.Inputbox input = new Dialogs.Inputbox(
                    "Choose an artist type for " + name,
                    new ArtistTypeSearch().PerformSearchToScalar().ToArray(),
                    "", false);

                if (input.ShowDialog() == DialogResult.OK) type = input.InputText;
                else return false;
            }
            else if (MessageBox.Show("Do you want to add " + type + " "
                + name + " to the database?",
                "Confirmation", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return false;

            Artist c = new Artist(name, type);
            if (c.ID != 0)
                MessageBox.Show("The new " + type + " you entered exists in the database as "
                + c.GetName(Artist.NameFormats.Last_First_Type) + ".\n It is therefore not added to database");
            else
                c.Insert();

            UpdateCompletionSource();
            SetTextAndSelect(c.GetName(Artist.NameFormats.Last_First));
            return true;
        }

        private void DeleteArtist(CustomForm.AutoCompleteDataEntry entry)
        {
            int id = int.Parse(entry.ValueMember.ToString());
            if (id == 0) return;

            // ask for confirmation
            if (MessageBox.Show("Are you sure to delete the artist?\nThis is not reversible!",
                "Delete Artist", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                return;

            Artist a = new Artist(id);
            a.Delete();
            UpdateCompletionSource();
            SetTextAndSelect(a.GetName(Artist.NameFormats.Last_First));
            SelectAll();
        }


    }
}
