using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomForm;
using libdb;
using System.Windows.Forms;
using System.ComponentModel;

namespace MusicLib
{
    class AutoSearchCompleteTextBox : CustomForm.AutoCompleteTextBox
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public SearchBase Search { get; set; }

        /// <summary>
        /// If true, leaving the textbox without a valid selection will either
        /// revert to the last selection or empty string
        /// </summary>
        public bool EnforceStrictSelection { get; set; }

        /// <summary>
        /// If true, UserAttemptedAddItem event will be fired even if the textbox
        /// has empty content.
        /// </summary>
        [DefaultValue(true)]
        public bool AllowAddingOnEmptyString { get; set; }

        public bool HasValidSelection
        {
            get { return (SelectedItem != null); }
        }

        public delegate void AutoCompleteEntryEventHandler(
            AutoSearchCompleteTextBox sender,
            AutoCompleteDataEntry entry);
            
        public event EventHandler UserAttemptedAddItem;
        public event AutoCompleteEntryEventHandler UserAttemptedRenameItem;
        public event AutoCompleteEntryEventHandler UserAttemptedDeleteItem;

        public AutoSearchCompleteTextBox() : base() 
        {
            EnforceStrictSelection = true;
        }

        public AutoSearchCompleteTextBox(SearchBase search, 
            object MatchMember, 
            object DisplayMember, 
            object ValueMember) : this()
        {
            SetFieldToSearch(search, MatchMember, DisplayMember, ValueMember);
        }

        public void SetFieldToSearch(SearchBase search)
        {
            if (!search.IsSimple) throw new InvalidOperationException(
                "For a non-simple search one must supply more information!");
            Search = search;

            UpdateCompletionSource();
        }

        public void SetFieldToSearch(SearchBase search, 
            object matchMember, 
            object displayMember, 
            object valueMember) 
        {
            Search = search;
            if (search == null) throw new NullReferenceException();

            search.SetFieldToSearch(new object[]
                {matchMember, displayMember, valueMember});

            UpdateCompletionSource();
        }

        public virtual void UpdateCompletionSource()
        {
            if (Search == null) throw new NullReferenceException();

            this.Items.Clear();
            if (Search.IsSimple)
                Items.AddRange(Search.PerformSearchToList(), 0, 0, 0);
            else
                Items.AddRange(Search.PerformSearchToList(), 0, 1, 2);
            ForceUpdateList();
        }

        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (!HasValidSelection || (AllowAddingOnEmptyString && !HasText()))
                    if (UserAttemptedAddItem != null)
                        UserAttemptedAddItem(this, new EventArgs());
            }
            else if (e.KeyData == Keys.F2 && HighlightedItem != null)
            {
                if (UserAttemptedRenameItem != null)
                    UserAttemptedRenameItem(this, (CustomForm.AutoCompleteDataEntry)HighlightedItem);
                HideAutoCompleteList();
                e.Handled = true;
            }
            else if (e.KeyData == Keys.Delete && HighlightedItem != null)
            {
                if (UserAttemptedDeleteItem != null)
                    UserAttemptedDeleteItem(this, HighlightedItem as AutoCompleteDataEntry);
                HideAutoCompleteList();
                e.Handled = true;
            }
            else if (e.KeyData == Keys.Escape)
                SelectAll();
            
            base.OnKeyDown(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            //if (!popup.Visible && !HasValidSelection && EnforceStrictSelection)
              //  Text = "";

            base.OnLostFocus(e);
        }

        //protected override void OnGotFocus(EventArgs e)
        //{
        //    SelectAll();
            
        //    base.OnGotFocus(e);
        //}
    }
}
