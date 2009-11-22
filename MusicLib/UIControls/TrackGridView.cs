using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using libdb;
using System.Drawing;

namespace MusicLib
{
    class TrackGridView : DataGridView
    {

        private readonly Color CellInEditColor = Color.Azure;
        private readonly string[] ColumnsWithRelatedValues = { "year", "performersobj", };
        private DataGridViewTextBoxColumn disc_num;
        private DataGridViewTextBoxColumn trackobj;
        private DataGridViewTextBoxColumn pieceobj;
        private DataGridViewTextBoxColumn performersobj;
        private DataGridViewTextBoxColumn piece_id;
        private DataGridViewTextBoxColumn track_num;
        private DataGridViewTextBoxColumn composer;
        private DataGridViewTextBoxColumn genre;
        private DataGridViewTextBoxColumn title;
        private DataGridViewTextBoxColumn performer;
        private DataGridViewTextBoxColumn year;
        private DataGridViewTextBoxColumn filename;
        private DataGridViewTextBoxColumn filesize;
        private DataGridViewTextBoxColumn length;

        bool IsImporting = false;
        DataGridViewSelectedCellCollection oldSelectedCells;

        public TrackGridView()
        {
            InitializeComponent();

            filesize.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            length.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        public void populateTracks(List<Track> tracks)
        {
            IsImporting = true;
            Rows.Clear();
            foreach (var track in tracks)
            {
                this.Rows.Add();
                int r = this.Rows.Count - 1;
                this[trackobj.Name, r].Value = track;
            }
            IsImporting = false;
        }
        public void addTrackInfo(List<Track> tracks)
        {
            IsImporting = true;
            List<int> rows = SelectedCells.Cast<DataGridViewCell>()
                .Select(x => x.RowIndex).Distinct().ToList<int>();

            bool singleSelected = false;
            if (rows.Count == 0) rows.Add(0); // start adding from first row if nothing is selected
            if (rows.Count == 1)
            {
                for (int i = 1; i < tracks.Count; i++)
                {
                    if (rows[rows.Count - 1] >= Rows.Count - 1) break;
                    rows.Add(rows[rows.Count - 1] + 1);
                }
                singleSelected = true;
            }
                
            for (int i = 0; i < Math.Min(tracks.Count, rows.Count); i++)
            {
                Track track = tracks[i];
                int r = rows[i];

                this[pieceobj.Name, r].Value = track.Piece;
                this[title.Name, r].Value = track.Name;
            }

            this.Invalidate();
            if (!this.Focused && singleSelected)
            {
                ClearSelection();
                Rows[(rows[rows.Count - 1] + 1>= Rows.Count ? Rows.Count - 1 : rows[rows.Count - 1] + 1)].Selected = true;
                FirstDisplayedScrollingRowIndex = (SelectedRows[0].Index - DisplayedRowCount(false) / 2 < 0) ? 
                                                   0 : SelectedRows[0].Index - DisplayedRowCount(false) / 2;
            }
            IsImporting = false;
        }
        public bool checkInfoCompleteness()
        {
            return Rows.Count == Rows.Cast<DataGridViewRow>()
                .Where(x => x.Cells[trackobj.Name].Value != null)
                .Where(x => x.Cells[pieceobj.Name].Value != null 
                    && ((Piece)x.Cells[pieceobj.Name].Value).ID != 0)
                .Count();
        }
        public List<Track> exportInfo()
        {
            foreach (DataGridViewRow row in Rows)
            {
                Track t = (Track)row.Cells[trackobj.Name].Value;
                t.Piece = (Piece)row.Cells[pieceobj.Name].Value;
                t.Performers = (List<Artist>)row.Cells[performersobj.Name].Value;
                t.Name = row.Cells[title.Name].Value.ToString();
                t.Year = int.Parse(row.Cells[year.Name].Value.ToString());
            }
            return Rows.Cast<DataGridViewRow>().
                Select(x => (Track)x.Cells[trackobj.Name].Value).ToList();
                
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter &&
                CurrentCell.ColumnIndex == Columns[performer.Name].Index)
            {
                List<Artist> artists = SelectMultipleArtistsDialog.GetResult(
                    (List<Artist>)this[performersobj.Name, CurrentCell.RowIndex].Value);
                if (artists != null)
                    this[performersobj.Name, CurrentCell.RowIndex].Value = artists;
            }
            base.OnKeyDown(e);
        }
        protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
        {

            // highlight the fact that the control is not in focus
            e.CellStyle.SelectionBackColor = this.Focused ?
                                             SystemColors.Highlight : SystemColors.InactiveCaption;

            // highlight the cell in edit mode
            if (this.IsCurrentCellInEditMode && this.CurrentCell == this[e.ColumnIndex, e.RowIndex])
                e.CellStyle.BackColor = CellInEditColor;

            base.OnCellFormatting(e);
        }
        protected override void OnCellValueChanged(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1) return; 

            Piece p;
            List<Artist> l;
            Track track; 
            if (Columns[e.ColumnIndex] == pieceobj && 
                (p = (Piece)this[pieceobj.Name, e.RowIndex].Value) != null)
            {
                this[composer.Name, e.RowIndex].Value = p.Composer.GetName(Artist.NameFormats.Last_First);
                this[genre.Name, e.RowIndex].Value = p.Genre;
            }          
            else if (Columns[e.ColumnIndex] == performersobj && 
                (l = (List<Artist>)this[performersobj.Name, e.RowIndex].Value) != null)
                this[performer.Name, e.RowIndex].Value = string.Join(" / ", l.ConvertAll<string>(
                        a => a.GetName(Artist.NameFormats.First_Last_Type)).ToArray());
            else if (Columns[e.ColumnIndex] == trackobj &&
                (track = (Track)this[trackobj.Name, e.RowIndex].Value) != null)
            {
                this[disc_num.Name, e.RowIndex].Value = track.DiscNumber;
                this[track_num.Name, e.RowIndex].Value = track.TrackNumber;
                this[pieceobj.Name, e.RowIndex].Value = track.Piece;
                this[title.Name, e.RowIndex].Value = track.Name;
                this[performersobj.Name, e.RowIndex].Value = track.Performers;
                this[year.Name, e.RowIndex].Value = track.Year == 0 ? "" : track.Year.ToString();
                this[filename.Name, e.RowIndex].Value = track.FileName;
                this[filesize.Name, e.RowIndex].Value = string.Format(new FileSizeFormatProvider(), "{0:fs}", track.Size * 1024);
                this[length.Name, e.RowIndex].Value = new TimeSpan(0, 0, track.Length).Duration().ToString().Substring(3, 5);
            }

            // the rest applies only when the user explicitly change values
            if (IsImporting) return;

            // set the same value for multiple selected cells in the same column
            var cells = SelectedCells.Cast<DataGridViewCell>().Where(x => (x.ColumnIndex == e.ColumnIndex)).ToList();
            foreach (var cell in cells)
                cell.Value = this[e.ColumnIndex, e.RowIndex].Value;

            // automatically set the same property for the pieces in the same set
            if (ColumnsWithRelatedValues.Contains(Columns[e.ColumnIndex].Name))
            {
                foreach (DataGridViewRow row in Rows)
                {
                    if (((Piece)row.Cells[pieceobj.Name].Value).ID == 
                        ((Piece)this[pieceobj.Name, e.RowIndex].Value).ID)
                        row.Cells[e.ColumnIndex].Value = this[e.ColumnIndex, e.RowIndex].Value;
                }
            }
            base.OnCellValueChanged(e);
        }
        protected override void OnLostFocus(EventArgs e)
        {
            oldSelectedCells = SelectedCells;
            
            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            foreach (DataGridViewCell cell in oldSelectedCells)
                Rows[cell.RowIndex].Selected = true;

            Invalidate();
            base.OnLostFocus(e);
        }
        protected override void OnGotFocus(EventArgs e)
        {
            this.SelectionMode = DataGridViewSelectionMode.CellSelect;
            ClearSelection();

            foreach (DataGridViewCell cell in oldSelectedCells)
                cell.Selected = true;

            Invalidate();
            base.OnGotFocus(e);
        }
        private void InitializeComponent()
        {
            this.disc_num = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trackobj = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pieceobj = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.performersobj = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.piece_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.track_num = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.composer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.genre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.performer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.year = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.filename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.filesize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.length = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // disc_num
            // 
            this.disc_num.HeaderText = "";
            this.disc_num.Name = "disc_num";
            this.disc_num.ReadOnly = true;
            this.disc_num.Width = 20;
            // 
            // trackobj
            // 
            this.trackobj.HeaderText = "trackobj";
            this.trackobj.Name = "trackobj";
            this.trackobj.Visible = false;
            // 
            // pieceobj
            // 
            this.pieceobj.HeaderText = "pieceobj";
            this.pieceobj.Name = "pieceobj";
            this.pieceobj.Visible = false;
            // 
            // performersobj
            // 
            this.performersobj.HeaderText = "performersobj";
            this.performersobj.Name = "performersobj";
            this.performersobj.Visible = false;
            // 
            // piece_id
            // 
            this.piece_id.HeaderText = "Piece ID";
            this.piece_id.Name = "piece_id";
            this.piece_id.Visible = false;
            // 
            // track_num
            // 
            this.track_num.HeaderText = "#";
            this.track_num.Name = "track_num";
            this.track_num.ReadOnly = true;
            this.track_num.Width = 20;
            // 
            // composer
            // 
            this.composer.HeaderText = "Composer";
            this.composer.Name = "composer";
            this.composer.ReadOnly = true;
            this.composer.Width = 60;
            // 
            // genre
            // 
            this.genre.HeaderText = "Genre";
            this.genre.Name = "genre";
            this.genre.ReadOnly = true;
            this.genre.Width = 60;
            // 
            // title
            // 
            this.title.HeaderText = "Track Title";
            this.title.Name = "title";
            this.title.Width = 400;
            // 
            // performer
            // 
            this.performer.HeaderText = "Performer";
            this.performer.Name = "performer";
            this.performer.ReadOnly = true;
            this.performer.Width = 200;
            // 
            // year
            // 
            this.year.HeaderText = "Year";
            this.year.Name = "year";
            this.year.Width = 40;
            // 
            // filename
            // 
            this.filename.HeaderText = "File name";
            this.filename.Name = "filename";
            this.filename.ReadOnly = true;
            this.filename.Width = 200;
            // 
            // filesize
            // 
            this.filesize.HeaderText = "Size";
            this.filesize.Name = "filesize";
            this.filesize.ReadOnly = true;
            this.filesize.Width = 70;
            // 
            // length
            // 
            this.length.HeaderText = "Length";
            this.length.Name = "length";
            this.length.ReadOnly = true;
            this.length.Width = 40;
            // 
            // TrackGridView
            // 
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AllowUserToResizeRows = false;
            this.BackgroundColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.ColumnHeadersHeight = 20;
            this.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.disc_num,
            this.trackobj,
            this.pieceobj,
            this.performersobj,
            this.piece_id,
            this.track_num,
            this.composer,
            this.genre,
            this.title,
            this.performer,
            this.year,
            this.filename,
            this.filesize,
            this.length});
            this.RowHeadersVisible = false;
            this.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

    }
}
