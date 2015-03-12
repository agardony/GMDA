namespace MapDrawingAnalyzer
{
    partial class advancedCoordsFileBuilder
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(advancedCoordsFileBuilder));
            this.dgv_coords = new System.Windows.Forms.DataGridView();
            this.landmarkName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.landmarkNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip_dgvCoords = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cut = new System.Windows.Forms.ToolStripMenuItem();
            this.copy = new System.Windows.Forms.ToolStripMenuItem();
            this.paste = new System.Windows.Forms.ToolStripMenuItem();
            this.tb_numLandmarks = new System.Windows.Forms.TextBox();
            this.tb_numLandmarksInstructions = new System.Windows.Forms.TextBox();
            this.b_step2 = new System.Windows.Forms.Button();
            this.b_reset = new System.Windows.Forms.Button();
            this.b_goback = new System.Windows.Forms.Button();
            this.lb_advancedMode = new System.Windows.Forms.Label();
            this.lb_basicMode = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.b_enter = new System.Windows.Forms.Button();
            this.pb_goBack = new System.Windows.Forms.PictureBox();
            this.pb_reset = new System.Windows.Forms.PictureBox();
            this.pb_nextStep = new System.Windows.Forms.PictureBox();
            this.picturebox_map = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_coords)).BeginInit();
            this.contextMenuStrip_dgvCoords.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_goBack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_reset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_nextStep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturebox_map)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_coords
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_coords.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_coords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_coords.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.landmarkName,
            this.landmarkNum});
            this.dgv_coords.ContextMenuStrip = this.contextMenuStrip_dgvCoords;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_coords.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_coords.Location = new System.Drawing.Point(5, 24);
            this.dgv_coords.Name = "dgv_coords";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_coords.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgv_coords.Size = new System.Drawing.Size(700, 700);
            this.dgv_coords.TabIndex = 0;
            this.dgv_coords.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_coords_CellValueChanged);
            this.dgv_coords.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgv_coords_KeyDown);
            // 
            // landmarkName
            // 
            this.landmarkName.HeaderText = "landmarkName";
            this.landmarkName.Name = "landmarkName";
            // 
            // landmarkNum
            // 
            this.landmarkNum.HeaderText = "landmarkNum";
            this.landmarkNum.Name = "landmarkNum";
            this.landmarkNum.ReadOnly = true;
            // 
            // contextMenuStrip_dgvCoords
            // 
            this.contextMenuStrip_dgvCoords.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cut,
            this.copy,
            this.paste});
            this.contextMenuStrip_dgvCoords.Name = "contextMenuStrip_dgvCoords";
            this.contextMenuStrip_dgvCoords.Size = new System.Drawing.Size(160, 92);
            // 
            // cut
            // 
            this.cut.Name = "cut";
            this.cut.ShortcutKeyDisplayString = "CTRL + X";
            this.cut.Size = new System.Drawing.Size(159, 22);
            this.cut.Text = "Cut";
            this.cut.Click += new System.EventHandler(this.cut_Click);
            // 
            // copy
            // 
            this.copy.Name = "copy";
            this.copy.ShortcutKeyDisplayString = "CTRL + C";
            this.copy.Size = new System.Drawing.Size(159, 22);
            this.copy.Text = "Copy";
            this.copy.Click += new System.EventHandler(this.copy_Click);
            // 
            // paste
            // 
            this.paste.Name = "paste";
            this.paste.ShortcutKeyDisplayString = "CTRL + V";
            this.paste.Size = new System.Drawing.Size(159, 22);
            this.paste.Text = "Paste";
            this.paste.Click += new System.EventHandler(this.paste_Click);
            // 
            // tb_numLandmarks
            // 
            this.tb_numLandmarks.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_numLandmarks.Location = new System.Drawing.Point(767, 120);
            this.tb_numLandmarks.Name = "tb_numLandmarks";
            this.tb_numLandmarks.Size = new System.Drawing.Size(60, 44);
            this.tb_numLandmarks.TabIndex = 1;
            this.tb_numLandmarks.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_numLandmarks.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numLandmarks_KeyDown);
            // 
            // tb_numLandmarksInstructions
            // 
            this.tb_numLandmarksInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_numLandmarksInstructions.Location = new System.Drawing.Point(716, 24);
            this.tb_numLandmarksInstructions.Multiline = true;
            this.tb_numLandmarksInstructions.Name = "tb_numLandmarksInstructions";
            this.tb_numLandmarksInstructions.ReadOnly = true;
            this.tb_numLandmarksInstructions.Size = new System.Drawing.Size(156, 90);
            this.tb_numLandmarksInstructions.TabIndex = 2;
            this.tb_numLandmarksInstructions.TabStop = false;
            this.tb_numLandmarksInstructions.Text = "Enter # of landmarks in environment";
            this.tb_numLandmarksInstructions.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // b_step2
            // 
            this.b_step2.Enabled = false;
            this.b_step2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_step2.Location = new System.Drawing.Point(759, 221);
            this.b_step2.Name = "b_step2";
            this.b_step2.Size = new System.Drawing.Size(75, 50);
            this.b_step2.TabIndex = 3;
            this.b_step2.Text = "Next Step";
            this.b_step2.UseVisualStyleBackColor = true;
            this.b_step2.Click += new System.EventHandler(this.b_step2_Click);
            // 
            // b_reset
            // 
            this.b_reset.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_reset.Location = new System.Drawing.Point(759, 277);
            this.b_reset.Name = "b_reset";
            this.b_reset.Size = new System.Drawing.Size(75, 50);
            this.b_reset.TabIndex = 4;
            this.b_reset.Text = "Start Over";
            this.b_reset.UseVisualStyleBackColor = true;
            this.b_reset.Click += new System.EventHandler(this.b_reset_Click);
            // 
            // b_goback
            // 
            this.b_goback.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_goback.Location = new System.Drawing.Point(759, 333);
            this.b_goback.Name = "b_goback";
            this.b_goback.Size = new System.Drawing.Size(75, 50);
            this.b_goback.TabIndex = 5;
            this.b_goback.Text = "Home Screen";
            this.b_goback.UseVisualStyleBackColor = true;
            this.b_goback.Click += new System.EventHandler(this.b_goback_Click);
            // 
            // lb_advancedMode
            // 
            this.lb_advancedMode.AutoSize = true;
            this.lb_advancedMode.BackColor = System.Drawing.Color.Yellow;
            this.lb_advancedMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_advancedMode.Location = new System.Drawing.Point(790, 395);
            this.lb_advancedMode.Name = "lb_advancedMode";
            this.lb_advancedMode.Size = new System.Drawing.Size(71, 17);
            this.lb_advancedMode.TabIndex = 39;
            this.lb_advancedMode.Text = "Advanced";
            // 
            // lb_basicMode
            // 
            this.lb_basicMode.AutoSize = true;
            this.lb_basicMode.BackColor = System.Drawing.SystemColors.Control;
            this.lb_basicMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_basicMode.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lb_basicMode.Location = new System.Drawing.Point(743, 395);
            this.lb_basicMode.Name = "lb_basicMode";
            this.lb_basicMode.Size = new System.Drawing.Size(42, 17);
            this.lb_basicMode.TabIndex = 38;
            this.lb_basicMode.Text = "Basic";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 730);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(884, 22);
            this.statusStrip1.TabIndex = 51;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(132, 17);
            this.toolStripStatusLabel.Text = "Coordinates File Builder";
            // 
            // b_enter
            // 
            this.b_enter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_enter.Location = new System.Drawing.Point(767, 170);
            this.b_enter.Name = "b_enter";
            this.b_enter.Size = new System.Drawing.Size(60, 29);
            this.b_enter.TabIndex = 2;
            this.b_enter.Text = "Enter";
            this.b_enter.UseVisualStyleBackColor = true;
            this.b_enter.Click += new System.EventHandler(this.b_enter_Click);
            // 
            // pb_goBack
            // 
            this.pb_goBack.Image = ((System.Drawing.Image)(resources.GetObject("pb_goBack.Image")));
            this.pb_goBack.Location = new System.Drawing.Point(716, 338);
            this.pb_goBack.Name = "pb_goBack";
            this.pb_goBack.Size = new System.Drawing.Size(40, 40);
            this.pb_goBack.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_goBack.TabIndex = 57;
            this.pb_goBack.TabStop = false;
            // 
            // pb_reset
            // 
            this.pb_reset.Image = ((System.Drawing.Image)(resources.GetObject("pb_reset.Image")));
            this.pb_reset.Location = new System.Drawing.Point(716, 282);
            this.pb_reset.Name = "pb_reset";
            this.pb_reset.Size = new System.Drawing.Size(40, 40);
            this.pb_reset.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_reset.TabIndex = 56;
            this.pb_reset.TabStop = false;
            // 
            // pb_nextStep
            // 
            this.pb_nextStep.Image = ((System.Drawing.Image)(resources.GetObject("pb_nextStep.Image")));
            this.pb_nextStep.Location = new System.Drawing.Point(716, 226);
            this.pb_nextStep.Name = "pb_nextStep";
            this.pb_nextStep.Size = new System.Drawing.Size(40, 40);
            this.pb_nextStep.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_nextStep.TabIndex = 55;
            this.pb_nextStep.TabStop = false;
            // 
            // picturebox_map
            // 
            this.picturebox_map.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picturebox_map.Location = new System.Drawing.Point(12, 108);
            this.picturebox_map.Name = "picturebox_map";
            this.picturebox_map.Size = new System.Drawing.Size(700, 700);
            this.picturebox_map.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picturebox_map.TabIndex = 4;
            this.picturebox_map.TabStop = false;
            this.picturebox_map.Tag = "homeScreen";
            this.picturebox_map.Paint += new System.Windows.Forms.PaintEventHandler(this.picturebox_map_Paint);
            this.picturebox_map.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picturebox_map_MouseMove);
            this.picturebox_map.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picturebox_map_MouseUp);
            // 
            // advancedCoordsFileBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 752);
            this.Controls.Add(this.pb_goBack);
            this.Controls.Add(this.pb_reset);
            this.Controls.Add(this.pb_nextStep);
            this.Controls.Add(this.b_enter);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.lb_advancedMode);
            this.Controls.Add(this.lb_basicMode);
            this.Controls.Add(this.picturebox_map);
            this.Controls.Add(this.b_goback);
            this.Controls.Add(this.b_reset);
            this.Controls.Add(this.b_step2);
            this.Controls.Add(this.tb_numLandmarksInstructions);
            this.Controls.Add(this.tb_numLandmarks);
            this.Controls.Add(this.dgv_coords);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.Name = "advancedCoordsFileBuilder";
            this.Text = "Advanced Coordinates File Builder";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.advancedCoordsFileBuilder_FormClosed);
            this.Shown += new System.EventHandler(this.advancedCoordsFileBuilder_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_coords)).EndInit();
            this.contextMenuStrip_dgvCoords.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_goBack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_reset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_nextStep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturebox_map)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_coords;
        private System.Windows.Forms.TextBox tb_numLandmarks;
        private System.Windows.Forms.TextBox tb_numLandmarksInstructions;
        private System.Windows.Forms.Button b_step2;
        private System.Windows.Forms.Button b_reset;
        private System.Windows.Forms.Button b_goback;
        private System.Windows.Forms.PictureBox picturebox_map;
        private System.Windows.Forms.DataGridViewTextBoxColumn landmarkName;
        private System.Windows.Forms.DataGridViewTextBoxColumn landmarkNum;
        private System.Windows.Forms.Label lb_advancedMode;
        private System.Windows.Forms.Label lb_basicMode;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.Button b_enter;
        private System.Windows.Forms.PictureBox pb_nextStep;
        private System.Windows.Forms.PictureBox pb_reset;
        private System.Windows.Forms.PictureBox pb_goBack;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_dgvCoords;
        private System.Windows.Forms.ToolStripMenuItem cut;
        private System.Windows.Forms.ToolStripMenuItem copy;
        private System.Windows.Forms.ToolStripMenuItem paste;
    }
}