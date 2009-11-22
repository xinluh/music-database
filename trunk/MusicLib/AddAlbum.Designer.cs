namespace MusicLib
{
    partial class AddAlbum
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txbPath = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbAlbumType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txbTotalTracks = new System.Windows.Forms.TextBox();
            this.nudTotalDiscs = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txbAlbumName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnAddSelected = new System.Windows.Forms.Button();
            this.btnChoosePiece = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.dg = new MusicLib.TrackGridView();
            this.browseControl = new MusicLib.Browse();
            this.txbLabel = new MusicLib.AutoSearchCompleteTextBox();
            this.txbAlbumArtist = new MusicLib.AutoSearchCompleteTextBox();
            this.btnAddAlbum = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTotalDiscs)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg)).BeginInit();
            this.SuspendLayout();
            // 
            // txbPath
            // 
            this.txbPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txbPath.Location = new System.Drawing.Point(12, 12);
            this.txbPath.Name = "txbPath";
            this.txbPath.ReadOnly = true;
            this.txbPath.Size = new System.Drawing.Size(528, 20);
            this.txbPath.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnAddAlbum);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.cmbAlbumType);
            this.groupBox1.Controls.Add(this.txbLabel);
            this.groupBox1.Controls.Add(this.txbAlbumArtist);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txbTotalTracks);
            this.groupBox1.Controls.Add(this.nudTotalDiscs);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txbAlbumName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 38);
            this.groupBox1.MaximumSize = new System.Drawing.Size(800, 2000);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(677, 113);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // cmbAlbumType
            // 
            this.cmbAlbumType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAlbumType.FormattingEnabled = true;
            this.cmbAlbumType.Location = new System.Drawing.Point(286, 43);
            this.cmbAlbumType.Name = "cmbAlbumType";
            this.cmbAlbumType.Size = new System.Drawing.Size(101, 21);
            this.cmbAlbumType.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(217, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Album T&ype";
            // 
            // txbTotalTracks
            // 
            this.txbTotalTracks.Location = new System.Drawing.Point(192, 70);
            this.txbTotalTracks.Name = "txbTotalTracks";
            this.txbTotalTracks.Size = new System.Drawing.Size(195, 20);
            this.txbTotalTracks.TabIndex = 11;
            // 
            // nudTotalDiscs
            // 
            this.nudTotalDiscs.Location = new System.Drawing.Point(75, 71);
            this.nudTotalDiscs.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudTotalDiscs.Name = "nudTotalDiscs";
            this.nudTotalDiscs.Size = new System.Drawing.Size(41, 20);
            this.nudTotalDiscs.TabIndex = 9;
            this.nudTotalDiscs.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(122, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Total &Tracks";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Total &Disc";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Rec. &Label";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(397, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Album Artist";
            // 
            // txbAlbumName
            // 
            this.txbAlbumName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txbAlbumName.Location = new System.Drawing.Point(75, 16);
            this.txbAlbumName.Name = "txbAlbumName";
            this.txbAlbumName.Size = new System.Drawing.Size(312, 20);
            this.txbAlbumName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Album &Name";
            // 
            // btnOpen
            // 
            this.btnOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpen.Location = new System.Drawing.Point(546, 8);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(70, 26);
            this.btnOpen.TabIndex = 1;
            this.btnOpen.Text = "&Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(622, 8);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(67, 26);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "&Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.browseControl);
            this.groupBox2.Controls.Add(this.btnAddSelected);
            this.groupBox2.Controls.Add(this.btnChoosePiece);
            this.groupBox2.Location = new System.Drawing.Point(12, 150);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(295, 478);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            // 
            // btnAddSelected
            // 
            this.btnAddSelected.Location = new System.Drawing.Point(125, 15);
            this.btnAddSelected.Name = "btnAddSelected";
            this.btnAddSelected.Size = new System.Drawing.Size(106, 26);
            this.btnAddSelected.TabIndex = 1;
            this.btnAddSelected.Text = "&Add selected";
            this.btnAddSelected.UseVisualStyleBackColor = true;
            // 
            // btnChoosePiece
            // 
            this.btnChoosePiece.Location = new System.Drawing.Point(10, 15);
            this.btnChoosePiece.Name = "btnChoosePiece";
            this.btnChoosePiece.Size = new System.Drawing.Size(106, 26);
            this.btnChoosePiece.TabIndex = 0;
            this.btnChoosePiece.Text = "&Choose new piece";
            this.btnChoosePiece.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(435, 65);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dg
            // 
            this.dg.AllowUserToAddRows = false;
            this.dg.AllowUserToDeleteRows = false;
            this.dg.AllowUserToResizeRows = false;
            this.dg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dg.BackgroundColor = System.Drawing.Color.White;
            this.dg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dg.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg.Location = new System.Drawing.Point(313, 157);
            this.dg.Name = "dg";
            this.dg.RowHeadersVisible = false;
            this.dg.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dg.Size = new System.Drawing.Size(376, 471);
            this.dg.TabIndex = 5;
            // 
            // browseControl
            // 
            this.browseControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.browseControl.DetailPaneVisible = true;
            this.browseControl.Location = new System.Drawing.Point(6, 47);
            this.browseControl.Name = "browseControl";
            this.browseControl.Size = new System.Drawing.Size(283, 425);
            this.browseControl.TabIndex = 2;
            // 
            // txbLabel
            // 
            this.txbLabel.AllowAddingOnEmptyString = false;
            this.txbLabel.DefaultText = "";
            this.txbLabel.EnforceStrictSelection = false;
            this.txbLabel.Location = new System.Drawing.Point(75, 43);
            this.txbLabel.MatchDisplayString = true;
            this.txbLabel.MatchMode = CustomForm.AutoCompleteTextBox.MatchModes.Contains;
            this.txbLabel.Name = "txbLabel";
            this.txbLabel.PopupBorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txbLabel.PopupOffset = new System.Drawing.Point(12, 0);
            this.txbLabel.PopupSelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.txbLabel.PopupSelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.txbLabel.PopupWidth = 100;
            this.txbLabel.Size = new System.Drawing.Size(136, 20);
            this.txbLabel.TabIndex = 5;
            // 
            // txbAlbumArtist
            // 
            this.txbAlbumArtist.AllowAddingOnEmptyString = false;
            this.txbAlbumArtist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txbAlbumArtist.DefaultText = "";
            this.txbAlbumArtist.EnforceStrictSelection = true;
            this.txbAlbumArtist.Location = new System.Drawing.Point(465, 17);
            this.txbAlbumArtist.MatchDisplayString = true;
            this.txbAlbumArtist.MatchMode = CustomForm.AutoCompleteTextBox.MatchModes.Contains;
            this.txbAlbumArtist.Name = "txbAlbumArtist";
            this.txbAlbumArtist.PopupBorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txbAlbumArtist.PopupOffset = new System.Drawing.Point(12, 0);
            this.txbAlbumArtist.PopupSelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.txbAlbumArtist.PopupSelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.txbAlbumArtist.PopupWidth = 100;
            this.txbAlbumArtist.Size = new System.Drawing.Size(206, 20);
            this.txbAlbumArtist.TabIndex = 3;
            // 
            // btnAddAlbum
            // 
            this.btnAddAlbum.Location = new System.Drawing.Point(529, 63);
            this.btnAddAlbum.Name = "btnAddAlbum";
            this.btnAddAlbum.Size = new System.Drawing.Size(75, 23);
            this.btnAddAlbum.TabIndex = 14;
            this.btnAddAlbum.Text = "Add Album!";
            this.btnAddAlbum.UseVisualStyleBackColor = true;
            this.btnAddAlbum.Click += new System.EventHandler(this.btnAddAlbum_Click);
            // 
            // AddAlbum
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 637);
            this.Controls.Add(this.dg);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txbPath);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.btnOpen);
            this.MinimumSize = new System.Drawing.Size(700, 529);
            this.Name = "AddAlbum";
            this.Text = "AddAlbum";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTotalDiscs)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txbPath;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txbAlbumName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txbTotalTracks;
        private System.Windows.Forms.NumericUpDown nudTotalDiscs;
        private Browse browse1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnChoosePiece;
        private System.Windows.Forms.Button btnAddSelected;
        private TrackGridView dg;
        private AutoSearchCompleteTextBox txbAlbumArtist;
        private AutoSearchCompleteTextBox txbLabel;
        private Browse browseControl;
        private System.Windows.Forms.ComboBox cmbAlbumType;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnAddAlbum;

    }
}