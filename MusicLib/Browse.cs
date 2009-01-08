using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using CustomForm;
using libdb;
using System.Runtime.InteropServices;

namespace MusicLib
{

    public partial class Browse : UserControl
    {
        const string TextBoxDefaultAll = "(All)";
        const string TextBoxDefaultNone = "(None)";
        private readonly Color CellInEditColor = Color.Azure;
        public event EventHandler StatusChanged;

        ArtistSearch artistSearch;
        PieceSearch pieceSearch;
        GenreSearch genreSearch;
        Timer status_reset_timer;
        Piece currentPiece;
        bool IsScrolling = false;
        bool SupressUpdate = true; // used to prevent unnecessary fetching of data

        public string Status { get; private set; }
        public bool DetailPaneVisible
        {
            get { return !splitContainer1.Panel2Collapsed; }
            set { splitContainer1.Panel2Collapsed = !value; }
        }

        public Browse()
        {
            InitializeComponent();

            status_reset_timer = new Timer();
            status_reset_timer.Interval = 6000; // reset status message to "Ready" after 6 sec
            status_reset_timer.Tick += (sender, e) =>
            {
                this.change_status("Ready", false);
                status_reset_timer.Enabled = false;
            };

            txbComposer.Text = TextBoxDefaultAll;
            txbComposer.PopupWidth = -1;
            txbComposer.ItemSelected += (sender, e) =>
            {
                UpdateGenreList();
                UpdatePieceList(false);

                if (txbComposer.SelectedItem != null)
                    txbFilter.Focus();
            };
            txbComposer.TextChanged += (sender, e) =>
            {
                SupressUpdate = true;
                txbGenre.Text = TextBoxDefaultAll;
                txbFilter.Text = TextBoxDefaultNone;
                SupressUpdate = false;
            };
            txbComposer.KeyDown += (sender, e) =>
            {
                if (e.KeyData == Keys.Enter)
                {
                    if (txbComposer.SelectedItem != null || txbComposer.Text == TextBoxDefaultAll)
                        txbFilter.Focus();
                    else
                    {
                        if (AddNewComposer(txbComposer.Text)) // add new composer and set focus to dg if add is successful
                            dg.Focus();
                    }
                }
                else if (e.KeyData == Keys.F2 && txbComposer.HighlightedItem != null)
                {
                    //change_status(txbComposer.HighlightedValue.ToString() + txbComposer.HighlightedItem.ToString());
                    int id = int.Parse(txbComposer.HighlightedValue.ToString());
                    txbComposer.HideAutoCompleteList();
                    RenameComposer(id, "");
                }
                else if (e.KeyData == Keys.Delete && txbComposer.HighlightedItem != null)
                {
                    int id = int.Parse(txbComposer.HighlightedValue.ToString());
                    txbComposer.HideAutoCompleteList();
                    DeleteComposer(id);
                }
                else return;

                e.Handled = true;
            };
            txbComposer.LostFocus += new EventHandler(txb_LostFocus);

            txbGenre.Text = TextBoxDefaultAll;
            txbGenre.PopupWidth = -1;
            txbGenre.LostFocus += txb_LostFocus;
            txbGenre.ItemSelected += (sender, e) => UpdatePieceList(false);

            txbFilter.Text = TextBoxDefaultNone;
            txbFilter.TextChanged += (sender, e) => UpdatePieceList(false);
            txbFilter.KeyDown += txb_KeyDown;
            txbFilter.KeyDown += (sender, e) =>
            {
                if (e.KeyData == Keys.Down)
                    dg.Focus();
            };

            dg.Rows.Clear();
            dg.CellFormatting += (sender, e) =>
            {
                // highlight the fact that the control is not in focus
                e.CellStyle.SelectionBackColor = ((DataGridView)sender).Focused ?
                                                 SystemColors.Highlight : SystemColors.InactiveCaption;

                // highlight the pieces that are "derived" from other pieces (i.e. a transcription, etc.)
                e.CellStyle.ForeColor = (dg[2, e.RowIndex].Value == null) ?
                                        SystemColors.ControlText : Color.Gray;

                // highlight the cell in edit mode
                if (dg.IsCurrentCellInEditMode && dg.CurrentCell == dg[e.ColumnIndex, e.RowIndex])
                    e.CellStyle.BackColor = CellInEditColor;
            };
            dg.SortCompare += (sender, e) =>
            {
                // use natural sorting
                if (dg[1, e.RowIndex1].Value == null || dg[1, e.RowIndex2].Value == null)
                    return;
                e.SortResult = NaturalSortComparer.Default.Compare(dg[1, e.RowIndex1].Value.ToString(),
                    dg[1, e.RowIndex2].Value.ToString());
                e.Handled = true;
            };
            dg.KeyDown += (sender, e) =>
            {
                if ((e.KeyData == Keys.Enter || e.KeyData == Keys.Tab))
                {
                    // move to next control
                    e.Handled = true;
                    if (clbDetail.Items.Count != 0) clbDetail.SelectedIndex = 0;
                    if (tabControl.SelectedTab == tabControl.TabPages[0] && clbDetail.Items.Count == 0)
                        tabControl.SelectedTab = tabControl.TabPages[1]; // goto text edit mode
                    tabControl.Focus();
                }
                else if (e.KeyData == Keys.Up || e.KeyData == Keys.Down)
                    IsScrolling = true; // don't fetch data if the user is scrolling fast
                else if (e.KeyData == Keys.Escape)
                    txbFilter.Focus();
                else if (e.KeyData == (Keys.Control | Keys.E))
                {
                    //start text edit mode
                    tabControl.SelectedTab = tabControl.TabPages[1];
                    tabControl.Focus();
                }
                else if (e.KeyData == Keys.Delete)
                {
                    DeletePiece();
                }
                else if (e.KeyData == (Keys.Home))
                {
                    dg.ClearSelection();
                    dg.CurrentCell = dg[1, 0];
                    dg.CurrentCell.Selected = true;
                }
                else if (e.KeyData == (Keys.End))
                {
                    dg.ClearSelection();
                    dg.CurrentCell = dg[1, dg.RowCount - 1];
                    dg.CurrentCell.Selected = true;
                }
            };
            dg.KeyUp += (sender, e) =>
            {
                if (e.KeyData == Keys.Up || e.KeyData == Keys.Down)
                {
                    // resume fetchting data after user stop fast scrolling
                    IsScrolling = false;

                    //force re-selection so that the SelectionChanged event will fire
                    dg.ClearSelection();
                    dg.Rows[dg.CurrentRow.Index].Selected = true;
                }
            };
            dg.SelectionChanged += (sender, e) => { if (!IsScrolling) UpdateDetail(); };
            dg.GotFocus += (sender, e) =>
            {
                if (dg.SelectedRows.Count == 0) //auto select first row if nothing is already selected
                    dg.Rows[0].Selected = true;
            };
            dg.CellEndEdit += (sender, e) =>
            {
                dg.CurrentCell = dg[e.ColumnIndex, e.RowIndex];
                if (dg[0, e.RowIndex].Value == null && dg.CurrentCell.Value != null)
                    AddNewPiece(dg.CurrentCell.Value.ToString());
                else if (!dg.CurrentRow.IsNewRow)
                    RenamePiece(dg.CurrentCell.Value.ToString());
            };

            clbDetail.KeyDown += new KeyEventHandler(clbDetail_KeyDown);
            clbDetail.LostFocus += (sender, e) => clbDetail.ClearSelected();
            clbDetail.SelectedIndexChanged += (sender, e) =>
            {
                // show preview of how the formatted name is like on statusbar
                if (!DetailPaneVisible) return; // no need to update UI
                if (clbDetail.SelectedIndex != -1 && currentPiece != null)
                {
                    change_status(((currentPiece.Connector == "<--") ?
                        "" : currentPiece.Name + currentPiece.Connector)
                        + clbDetail.SelectedItem.ToString());
                }
            };

            // don't focus on the tabs themselves
            tabControl.GotFocus += (sender, e) => this.SelectNextControl(tabControl, true, true, true, true);
            tabControl.Deselecting += (sender, e) =>
            {
                if (e.TabPage == tabPage2)
                {
                    if (string.IsNullOrEmpty(txbEdit.Text.Trim()) || ((currentPiece.Details != null &&
                        string.Join("", currentPiece.Details) == string.Join("", txbEdit.Lines))))
                        return;
                    DialogResult ret = MessageBox.Show(
                        "Your changes have not been saved; you want to save the changes?",
                        "Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                    switch (ret)
                    {
                        case DialogResult.Cancel:
                            e.Cancel = true;
                            txbEdit.Focus();
                            break;
                        case DialogResult.No:
                            UpdateDetail();
                            break;
                        case DialogResult.Yes:
                            ChangeDetail(txbEdit.Lines);
                            break;
                        default:
                            break;
                    }
                }
            };

            txbEdit.KeyDown += (sender, e) =>
            {
                if (e.KeyData == Keys.Escape)
                    tabControl.SelectTab(0);
                if (e.KeyData == (Keys.Control | Keys.Enter)) // submit changes and leave edit mode
                {
                    ChangeDetail(txbEdit.Lines);
                    tabControl.SelectTab(0);
                }
                if (e.KeyData == (Keys.Shift | Keys.Enter))
                    ChangeDetail(txbEdit.Lines); // submit changes but not leaving edit mode
            };
            //txbEdit.LostFocus += (sender, e) =>
            //{
            //    if (txbEdit.Text.Trim() == "" || currentPiece.Details == txbEdit.Lines) return;
            //    change_status("aa");
            //    //if (MessageBox.Show("Your changes have not been saved; are you sure you want to leave and discard the changes?",
            //    //    "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            //    //    txbEdit.Focus();
            //    //else
            //    //    txbEdit.Lines = currentPiece.Details;
            //};

            txbEdit.PreviewKeyDown += (sender, e) =>
            {
                if (e.KeyData == (Keys.Shift | Keys.Enter) || e.KeyData == (Keys.Control | Keys.Enter))
                    e.IsInputKey = false; //shift+enter and ctrl+enter is used for special purposes, not input...
            };
            //txbEdit.GotFocus += (sender,e) => txbEdit.Lines = currentPiece.Details;
        }

        
        
        /// <summary>
        /// Must call before using; handles important procedures such as connecting to DB.
        /// </summary>
        public void Initialize()
        {
            Database.Open("music.db3");

            artistSearch = new ArtistSearch(ArtistSearch.Fields.ID, ArtistSearch.Fields.FullName);
            artistSearch.AddTypeToSearch(ArtistSearch.TypeCategory.Composers);

            genreSearch = new GenreSearch(GenreSearch.Fields.ID, GenreSearch.Fields.Name);

            pieceSearch = new PieceSearch(PieceSearch.Fields.ID, PieceSearch.Fields.Name,
                PieceSearch.Fields.ParentPieceID);


            SupressUpdate = false;

            UpdateArtistList();
            UpdateGenreList();
            UpdatePieceList(false);
        }

        /// <summary>
        /// Clear the current search and start a new one.
        /// </summary>
        public void StartNewSearch()
        {
            txbComposer.Focus();
            txbComposer.SelectAll();
        }
        /// <summary>
        /// Update the screen (get data again from database).
        /// </summary>
        public void UpdateData()
        {
            UpdatePieceList(true);
        }

        private void RenameComposer(int id, string newName)
        {
            Artist a;
            if (!(id > 0 && (a = new Artist(id)).ID > 0)) return;

            if (newName == "")
            {   // ask for a new name
                Dialogs.Inputbox input = new Dialogs.Inputbox("Enter a new name for composer " 
                    + a.GetName(Artist.NameFormats.First_Last),
                    a.GetName(Artist.NameFormats.First_Last), false);

                if (input.ShowDialog() == DialogResult.OK) newName = input.InputText;
                else return;
            }

            try
            {
                change_status("Renaming composer...");
                this.Cursor = Cursors.WaitCursor;
                Application.DoEvents();

                a.Name = newName;
                a.Update();

                UpdateArtistList();
                txbComposer.SetTextAndSelect(a.GetName(Artist.NameFormats.Last_First));
                txbComposer.SelectAll();
                UpdatePieceList(false);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                change_status("Rename successful");
            }


        }


        private void RenamePiece(string newName)
        {
            if (currentPiece == null) return;
            if (SupressUpdate) return;

            if (newName == "")
            {   // ask for a new name
                Dialogs.Inputbox input = new Dialogs.Inputbox("Enter a new name for the piece",
                    currentPiece.Name, false);

                if (input.ShowDialog() == DialogResult.OK) newName = input.InputText;
                else return;
            }

            if (currentPiece.Name == newName) return;

            try
            {
                change_status("Renaming piece...");
                this.Cursor = Cursors.WaitCursor;

                currentPiece.Name = newName;
                currentPiece.Update();

                SupressUpdate = true;
                dg.CurrentCell.Value = newName;
                SupressUpdate = false;

                UpdatePieceList(true);
                UpdateDetail();
            }
            finally
            {
                this.Cursor = Cursors.Default;
                change_status("Rename successful");
            }
        }
        private void ChangeDetail(string[] lines)
        {
            if (string.IsNullOrEmpty(txbEdit.Text.Trim()) || ((currentPiece.Details != null &&
                        string.Join("", currentPiece.Details) == string.Join("", lines))))
                return;

            currentPiece.Details = lines;
            currentPiece.Update();
            UpdateDetail();
            change_status("Details update successful");
        }

        private bool AddNewComposer(string name)
        {
            try
            {
                SupressUpdate = true;
                if (name == "")
                {   // ask for a new name
                    Dialogs.Inputbox input = new Dialogs.Inputbox("Enter a new name for the new composer",
                        currentPiece.Name, false);

                    if (input.ShowDialog() == DialogResult.OK) name = input.InputText;
                    else return false;
                }
                else if (MessageBox.Show("Do you want to add composer " + txbComposer.Text + " to the database?",
                    "Confirmation", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return false;

                Artist c = new Artist(txbComposer.Text, "composer", false);
                if (c.ID != 0)
                    MessageBox.Show("The new composer you entered exists in the database as "
                    + c.GetName(Artist.NameFormats.Last_First_Type) + ".\n It is therefore not added to database");
                else
                    c.Insert();

                SupressUpdate = false;
                UpdateArtistList();
                txbComposer.SetTextAndSelect(c.GetName(Artist.NameFormats.Last_First));
                change_status("Composer " + c.GetName(Artist.NameFormats.First_Last) + " has been added to database.",false);
                return true;
            }
            finally
            {
                SupressUpdate = false;
            }

        }

        private void AddNewPiece(string name)
        {
            //if (SupressUpdate) return;

            try
            {
                SupressUpdate = true;

                //if (dg.CurrentCell.RowIndex != dg.NewRowIndex)
                //{
                //    //set current cell to the new row
                //    dg.CurrentCell = dg[1, dg.NewRowIndex];
                //    dg.CurrentCell.Value = name;
                //}

                if (txbComposer.SelectedValue == null)
                {
                    MessageBox.Show("Please choose a composer before adding a new piece");
                    UpdatePieceList(false);
                    return;
                }

                if (txbGenre.SelectedValue == null)
                {
                    //TODO: show a dialog box here instead of aborting.
                    MessageBox.Show("Please choose a genre before adding a new piece");
                    UpdatePieceList(false);
                    return;
                }

                Piece p = new Piece
                {
                    Name = name,
                    Composer = new Artist(int.Parse(txbComposer.SelectedValue.ToString())),
                    Genre = txbGenre.Text
                };

                p.Insert();

                dg[0, dg.CurrentCell.RowIndex].Value = p.ID;

            }
            finally
            {
                SupressUpdate = false;
            }
        }

        private void DeleteComposer(int id)
        {
            if (id == 0) return;

            // ask for confirmation
            if (MessageBox.Show("Are you sure to delete the composer?\nThis is not reversible!",
                "Delete Composer", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                return;

            Artist a = new Artist(id);
            a.Delete();
            UpdateArtistList();
            txbComposer.SetTextAndSelect(a.GetName(Artist.NameFormats.Last_First));
            txbComposer.SelectAll();
        }

        private void DeletePiece()
        {
            if (currentPiece == null) { MessageBox.Show("null1"); return; }
            if (SupressUpdate) return;

            try
            {
                SupressUpdate = true;

                // ask for confirmation
                if (MessageBox.Show("Are you sure to delete the selected piece(s)?\nThis is not reversible!",
                    "Delete Piece", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                    return;

                currentPiece.Delete();
                dg.ClearSelection();
                dg.Rows.Remove(dg.Rows[dg.CurrentCell.RowIndex]);
                //dg.CurrentCell = dg[1, (dg.CurrentCell.RowIndex == dg.Rows.Count) ?
                //                        dg.CurrentCell.RowIndex - 1 : dg.CurrentCell.RowIndex + 1];

                SupressUpdate = false;
                dg.CurrentCell.Selected = true;
                UpdatePieceList(true);
            }
            finally
            {
                SupressUpdate = false;
            }
        }

        private void UpdateDetail()
        {
            if (SupressUpdate) return;

            clbDetail.Items.Clear();
            txbEdit.Clear();
            currentPiece = null;
            if (dg.SelectedRows.Count != 1 || dg[0, dg.SelectedRows[0].Index].Value == null)
                return;

            currentPiece =
                new Piece(int.Parse(dg[0, dg.SelectedRows[0].Index].Value.ToString()));

            if (!DetailPaneVisible) return; // no need to update UI

            if (currentPiece.Details != null)
            {
                clbDetail.Items.AddRange(currentPiece.Details);

                //check all
                for (int i = 0; i < clbDetail.Items.Count; i++)
                    clbDetail.SetItemChecked(i, true);

                clbDetail.ClearSelected();
                txbEdit.Lines = currentPiece.Details;
            }

        }

        private void UpdatePieceList(bool TryKeepOldSelection)
        {
            if (SupressUpdate) return;
            DateTime then, now;
            then = DateTime.Now;
            try
            {
                SupressUpdate = true;
                this.Cursor = Cursors.WaitCursor;
                change_status("Updating pieces list...", false);
                Application.DoEvents();

                pieceSearch.ClearFilters();
                string current_value = "";
                if (dg != null && dg.CurrentCell != null && TryKeepOldSelection)
                    current_value = dg[1, dg.CurrentRow.Index].Value.ToString();

                SupressUpdate = false;
                dg.ClearSelection();
                dg.Rows.Clear();
                SupressUpdate = true;

                if (txbComposer.SelectedValue != null)
                    pieceSearch.AddFilter(PieceSearch.Fields.ComposerID, "= "
                                          + txbComposer.SelectedValue.ToString());
                else if (txbComposer.Text != TextBoxDefaultAll) return;

                if (txbGenre.SelectedValue != null)
                    pieceSearch.AddFilter(PieceSearch.Fields.GenreID, "= "
                                          + txbGenre.SelectedValue.ToString());
                else if (txbGenre.Text != TextBoxDefaultAll) return;

                if (txbFilter.Text != TextBoxDefaultNone)
                    pieceSearch.AddWordFilter(PieceSearch.Fields.Name, txbFilter.Text);

                pieceSearch.PerformSearchToDataGridView(dg);
                dg.Sort(dg.Columns[1], ListSortDirection.Ascending);

                if (TryKeepOldSelection && current_value != "")
                {
                    Application.DoEvents();
                    dg.ClearSelection();
                    int foundrow = -1;
                    if ((foundrow = dg.FindFirstRow(current_value, 1)) >= 0)
                    {
                        SupressUpdate = false;
                        dg.CurrentCell = dg[1, foundrow];
                        dg.CurrentCell.Selected = true;
                        return;
                    }
                }
                else
                {
                    SupressUpdate = false;
                    dg.CurrentCell = dg[1, 0];
                    dg.ClearSelection();
                }
            }
            finally
            {
                now = DateTime.Now;
                SupressUpdate = false;
                this.Cursor = Cursors.Default;
                change_status("Search took " + Math.Round((now - then).TotalSeconds, 3)
                              + "s for " + (dg.Rows.Count - 1) + " results");
                Application.DoEvents();
            }
        }

        private void UpdateArtistList()
        {
            if (SupressUpdate) return;

            txbComposer.Items.Clear();
            txbComposer.Items.AddRange(artistSearch.PerformSearchToList(),
                                       1,  //match FullName
                                       1,  //display FullName
                                       0  //value member is ID
                                       );
            txbComposer.ForceUpdateList();
        }

        private void UpdateGenreList()
        {
            if (SupressUpdate) return;

            genreSearch.ClearFilters();

            if (txbComposer.SelectedValue != null)
                genreSearch.AddFilter(GenreSearch.Fields.ComposerID, " = "
                                      + txbComposer.SelectedValue.ToString());

            txbGenre.Items.Clear();
            txbGenre.Items.AddRange(genreSearch.PerformSearchToList(),
                                       1,  //match Name
                                       1,  //display Name
                                       0  //value member is ID
                                       );
            txbGenre.ForceUpdateList();
        }

        private void change_status(string status, bool autoReset)
        {
            status_reset_timer.Stop();
            Status = status;
            if (StatusChanged != null) // fire up event
                StatusChanged(this, new EventArgs());
            if (autoReset)
                status_reset_timer.Start();
        }

        private void change_status(string status)
        {
            change_status(status, true);
        }

        // a few common keyboard shortcuts for textboxes
        private void txb_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (e.KeyData == Keys.Escape)
            {
                if (tb.Text == "")
                {
                    if (sender == txbFilter) tb.Text = TextBoxDefaultNone;
                    else tb.Text = TextBoxDefaultAll;
                }
                else
                    tb.Text = "";

                tb.SelectAll();

                UpdatePieceList(false);
            }
            else if (e.KeyData == Keys.Enter && tb != txbComposer)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        void txb_LostFocus(object sender, EventArgs e)
        {
            TextBox tb = ((TextBox)sender);
            tb.SelectionStart = 0;
            tb.ScrollToCaret();
            if (tb.Text == "") tb.Text = TextBoxDefaultAll;
        }

        void clbDetail_KeyDown(object sender, KeyEventArgs e)
        {
            CheckedListBox clb = (CheckedListBox)sender;
            if (e.KeyData == Keys.Escape)
            {
                dg.Focus();
            }
            else if (e.KeyData == (Keys.Control | Keys.A))
            {
                if (clb.Items.Count == 0) return;

                bool check = (clb.CheckedItems.Count > 0);

                for (int i = 0; i < clb.Items.Count; i++)
                {
                    clb.SetItemChecked(i, (!check));
                }
                clb.ClearSelected();
            }
            else if (e.KeyData == Keys.F2)
            {
                RenamePiece("");
            }
            else if (e.KeyData == (Keys.Control | Keys.E))
            {
                tabControl.SelectedTab = tabControl.TabPages[1];
                tabControl.Focus();
            }
        }

        private void TrappedKeyDown(KeyEventArgs e)
        {
            if (e.KeyData == Keys.F6)
                StartNewSearch();
            else if (e.KeyData == Keys.F5)
                UpdateData();
            else if (e.KeyData == (Keys.Control | Keys.O))
            {
                Database.Close();
                Database.Open("../../../libdb/music.db3");
                UpdateArtistList();
                UpdatePieceList(true);
                MessageBox.Show("DEBUG ONLY: now it is switched to the read database; be careful of making changes!",
                    "",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                this.BackColor = Color.Red;
            }
            else
                return;
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
        

        #region Trap key events for entire user control

        // http://www.codeproject.com/KB/cs/ProcessKeyPreview.aspx
        //----------------------------------------------
        // Define the PeekMessage API call
        //----------------------------------------------

        private struct MSG
        {
            public IntPtr hwnd;
            public int message;
            public IntPtr wParam;
            public IntPtr lParam;
            public int time;
            public int pt_x;
            public int pt_y;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool PeekMessage([In, Out] ref MSG msg,
            HandleRef hwnd, int msgMin, int msgMax, int remove);

        //----------------------------------------------

        /// <summary> 
        /// Trap any keypress before child controls get hold of them
        /// </summary>
        /// <param name="m">Windows message</param>
        /// <returns>True if the keypress is handled</returns>
        protected override bool ProcessKeyPreview(ref Message m)
        {
            const int WM_KEYDOWN = 0x100;
            //const int WM_KEYUP = 0x101;
            const int WM_CHAR = 0x102;
            const int WM_SYSCHAR = 0x106;
            const int WM_SYSKEYDOWN = 0x104;
            //const int WM_SYSKEYUP = 0x105;
            const int WM_IME_CHAR = 0x286;

            KeyEventArgs e = null;

            if ((m.Msg != WM_CHAR) && (m.Msg != WM_SYSCHAR) && (m.Msg != WM_IME_CHAR))
            {
                e = new KeyEventArgs(((Keys)((int)((long)m.WParam))) | ModifierKeys);
                if ((m.Msg == WM_KEYDOWN) || (m.Msg == WM_SYSKEYDOWN))
                {
                    TrappedKeyDown(e);
                }
                //else
                //{
                //    TrappedKeyUp(e);
                //}

                // Remove any WM_CHAR type messages if supresskeypress is true.
                if (e.SuppressKeyPress)
                {
                    this.RemovePendingMessages(WM_CHAR, WM_CHAR);
                    this.RemovePendingMessages(WM_SYSCHAR, WM_SYSCHAR);
                    this.RemovePendingMessages(WM_IME_CHAR, WM_IME_CHAR);
                }

                if (e.Handled)
                {
                    return e.Handled;
                }
            }
            return base.ProcessKeyPreview(ref m);
        }

        private void RemovePendingMessages(int msgMin, int msgMax)
        {
            if (!this.IsDisposed)
            {
                MSG msg = new MSG();
                IntPtr handle = this.Handle;
                while (PeekMessage(ref msg,
                new HandleRef(this, handle), msgMin, msgMax, 1))
                {
                }
            }
        }
        #endregion



    }
}
