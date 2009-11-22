namespace MusicLib
{
    partial class Browse
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControlMode = new System.Windows.Forms.TabControl();
            this.tabPageSearch = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.txbGenre = new CustomForm.AutoCompleteTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txbComposer = new CustomForm.AutoCompleteTextBox();
            this.tabPageTray = new System.Windows.Forms.TabPage();
            this.txbFilter = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dg = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PieceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParentID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageDetail = new System.Windows.Forms.TabPage();
            this.clbDetail = new System.Windows.Forms.CheckedListBox();
            this.tabPageDetailEdit = new System.Windows.Forms.TabPage();
            this.txbEdit = new System.Windows.Forms.TextBox();
            this.tabPageProperties = new System.Windows.Forms.TabPage();
            this.pgrid = new System.Windows.Forms.PropertyGrid();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControlMode.SuspendLayout();
            this.tabPageSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabPageDetail.SuspendLayout();
            this.tabPageDetailEdit.SuspendLayout();
            this.tabPageProperties.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl);
            this.splitContainer1.Size = new System.Drawing.Size(256, 402);
            this.splitContainer1.SplitterDistance = 222;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.TabStop = false;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer2.Panel1.Controls.Add(this.tabControlMode);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.txbFilter);
            this.splitContainer2.Panel2.Controls.Add(this.label3);
            this.splitContainer2.Panel2.Controls.Add(this.dg);
            this.splitContainer2.Size = new System.Drawing.Size(256, 222);
            this.splitContainer2.SplitterDistance = 36;
            this.splitContainer2.SplitterWidth = 1;
            this.splitContainer2.TabIndex = 0;
            this.splitContainer2.TabStop = false;
            // 
            // tabControlMode
            // 
            this.tabControlMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlMode.Controls.Add(this.tabPageSearch);
            this.tabControlMode.Controls.Add(this.tabPageTray);
            this.tabControlMode.Location = new System.Drawing.Point(0, -24);
            this.tabControlMode.Name = "tabControlMode";
            this.tabControlMode.SelectedIndex = 0;
            this.tabControlMode.Size = new System.Drawing.Size(256, 65);
            this.tabControlMode.TabIndex = 4;
            this.tabControlMode.TabStop = false;
            // 
            // tabPageSearch
            // 
            this.tabPageSearch.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageSearch.Controls.Add(this.label2);
            this.tabPageSearch.Controls.Add(this.txbGenre);
            this.tabPageSearch.Controls.Add(this.label1);
            this.tabPageSearch.Controls.Add(this.txbComposer);
            this.tabPageSearch.Location = new System.Drawing.Point(4, 22);
            this.tabPageSearch.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageSearch.Name = "tabPageSearch";
            this.tabPageSearch.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSearch.Size = new System.Drawing.Size(248, 39);
            this.tabPageSearch.TabIndex = 1;
            this.tabPageSearch.Text = "tabPage5";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-2, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "&Genre(s):";
            // 
            // txbGenre
            // 
            this.txbGenre.BackColor = System.Drawing.SystemColors.Control;
            this.txbGenre.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txbGenre.DefaultText = "";
            this.txbGenre.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txbGenre.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.txbGenre.Location = new System.Drawing.Point(61, 21);
            this.txbGenre.MatchDisplayString = true;
            this.txbGenre.MatchMode = CustomForm.AutoCompleteTextBox.MatchModes.Contains;
            this.txbGenre.Name = "txbGenre";
            this.txbGenre.PopupBorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txbGenre.PopupOffset = new System.Drawing.Point(12, 0);
            this.txbGenre.PopupSelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.txbGenre.PopupSelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.txbGenre.PopupWidth = 300;
            this.txbGenre.Size = new System.Drawing.Size(186, 14);
            this.txbGenre.TabIndex = 3;
            this.txbGenre.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txb_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-2, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Composer:";
            // 
            // txbComposer
            // 
            this.txbComposer.BackColor = System.Drawing.SystemColors.Control;
            this.txbComposer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txbComposer.DefaultText = "";
            this.txbComposer.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txbComposer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.txbComposer.Location = new System.Drawing.Point(61, 4);
            this.txbComposer.MatchDisplayString = true;
            this.txbComposer.MatchMode = CustomForm.AutoCompleteTextBox.MatchModes.Contains;
            this.txbComposer.Name = "txbComposer";
            this.txbComposer.PopupBorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txbComposer.PopupOffset = new System.Drawing.Point(12, 0);
            this.txbComposer.PopupSelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.txbComposer.PopupSelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.txbComposer.PopupWidth = 300;
            this.txbComposer.Size = new System.Drawing.Size(186, 14);
            this.txbComposer.TabIndex = 1;
            this.txbComposer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txb_KeyDown);
            // 
            // tabPageTray
            // 
            this.tabPageTray.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageTray.Location = new System.Drawing.Point(4, 22);
            this.tabPageTray.Name = "tabPageTray";
            this.tabPageTray.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTray.Size = new System.Drawing.Size(248, 39);
            this.tabPageTray.TabIndex = 0;
            this.tabPageTray.Text = "tabPage4";
            // 
            // txbFilter
            // 
            this.txbFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txbFilter.BackColor = System.Drawing.SystemColors.Window;
            this.txbFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txbFilter.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txbFilter.Location = new System.Drawing.Point(41, 0);
            this.txbFilter.Name = "txbFilter";
            this.txbFilter.Size = new System.Drawing.Size(213, 20);
            this.txbFilter.TabIndex = 1;
            this.txbFilter.Text = "(none)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "&Filter:";
            // 
            // dg
            // 
            this.dg.AllowUserToDeleteRows = false;
            this.dg.AllowUserToResizeColumns = false;
            this.dg.AllowUserToResizeRows = false;
            this.dg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dg.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dg.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dg.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg.ColumnHeadersVisible = false;
            this.dg.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.PieceName,
            this.ParentID});
            this.dg.Location = new System.Drawing.Point(0, 26);
            this.dg.Name = "dg";
            this.dg.RowHeadersVisible = false;
            this.dg.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dg.RowTemplate.Height = 16;
            this.dg.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dg.Size = new System.Drawing.Size(256, 159);
            this.dg.StandardTab = true;
            this.dg.TabIndex = 2;
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
            this.PieceName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PieceName.HeaderText = "Name";
            this.PieceName.Name = "PieceName";
            // 
            // ParentID
            // 
            this.ParentID.HeaderText = "ParentID";
            this.ParentID.Name = "ParentID";
            this.ParentID.Visible = false;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageDetail);
            this.tabControl.Controls.Add(this.tabPageDetailEdit);
            this.tabControl.Controls.Add(this.tabPageProperties);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(256, 174);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageDetail
            // 
            this.tabPageDetail.Controls.Add(this.clbDetail);
            this.tabPageDetail.Location = new System.Drawing.Point(4, 22);
            this.tabPageDetail.Name = "tabPageDetail";
            this.tabPageDetail.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDetail.Size = new System.Drawing.Size(248, 148);
            this.tabPageDetail.TabIndex = 0;
            this.tabPageDetail.Text = "Movements";
            this.tabPageDetail.UseVisualStyleBackColor = true;
            // 
            // clbDetail
            // 
            this.clbDetail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.clbDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbDetail.FormattingEnabled = true;
            this.clbDetail.Items.AddRange(new object[] {
            "a",
            "b",
            "d",
            "d",
            "c",
            "c",
            "e",
            "e"});
            this.clbDetail.Location = new System.Drawing.Point(3, 3);
            this.clbDetail.Name = "clbDetail";
            this.clbDetail.Size = new System.Drawing.Size(242, 135);
            this.clbDetail.TabIndex = 0;
            // 
            // tabPageDetailEdit
            // 
            this.tabPageDetailEdit.Controls.Add(this.txbEdit);
            this.tabPageDetailEdit.Location = new System.Drawing.Point(4, 22);
            this.tabPageDetailEdit.Name = "tabPageDetailEdit";
            this.tabPageDetailEdit.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDetailEdit.Size = new System.Drawing.Size(248, 148);
            this.tabPageDetailEdit.TabIndex = 1;
            this.tabPageDetailEdit.Text = "Edit";
            this.tabPageDetailEdit.UseVisualStyleBackColor = true;
            // 
            // txbEdit
            // 
            this.txbEdit.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txbEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txbEdit.Location = new System.Drawing.Point(3, 3);
            this.txbEdit.Multiline = true;
            this.txbEdit.Name = "txbEdit";
            this.txbEdit.Size = new System.Drawing.Size(242, 142);
            this.txbEdit.TabIndex = 3;
            // 
            // tabPageProperties
            // 
            this.tabPageProperties.Controls.Add(this.pgrid);
            this.tabPageProperties.Location = new System.Drawing.Point(4, 22);
            this.tabPageProperties.Name = "tabPageProperties";
            this.tabPageProperties.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProperties.Size = new System.Drawing.Size(248, 148);
            this.tabPageProperties.TabIndex = 2;
            this.tabPageProperties.Text = "Properties";
            this.tabPageProperties.UseVisualStyleBackColor = true;
            // 
            // pgrid
            // 
            this.pgrid.CommandsVisibleIfAvailable = false;
            this.pgrid.HelpVisible = false;
            this.pgrid.LineColor = System.Drawing.SystemColors.Control;
            this.pgrid.Location = new System.Drawing.Point(-4, -2);
            this.pgrid.Margin = new System.Windows.Forms.Padding(0);
            this.pgrid.Name = "pgrid";
            this.pgrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.pgrid.Size = new System.Drawing.Size(256, 154);
            this.pgrid.TabIndex = 0;
            this.pgrid.ToolbarVisible = false;
            // 
            // Browse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "Browse";
            this.Size = new System.Drawing.Size(256, 402);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            this.splitContainer2.ResumeLayout(false);
            this.tabControlMode.ResumeLayout(false);
            this.tabPageSearch.ResumeLayout(false);
            this.tabPageSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabPageDetail.ResumeLayout(false);
            this.tabPageDetailEdit.ResumeLayout(false);
            this.tabPageDetailEdit.PerformLayout();
            this.tabPageProperties.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageDetail;
        private System.Windows.Forms.TabPage tabPageDetailEdit;
        private System.Windows.Forms.DataGridView dg;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txbFilter;
        private CustomForm.AutoCompleteTextBox txbComposer;
        private CustomForm.AutoCompleteTextBox txbGenre;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn PieceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParentID;
        private System.Windows.Forms.TabPage tabPageProperties;
        private System.Windows.Forms.CheckedListBox clbDetail;
        private System.Windows.Forms.TextBox txbEdit;
        private System.Windows.Forms.TabControl tabControlMode;
        private System.Windows.Forms.TabPage tabPageTray;
        private System.Windows.Forms.TabPage tabPageSearch;
        private System.Windows.Forms.PropertyGrid pgrid;
    }
}
