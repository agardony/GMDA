namespace MapDrawingAnalyzer
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tb_legend = new System.Windows.Forms.TextBox();
            this.d_saveImage = new System.Windows.Forms.SaveFileDialog();
            this.textbox_numLandmarks = new System.Windows.Forms.TextBox();
            this.toolTip_button_landmarkNum = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip_textbox_mapNum = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip_missingLandmarks = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip_loadMapImageToolStripMenuItem = new System.Windows.Forms.ToolTip(this.components);
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.coordinatesFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.loadMapImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.saveConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveMapImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.batchReanalysisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreDefaultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bDROptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actualMapIVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actualMapDVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.movableZoomWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cb_coordsFiles = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.trackBar_zoomLevel = new System.Windows.Forms.TrackBar();
            this.lb_trackBar = new System.Windows.Forms.Label();
            this.lb_basicMode = new System.Windows.Forms.Label();
            this.lb_advancedMode = new System.Windows.Forms.Label();
            this.pb_calculate = new System.Windows.Forms.ProgressBar();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel_highlight_labels = new System.Windows.Forms.Panel();
            this.panel_highlight_MapID = new System.Windows.Forms.Panel();
            this.b_rotate_cw = new System.Windows.Forms.Button();
            this.b_rotate_ccw = new System.Windows.Forms.Button();
            this.pb_rectCenCol_white = new System.Windows.Forms.PictureBox();
            this.pb_rectCol_white = new System.Windows.Forms.PictureBox();
            this.pb_rectCenCol_yellow = new System.Windows.Forms.PictureBox();
            this.pb_rectCenCol_blue = new System.Windows.Forms.PictureBox();
            this.pb_rectCenCol_green = new System.Windows.Forms.PictureBox();
            this.pb_rectCenCol_red = new System.Windows.Forms.PictureBox();
            this.pb_rectCol_yellow = new System.Windows.Forms.PictureBox();
            this.pb_rectCol_blue = new System.Windows.Forms.PictureBox();
            this.pb_rectCol_green = new System.Windows.Forms.PictureBox();
            this.pb_rectCol_red = new System.Windows.Forms.PictureBox();
            this.picZoom = new System.Windows.Forms.PictureBox();
            this.button_preview = new System.Windows.Forms.Button();
            this.button_calculate = new System.Windows.Forms.Button();
            this.picturebox_missingLandmarks = new System.Windows.Forms.PictureBox();
            this.picturebox_map = new System.Windows.Forms.PictureBox();
            this.mainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_zoomLevel)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_rectCenCol_white)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_rectCol_white)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_rectCenCol_yellow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_rectCenCol_blue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_rectCenCol_green)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_rectCenCol_red)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_rectCol_yellow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_rectCol_blue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_rectCol_green)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_rectCol_red)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturebox_missingLandmarks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturebox_map)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // tb_legend
            // 
            this.tb_legend.BackColor = System.Drawing.Color.White;
            this.tb_legend.Enabled = false;
            this.tb_legend.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_legend.Location = new System.Drawing.Point(706, 431);
            this.tb_legend.Multiline = true;
            this.tb_legend.Name = "tb_legend";
            this.tb_legend.ReadOnly = true;
            this.tb_legend.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_legend.Size = new System.Drawing.Size(208, 354);
            this.tb_legend.TabIndex = 22;
            this.tb_legend.TabStop = false;
            this.tb_legend.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tb_legend_MouseClick);
            this.tb_legend.Enter += new System.EventHandler(this.tb_legend_Enter);
            this.tb_legend.Leave += new System.EventHandler(this.tb_legend_Leave);
            // 
            // d_saveImage
            // 
            this.d_saveImage.Filter = "\"JPG files (*.jpg)|*.jpg|JPEG files (*.jpeg)|*.jpeg|All files (*.*)|*.*\"";
            // 
            // textbox_numLandmarks
            // 
            this.textbox_numLandmarks.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textbox_numLandmarks.Enabled = false;
            this.textbox_numLandmarks.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.textbox_numLandmarks.Location = new System.Drawing.Point(611, 729);
            this.textbox_numLandmarks.Name = "textbox_numLandmarks";
            this.textbox_numLandmarks.ReadOnly = true;
            this.textbox_numLandmarks.Size = new System.Drawing.Size(54, 22);
            this.textbox_numLandmarks.TabIndex = 20;
            this.textbox_numLandmarks.TabStop = false;
            this.textbox_numLandmarks.Text = "48";
            this.textbox_numLandmarks.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolTip_button_landmarkNum
            // 
            this.toolTip_button_landmarkNum.AutoPopDelay = 20000;
            this.toolTip_button_landmarkNum.InitialDelay = 500;
            this.toolTip_button_landmarkNum.ReshowDelay = 100;
            // 
            // toolTip_textbox_mapNum
            // 
            this.toolTip_textbox_mapNum.AutoPopDelay = 20000;
            this.toolTip_textbox_mapNum.InitialDelay = 500;
            this.toolTip_textbox_mapNum.ReshowDelay = 100;
            // 
            // toolTip_missingLandmarks
            // 
            this.toolTip_missingLandmarks.AutoPopDelay = 20000;
            this.toolTip_missingLandmarks.InitialDelay = 500;
            this.toolTip_missingLandmarks.ReshowDelay = 100;
            this.toolTip_missingLandmarks.ToolTipTitle = "Missing Landmarks";
            // 
            // mainMenu
            // 
            this.mainMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.mainMenu.Size = new System.Drawing.Size(914, 24);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.coordinatesFileToolStripMenuItem,
            this.toolStripSeparator1,
            this.loadMapImageToolStripMenuItem,
            this.loadConfigurationToolStripMenuItem,
            this.toolStripSeparator2,
            this.saveConfigurationToolStripMenuItem,
            this.saveMapImageToolStripMenuItem,
            this.toolStripSeparator3,
            this.batchReanalysisToolStripMenuItem,
            this.resetToolStripMenuItem,
            this.restoreDefaultsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // coordinatesFileToolStripMenuItem
            // 
            this.coordinatesFileToolStripMenuItem.Name = "coordinatesFileToolStripMenuItem";
            this.coordinatesFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.coordinatesFileToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.coordinatesFileToolStripMenuItem.Text = "&New Coordinates File";
            this.coordinatesFileToolStripMenuItem.ToolTipText = "Build a coordinates file for the target environment";
            this.coordinatesFileToolStripMenuItem.Click += new System.EventHandler(this.coordinatesFileToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(226, 6);
            // 
            // loadMapImageToolStripMenuItem
            // 
            this.loadMapImageToolStripMenuItem.Name = "loadMapImageToolStripMenuItem";
            this.loadMapImageToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.loadMapImageToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.loadMapImageToolStripMenuItem.Tag = "";
            this.loadMapImageToolStripMenuItem.Text = "&Open Map Image";
            this.loadMapImageToolStripMenuItem.ToolTipText = "Note: Loaded image must be square i.e. the height and width must be identical";
            this.loadMapImageToolStripMenuItem.Click += new System.EventHandler(this.loadMapImageToolStripMenuItem_Click);
            // 
            // loadConfigurationToolStripMenuItem
            // 
            this.loadConfigurationToolStripMenuItem.Name = "loadConfigurationToolStripMenuItem";
            this.loadConfigurationToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.loadConfigurationToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.loadConfigurationToolStripMenuItem.Text = "&Load Configuration";
            this.loadConfigurationToolStripMenuItem.Click += new System.EventHandler(this.loadConfigurationToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(226, 6);
            // 
            // saveConfigurationToolStripMenuItem
            // 
            this.saveConfigurationToolStripMenuItem.Enabled = false;
            this.saveConfigurationToolStripMenuItem.Name = "saveConfigurationToolStripMenuItem";
            this.saveConfigurationToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveConfigurationToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.saveConfigurationToolStripMenuItem.Text = "&Save Configuration";
            this.saveConfigurationToolStripMenuItem.ToolTipText = "Save the current arrangement of landmark labels to a configuration file";
            this.saveConfigurationToolStripMenuItem.Click += new System.EventHandler(this.saveConfigurationToolStripMenuItem_Click);
            // 
            // saveMapImageToolStripMenuItem
            // 
            this.saveMapImageToolStripMenuItem.Name = "saveMapImageToolStripMenuItem";
            this.saveMapImageToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.saveMapImageToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.saveMapImageToolStripMenuItem.Text = "&Take Screenshot";
            this.saveMapImageToolStripMenuItem.ToolTipText = "Save an image of the applications current state";
            this.saveMapImageToolStripMenuItem.Click += new System.EventHandler(this.saveMapImageToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(226, 6);
            // 
            // batchReanalysisToolStripMenuItem
            // 
            this.batchReanalysisToolStripMenuItem.Enabled = false;
            this.batchReanalysisToolStripMenuItem.Name = "batchReanalysisToolStripMenuItem";
            this.batchReanalysisToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.batchReanalysisToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.batchReanalysisToolStripMenuItem.Text = "&Batch Reanalysis";
            this.batchReanalysisToolStripMenuItem.Click += new System.EventHandler(this.batchReanalysisToolStripMenuItem_Click);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.resetToolStripMenuItem.Text = "Sta&rt Over";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // restoreDefaultsToolStripMenuItem
            // 
            this.restoreDefaultsToolStripMenuItem.Name = "restoreDefaultsToolStripMenuItem";
            this.restoreDefaultsToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.restoreDefaultsToolStripMenuItem.Text = "Restore &Defaults";
            this.restoreDefaultsToolStripMenuItem.Click += new System.EventHandler(this.restoreDefaultsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bDROptionsToolStripMenuItem,
            this.movableZoomWindowToolStripMenuItem,
            this.checkForUpdatesToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // bDROptionsToolStripMenuItem
            // 
            this.bDROptionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.actualMapIVToolStripMenuItem,
            this.actualMapDVToolStripMenuItem});
            this.bDROptionsToolStripMenuItem.Name = "bDROptionsToolStripMenuItem";
            this.bDROptionsToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.bDROptionsToolStripMenuItem.Text = "BDR Options";
            // 
            // actualMapIVToolStripMenuItem
            // 
            this.actualMapIVToolStripMenuItem.Checked = true;
            this.actualMapIVToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.actualMapIVToolStripMenuItem.Name = "actualMapIVToolStripMenuItem";
            this.actualMapIVToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.actualMapIVToolStripMenuItem.Text = "Sketch Map = DV";
            this.actualMapIVToolStripMenuItem.Click += new System.EventHandler(this.actualMapIVToolStripMenuItem_Click);
            // 
            // actualMapDVToolStripMenuItem
            // 
            this.actualMapDVToolStripMenuItem.Name = "actualMapDVToolStripMenuItem";
            this.actualMapDVToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.actualMapDVToolStripMenuItem.Text = "Sketch Map = IV";
            this.actualMapDVToolStripMenuItem.Click += new System.EventHandler(this.actualMapDVToolStripMenuItem_Click);
            // 
            // movableZoomWindowToolStripMenuItem
            // 
            this.movableZoomWindowToolStripMenuItem.Checked = true;
            this.movableZoomWindowToolStripMenuItem.CheckOnClick = true;
            this.movableZoomWindowToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.movableZoomWindowToolStripMenuItem.Name = "movableZoomWindowToolStripMenuItem";
            this.movableZoomWindowToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.movableZoomWindowToolStripMenuItem.Text = "Movable Zoom Window";
            this.movableZoomWindowToolStripMenuItem.Click += new System.EventHandler(this.movableZoomWindowToolStripMenuItem_Click);
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.checkForUpdatesToolStripMenuItem.Text = "Check for Updates";
            this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
            // 
            // cb_coordsFiles
            // 
            this.cb_coordsFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_coordsFiles.FormattingEnabled = true;
            this.cb_coordsFiles.Location = new System.Drawing.Point(323, 757);
            this.cb_coordsFiles.Name = "cb_coordsFiles";
            this.cb_coordsFiles.Size = new System.Drawing.Size(137, 28);
            this.cb_coordsFiles.TabIndex = 31;
            this.cb_coordsFiles.TabStop = false;
            this.cb_coordsFiles.SelectionChangeCommitted += new System.EventHandler(this.cb_coordsFiles_SelectionChangeCommitted);
            this.cb_coordsFiles.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckKeys);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(476, 729);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(129, 20);
            this.textBox1.TabIndex = 32;
            this.textBox1.TabStop = false;
            this.textBox1.Text = "# of landmarks";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.Control;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Enabled = false;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(311, 729);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(159, 49);
            this.textBox2.TabIndex = 32;
            this.textBox2.TabStop = false;
            this.textBox2.Text = "Map ID";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // trackBar_zoomLevel
            // 
            this.trackBar_zoomLevel.LargeChange = 1;
            this.trackBar_zoomLevel.Location = new System.Drawing.Point(792, 353);
            this.trackBar_zoomLevel.Maximum = 6;
            this.trackBar_zoomLevel.Minimum = 2;
            this.trackBar_zoomLevel.Name = "trackBar_zoomLevel";
            this.trackBar_zoomLevel.Size = new System.Drawing.Size(120, 45);
            this.trackBar_zoomLevel.TabIndex = 34;
            this.trackBar_zoomLevel.TabStop = false;
            this.trackBar_zoomLevel.Value = 2;
            this.trackBar_zoomLevel.ValueChanged += new System.EventHandler(this.trackBar_zoomLevel_ValueChanged);
            // 
            // lb_trackBar
            // 
            this.lb_trackBar.AutoSize = true;
            this.lb_trackBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_trackBar.Location = new System.Drawing.Point(803, 385);
            this.lb_trackBar.Name = "lb_trackBar";
            this.lb_trackBar.Size = new System.Drawing.Size(106, 17);
            this.lb_trackBar.TabIndex = 35;
            this.lb_trackBar.Text = "2x      4x        6x";
            // 
            // lb_basicMode
            // 
            this.lb_basicMode.AutoSize = true;
            this.lb_basicMode.BackColor = System.Drawing.SystemColors.Control;
            this.lb_basicMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_basicMode.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lb_basicMode.Location = new System.Drawing.Point(794, 411);
            this.lb_basicMode.Name = "lb_basicMode";
            this.lb_basicMode.Size = new System.Drawing.Size(42, 17);
            this.lb_basicMode.TabIndex = 36;
            this.lb_basicMode.Text = "Basic";
            this.lb_basicMode.Click += new System.EventHandler(this.lb_basicMode_Click);
            // 
            // lb_advancedMode
            // 
            this.lb_advancedMode.AutoSize = true;
            this.lb_advancedMode.BackColor = System.Drawing.SystemColors.Control;
            this.lb_advancedMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_advancedMode.Location = new System.Drawing.Point(841, 411);
            this.lb_advancedMode.Name = "lb_advancedMode";
            this.lb_advancedMode.Size = new System.Drawing.Size(71, 17);
            this.lb_advancedMode.TabIndex = 37;
            this.lb_advancedMode.Text = "Advanced";
            this.lb_advancedMode.Click += new System.EventHandler(this.lb_advanced_Click);
            // 
            // pb_calculate
            // 
            this.pb_calculate.Location = new System.Drawing.Point(478, 760);
            this.pb_calculate.Name = "pb_calculate";
            this.pb_calculate.Size = new System.Drawing.Size(171, 18);
            this.pb_calculate.Step = 1;
            this.pb_calculate.TabIndex = 38;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 786);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(914, 22);
            this.statusStrip1.TabIndex = 49;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(78, 17);
            this.toolStripStatusLabel.Text = "Home Screen";
            // 
            // panel_highlight_labels
            // 
            this.panel_highlight_labels.Location = new System.Drawing.Point(707, 29);
            this.panel_highlight_labels.Name = "panel_highlight_labels";
            this.panel_highlight_labels.Size = new System.Drawing.Size(80, 399);
            this.panel_highlight_labels.TabIndex = 51;
            this.panel_highlight_labels.Visible = false;
            // 
            // panel_highlight_MapID
            // 
            this.panel_highlight_MapID.Location = new System.Drawing.Point(311, 729);
            this.panel_highlight_MapID.Name = "panel_highlight_MapID";
            this.panel_highlight_MapID.Size = new System.Drawing.Size(159, 56);
            this.panel_highlight_MapID.TabIndex = 52;
            this.panel_highlight_MapID.Visible = false;
            // 
            // b_rotate_cw
            // 
            this.b_rotate_cw.Image = global::MapDrawingAnalyzer.Properties.Resources.rotate_cw;
            this.b_rotate_cw.Location = new System.Drawing.Point(853, 30);
            this.b_rotate_cw.Name = "b_rotate_cw";
            this.b_rotate_cw.Size = new System.Drawing.Size(50, 50);
            this.b_rotate_cw.TabIndex = 54;
            this.b_rotate_cw.TabStop = false;
            this.b_rotate_cw.UseVisualStyleBackColor = true;
            this.b_rotate_cw.Click += new System.EventHandler(this.b_rotate_cw_Click);
            // 
            // b_rotate_ccw
            // 
            this.b_rotate_ccw.Image = global::MapDrawingAnalyzer.Properties.Resources.rotate_ccw;
            this.b_rotate_ccw.Location = new System.Drawing.Point(793, 30);
            this.b_rotate_ccw.Name = "b_rotate_ccw";
            this.b_rotate_ccw.Size = new System.Drawing.Size(50, 50);
            this.b_rotate_ccw.TabIndex = 53;
            this.b_rotate_ccw.TabStop = false;
            this.b_rotate_ccw.UseVisualStyleBackColor = true;
            this.b_rotate_ccw.Click += new System.EventHandler(this.b_rotate_ccw_Click);
            // 
            // pb_rectCenCol_white
            // 
            this.pb_rectCenCol_white.BackColor = System.Drawing.Color.Black;
            this.pb_rectCenCol_white.Image = ((System.Drawing.Image)(resources.GetObject("pb_rectCenCol_white.Image")));
            this.pb_rectCenCol_white.Location = new System.Drawing.Point(737, 133);
            this.pb_rectCenCol_white.Name = "pb_rectCenCol_white";
            this.pb_rectCenCol_white.Size = new System.Drawing.Size(25, 25);
            this.pb_rectCenCol_white.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_rectCenCol_white.TabIndex = 48;
            this.pb_rectCenCol_white.TabStop = false;
            this.pb_rectCenCol_white.Tag = "4";
            this.pb_rectCenCol_white.Visible = false;
            this.pb_rectCenCol_white.Click += new System.EventHandler(this.pb_rectCenCol_Click);
            // 
            // pb_rectCol_white
            // 
            this.pb_rectCol_white.BackColor = System.Drawing.Color.Black;
            this.pb_rectCol_white.Image = ((System.Drawing.Image)(resources.GetObject("pb_rectCol_white.Image")));
            this.pb_rectCol_white.Location = new System.Drawing.Point(711, 133);
            this.pb_rectCol_white.Name = "pb_rectCol_white";
            this.pb_rectCol_white.Size = new System.Drawing.Size(25, 25);
            this.pb_rectCol_white.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_rectCol_white.TabIndex = 47;
            this.pb_rectCol_white.TabStop = false;
            this.pb_rectCol_white.Tag = "4";
            this.pb_rectCol_white.Visible = false;
            this.pb_rectCol_white.Click += new System.EventHandler(this.pb_rectCol_Click);
            // 
            // pb_rectCenCol_yellow
            // 
            this.pb_rectCenCol_yellow.BackColor = System.Drawing.Color.Black;
            this.pb_rectCenCol_yellow.Image = ((System.Drawing.Image)(resources.GetObject("pb_rectCenCol_yellow.Image")));
            this.pb_rectCenCol_yellow.Location = new System.Drawing.Point(737, 107);
            this.pb_rectCenCol_yellow.Name = "pb_rectCenCol_yellow";
            this.pb_rectCenCol_yellow.Size = new System.Drawing.Size(25, 25);
            this.pb_rectCenCol_yellow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_rectCenCol_yellow.TabIndex = 46;
            this.pb_rectCenCol_yellow.TabStop = false;
            this.pb_rectCenCol_yellow.Tag = "3";
            this.pb_rectCenCol_yellow.Visible = false;
            this.pb_rectCenCol_yellow.Click += new System.EventHandler(this.pb_rectCenCol_Click);
            // 
            // pb_rectCenCol_blue
            // 
            this.pb_rectCenCol_blue.BackColor = System.Drawing.Color.Black;
            this.pb_rectCenCol_blue.Image = ((System.Drawing.Image)(resources.GetObject("pb_rectCenCol_blue.Image")));
            this.pb_rectCenCol_blue.Location = new System.Drawing.Point(737, 81);
            this.pb_rectCenCol_blue.Name = "pb_rectCenCol_blue";
            this.pb_rectCenCol_blue.Size = new System.Drawing.Size(25, 25);
            this.pb_rectCenCol_blue.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_rectCenCol_blue.TabIndex = 45;
            this.pb_rectCenCol_blue.TabStop = false;
            this.pb_rectCenCol_blue.Tag = "2";
            this.pb_rectCenCol_blue.Visible = false;
            this.pb_rectCenCol_blue.Click += new System.EventHandler(this.pb_rectCenCol_Click);
            // 
            // pb_rectCenCol_green
            // 
            this.pb_rectCenCol_green.BackColor = System.Drawing.Color.Black;
            this.pb_rectCenCol_green.Image = ((System.Drawing.Image)(resources.GetObject("pb_rectCenCol_green.Image")));
            this.pb_rectCenCol_green.Location = new System.Drawing.Point(737, 55);
            this.pb_rectCenCol_green.Name = "pb_rectCenCol_green";
            this.pb_rectCenCol_green.Size = new System.Drawing.Size(25, 25);
            this.pb_rectCenCol_green.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_rectCenCol_green.TabIndex = 44;
            this.pb_rectCenCol_green.TabStop = false;
            this.pb_rectCenCol_green.Tag = "1";
            this.pb_rectCenCol_green.Visible = false;
            this.pb_rectCenCol_green.Click += new System.EventHandler(this.pb_rectCenCol_Click);
            // 
            // pb_rectCenCol_red
            // 
            this.pb_rectCenCol_red.BackColor = System.Drawing.Color.Black;
            this.pb_rectCenCol_red.Image = ((System.Drawing.Image)(resources.GetObject("pb_rectCenCol_red.Image")));
            this.pb_rectCenCol_red.Location = new System.Drawing.Point(737, 29);
            this.pb_rectCenCol_red.Name = "pb_rectCenCol_red";
            this.pb_rectCenCol_red.Size = new System.Drawing.Size(25, 25);
            this.pb_rectCenCol_red.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_rectCenCol_red.TabIndex = 43;
            this.pb_rectCenCol_red.TabStop = false;
            this.pb_rectCenCol_red.Tag = "0";
            this.pb_rectCenCol_red.Visible = false;
            this.pb_rectCenCol_red.Click += new System.EventHandler(this.pb_rectCenCol_Click);
            // 
            // pb_rectCol_yellow
            // 
            this.pb_rectCol_yellow.BackColor = System.Drawing.Color.Black;
            this.pb_rectCol_yellow.Image = ((System.Drawing.Image)(resources.GetObject("pb_rectCol_yellow.Image")));
            this.pb_rectCol_yellow.Location = new System.Drawing.Point(711, 107);
            this.pb_rectCol_yellow.Name = "pb_rectCol_yellow";
            this.pb_rectCol_yellow.Size = new System.Drawing.Size(25, 25);
            this.pb_rectCol_yellow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_rectCol_yellow.TabIndex = 42;
            this.pb_rectCol_yellow.TabStop = false;
            this.pb_rectCol_yellow.Tag = "3";
            this.pb_rectCol_yellow.Visible = false;
            this.pb_rectCol_yellow.Click += new System.EventHandler(this.pb_rectCol_Click);
            // 
            // pb_rectCol_blue
            // 
            this.pb_rectCol_blue.BackColor = System.Drawing.Color.Black;
            this.pb_rectCol_blue.Image = ((System.Drawing.Image)(resources.GetObject("pb_rectCol_blue.Image")));
            this.pb_rectCol_blue.Location = new System.Drawing.Point(711, 81);
            this.pb_rectCol_blue.Name = "pb_rectCol_blue";
            this.pb_rectCol_blue.Size = new System.Drawing.Size(25, 25);
            this.pb_rectCol_blue.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_rectCol_blue.TabIndex = 41;
            this.pb_rectCol_blue.TabStop = false;
            this.pb_rectCol_blue.Tag = "2";
            this.pb_rectCol_blue.Visible = false;
            this.pb_rectCol_blue.Click += new System.EventHandler(this.pb_rectCol_Click);
            // 
            // pb_rectCol_green
            // 
            this.pb_rectCol_green.BackColor = System.Drawing.Color.Black;
            this.pb_rectCol_green.Image = ((System.Drawing.Image)(resources.GetObject("pb_rectCol_green.Image")));
            this.pb_rectCol_green.Location = new System.Drawing.Point(711, 55);
            this.pb_rectCol_green.Name = "pb_rectCol_green";
            this.pb_rectCol_green.Size = new System.Drawing.Size(25, 25);
            this.pb_rectCol_green.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_rectCol_green.TabIndex = 40;
            this.pb_rectCol_green.TabStop = false;
            this.pb_rectCol_green.Tag = "1";
            this.pb_rectCol_green.Visible = false;
            this.pb_rectCol_green.Click += new System.EventHandler(this.pb_rectCol_Click);
            // 
            // pb_rectCol_red
            // 
            this.pb_rectCol_red.BackColor = System.Drawing.Color.Black;
            this.pb_rectCol_red.Image = ((System.Drawing.Image)(resources.GetObject("pb_rectCol_red.Image")));
            this.pb_rectCol_red.Location = new System.Drawing.Point(711, 29);
            this.pb_rectCol_red.Name = "pb_rectCol_red";
            this.pb_rectCol_red.Size = new System.Drawing.Size(25, 25);
            this.pb_rectCol_red.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_rectCol_red.TabIndex = 39;
            this.pb_rectCol_red.TabStop = false;
            this.pb_rectCol_red.Tag = "0";
            this.pb_rectCol_red.Visible = false;
            this.pb_rectCol_red.Click += new System.EventHandler(this.pb_rectCol_Click);
            // 
            // picZoom
            // 
            this.picZoom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picZoom.Location = new System.Drawing.Point(792, 227);
            this.picZoom.Name = "picZoom";
            this.picZoom.Size = new System.Drawing.Size(120, 120);
            this.picZoom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picZoom.TabIndex = 33;
            this.picZoom.TabStop = false;
            // 
            // button_preview
            // 
            this.button_preview.Enabled = false;
            this.button_preview.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_preview.Image = ((System.Drawing.Image)(resources.GetObject("button_preview.Image")));
            this.button_preview.Location = new System.Drawing.Point(784, 89);
            this.button_preview.Margin = new System.Windows.Forms.Padding(0);
            this.button_preview.Name = "button_preview";
            this.button_preview.Size = new System.Drawing.Size(130, 65);
            this.button_preview.TabIndex = 19;
            this.button_preview.TabStop = false;
            this.button_preview.UseVisualStyleBackColor = true;
            this.button_preview.Click += new System.EventHandler(this.button_preview_Click);
            // 
            // button_calculate
            // 
            this.button_calculate.Enabled = false;
            this.button_calculate.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_calculate.Image = ((System.Drawing.Image)(resources.GetObject("button_calculate.Image")));
            this.button_calculate.Location = new System.Drawing.Point(784, 159);
            this.button_calculate.Margin = new System.Windows.Forms.Padding(0);
            this.button_calculate.Name = "button_calculate";
            this.button_calculate.Size = new System.Drawing.Size(130, 65);
            this.button_calculate.TabIndex = 19;
            this.button_calculate.TabStop = false;
            this.button_calculate.UseVisualStyleBackColor = true;
            this.button_calculate.Click += new System.EventHandler(this.button_calculate_Click);
            // 
            // picturebox_missingLandmarks
            // 
            this.picturebox_missingLandmarks.Image = ((System.Drawing.Image)(resources.GetObject("picturebox_missingLandmarks.Image")));
            this.picturebox_missingLandmarks.Location = new System.Drawing.Point(5, 734);
            this.picturebox_missingLandmarks.Name = "picturebox_missingLandmarks";
            this.picturebox_missingLandmarks.Size = new System.Drawing.Size(300, 50);
            this.picturebox_missingLandmarks.TabIndex = 18;
            this.picturebox_missingLandmarks.TabStop = false;
            this.picturebox_missingLandmarks.Paint += new System.Windows.Forms.PaintEventHandler(this.picturebox_missingLandmarks_Paint);
            // 
            // picturebox_map
            // 
            this.picturebox_map.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picturebox_map.Image = global::MapDrawingAnalyzer.Properties.Resources.homeScreen;
            this.picturebox_map.Location = new System.Drawing.Point(5, 29);
            this.picturebox_map.Name = "picturebox_map";
            this.picturebox_map.Size = new System.Drawing.Size(700, 700);
            this.picturebox_map.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picturebox_map.TabIndex = 0;
            this.picturebox_map.TabStop = false;
            this.picturebox_map.Tag = "homeScreen";
            this.picturebox_map.Paint += new System.Windows.Forms.PaintEventHandler(this.picturebox_map_Paint);
            this.picturebox_map.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picturebox_map_MouseMove);
            this.picturebox_map.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picturebox_MouseUp);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(914, 808);
            this.Controls.Add(this.b_rotate_cw);
            this.Controls.Add(this.b_rotate_ccw);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pb_rectCenCol_white);
            this.Controls.Add(this.pb_rectCol_white);
            this.Controls.Add(this.pb_rectCenCol_yellow);
            this.Controls.Add(this.pb_rectCenCol_blue);
            this.Controls.Add(this.pb_rectCenCol_green);
            this.Controls.Add(this.pb_rectCenCol_red);
            this.Controls.Add(this.pb_rectCol_yellow);
            this.Controls.Add(this.pb_rectCol_blue);
            this.Controls.Add(this.pb_rectCol_green);
            this.Controls.Add(this.pb_rectCol_red);
            this.Controls.Add(this.pb_calculate);
            this.Controls.Add(this.lb_advancedMode);
            this.Controls.Add(this.lb_basicMode);
            this.Controls.Add(this.lb_trackBar);
            this.Controls.Add(this.trackBar_zoomLevel);
            this.Controls.Add(this.picZoom);
            this.Controls.Add(this.textbox_numLandmarks);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.cb_coordsFiles);
            this.Controls.Add(this.tb_legend);
            this.Controls.Add(this.button_preview);
            this.Controls.Add(this.button_calculate);
            this.Controls.Add(this.picturebox_missingLandmarks);
            this.Controls.Add(this.picturebox_map);
            this.Controls.Add(this.mainMenu);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.panel_highlight_labels);
            this.Controls.Add(this.panel_highlight_MapID);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.KeyPreview = true;
            this.MainMenuStrip = this.mainMenu;
            this.Name = "Form1";
            this.Text = "Gardony Map Drawing Analyzer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_zoomLevel)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_rectCenCol_white)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_rectCol_white)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_rectCenCol_yellow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_rectCenCol_blue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_rectCenCol_green)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_rectCenCol_red)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_rectCol_yellow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_rectCol_blue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_rectCol_green)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_rectCol_red)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturebox_missingLandmarks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturebox_map)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picturebox_map;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button_calculate;
        private System.Windows.Forms.TextBox tb_legend;
        private System.Windows.Forms.SaveFileDialog d_saveImage;
        private System.Windows.Forms.TextBox textbox_numLandmarks;
        private System.Windows.Forms.ToolTip toolTip_button_landmarkNum;
        private System.Windows.Forms.ToolTip toolTip_textbox_mapNum;
        private System.Windows.Forms.ToolTip toolTip_missingLandmarks;
        private System.Windows.Forms.ToolTip toolTip_loadMapImageToolStripMenuItem;
        private System.Windows.Forms.Button button_preview;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadMapImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveMapImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem coordinatesFileToolStripMenuItem;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolStripMenuItem restoreDefaultsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.PictureBox picZoom;
        private System.Windows.Forms.TrackBar trackBar_zoomLevel;
        private System.Windows.Forms.Label lb_trackBar;
        private System.Windows.Forms.ComboBox cb_coordsFiles;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.Label lb_basicMode;
        private System.Windows.Forms.Label lb_advancedMode;
        private System.Windows.Forms.PictureBox picturebox_missingLandmarks;
        private System.Windows.Forms.ProgressBar pb_calculate;
        private System.Windows.Forms.PictureBox pb_rectCol_red;
        private System.Windows.Forms.PictureBox pb_rectCol_green;
        private System.Windows.Forms.PictureBox pb_rectCol_blue;
        private System.Windows.Forms.PictureBox pb_rectCol_yellow;
        private System.Windows.Forms.PictureBox pb_rectCenCol_yellow;
        private System.Windows.Forms.PictureBox pb_rectCenCol_blue;
        private System.Windows.Forms.PictureBox pb_rectCenCol_green;
        private System.Windows.Forms.PictureBox pb_rectCenCol_red;
        private System.Windows.Forms.PictureBox pb_rectCenCol_white;
        private System.Windows.Forms.PictureBox pb_rectCol_white;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem batchReanalysisToolStripMenuItem;
        private System.Windows.Forms.Panel panel_highlight_labels;
        private System.Windows.Forms.Panel panel_highlight_MapID;
        private System.Windows.Forms.Button b_rotate_ccw;
        private System.Windows.Forms.Button b_rotate_cw;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bDROptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem actualMapIVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem actualMapDVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem movableZoomWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    }
}

