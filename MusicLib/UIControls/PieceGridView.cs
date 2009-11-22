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
    class PieceGridView : DataGridView
    {
        private readonly Color CellInEditColor = Color.Azure;
        private DataGridViewTextBoxColumn id;
        private DataGridViewTextBoxColumn PieceName;
        private DataGridViewTextBoxColumn ParentID;
        public event EventHandler StatusChanged;

        Timer status_reset_timer;
        Piece currentPiece;
        //List<Piece> currentPieces;
        bool IsScrolling = false;

        public string Status { get; private set; }
        public bool SupressUpdate { get; set; } // used to prevent unnecessary fetching of data
        PieceSearch Search;

        public PieceGridView()
        {
            SupressUpdate = true;

            this.id = new DataGridViewTextBoxColumn();
            this.PieceName = new DataGridViewTextBoxColumn();
            this.ParentID = new DataGridViewTextBoxColumn();
            AllowUserToDeleteRows = false;
            AllowUserToResizeColumns = false;
            AllowUserToResizeRows = false;
            BackgroundColor = System.Drawing.SystemColors.Window;
            BorderStyle = BorderStyle.None;
            CellBorderStyle = DataGridViewCellBorderStyle.None;
            ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ColumnHeadersVisible = false;
            Columns.AddRange(new DataGridViewColumn[] { id, PieceName, ParentID });
            RowHeadersVisible = false;
            RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            RowTemplate.Height = 16;
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            StandardTab = true;
            // 
            // id
            // 
            this.id.HeaderText = "ID";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            // 
            // PieceName
            // 
            this.PieceName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.PieceName.HeaderText = "Name";
            this.PieceName.Name = "PieceName";
            // 
            // ParentID
            // 
            this.ParentID.HeaderText = "ParentID";
            this.ParentID.Name = "ParentID";
            this.ParentID.Visible = false;


            Search = new PieceSearch(PieceSearch.Fields.ID, PieceSearch.Fields.Name,
                                     PieceSearch.Fields.ParentPieceID);


            status_reset_timer = new Timer();
            status_reset_timer.Interval = 6000; // reset status message to "Ready" after 6 sec
            status_reset_timer.Tick += (sender, e) =>
            {
                this.change_status("Ready", false);
                status_reset_timer.Enabled = false;
            };

            this.Rows.Clear();
            this.CellFormatting += (sender, e) =>
            {
                // highlight the fact that the control is not in focus
                e.CellStyle.SelectionBackColor = ((DataGridView)sender).Focused ?
                                                 SystemColors.Highlight : SystemColors.InactiveCaption;

                // highlight the pieces that are "derived" from other pieces (i.e. a transcription, etc.)
                e.CellStyle.ForeColor = (this[2, e.RowIndex].Value == null) ?
                                        SystemColors.ControlText : Color.Gray;

                // highlight the cell in edit mode
                if (this.IsCurrentCellInEditMode && this.CurrentCell == this[e.ColumnIndex, e.RowIndex])
                    e.CellStyle.BackColor = CellInEditColor;
            };
            this.SortCompare += (sender, e) =>
            {
                // use natural sorting
                if (this[1, e.RowIndex1].Value == null || this[1, e.RowIndex2].Value == null)
                    return;
                e.SortResult = NaturalSortComparer.Default.Compare(this[1, e.RowIndex1].Value.ToString(),
                    this[1, e.RowIndex2].Value.ToString());
                e.Handled = true;
            };
            this.KeyDown += (sender, e) =>
            {
                if (e.KeyData == Keys.Up || e.KeyData == Keys.Down)
                    IsScrolling = true; // don't fetch data if the user is scrolling fast
                else if (e.KeyData == (Keys.Home))
                {
                    this.ClearSelection();
                    this.CurrentCell = this[1, 0];
                    this.CurrentCell.Selected = true;
                }
                else if (e.KeyData == (Keys.End))
                {
                    this.ClearSelection();
                    this.CurrentCell = this[1, this.RowCount - 1];
                    this.CurrentCell.Selected = true;
                }
            };
            this.KeyUp += (sender, e) =>
            {
                if (e.KeyData == Keys.Up || e.KeyData == Keys.Down)
                {
                    // resume fetchting data after user stop fast scrolling
                    IsScrolling = false;

                    //force re-selection so that the SelectionChanged event will fire
                    this.ClearSelection();
                    this.Rows[this.CurrentRow.Index].Selected = true;
                }
                else if (e.KeyData == Keys.Delete)
                {
                    DeletePiece();
                }
            };
            this.GotFocus += (sender, e) =>
            {
                if (this.SelectedRows.Count == 0) //auto select first row if nothing is already selected
                    this.Rows[0].Selected = true;
            };
            this.CellEndEdit += (sender, e) =>
            {
                this.CurrentCell = this[e.ColumnIndex, e.RowIndex];
                if (this[0, e.RowIndex].Value == null && this.CurrentCell.Value != null)
                {
                    if (!AddNewPiece(this.CurrentCell.Value.ToString()))
                        this.Rows.Remove(this.Rows[this.CurrentCell.RowIndex]);
                }
                else if (!this.CurrentRow.IsNewRow)
                    RenamePiece(this.CurrentCell.Value.ToString());
            };

        }

        public void UpdateList()
        {
            UpdateList(true);
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
                this.CurrentCell.Value = newName;
                SupressUpdate = false;

                UpdateList(true);
                //UpdateDetail();
            }
            finally
            {
                this.Cursor = Cursors.Default;
                change_status("Rename successful");
            }
        }

        private bool AddNewPiece(string name)
        {
            try
            {
                //SupressUpdate = true;
                //
                ////if (this.CurrentCell.RowIndex != this.NewRowIndex)
                ////{
                ////    //set current cell to the new row
                ////    this.CurrentCell = this[1, this.NewRowIndex];
                ////    this.CurrentCell.Value = name;
                ////}
                //
                //string genre;
                //
                //if (txbComposer.SelectedValue == null)
                //{
                //    MessageBox.Show("Please choose a composer before adding a new piece");
                //    UpdateList(false);
                //    return false;
                //}
                //
                //if (txbGenre.SelectedValue == null)
                //    genre = Dialogs.Inputbox.GetGenre("");
                //else
                //    genre = txbGenre.Text;
                //
                //Piece p = new Piece
                //{
                //    Name = name,
                //    Composer = new Artist(int.Parse(txbComposer.SelectedValue.ToString())),
                //    Genre = genre
                //};
                //
                //p.Insert();
                //
                //this[0, this.CurrentCell.RowIndex].Value = p.ID;
                //
                //return true;
                throw new System.Exception("not implemented");
            }
            finally
            {
                SupressUpdate = false;
            }
        }

        private void DeletePiece()
        {
            if (currentPiece == null && this.SelectedRows.Count == 0) return;
            if (SupressUpdate) return;

            try
            {
                SupressUpdate = true;

                // ask for confirmation
                if (MessageBox.Show("Are you sure to delete the selected piece(s)?\nThis is not reversible!",
                    "Delete Piece", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                    return;

                Piece p;
                bool allsuccessful = true;
                foreach (DataGridViewRow r in this.SelectedRows)
                {
                    p = new Piece(int.Parse(r.Cells[0].Value.ToString()));
                    try
                    {
                        p.Delete();
                        this.Rows.Remove(r);
                    }
                    catch (SqliteClient.SQLiteException e)
                    {
                        if (e.Message.Contains("violates foreign key constraint"))
                            allsuccessful = false;
                        else
                            throw;
                    }
                }

                if (!allsuccessful)
                    MessageBox.Show("Some pieces cannot be delete as they are being referenced "
                                    + "by other records in database; \nmaybe you want to rename the piece instead?");

                SupressUpdate = false;
                this.ClearSelection();
                this.CurrentCell.Selected = true;
            }
            finally
            {
                SupressUpdate = false;
            }
        }

        private void UpdateList(bool TryKeepOldSelection)
        {
            if (SupressUpdate) return;
            try
            {
                SupressUpdate = true;

                string current_value = "";
                if (this.SelectedRows.Count == 1) this.CurrentCell = this[1, this.SelectedRows[0].Index];
                if (TryKeepOldSelection && this != null && this.CurrentCell != null && !this.CurrentRow.IsNewRow)
                    current_value = this[1, this.CurrentRow.Index].Value.ToString();

                SupressUpdate = false;
                this.ClearSelection();
                this.Rows.Clear();
                SupressUpdate = true;

                Search.PerformSearchToDataGridView(this);
                this.Sort(this.Columns[1], ListSortDirection.Ascending);

                if (TryKeepOldSelection && current_value != "")
                {
                    Application.DoEvents();
                    this.ClearSelection();
                    int foundrow = -1;
                    if ((foundrow = this.FindFirstRow(current_value, 1)) >= 0)
                    {
                        SupressUpdate = false;
                        this.CurrentCell = this[1, foundrow];
                        this.CurrentCell.Selected = true;
                        return;
                    }
                }
                else
                {
                    SupressUpdate = false;
                    this.CurrentCell = this[1, 0];
                    this.ClearSelection();
                }
            }
            finally
            {
                SupressUpdate = false;
            }
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
    }
}

