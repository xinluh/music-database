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
            this.txbGenre = new CustomForm.AutoCompleteTextBox();
            this.txbComposer = new CustomForm.AutoCompleteTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txbFilter = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dg = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PieceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParentID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.clbDetail = new System.Windows.Forms.CheckedListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txbEdit = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
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
            this.splitContainer2.Panel1.Controls.Add(this.txbGenre);
            this.splitContainer2.Panel1.Controls.Add(this.txbComposer);
            this.splitContainer2.Panel1.Controls.Add(this.label2);
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.txbFilter);
            this.splitContainer2.Panel2.Controls.Add(this.label3);
            this.splitContainer2.Panel2.Controls.Add(this.dg);
            this.splitContainer2.Size = new System.Drawing.Size(256, 222);
            this.splitContainer2.SplitterDistance = 41;
            this.splitContainer2.SplitterWidth = 1;
            this.splitContainer2.TabIndex = 0;
            this.splitContainer2.TabStop = false;
            // 
            // txbGenre
            // 
            this.txbGenre.BackColor = System.Drawing.SystemColors.Control;
            this.txbGenre.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txbGenre.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txbGenre.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.txbGenre.Location = new System.Drawing.Point(66, 20);
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
            // txbComposer
            // 
            this.txbComposer.BackColor = System.Drawing.SystemColors.Control;
            this.txbComposer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txbComposer.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txbComposer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.txbComposer.Location = new System.Drawing.Point(66, 3);
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "&Genre(s):";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Composer:";
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
            this.txbFilter.Size = new System.Drawing.Size(212, 20);
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
            this.label3.Text = "Filter:";
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
            this.dg.RowTemplate.Height = 16;
            this.dg.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dg.Size = new System.Drawing.Size(256, 154);
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
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tabPage3);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(256, 174);
            this.tabControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.clbDetail);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(248, 148);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Movements";
            this.tabPage1.UseVisualStyleBackColor = true;
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
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txbEdit);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(248, 148);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Edit";
            this.tabPage2.UseVisualStyleBackColor = true;
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
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(248, 148);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Properties";
            this.tabPage3.UseVisualStyleBackColor = true;
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
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dg)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dg;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txbFilter;
        private CustomForm.AutoCompleteTextBox txbComposer;
        private CustomForm.AutoCompleteTextBox txbGenre;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn PieceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParentID;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.CheckedListBox clbDetail;
        private System.Windows.Forms.TextBox txbEdit;
    }
}
