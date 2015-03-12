using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using UserRectDemo;
using PointDClass;
using Plexiglass;
using AppLimit.NetSparkle;
// Copyright 2013 Aaron Gardony
// This program is distributed under the terms of the GNU General Public License
//Gardony Map Drawing Analyzer last update: 10/15/13 by Aaron Gardony
namespace MapDrawingAnalyzer
{
    public partial class Form1 : Form
    {
        private Sparkle _sparkle;
        private int x;
        private int y;
        private int currentRot = 0;
        private double maxDist = -1.0;
        private double maxInterBoxDist = -1.0;
        private List<double> maxInterBoxDistList = new List<double>();
        private List<double>[] actualMapCoordinates;
        private List<string>[] participantsMapCoordinates;
        private List<double>[] A_prime_B_prime;
        private List<string>[] allCombos;
        private List<string>[] allBoxCombos;
        private PictureBox[] nodes;
        private bool[] nodesMissing;
        private PictureBox lastMovedNode;
        private UserRect lastMovedUserRect;
        private Point[] labelStartPos;
        private Point[] boxStartPos;
        private List<string> landmarkNames = new List<string>();
        private List<ToolTip> landmarkTTs = new List<ToolTip>();
        private StreamWriter outfile;
        private StreamWriter configOutfile;
        private Point oldMouseLocation = Point.Empty;
        private Size imageOffset = Size.Empty;
        private List<UserRect> allRects = new List<UserRect>();
        private List<PictureBox>[] colorPalette = new List<PictureBox>[2];
        private bool allLabelsBoxesLoaded = false;
        private Point picZoomLoc = new Point(792, 227);

        public Form1()
        {
            InitializeComponent();
            _sparkle = new Sparkle("http://aarongardony.com/GMDA/sparkleGMDA.xml");
            _sparkle.StartLoop(true);
            NetSparkleAppCastItem _frm = new NetSparkleAppCastItem();
            if (_sparkle.IsUpdateRequired(_sparkle.GetApplicationConfig(), out _frm))
            {
                _sparkle.ShowUpdateNeededUI(_frm);
            }
        }

        // Initializing, Arranging, & Updating

        // Initializing

        public void loadForm() // procedures run when main form is loaded
        {
            
            picturebox_map.Focus();
            toggleMode();
            if (trackBar_zoomLevel.Value == 0) { trackBar_zoomLevel.Value = 4; }
            trackBar_zoomLevel.Value = Properties.Settings.Default.prevZoomLevel;
            picZoom.Location = picZoomLoc;
            string currentBDR_mode = Properties.Settings.Default.BDR_IV;
            switch (currentBDR_mode)
            {
                case "actual":
                    this.actualMapIVToolStripMenuItem.Checked = true;
                    this.actualMapDVToolStripMenuItem.Checked = false;
                    break;
                case "participant's":
                    this.actualMapIVToolStripMenuItem.Checked = false;
                    this.actualMapDVToolStripMenuItem.Checked = true;
                    break;
            }
            switch (Properties.Settings.Default.movableZoomWindow)
            {
                case true:
                    this.movableZoomWindowToolStripMenuItem.Checked = true;
                    break;
                case false:
                    this.movableZoomWindowToolStripMenuItem.Checked = false;
                    break;
            }
            toolStripStatusLabel.Text = "Home Screen";
            // check if Resources folder exists, if not create it
            if (!Directory.Exists(Application.StartupPath + "\\Resources"))
            {
                Directory.CreateDirectory(Application.StartupPath + "\\Resources");
            }
            for (int i = 0; i < colorPalette.Length; i++)
            {
                colorPalette[i] = new List<PictureBox>();
                if (i == 0)
                {
                    colorPalette[i].Add(pb_rectCol_red);
                    colorPalette[i].Add(pb_rectCol_green);
                    colorPalette[i].Add(pb_rectCol_blue);
                    colorPalette[i].Add(pb_rectCol_yellow);
                    colorPalette[i].Add(pb_rectCol_white);
                }
                if (i == 1)
                {

                    colorPalette[i].Add(pb_rectCenCol_red);
                    colorPalette[i].Add(pb_rectCenCol_green);
                    colorPalette[i].Add(pb_rectCenCol_blue);
                    colorPalette[i].Add(pb_rectCenCol_yellow);
                    colorPalette[i].Add(pb_rectCenCol_white);
                }
            }
            populateCoordsList(); // put coordinates files into combo box

        }

        private bool createLandmarkLabels(int numLandmarks) // create & arrange numLandmarks landmark labels
        {
            string currentMode = Properties.Settings.Default.currentMode;
            bool retBool = false;
            removeNodes();
            switch (currentMode)
            {
                case "basic":
                    // dynamically create landmark labels
                    // if labels already exist remove them
                    
                    nodes = new PictureBox[numLandmarks];
                    nodesMissing = new bool[numLandmarks];
                    labelStartPos = new Point[numLandmarks];
                    System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
                    for (int i = 0; i < nodes.Count(); i++)
                    {
                        nodes[i] = new PictureBox();
                        nodesMissing[i] = new bool();

                        try { nodes[i].Image = GetImage("_" + (i + 1)); }
                        catch { return false; }
                        nodesMissing[i] = false;
                        nodes[i].InitialImage = null;
                        nodes[i].Name = "picturebox_node" + Convert.ToString(i + 1);
                        nodes[i].Size = new System.Drawing.Size(20, 20);
                        defaultPosition(i);
                        nodes[i].MouseDown += new System.Windows.Forms.MouseEventHandler(this.picturebox_MouseDown);
                        nodes[i].MouseMove += new System.Windows.Forms.MouseEventHandler(this.picturebox_MouseMove);
                        nodes[i].MouseUp += new MouseEventHandler(this.picturebox_MouseUp);
                        nodes[i].DoubleClick += new EventHandler(this.picturebox_DoubleClick);
                        nodes[i].PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.picturebox_PreviewKeyDown);
                        nodes[i].BringToFront();
                        nodes[i].Visible = true;
                       

                        // add tool tips
                        ToolTip tempTT = new ToolTip();
                        tempTT.AutomaticDelay = 50;
                        tempTT.IsBalloon = true;
                        tempTT.AutoPopDelay = 2000;
                        tempTT.SetToolTip(this.nodes[i], landmarkNames[i]);
                        landmarkTTs.Add(tempTT);

                        this.Controls.Add(nodes[i]);
                    }
                    allLabelsBoxesLoaded = true;
                    retBool = true;
                    break;
                case "advanced":
                    boxStartPos = new Point[numLandmarks];
                    for (int i = 0; i < numLandmarks; i++)
                    {
                        defaultPosition(i);
                        allRects[i].SetPictureBox(this.picturebox_map, this.picturebox_missingLandmarks);
                        allRects[i].setRectColor(getRectColor(Properties.Settings.Default.rectColor));
                        allRects[i].setRectCenterColor(getRectColor(Properties.Settings.Default.rectCentColor));
                    }
                    picturebox_map.Invalidate();
                    for (int i = 0; i < colorPalette.Length; i++)
                    {
                        foreach (PictureBox pb in colorPalette[i])
                        {
                            pb.Visible = true;
                            pb.BorderStyle = BorderStyle.None;
                            pb.Padding = new Padding(0);
                        }
                        colorPalette[0][Properties.Settings.Default.rectColor].Padding = new Padding(4);
                        colorPalette[1][Properties.Settings.Default.rectCentColor].Padding = new Padding(4);
                    }
                    allLabelsBoxesLoaded = true;
                    retBool = true;
                    break;

            }
            return retBool;
        }

        private void populateCoordsList() // populate combo box with coordinates files located in /Resources/
        {
            // NEEDS CASE
            string currentMode = Properties.Settings.Default.currentMode;
            string searchString = "";
            switch (currentMode)
            {
                case "basic":
                    searchString = "xycoords_map_";
                    break;
                case "advanced":
                    searchString = "xycoords_mapAdvanced_";
                    break;
            }
            if (cb_coordsFiles != null) { cb_coordsFiles.Items.Clear(); }
            List<string> coordinatesFileNames = new List<string>();
            string[] filePaths = Directory.GetFiles(Application.StartupPath + "\\Resources", "*.csv");
            string[] tempArray;
            if (filePaths.Length > 0)
            {
                for (int i = 0; i < filePaths.Length; i++)
                {
                    string extension = System.IO.Path.GetExtension(filePaths[i]);
                    string result = filePaths[i].Substring(0, filePaths[i].Length - extension.Length); // remove extension
                    if (result.Contains(searchString))// check valid config file
                    {
                        tempArray = result.Split(new string[] { searchString }, StringSplitOptions.None);
                        coordinatesFileNames.Add(tempArray[tempArray.Length - 1]); // add file name
                        cb_coordsFiles.Items.Add(tempArray[tempArray.Length - 1]);
                    }
                }
                try
                {
                    // attempt to load previously used configuration file
                    mainMenu.Focus();
                    cb_coordsFiles.SelectedItem = Properties.Settings.Default.prevCoordsFile;
                    loadCoordinatesFile(cb_coordsFiles.SelectedItem.ToString());
                }
                catch { }
            }
            else // no coordinates files found
            {
                MessageBox.Show("No coordinates files were found in:\n\n" + Application.StartupPath + "\\Resources\n\n" +
                "To make a new coordinates file click File --> New Coordinates File (CTRL + N).", "Message");
            }
        }

        private void loadCoordinatesFile(string selectedItem) // loaded coordinates file for selected item
        {
            // currently broken for advanced mode
            string currentMode = Properties.Settings.Default.currentMode;
            string searchString = "";
            switch (currentMode)
            {
                case "basic":
                    searchString = "xycoords_map_";
                    break;
                case "advanced":
                    searchString = "xycoords_mapAdvanced_";
                    break;
            }

            // enable and clear legend
            tb_legend.Enabled = true;
            tb_legend.Text = "";
            int numLandmarks = 0;
            if (cb_coordsFiles.SelectedItem != null)
            { //numLandmarks = loadCoordinates(selectedItem); }
                string thisLine;
                string[] tempArray;
                StreamReader xy_coords;
                try
                {
                    xy_coords = new StreamReader(Application.StartupPath + "//Resources//" + searchString + selectedItem + ".csv");
                    thisLine = xy_coords.ReadLine();
                    tempArray = thisLine.Split(',');
                    int numCols = tempArray.Length;
                    actualMapCoordinates = new List<double>[numCols - 1];
                    participantsMapCoordinates = new List<string>[numCols - 1];
                    for (int i = 0; i < actualMapCoordinates.Length; i++)
                    {
                        actualMapCoordinates[i] = new List<double>();
                        participantsMapCoordinates[i] = new List<string>();
                    }
                    landmarkNames.Clear();
                    string lastLandmark = "";

                    while (!xy_coords.EndOfStream)
                    {
                        switch (currentMode)
                        {
                            case "basic":
                                thisLine = xy_coords.ReadLine();
                                numLandmarks++;
                                tempArray = thisLine.Split(',');
                                tb_legend.Text += tempArray[1] + ". " + tempArray[0] + "\r\n"; // update legend
                                landmarkNames.Add(tempArray[0]);
                                break;
                            case "advanced":
                                thisLine = xy_coords.ReadLine();
                                tempArray = thisLine.Split(',');
                                if (!tempArray[0].Equals(lastLandmark))
                                {
                                    tb_legend.Text += Convert.ToString(1 + Convert.ToInt32(tempArray[1]) / 8) + ". " + tempArray[0] + "\r\n"; // update legend
                                    landmarkNames.Add(tempArray[0]);
                                    lastLandmark = tempArray[0];
                                    numLandmarks++;
                                }
                                break;
                        }
                        for (int i = 0; i < tempArray.Length - 1; i++)
                        {
                            // add actual map coordinates to list
                            actualMapCoordinates[i].Add(Convert.ToDouble(tempArray[i + 1]));
                            // add dummy values for participant's map coordinates
                            //if (i == 0) { participantsMapCoordinates[i].Add(tempArray[i + 1]); }
                            //else { participantsMapCoordinates[i].Add("M"); }
                        }
                    }
                    xy_coords.Close();
                    bool pass = createLandmarkLabels(numLandmarks);
                    if (!pass) { numLandmarks = 49; }
                }


                catch (System.IO.IOException)
                {
                    numLandmarks = -1; // -1 means error
                }
                if (numLandmarks > 1 && numLandmarks < 49) // num landmarks must be > 1 & < 49
                {
                    // loaded coordinates file passes error checking
                    // generate combinations
                    allCombos = new List<string>[7];
                    allBoxCombos = new List<string>[7];
                    for (int i = 0; i < allCombos.Length; i++) { allCombos[i] = new List<string>(); }
                    for (int i = 0; i < allCombos.Length; i++) { allBoxCombos[i] = new List<string>(); }
                    generateCombinations(ref allCombos, actualMapCoordinates[0].Count);
                    textbox_numLandmarks.Text = numLandmarks.ToString(); ;
                    //button_calculate.Enabled = true;
                    //button_preview.Enabled = true;
                    if (picturebox_map.Tag.Equals("map")) 
                    {
                        b_rotate_ccw.Enabled = true;
                        b_rotate_cw.Enabled = true;
                    }
                    batchReanalysisToolStripMenuItem.Enabled = true;
                    panel_highlight_labels.SendToBack();
                }
                else
                {
                    if (numLandmarks > -1 && numLandmarks < 2)
                    {
                        MessageBox.Show("Unable to load coordinates file.\n\n" +
                        "File contains 1 or fewer landmarks.\n\nFile Location: "
                        + Application.StartupPath + "\\Resources\\xycoords_map_" + selectedItem + ".csv");
                    }
                    else if (numLandmarks > 48)
                    {
                        MessageBox.Show("Unable to load coordinates file.\n\n" +
                        "File contains more than 48 landmarks.\n\nFile Location: "
                        + Application.StartupPath + "\\Resources\\xycoords_map_" + selectedItem + ".csv");
                    }
                    else if (numLandmarks == -1) // IO exception
                    {
                        MessageBox.Show("Can't access xycoords_map_" + selectedItem + ".csv in:\n\n" +
                                            Application.StartupPath + "\\Resources" +
                                            "\n\nPlease make sure the file is not currently open in an external program.", "Message");
                    }
                    removeNodes();
                    textbox_numLandmarks.Text = "";
                    tb_legend.Text = "";
                }
            }
        }

        private void fillParticipantsMapCoordinates() // need comment
        {
            string currentMode = Properties.Settings.Default.currentMode;
            if (participantsMapCoordinates != null)
            {
                for (int i = 0; i < participantsMapCoordinates.Length; i++)
                {
                    participantsMapCoordinates[i].Clear();
                }
            }
            switch (currentMode)
            {
                case "basic":
                    for (int i = 0; i < landmarkNames.Count; i++)
                    {
                        participantsMapCoordinates[0].Add(Convert.ToString(i + 1));
                        PointD p = getCartesianCoords(i + 1);
                        if (!landmarkMissing(i + 1))
                        {
                            participantsMapCoordinates[1].Add(Convert.ToString(p.X));
                            participantsMapCoordinates[2].Add(Convert.ToString(p.Y));
                        }
                        else
                        {
                            participantsMapCoordinates[1].Add("M");
                            participantsMapCoordinates[2].Add("M");
                        }
                    }

                    break;
                case "advanced":
                    int counter = 1;
                    for (int i = 0; i < landmarkNames.Count; i++)
                    {
                        List<PointD> cartCoords = allRects[i].getRectBorderCartesianCoordinates();
                        cartCoords = advancedRotationAdjustment(cartCoords);
                        for (int j = 0; j < cartCoords.Count; j++)
                        {
                            participantsMapCoordinates[0].Add(Convert.ToString(counter));
                            if (allRects[i].getMissing() == false)
                            {
                                participantsMapCoordinates[1].Add(Convert.ToString(cartCoords[j].X));
                                participantsMapCoordinates[2].Add(Convert.ToString(cartCoords[j].Y));
                            }
                            else
                            {
                                participantsMapCoordinates[1].Add("M");
                                participantsMapCoordinates[2].Add("M");
                            }
                            counter++;
                        }
                    }
                    break;
            }
        }

        // Arranging
        
        private void removeNodes() // clears and diposes landmark labels from form
        {
            
            if (nodes != null)
            {
                for (int i = 0; i < nodes.Length; i++)
                {
                    this.Controls.Remove(nodes[i]);
                    nodes[i] = null;
                    nodesMissing[i] = false;
                }
                nodes = null;
                labelStartPos = null;
                allLabelsBoxesLoaded = false;
            }

            if (allRects.Count != 0)
            {
                int tempCount = allRects.Count;
                for (int i = 0; i < tempCount; i++)
                {
                    allRects[0].DiposePictureBox();
                    allRects[0] = null;
                    allRects.RemoveAt(0);
                }
                allRects.Clear();

                boxStartPos = null;
                allLabelsBoxesLoaded = false;
            }           
        }

        private void defaultPosition(int nodeIndex) // places landmark labels in their default position as calculated by their index
        {
            int nodeXPos = 0;
            int nodeYPos = 0;
            int nodeXPosRect = 0;
            int nodeYPosRect = 0;
            string currentMode = Properties.Settings.Default.currentMode;
            nodeXPos = 710 + (nodeIndex / 16) * 25;
            nodeYPosRect = 20 + (nodeIndex / 8) * 80;
            switch (currentMode)
            {
                case "basic":
                    {
                        if (nodeIndex < 16)
                        {
                            nodeYPos = 31 + (nodeIndex * 25);
                        }
                        else
                        {
                            nodeYPos = 31 + (nodeIndex % 16) * 25;
                        }
                        nodes[nodeIndex].Location = new System.Drawing.Point(nodeXPos, nodeYPos);
                        labelStartPos[nodeIndex].X = nodeXPos;
                        labelStartPos[nodeIndex].Y = nodeYPos;
                        break;
                    }
                case "advanced":
                    {
                        if (nodeIndex < 8)
                        {
                            nodeXPosRect = 30 + (nodeIndex * 80);
                        }
                        else
                        {
                            nodeXPosRect = 30 + (nodeIndex % 8) * 80;
                        }
                        allRects.Add(new UserRect(new Rectangle(new System.Drawing.Point(nodeXPosRect, nodeYPosRect), new Size(30, 30)), landmarkNames[nodeIndex]));
                        boxStartPos[nodeIndex].X = nodeXPosRect;
                        boxStartPos[nodeIndex].Y = nodeYPosRect;
                        break;
                    }
            }
        }

        private Point missingDefaultPosition(int nodeIndex) // places landmark labels in their default position when marked MISSING (as calculated by their index)
        {
            int nodeXPos = 0;
            int nodeYPos = 0;
            nodeXPos = 15 + (nodeIndex % 14) * 20;
            nodeYPos = 739 + ((nodeIndex / 14) % 2) * 20;
            Point missingPos = new System.Drawing.Point(nodeXPos, nodeYPos);
            nodes[nodeIndex].Location = missingPos;
            nodes[nodeIndex].BringToFront();
            return missingPos;
        }

        // Updating

        private void updateZoomWindow(int XPos, int YPos) // draws updates to zoom window
        {
            if (picturebox_map.Image != null)
            {
                int ZoomFactor = trackBar_zoomLevel.Value;

                int zoomWidth = picZoom.Width / ZoomFactor;
                int zoomHeight = picZoom.Height / ZoomFactor;

                int halfWidth = zoomWidth / 2;
                int halfHeight = zoomHeight / 2;


                Bitmap bmpCropped = new Bitmap(zoomWidth, zoomHeight,
                                               PixelFormat.Format24bppRgb);
                //create the graphics object to draw with
                Graphics g = Graphics.FromImage(bmpCropped);
                g.Clear(picZoom.BackColor);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                Rectangle rectCropArea;
                Rectangle rectDestination = new Rectangle(0, 0, bmpCropped.Width, bmpCropped.Height);
                if (currentRot % 2 == 0) // Image rotated 0 or 180 degrees
                {
                    rectCropArea = new Rectangle(XPos - halfWidth - imageOffset.Width, YPos - halfHeight - imageOffset.Height, zoomWidth, zoomHeight);
                }
                else // Image rotated 90 or 270 degrees
                {
                    rectCropArea = new Rectangle(XPos - halfWidth - imageOffset.Height, YPos - halfHeight - imageOffset.Width, zoomWidth, zoomHeight);
                }

              /*  if (lastMovedUserRect != null) 
                {
                    rectCropArea = lastMovedUserRect.rect; 
                }*/ 
                //draw the rectCropArea of the original image to the rectDestination of bmpCropped

                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(picturebox_map.Image, rectDestination, rectCropArea, GraphicsUnit.Pixel);
                picZoom.Image = bmpCropped;
                // Draw a crosshair on the bitmap to simulate the cursor position
                g.DrawLine(Pens.Black, halfWidth - 2, halfHeight, halfWidth - 6, halfHeight); // left line
                g.DrawLine(Pens.Black, halfWidth + 2, halfHeight, halfWidth + 6, halfHeight); // right line
                g.DrawLine(Pens.Black, halfWidth, halfHeight - 2, halfWidth, halfHeight - 6); // top line
                g.DrawLine(Pens.Black, halfWidth, halfHeight + 6, halfWidth, halfHeight + 2); // top line

                g.Dispose();
                picZoom.Refresh();
            }
        }

        private void updatePrevCoordsFile() // updates default val for prev coordinates file
        {
            Properties.Settings.Default.prevCoordsFile = cb_coordsFiles.SelectedItem.ToString();
            Properties.Settings.Default.Save();
        }

        private void updateCurrentMode(string s) // need comment
        {
            Properties.Settings.Default.currentMode = s;
            Properties.Settings.Default.Save();
        }

        private void updateBoxBorderColor(PictureBox p) // need comment
        {
            foreach (PictureBox pb in colorPalette[0])
            {
                pb.Padding = new Padding(0);
                pb.Refresh();
            }
            p.Padding = new Padding(4);
            p.Refresh();
            Properties.Settings.Default.rectColor = Convert.ToInt32(p.Tag);
            Properties.Settings.Default.Save();
            foreach (UserRect ur in allRects)
            {
                if (ur.getMissing() == false) { ur.setRectColor(getRectColor(Properties.Settings.Default.rectColor)); }
            }
            picturebox_map.Refresh();
        }

        private void updateBoxCenterColor(PictureBox p) // need comment
        {
            foreach (PictureBox pb in colorPalette[1])
            {
                pb.Padding = new Padding(0);
                pb.Refresh();
            }
            p.Padding = new Padding(4);
            p.Refresh();
            Properties.Settings.Default.rectCentColor = Convert.ToInt32(p.Tag);
            Properties.Settings.Default.Save();
            foreach (UserRect ur in allRects)
            {
                if (ur.getMissing() == false) { ur.setRectCenterColor(getRectColor(Properties.Settings.Default.rectCentColor)); }
            }
            picturebox_map.Refresh();
        }

        // Core Functions

        private void newCoordinatesFile() // need comment
        {
            string currentMode = Properties.Settings.Default.currentMode;
            switch (currentMode)
            {
                case "basic":
                    Form2 bcfb = new Form2(this);
                    this.Hide();
                    bcfb.Show();
                    break;
                case "advanced":
                    advancedCoordsFileBuilder acfb = new advancedCoordsFileBuilder(this);
                    this.Hide();
                    acfb.Show();
                    break;
            }
        }

        private bool openMapImage() // need comment
        {
            string currentMode = Properties.Settings.Default.currentMode;
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = "Open Map Image";
            openFile.Filter = "JPG files (*.jpg)|*.jpg|JPEG files (*.jpeg)|*.jpeg|PNG files (*.png)|*.png|GIF files (*.gif)|*.gif|BMP files (*.bmp)|*.bmp|All files (*.*)|*.*";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                //picturebox_map.Image = new Bitmap(openFile.OpenFile());
                Image tempImage = new Bitmap(openFile.OpenFile());
                if (tempImage.Width != tempImage.Height)
                {
                    MessageBox.Show("Note: For best results the loaded map image should be square\ni.e. the height and width (in pixels) are the same.\n\n" +
                                    "We recommend that you use photo editing software such as Photoshop or Gimp to crop the image so that it is square and reload it.\n\n" +
                                    "For now the image has been resized to fit the window.", "Message");
                }
                int newWidth = picturebox_map.Width;
                int newHeight = picturebox_map.Height;
                Bitmap finalImg = new Bitmap(tempImage, newWidth, newHeight);
                // Fit image into picturebox_map without distorting it
                if (tempImage.Width > picturebox_map.Width || tempImage.Height > picturebox_map.Height) // Case 1: loaded image is larger than picturebox_map.size one one or more dimensions
                {
                    double downscale = 1.0;
                    if (tempImage.Width >= tempImage.Height) // Case 1a: the width of the loaded image exceeds or is is equal to its height
                    {
                        downscale = (double)picturebox_map.Width / (double)tempImage.Width;
                        newHeight = Convert.ToInt32(downscale * tempImage.Height);
                        finalImg = new Bitmap(tempImage, picturebox_map.Width, newHeight);
                    }
                    else if (tempImage.Height > tempImage.Width) // Case 1b: the height of the loaded image exceeds its width
                    {
                        downscale = (double)picturebox_map.Height / (double)tempImage.Height;
                        newWidth = Convert.ToInt32(downscale * tempImage.Width);
                        finalImg = new Bitmap(tempImage, newWidth, picturebox_map.Height);
                    }
                }
                else if (tempImage.Width < picturebox_map.Width || tempImage.Height < picturebox_map.Height)// Case 2: loaded image is smaller than picturebox_map.size one one or more dimensions
                {
                    double upscale = 1.0;
                    if (tempImage.Width >= tempImage.Height) // Case 2a: the width of the loaded image exceeds or is is equal to its height
                    {
                        upscale = (double)picturebox_map.Width / (double)tempImage.Width;
                        newHeight = Convert.ToInt32(upscale * tempImage.Height);
                        finalImg = new Bitmap(tempImage, picturebox_map.Width, newHeight);
                    }
                    else if (tempImage.Height > tempImage.Width) // Case 2b: the height of the loaded image exceeds its width
                    {
                        upscale = (double)picturebox_map.Height / (double)tempImage.Height;
                        newWidth = Convert.ToInt32(upscale * tempImage.Width);
                        finalImg = new Bitmap(tempImage, newWidth, picturebox_map.Height);
                    }
                }
                // load offsets into global var, needed for updateZoomWindow()
                imageOffset.Width = (picturebox_map.Width - newWidth) / 2;
                imageOffset.Height = (picturebox_map.Height - newHeight) / 2;
                // create graphics object and insert it centered into picturebox_map
                Graphics gfx = Graphics.FromImage(finalImg);
                gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                picturebox_map.Image = null;
                picturebox_map.SizeMode = PictureBoxSizeMode.CenterImage;
                picturebox_map.Image = finalImg;
                picturebox_map.SendToBack();
                picturebox_map.Tag = "map";
                picturebox_map.Refresh();
                b_rotate_ccw.Enabled = true;
                b_rotate_cw.Enabled = true;
                toolStripStatusLabel.Text = "Currently opened image: " + System.IO.Path.GetFileName(openFile.FileName);
                if (allLabelsMoved())
                {
                    button_calculate.Enabled = true;
                    button_preview.Enabled = true;
                    saveConfigurationToolStripMenuItem.Enabled = true;
                }
                else
                {
                    if (currentMode.Equals("basic"))
                    {
                        if (allLabelsBoxesLoaded)
                        {
                            highlightPanel(this.panel_highlight_labels); // bring attention to landmark labels
                        }
                        else
                        {
                            highlightPanel(this.panel_highlight_MapID);
                        }
                    }
                    if (currentMode.Equals("advanced"))
                    {
                        if (!allLabelsBoxesLoaded)
                        {
                            highlightPanel(this.panel_highlight_MapID);
                        }
                    }

                }
                return true;
            }
            else { return false; }

        }

        private void loadConfigOpenDialog(bool multifile) // need comment
        {
            string currentMode = Properties.Settings.Default.currentMode;
            string initalDirectory = Application.StartupPath + "\\Configurations";

            if (!Directory.Exists(initalDirectory))
            {
                Directory.CreateDirectory(initalDirectory);
            }
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.InitialDirectory = initalDirectory;
            openFile.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            if (!multifile)
            {
                openFile.Multiselect = false;
                openFile.Title = "Load Configuration File";
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    loadConfiguration(openFile.FileName, multifile);
                }
            }
            else
            {
                DialogResult okCancel = MessageBox.Show("When doing a batch reanalysis make sure all the selected maps are for the same environment and that they match the current map ID.", "Message", MessageBoxButtons.OKCancel);
                if (okCancel == DialogResult.OK)
                {
                    openFile.Multiselect = true;
                    openFile.Title = "Load Configuration Files";

                    if (openFile.ShowDialog() == DialogResult.OK)
                    {
                        batchAnalysisProgressBar pb_batchAnalysis = new batchAnalysisProgressBar(this);
                        pb_batchAnalysis.Show();
                        pb_batchAnalysis.Refresh();
                        this.Hide();
                        string searchString = "";
                        int nFiles = openFile.FileNames.Length;
                        int fileCounter = 1;
                        bool newVersionConfig = false;
                        try
                        {
                            foreach (string file in openFile.FileNames)
                            { 
                                reset();
                                try { newVersionConfig = loadConfiguration(file, multifile); }
                                catch
                                {
                                    throw new Exception();
                                    
                                }
                                if (newVersionConfig)
                                {
                                    if (currentMode.Equals("basic")) { searchString = "ConfigBasic_"; }
                                    else if (currentMode.Equals("advanced")) { searchString = "ConfigAdvanced_"; }
                                }
                                else
                                {
                                    if (currentMode.Equals("basic")) { searchString = "Config_"; }
                                    else
                                    {
                                        MessageBox.Show("Unable to continue with analysis. Check if the program is in basic mode.", "Error");
                                        throw new Exception();
                                    }
                                }
                                string extension = System.IO.Path.GetExtension(file);
                                string result = file.Substring(0, file.Length - extension.Length); // remove extension
                                string[] tempArray = result.Split(new string[] { searchString }, StringSplitOptions.None);
                                calculate(false, multifile, tempArray[tempArray.Length - 1]);
                                int pbVal = Convert.ToInt32((Convert.ToDouble(fileCounter) / Convert.ToDouble(nFiles)) * 100);
                                pb_batchAnalysis.setPB(pbVal);
                                fileCounter++;
                            }
                            pb_batchAnalysis.clearPB();
                            pb_batchAnalysis.Close();
                            MessageBox.Show("Batch Reanalysis Complete!", "Message");
                            reset();
                            this.Show(); 
                        }
                        catch 
                        {
                            pb_batchAnalysis.clearPB();
                            pb_batchAnalysis.Close();
                            MessageBox.Show("Batch Reanalysis interrupted due to Error!", "Error");
                            reset();
                            this.Show(); 
                        }
                    }
                }
            }
        }

        private bool loadConfiguration(string path, bool multifile) // load configuration file, params: file path to load, are multiple files being loaded?
        {
            string currentMode = Properties.Settings.Default.currentMode;
            string thisLine;
            String[] tempArray = new string[3];
            List<int> left = new List<int>();
            List<int> top = new List<int>();
            List<int> backupLeft = new List<int>();
            List<int> backupTop = new List<int>();
            List<UserRect> tempUserRects = new List<UserRect>();
            int backupRot = 0;
            int tempRot1 = 0;
            int tempRot2 = 0;
            bool loadSuccessful = true;
            bool oldVersionConfigFile = false;
            int oldVersionYAdjustment = 0;
            string filename = "";
            bool retbool = true;

            switch (currentMode)
            {

                case "basic":

                    try
                    {
                        var fileInfo = new FileInfo(path);
                        filename = System.IO.Path.GetFileName(path);
                        DateTime lastWriteTime = fileInfo.LastWriteTime;
                        if (lastWriteTime.CompareTo(new DateTime(2013, 2, 13)) < 0)
                        {
                            oldVersionConfigFile = true;
                            oldVersionYAdjustment = 24;
                            retbool = false;
                        }
                        if (!multifile)
                        {
                            // mark all landmarks NOT missing
                            for (int i = 0; i < nodes.Length; i++)
                            {
                                defaultPosition(i);
                                nodesMissing[i] = false;
                            }
                        }
                        StreamReader infile = new StreamReader(path);
                        while (!infile.EndOfStream)
                        {
                            thisLine = infile.ReadLine();
                            tempArray = thisLine.Split(',');

                            if (!tempArray[0].Equals("currentRot = ") && !tempArray[0].Equals("Max d = "))
                            {
                                if (tempArray.Length != 3) { throw new FormatException(); } // tests to see if config file is from incorrect mode (advanced)
                                // load configuration
                                left.Add(Convert.ToInt32(tempArray[1]));
                                top.Add(Convert.ToInt32(tempArray[2]));
                            }
                            else
                            {
                                if (tempArray[0].Equals("currentRot = "))
                                {
                                    // CASE: config file is version 2 (includes rotation info)
                                    backupRot = currentRot;
                                    tempRot1 = Convert.ToInt32(tempArray[1]);
                                }
                            }
                        }
                        infile.Close();
                        try
                        {
                            if (nodes.Length != left.Count) { throw new ArgumentOutOfRangeException(); }
                            // move landmark labels
                            for (int i = 0; i < nodes.Length; i++)
                            {
                                nodes[i].Left = left[i];
                                nodes[i].Top = top[i] + oldVersionYAdjustment;
                                nodes[i].BringToFront();
                            }
                            // rotate workspace
                            while (currentRot != 0) { rotateWorkspace(false, "cw"); } // rotate map to starting rotation
                            while (tempRot1 != currentRot) { rotateWorkspace(false, "cw"); }
                            currentRot = tempRot1;
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            while (currentRot != backupRot) { rotateWorkspace(false, "cw"); } // reset rotation to original
                            loadSuccessful = false;
                            MessageBox.Show("The number of on screen landmark labels did not match the number of landmarks in the configuration file.\n\n" +
                                "Did you enter the wrong map or load the wrong configuration file?\n\n", "Message");
                        }
                        if (loadSuccessful)
                        {
                            saveConfigurationToolStripMenuItem.Enabled = true;
                            b_rotate_ccw.Enabled = true;
                            b_rotate_cw.Enabled = true;
                            button_calculate.Enabled = true;
                            button_preview.Enabled = true;
                        }
                        if (loadSuccessful && !oldVersionConfigFile && !multifile) { MessageBox.Show("Configuration Loaded", "Message"); }
                        if (loadSuccessful && oldVersionConfigFile && !multifile) { MessageBox.Show("Configuration Loaded\nNote: Old version configuration file loaded and converted.\nPlease save a new configuration file.", "Message"); }
                    }
                    catch (NullReferenceException)
                    {
                        MessageBox.Show("Loading configuration failed.\n\nHave you specified the map?", "Message");
                        highlightPanel(this.panel_highlight_MapID);
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Loading configuration failed.\n\n" +
                        "Filename: " + filename + "\n\n" +
                        "Possible Solutions:\n\n" +
                        "1. The file may not be a valid configuration file. You may have loaded an analysis file for example.\n\n" +
                        "2. The file may be valid but was created in a different mode. Make sure the configuration file was created in " + currentMode + " mode.", "Message");
                        if (multifile) { throw new Exception(); }
                    }
                    break;

                case "advanced":

                    try
                    {
                        var fileInfo = new FileInfo(path);
                        filename = System.IO.Path.GetFileName(path);
                        DateTime lastWriteTime = fileInfo.LastWriteTime;
                        if (lastWriteTime.CompareTo(new DateTime(2013, 2, 13)) < 0)
                        {
                            oldVersionConfigFile = true;
                            oldVersionYAdjustment = 24;
                            retbool = false;
                        }
                        if (!multifile)
                        {
                            // mark all landmarks NOT missing
                            foreach (UserRect ur in allRects)
                            {
                                while (ur.getMissing().Equals(true)) { ur.toggleMissing(); }
                            }
                        }
                        StreamReader infile = new StreamReader(path);
                        while (!infile.EndOfStream)
                        {
                            thisLine = infile.ReadLine();
                            tempArray = thisLine.Split(',');
                            if (!tempArray[0].Equals("currentRot = ") && !tempArray[0].Equals("Max d = "))
                            {
                                if (tempArray.Length != 6) { throw new FormatException(); } // tests to see if config file is from incorrect mode (basic)
                                Rectangle tempRect = new Rectangle(new Point(Convert.ToInt32(tempArray[1]), Convert.ToInt32(tempArray[2])), new Size(Convert.ToInt32(tempArray[3]), Convert.ToInt32(tempArray[4])));
                                tempUserRects.Add(new UserRect(tempRect, "dummy"));
                                tempUserRects[tempUserRects.Count - 1].SetPictureBox(this.picturebox_map, this.picturebox_missingLandmarks);
                                tempUserRects[tempUserRects.Count - 1].setMissing(Convert.ToBoolean(tempArray[5]));
                            }
                            else
                            {
                                if (tempArray[0].Equals("currentRot = "))
                                {
                                    // CASE: config file is version 2 (includes rotation info)
                                    backupRot = currentRot;
                                    while (currentRot != 0) { rotateWorkspace(false, "cw"); } // rotate map to starting rotation
                                    tempRot2 = Convert.ToInt32(tempArray[1]);
                                    while (tempRot2 != currentRot) { rotateWorkspace(false, "cw"); }
                                    currentRot = tempRot2;
                                }
                            }
                        }
                        infile.Close();
                        picturebox_map.SendToBack();
                        picturebox_map.Refresh();
                        try
                        {
                            if (allRects.Count != tempUserRects.Count) { throw new ArgumentOutOfRangeException(); }
                            for (int i = 0; i < allRects.Count; i++)
                            {
                                allRects[i].setRect(tempUserRects[i].rect.Location, tempUserRects[i].rect.Size);
                                allRects[i].setMissing(tempUserRects[i].getMissing());
                            }
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            loadSuccessful = false;
                            while (currentRot != backupRot) { rotateWorkspace(false, "cw"); } // reset rotation to original
                            MessageBox.Show("The number of on screen landmark boxes did not match the number of landmarks in the configuration file.\n\n" +
                                "Did you enter the wrong map or load the wrong configuration file?\n\n", "Message");
                        }
                        if (loadSuccessful)
                        {
                            saveConfigurationToolStripMenuItem.Enabled = true;
                            b_rotate_ccw.Enabled = true;
                            b_rotate_cw.Enabled = true;
                            button_calculate.Enabled = true;
                            button_preview.Enabled = true;
                        }
                        if (loadSuccessful && !oldVersionConfigFile && !multifile) { MessageBox.Show("Configuration Loaded", "Message"); }
                        if (loadSuccessful && oldVersionConfigFile && !multifile) { MessageBox.Show("Configuration Loaded\nNote: Old version configuration file loaded and converted.\nPlease save a new configuration file.", "Message"); }
                    }
                    catch (NullReferenceException)
                    {
                        MessageBox.Show("Loading configuration failed.\n\nHave you specified the map?", "Message");
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Loading configuration failed.\n\n" +
                        "Filename: " + filename + "\n\n" +
                        "Possible Solutions:\n\n" +
                        "1. The file may not be a valid configuration file. You may have loaded an analysis file for example.\n\n" +
                        "2. The file may be valid but was created in a different mode. Make sure the configuration file was created in " + currentMode + " mode.", "Message");
                    }
                    break;
            }
            return retbool;
        }  

        private string saveConfiguration(string sn = "") // save configuration, opt param is file name of saved file
        {
            string currentMode = Properties.Settings.Default.currentMode;
            string fullPath = "";
            bool alm = allLabelsMoved();
            string saveName = sn;
            bool autoWrite = false;
            switch (currentMode)
            {
                case "basic":
                    try
                    {
                        if (!alm) { throw new System.ArgumentException(); }
                        if (maxDist == -1.0) { calculateMaxDistance(); }
                        DateTime now = DateTime.Now;
                        if (!Directory.Exists(Application.StartupPath + "\\Configurations"))
                        {
                            Directory.CreateDirectory(Application.StartupPath + "\\Configurations");
                        }
                        if (saveName.Equals("")) { saveName = Microsoft.VisualBasic.Interaction.InputBox("Enter the file name", "Save Configuration File"); }
                        else { autoWrite = true; }
                        if (!saveName.Equals(""))
                        {
                            fullPath = Application.StartupPath + "//Configurations//ConfigBasic_" + saveName + "_map_" + cb_coordsFiles.SelectedItem + "_" +
                                string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", now) + ".csv"; ;
                            configOutfile = new StreamWriter(fullPath);
                            fullPath = Application.StartupPath + "\\Configurations\\ConfigBasic_" + saveName + "_map_" + cb_coordsFiles.SelectedItem + "_" +
                                string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", now) + ".csv"; ;
                            for (int i = 0; i < nodes.Length; i++)
                            {
                                configOutfile.WriteLine(i + "," + nodes[i].Left + "," + nodes[i].Top);
                            }
                            configOutfile.WriteLine("currentRot = ," + currentRot.ToString());
                            configOutfile.WriteLine("Max d = ," + maxDist);
                            configOutfile.Close();
                            if (!autoWrite)
                            {
                                MessageBox.Show("Configuration Saved\n\nFile Location:\n\n" + fullPath, "Message");
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Unable to save configuration!\n\nDid you arrange all of the landmark labels?\n\n", "Message");
                    }
                    break;
                case "advanced":
                    try
                    {
                        if (!alm) { throw new System.ArgumentException(); }
                        fillParticipantsMapCoordinates();
                        if (maxDist == -1.0) { calculateMaxDistance(); } // need to implement for advanced mode
                        DateTime now = DateTime.Now;
                        if (!Directory.Exists(Application.StartupPath + "\\Configurations"))
                        {
                            Directory.CreateDirectory(Application.StartupPath + "\\Configurations");
                        }
                        if (saveName.Equals("")) { saveName = Microsoft.VisualBasic.Interaction.InputBox("Enter the file name", "Save Configuration File"); }
                        else { autoWrite = true; }
                        if (!saveName.Equals(""))
                        {
                            fullPath = Application.StartupPath + "//Configurations//ConfigAdvanced_" + saveName + "_map_" + cb_coordsFiles.SelectedItem + "_" +
                                string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", now) + ".csv"; ;
                            configOutfile = new StreamWriter(fullPath);
                            fullPath = Application.StartupPath + "\\Configurations\\ConfigAdvanced_" + saveName + "_map_" + cb_coordsFiles.SelectedItem + "_" +
                                string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", now) + ".csv"; ;
                            for (int i = 0; i < allRects.Count; i++)
                            {
                                configOutfile.WriteLine(i + "," + allRects[i].getRect().Location.X.ToString() + "," + allRects[i].getRect().Location.Y.ToString() + "," + allRects[i].getRect().Size.Width.ToString() + "," + allRects[i].getRect().Size.Height.ToString() + "," + allRects[i].getMissing().ToString());
                            }
                            configOutfile.WriteLine("currentRot = ," + currentRot.ToString());
                            configOutfile.WriteLine("Max d = ," + maxDist); // need to implement for advanced mode
                            configOutfile.Close();
                            if (!autoWrite)
                            {
                                MessageBox.Show("Configuration Saved\n\nFile Location:\n\n" + fullPath, "Message");
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Unable to save configuration!\n\nDid you arrange all of the landmark boxes?\n\n", "Message");
                    }
                    break;
            }
            return fullPath;
        }

        private void takeScreenshot() // need comment
        {
            // reset fileName, to remove any previous saved file paths
            d_saveImage.FileName = "";
            d_saveImage.Title = "Save Screenshot";

            if (d_saveImage.ShowDialog() == DialogResult.OK)
            {
                // delay to ensure no menus are caught in the capture (.3 seconds)
                // If null expection error occurs when saving, comment out delay to resolve
                System.Threading.Thread.Sleep(300);

                Rectangle bounds = Form1.ActiveForm.Bounds; // Dimensions of Form1

                // create bitmap with the same dimension of the current active window (Form1)
                using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(Form1.ActiveForm.Location.X,
                                         Form1.ActiveForm.Location.Y,
                                         0,
                                         0,
                                         bounds.Size);
                    }

                    bitmap.Save(d_saveImage.FileName, ImageFormat.Jpeg);
                }
            }
        }

        private void batchReanalysis() // need comment
        {
            bool multifile = true;
            loadConfigOpenDialog(multifile);
        }

        private void reset() // need comment
        {
            // reinitialize all global vars
            x = 0;
            y = 0;
            currentRot = 0;
            maxDist = -1.0;
            if (actualMapCoordinates != null) { for (int i = 0; i < actualMapCoordinates.Length; i++) { actualMapCoordinates[i] = null; } }
            if (participantsMapCoordinates != null) { for (int i = 0; i < participantsMapCoordinates.Length; i++) { participantsMapCoordinates[i] = null; } }
            if (allCombos != null) { for (int i = 0; i < allCombos.Length; i++) { allCombos[i] = null; } }
            picturebox_map.Image = Properties.Resources.homeScreen;
            picturebox_map.Tag = "homeScreen";
            picturebox_map.Invalidate();
            for (int i = 0; i < colorPalette.Length; i++)
            {
                foreach (PictureBox pb in colorPalette[i])
                {
                    pb.Visible = false;
                    pb.BorderStyle = BorderStyle.None;
                    pb.Padding = new Padding(0);
                }
            }
            removeNodes();
            lastMovedNode = null;
            landmarkNames = new List<string>();
            landmarkTTs = new List<ToolTip>();
            // blank fields and disable/enable controls
            tb_legend.Text = "";
            textbox_numLandmarks.Text = "";
            toolStripStatusLabel.Text = "Home Screen";
            textbox_numLandmarks.Enabled = false;
            button_calculate.Enabled = false;
            button_preview.Enabled = false;
        //    b_rotate_ccw.Enabled = false;
         //   b_rotate_cw.Enabled = false;
            saveConfigurationToolStripMenuItem.Enabled = false;
            batchReanalysisToolStripMenuItem.Enabled = false;
            mainMenu.Focus();
            cb_coordsFiles.Items.Clear();
            loadForm();
        }

        private void restoreDefaults() // need comment
        {
            Properties.Settings.Default.prevCoordsFile = "template";
            Properties.Settings.Default.prevZoomLevel = 4;
            Properties.Settings.Default.currentMode = "basic";
            Properties.Settings.Default.rectColor = 0;
            Properties.Settings.Default.rectCentColor = 2;
            Properties.Settings.Default.BDR_IV = "actual";
            Properties.Settings.Default.movableZoomWindow = true;
            Properties.Settings.Default.Save();
            reset();
        }

        private void rotateWorkspace(bool moveLabels, string direction) // rotates map image and landmark labels 90 degrees CW
        {
            string currentMode = Properties.Settings.Default.currentMode;
            int numCWRots = 1;
            if (direction.Equals("ccw")) { numCWRots = 3; }
            Bitmap tempBitmap = new Bitmap(picturebox_map.Image);
            for (int i = 0; i < numCWRots; i++) { tempBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone); }    
            picturebox_map.Image = tempBitmap;
            if (moveLabels)
            {
                switch (currentMode)
                {
                    case "basic":

                        if (picturebox_map.Image != null)
                        {
                            for (int i = 0; i < numCWRots; i++)
                            {
                                foreach (PictureBox P in nodes)
                                {
                                    if (P.Top < 734 && P.Left < 706)
                                    {
                                        int j = P.Left;
                                        P.Left = (724 - P.Location.Y) + 10;
                                        P.Top = j + 24;
                                    }
                                }
                                currentRot++;
                                if (currentRot == 4) { currentRot = 0; }
                            }
                        }
                        break;
                    case "advanced":
                        if (allRects != null)
                        {
                            for (int i = 0; i < numCWRots; i++)
                            {
                                foreach (UserRect ur in allRects)
                                {
                                    if (ur.getMissing() == false)
                                    {
                                        if (currentRot % 2 == 0)
                                        {
                                            int tempWidth = ur.rect.Width;
                                            ur.rect.Width = ur.rect.Height;
                                            ur.rect.Height = tempWidth;
                                        }
                                        else
                                        {
                                            int tempHeight = ur.rect.Height;
                                            ur.rect.Height = ur.rect.Width;
                                            ur.rect.Width = tempHeight;
                                        }
                                        int j = ur.rect.X;
                                        ur.rect.X = (700 - ur.rect.Y - ur.rect.Width);
                                        ur.rect.Y = j;
                                    }
                                }
                                currentRot++;
                                if (currentRot == 4) { currentRot = 0; }
                            }
                        }
                        break;
                }
            }
            else
            {
                currentRot++;
                if (currentRot == 4) { currentRot = 0; }
            }
        }
        
        // Helper Functions

        private void toggleMode() // toggles mode of program, basic -> advanced, advanced -> basic
        {
            string currentMode = Properties.Settings.Default.currentMode;
            switch (currentMode)
            {
                case "basic":
                    lb_basicMode.BackColor = Color.Yellow;
                    lb_advancedMode.BackColor = SystemColors.Control;
                    movableZoomWindowToolStripMenuItem.Enabled = true;
                    break;
                case "advanced":
                    lb_advancedMode.BackColor = Color.Yellow;
                    lb_basicMode.BackColor = SystemColors.Control;
                    movableZoomWindowToolStripMenuItem.Enabled = false;
                    break;
            }
        }

        public static Image GetImage(string name) // returns landmark label image from Application's Resources
        {
            return (Image)typeof(MapDrawingAnalyzer.Properties.Resources).GetProperty(name, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).GetValue(null, null);
        }

        private bool landmarkMissing(int landmarkNum) // checks if landmark (as denoted by landmarkNum) is missing, returns true if missing
        {
            string currentMode = Properties.Settings.Default.currentMode;
            bool retbool = false;
            switch (currentMode)
            {
                case "basic":
                    if (nodes[landmarkNum - 1].Top < 734) // if the landmark label is not in the missing landmark box
                    {
                        retbool = false;
                    }
                    else { retbool = true; }
                    break;
                case "advanced":
                    int index = (landmarkNum - 1) / 8;
                    if (allRects[index].getMissing() == true) { retbool = true; }
                    break;
            }
            return retbool;


        }

        private bool allLabelsMoved() // checks if all the landmark labels were moved by user, yes = returns true
        {
            string currentMode = Properties.Settings.Default.currentMode;
            bool allLabelsMoved = true;
            switch (currentMode)
            {
                case "basic":
                    if (nodes != null)
                    {
                        for (int i = 0; i < nodes.Length; i++)
                        {
                            if (nodes[i].Location.X.Equals(labelStartPos[i].X) && nodes[i].Location.Y.Equals(labelStartPos[i].Y))
                            {
                                // one of the labels was not moved
                                allLabelsMoved = false;
                            }
                        }
                    }
                    else { allLabelsMoved = false; }
                    break;
                case "advanced":
                    if (allRects.Count != 0)
                    {
                        for (int i = 0; i < allRects.Count; i++)
                        {
                            if (allRects[i].rect.Location.X.Equals(boxStartPos[i].X) && allRects[i].rect.Location.Y.Equals(boxStartPos[i].Y))
                            {
                                // one of the labels was not moved
                                allLabelsMoved = false;
                            }
                        }
                    }
                    else { allLabelsMoved = false; }
                    break;
            }
            return allLabelsMoved;
        }

        private Color getRectColor(int colCode)
        {
            Color retCol = Color.Black;
            switch (colCode)
            {
                case 0:
                    retCol = Color.Red;
                    break;
                case 1:
                    retCol = Color.Green;
                    break;
                case 2:
                    retCol = Color.Blue;
                    break;
                case 3:
                    retCol = Color.Yellow;
                    break;
                case 4:
                    retCol = Color.White;
                    break;
            }
            return retCol;
        }

        private void highlightPanel(System.Windows.Forms.Panel panel)
        {
            Form pg_form = new Plexiglass.Plexiglass(panel, this);
            for (int i = 0; i < 4; i++)
            {
                pg_form.Show();
                System.Threading.Thread.Sleep(100);
                pg_form.Hide();
                System.Threading.Thread.Sleep(100);
            }
            pg_form.Close();
        }

        // Calculation

        private void calculate(bool isPreview, bool multifile = false, string fileName = "") // calculate measures, opt params: are multiple files being processed, file name of saved file
        {
            DateTime now = DateTime.Now;
            int numCorrect = 0;
            int numMissing = 0;
            int numDistanceCorrect = 0;
            double distanceSum = 0.0;
            double angleSum = 0.0;
            double ABSdistanceSum = 0.0;
            double ABSangleSum = 0.0;
            double percentCorrect = 0;
            double percentObservedCorrect = 0;
            string[] tempArray;
            double OminusADistance = 0.0;
            double OminusAAngle = 0.0;
            double ABSOminusADistance = 0.0;
            double ABSOminusAAngle = 0.0;
            string currentMode = Properties.Settings.Default.currentMode;
            string saveName = "";

            bool allLabelsWereMoved = allLabelsMoved();
            if (!allLabelsWereMoved)
            {
                // add if else for current mode, to change error message
                DialogResult result;
                if (currentMode.Equals("basic")) { result = MessageBox.Show("Not all landmark labels were moved.\n\nUnable to continue with analysis.", "Message"); }
                if (currentMode.Equals("advanced")) { result = MessageBox.Show("Not all landmark boxes were moved.\n\nUnable to continue with analysis.", "Message"); }
                return;
            }

            if (!isPreview)
            {
                if (!Directory.Exists(Application.StartupPath + "\\Data"))
                {
                    Directory.CreateDirectory(Application.StartupPath + "\\Data");
                }
                if (!multifile) { saveName = Microsoft.VisualBasic.Interaction.InputBox("Enter the file name", "Calculate and Save Data File"); }
                else { saveName = fileName; }
                if (!saveName.Equals(""))
                {
                    if (!multifile)
                    {
                        outfile = new StreamWriter(Application.StartupPath + "//Data//GMDA_" + saveName + "_map_" + cb_coordsFiles.SelectedItem + "_" +
                            string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", now) + ".csv");
                    }
                    else
                    {
                        outfile = new StreamWriter(Application.StartupPath + "//Data//GMDA_" + saveName + ".csv");
                    }
                }
                else { return; }
                // write header
                outfile.WriteLine("Source,Target,Actual(N/S),Actual(E/W),Observed(N/S),Observed(E/W),Actual D ratio,Actual Angle,Observed D ratio,Observed Angle,O-A Distance,O-A Angle,ABS(O-A) Distance, ABS(O-A) Angle");
            }

            // get cartesian coordinates of landmark labels, store in participantsMapCoordinates for bidimensional regression

            fillParticipantsMapCoordinates();
            pb_calculate.Value += 17; // 1
            pb_calculate.Value--;
            // Table of Contents of allCombos[7]
            // index 0 = first landmark in landmark pair
            // index 1 = 2nd landmark in landmark pair
            // index 2 = N/S judgment between landmark pair
            // index 3 = E/W judgment between landmark pair
            // index 4 = distance between landmark pair
            // index 5 = distance ratio between landmark pair
            // index 6 = angle between landmark pair

            generateCanonicalJudgments(ref allCombos, ref actualMapCoordinates);
     //       if(currentMode.Equals("advanced")){generateCanonicalJudgments(ref allBoxCombos, ref actualMapCoordinates);}
            pb_calculate.Value += 17;// 2
            pb_calculate.Value--;
            calculateDistancesAndAngles(ref allCombos, ref actualMapCoordinates);
            if (currentMode.Equals("advanced")) { calculateDistancesAndAngles(ref allBoxCombos, ref actualMapCoordinates); }
            pb_calculate.Value += 17;// 3
            pb_calculate.Value--;
            // all calculations of model environment calculated

            // calculate max Dist for user's configuration
            calculateMaxDistance();
            pb_calculate.Value += 17;// 4
            pb_calculate.Value--;
 
            double[] BDR_parameters = computeBDR(actualMapCoordinates, participantsMapCoordinates);
            pb_calculate.Value += 17;// 5
            pb_calculate.Value--;
            for (int i = 0; i < allBoxCombos[0].Count; i++) // make comparisons, update counters
            {
                tempArray = compareLandmarks((Convert.ToInt32(allBoxCombos[0][i]) + 1), (Convert.ToInt32(allBoxCombos[1][i]) + 1));
                if (tempArray[0].Equals(allBoxCombos[2][i])) { numCorrect++; }
                if (tempArray[1].Equals(allBoxCombos[3][i])) { numCorrect++; }
                if (tempArray[0].Equals("M"))
                {
                    numMissing += 2;
                    OminusADistance = 0.0;
                    ABSOminusADistance = 0.0;
                    OminusAAngle = 0.0;
                    ABSOminusAAngle = 0.0;
                }
                if (!tempArray[0].Equals("M"))
                {
                    OminusADistance = Convert.ToDouble(tempArray[2]) - Convert.ToDouble(allBoxCombos[5][i]);
                    ABSOminusADistance = Math.Abs(OminusADistance);

                    distanceSum += OminusADistance;
                    ABSdistanceSum += ABSOminusADistance;

                    OminusAAngle = Convert.ToDouble(tempArray[3]) - Convert.ToDouble(allBoxCombos[6][i]);
                    // Convert angle differences to +- 180 scale - update as of august 2012
                    if (OminusAAngle <= -180.0) { OminusAAngle = 360 + OminusAAngle; }
                    if (OminusAAngle >= 180.0) { OminusAAngle = -(360 - OminusAAngle); }
                    ABSOminusAAngle = Math.Abs(OminusAAngle);

                    angleSum += OminusAAngle;
                    ABSangleSum += ABSOminusAAngle;
                    numDistanceCorrect++;
                }
                if (!isPreview)
                {
                    int sourceLandmarkIndex = 0;
                    int targetLandmarkIndex = 0;
                    string sourceLandmark = "";
                    string targetLandmark = "";
                    switch (currentMode)
                    {
                        case "basic":
                            sourceLandmarkIndex = Convert.ToInt32(allCombos[0][i]);
                            targetLandmarkIndex = Convert.ToInt32(allCombos[1][i]);
                            sourceLandmark = landmarkNames[sourceLandmarkIndex];
                            targetLandmark = landmarkNames[targetLandmarkIndex];
                            break;
                        case "advanced":
                            sourceLandmarkIndex = Convert.ToInt32(allCombos[0][i]) / 8;
                            targetLandmarkIndex = Convert.ToInt32(allCombos[1][i]) / 8;
                            sourceLandmark = landmarkNames[sourceLandmarkIndex] + "_" + allCombos[0][i];
                            targetLandmark = landmarkNames[targetLandmarkIndex] + "_" + allCombos[1][i];
                            break;
                    }
                    // Write calculations to outfile
                    
                    outfile.WriteLine(sourceLandmark + "," // Source Landmark
                        + targetLandmark + "," // Target Landmark
                        + allCombos[2][i] + "," // Actual(N/S)
                        + allCombos[3][i] + "," // Actual(E/W)
                        + tempArray[0] + "," // Observed(N/S)
                        + tempArray[1] // Observed(E/W)
                        + "," + allCombos[5][i].ToString() // Actual D ratio
                        + "," + allCombos[6][i].ToString() // Actual Angle
                        + "," + tempArray[2] // Observed D ratio
                        + "," + tempArray[3] // Observed Angle
                        + "," + OminusADistance.ToString() // O-A Distance
                        + "," + OminusAAngle.ToString() // O-A Angle
                        + "," + ABSOminusADistance.ToString() // ABS(O-A) Distance
                        + "," + ABSOminusAAngle.ToString()); // ABS(O-A) Angle
                }
            }
            // loop completed
            // Info at end of output file
            pb_calculate.Maximum = 1000;
            pb_calculate.Value = 1000;
            pb_calculate.Value = 999;
            pb_calculate.Maximum = 100;
            double numComparisons = allCombos[0].Count * 2.0;
            percentCorrect = numCorrect / numComparisons;
            percentObservedCorrect = numCorrect / (numComparisons - numMissing);
            numMissing = 0;
            switch (currentMode)
            {
                case "basic":
                    for (int i = 0; i < landmarkNames.Count; i++) { if (landmarkMissing(i + 1)) { numMissing++; } }
                    break;
                case "advanced":
                    for (int i = 0; i < landmarkNames.Count; i++) { if (allRects[i].getMissing() == true) { numMissing++; } }
                    break;
            }


            if (!isPreview)
            {
                outfile.WriteLine("\nGMDA Measures\n");
                outfile.WriteLine("Total Correct = ," + numCorrect);
                outfile.WriteLine("Canonical Organization = ," + percentCorrect);
                outfile.WriteLine("SQRT(Canonical Organization) = ," + Math.Sqrt(percentCorrect));
                outfile.WriteLine("Canonical Accuracy = ," + percentObservedCorrect);
                outfile.WriteLine("Num Landmarks Missing = ," + numMissing);
                outfile.WriteLine("O-A distance mean = ," + distanceSum / numDistanceCorrect);
                outfile.WriteLine("O-A angle mean = ," + angleSum / numDistanceCorrect);
                outfile.WriteLine("O-A distance mean (ABS) = ," + ABSdistanceSum / numDistanceCorrect);
                outfile.WriteLine("O-A angle mean (ABS) = ," + ABSangleSum / numDistanceCorrect);
                outfile.WriteLine("Distance Accuracy = ," + (1 - (ABSdistanceSum / numDistanceCorrect)).ToString());
                outfile.WriteLine("Angle Accuracy = ," + (1 - ((ABSangleSum / numDistanceCorrect) / 180.0)).ToString());
                outfile.WriteLine("\nBDR Parameters\n");
                if (BDR_parameters[0] == 0.0) { outfile.WriteLine("Unable to calculate BDR parameters"); }
                else
                {
                    // write BDR parameters
                    outfile.WriteLine("r = ," + Convert.ToString(BDR_parameters[1]));
                    outfile.WriteLine("alpha 1 = ," + Convert.ToString(BDR_parameters[2]));
                    outfile.WriteLine("alpha 2 = ," + Convert.ToString(BDR_parameters[3]));
                    outfile.WriteLine("beta 1 = ," + Convert.ToString(BDR_parameters[4]));
                    outfile.WriteLine("beta 2 = ," + Convert.ToString(BDR_parameters[5]));
                    outfile.WriteLine("scale = ," + Convert.ToString(BDR_parameters[6]));
                    outfile.WriteLine("theta = ," + Convert.ToString(BDR_parameters[7]));
                    outfile.WriteLine("DMax = ," + Convert.ToString(BDR_parameters[8]));
                    outfile.WriteLine("D = ," + Convert.ToString(BDR_parameters[9]));
                    outfile.WriteLine("DI = ," + Convert.ToString(BDR_parameters[10]));
                    outfile.WriteLine("\nLandmark,Independent,,Dependent,,Predicted");
                    outfile.WriteLine(",X,Y,A,B,A',B'");
                    // write Independent, Dependent, and Predicted values from BDR;
                    string currentBDR_mode = Properties.Settings.Default.BDR_IV;
                    for (int j = 0; j < A_prime_B_prime[0].Count; j++)
                    {
                        double x = 0.0, y = 0.0, a = 0.0, b = 0.0;
                        int currentIndex = (int)A_prime_B_prime[0][j];
                        if(currentMode.Equals("basic")){currentIndex--;}
                        switch (currentBDR_mode)
                        {
                            case "actual":
                                x = Convert.ToDouble(actualMapCoordinates[1][currentIndex]);
                                y = Convert.ToDouble(actualMapCoordinates[2][currentIndex]);
                                a = Convert.ToDouble(participantsMapCoordinates[1][currentIndex]);
                                b = Convert.ToDouble(participantsMapCoordinates[2][currentIndex]);
                                break;
                            case "participant's":
                                x = Convert.ToDouble(participantsMapCoordinates[1][currentIndex]);
                                y = Convert.ToDouble(participantsMapCoordinates[2][currentIndex]);
                                a = Convert.ToDouble(actualMapCoordinates[1][currentIndex]);
                                b = Convert.ToDouble(actualMapCoordinates[2][currentIndex]);
                                break;
                        }
                        //= Convert.ToDouble(actualMapCoordinates[1][j]);
                        
                        double a_prime = A_prime_B_prime[1][j];
                        double b_prime = A_prime_B_prime[2][j];
                        if (currentMode.Equals("advanced")) { currentIndex = currentIndex / 8; }
                        string l = landmarkNames[currentIndex];
                        if (currentMode.Equals("advanced")) { l += j.ToString(); }
                        outfile.WriteLine(l + "," + x + "," + y + "," + a + "," + b + "," + a_prime + "," + b_prime);
                    }
                }
                outfile.Close();
            }
            string messageBoxString = "";
            if (isPreview) { messageBoxString = "Preview Mode\n\n"; }
            else
            {
                messageBoxString = "Data file saved\n\n";
                messageBoxString += "File Location:\n\n" + Application.StartupPath + "\\Data\\GMDA_" + saveName + "_map_" + cb_coordsFiles.SelectedItem + "_" +
                string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", now) + ".csv";
                messageBoxString += "\n\n";
                if (!multifile && !isPreview)
                {
                    string configPath = saveConfiguration(saveName);
                    messageBoxString += "Configuration file saved\n\n";
                    messageBoxString += "File Location:\n\n" + configPath + "\n\n";
                }
            }
            messageBoxString += "GMDA Measures:\n\n" +
                                "Canonical Organization = " + Math.Round(percentCorrect, 3) +
                                "\nSQRT(Canonical Organization) = " + Math.Round(Math.Sqrt(percentCorrect), 3) +
                                "\nCanonical Accuracy = " + Math.Round(percentObservedCorrect, 3) +
                                "\nO-A distance mean (ABS) = " + Math.Round(ABSdistanceSum / numDistanceCorrect, 3) +
                                "\nO-A angle mean (ABS) = " + Math.Round(ABSangleSum / numDistanceCorrect, 3) +
                                "\nDistance Accuracy = " + Math.Round((1 - (ABSdistanceSum / numDistanceCorrect)), 3).ToString() +
                                "\nAngle Accuracy = " + Math.Round((1 - ((ABSangleSum / numDistanceCorrect) / 180.0)), 3).ToString();
            if (BDR_parameters[0] == 0.0)
            {
                messageBoxString += "\n\nUnable to calculate BDR parameters";
            }
            else
            {
                messageBoxString += "\n\nBDR Parameters\n\n" +
                                    "r = " + Convert.ToString(Math.Round(BDR_parameters[1], 3)) +
                                    "\nalpha 1 = " + Convert.ToString(Math.Round(BDR_parameters[2], 3)) +
                                    "\nalpha 2 = " + Convert.ToString(Math.Round(BDR_parameters[3], 3)) +
                                    "\nbeta 1 = " + Convert.ToString(Math.Round(BDR_parameters[4], 3)) +
                                    "\nbeta 2 = " + Convert.ToString(Math.Round(BDR_parameters[5], 3)) +
                                    "\nscale = " + Convert.ToString(Math.Round(BDR_parameters[6], 3)) +
                                    "\ntheta = " + Convert.ToString(Math.Round(BDR_parameters[7], 3)) +
                                    "\nDMax = " + Convert.ToString(Math.Round(BDR_parameters[8], 3)) +
                                    "\nD = " + Convert.ToString(Math.Round(BDR_parameters[9], 3)) +
                                    "\nDI = " + Convert.ToString(Math.Round(BDR_parameters[10], 3));
            }

            if (!multifile) { MessageBox.Show(messageBoxString, "Data"); }
            pb_calculate.Value = 0;
        }

        private string[] compareLandmarks(int L1, int L2)// compares two landmarks on participant's map using categorical NSEW measures and quantitative distance and angle measures
        {
            string currentMode = Properties.Settings.Default.currentMode;
            string[] NSEW = new string[4];
            // Canonical Comparison
            if (!landmarkMissing(L1) & !landmarkMissing(L2))// If the landmarks labels are not in the missing landmark zone...
            {
                switch (currentMode)
                {
                    case "basic":
                        if (nodes[L2 - 1].Top - nodes[L1 - 1].Top > 0) { NSEW[0] = "N"; }
                        if (nodes[L2 - 1].Top - nodes[L1 - 1].Top < 0) { NSEW[0] = "S"; }
                        if (nodes[L2 - 1].Top - nodes[L1 - 1].Top == 0) { NSEW[0] = "F"; }// If on the same Y axis, code as wrong
                        if (nodes[L2 - 1].Left - nodes[L1 - 1].Left > 0) { NSEW[1] = "W"; }
                        if (nodes[L2 - 1].Left - nodes[L1 - 1].Left < 0) { NSEW[1] = "E"; }
                        if (nodes[L2 - 1].Left - nodes[L1 - 1].Left == 0) { NSEW[1] = "F"; }// If on the same X axis, code as wrong
                        break;
                    case "advanced":
                        int l1_x = Convert.ToInt32(participantsMapCoordinates[1][L1 - 1]);
                        int l1_y = Convert.ToInt32(participantsMapCoordinates[2][L1 - 1]);
                        int l2_x = Convert.ToInt32(participantsMapCoordinates[1][L2 - 1]);
                        int l2_y = Convert.ToInt32(participantsMapCoordinates[2][L2 - 1]);

                        if (l1_y > l2_y) { NSEW[0] = "N"; }
                        if (l1_y < l2_y) { NSEW[0] = "S"; }
                        if (l1_y == l2_y) { NSEW[0] = "F"; }
                        if (l1_x > l2_x) { NSEW[1] = "E"; }
                        if (l1_x < l2_x) { NSEW[1] = "W"; }
                        if (l1_x == l2_x) { NSEW[1] = "F"; }
                        break;
                }
                
            }
            else
            {
                // One or both of L1 and L2 is missing
                NSEW[0] = "M";
                NSEW[1] = "M";
                NSEW[2] = "M";
                NSEW[3] = "M";
            }
            // Quantitative Comparison
            if (!NSEW[0].Equals("M")) // only do this comparison if the two landmarks are not missing
            {
                NSEW[2] = getDistanceRatio(L1, L2);
                NSEW[3] = getAngle(L1, L2);
            }
            // NSEW [0] = NS relationship, [1] = EW, [2] = distance ratio, [3] = angle
            return NSEW;
        }

        private string getDistanceRatio(int L1, int L2)// L1 = Source, L2 = Target, gets distance between L1 and L2 relative to max distance
        {
            string distanceRatio = "";
            double dblDistanceRatio = 0.0;
            dblDistanceRatio = getDistance(L1, L2) / maxDist;
            distanceRatio = dblDistanceRatio.ToString();
            return distanceRatio;
        }

        private double getDistance(int L1, int L2) // L1 = Source, L2 = Target, gets distance between L1 and L2
        {
            string currentMode = Properties.Settings.Default.currentMode;
            double distance = 0.0;
            PointD p_L1 = new PointD(0.0,0.0);
            PointD p_L2 = new PointD(0.0,0.0);
            switch (currentMode)
            {
                case "basic":
                    p_L1 = getCartesianCoords(L1);
                    p_L2 = getCartesianCoords(L2);
                    break;
                case "advanced":
                    p_L1 = new PointD(Convert.ToDouble(participantsMapCoordinates[1][L1 - 1]), Convert.ToDouble(participantsMapCoordinates[2][L1 - 1]));
                    p_L2 = new PointD(Convert.ToDouble(participantsMapCoordinates[1][L2 - 1]), Convert.ToDouble(participantsMapCoordinates[2][L2 - 1]));
                    break;
            }
            
            distance = Math.Sqrt(Math.Pow((p_L2.X - p_L1.X), 2) + Math.Pow((p_L2.Y - p_L1.Y), 2));
            return distance;
        }

        private string getAngle(int L1, int L2) // L1 = Source, L2 = Target, gets angle between L1 and L2
        {
            string currentMode = Properties.Settings.Default.currentMode;
            string angle = "";
            double dblAngle = 0.0;
            PointD p_L1 = new PointD(0.0, 0.0);
            PointD p_L2 = new PointD(0.0, 0.0);
            switch (currentMode)
            {
                case "basic":
                    p_L1 = getCartesianCoords(L1);
                    p_L2 = getCartesianCoords(L2);
                    break;
                case "advanced":
                    p_L1 = new PointD(Convert.ToDouble(participantsMapCoordinates[1][L1 - 1]), Convert.ToDouble(participantsMapCoordinates[2][L1 - 1]));
                    p_L2 = new PointD(Convert.ToDouble(participantsMapCoordinates[1][L2 - 1]), Convert.ToDouble(participantsMapCoordinates[2][L2 - 1]));
                    break;
            }
            
            dblAngle = Math.Atan2(p_L2.X - p_L1.X, p_L2.Y - p_L1.Y);
            dblAngle = (180.0 / Math.PI) * dblAngle;// convert rads to degrees
            angle = dblAngle.ToString();
            return angle;
        }

        private PointD getCartesianCoords(int landmarkNum) // converts c# coordinate system to cartesian plane
        {
            PointD retPoint = new PointD(0.0, 0.0);
            retPoint.X = (0 - (350 + (picturebox_map.Location.X) - nodes[landmarkNum - 1].Left)); // picturebox location is 5,29
            retPoint.Y = (0 + (350 + (picturebox_map.Location.Y) - nodes[landmarkNum - 1].Top));
            return retPoint;
        }

        private void calculateMaxDistance() // calculate max distance between landmarks in participant's map
        {
            string currentMode = Properties.Settings.Default.currentMode;
            string[] tempArray;
            double currentDist = 0.0;
            for (int i = 0; i < allCombos[0].Count; i++)
            {
                tempArray = compareLandmarks((Convert.ToInt32(allCombos[0][i]) + 1), (Convert.ToInt32(allCombos[1][i]) + 1));
                if (!tempArray[0].Equals("M")) // if both landmarks are not missing
                {
                    currentDist = getDistance((Convert.ToInt32(allCombos[0][i]) + 1), (Convert.ToInt32(allCombos[1][i]) + 1)); // get distance b/w the two landmarks
                    if (currentDist > maxDist)
                    {
                        maxDist = currentDist; // update max if nec
                    }
                }
            }
            if (currentMode.Equals("advanced"))
            {
                currentDist = 0.0;
                for (int i = 0; i < allBoxCombos[0].Count; i++)
                {
                    tempArray = compareLandmarks((Convert.ToInt32(allBoxCombos[0][i]) + 1), (Convert.ToInt32(allBoxCombos[1][i]) + 1));
                    if (!tempArray[0].Equals("M")) // if both landmarks are not missing
                    {
                        currentDist = getDistance((Convert.ToInt32(allBoxCombos[0][i]) + 1), (Convert.ToInt32(allBoxCombos[1][i]) + 1)); // get distance b/w the two landmarks
                        if (currentDist > maxInterBoxDist)
                        {
                            maxInterBoxDist = currentDist; // update max if nec
                        }
                    }
                }
            }
        }

        private void generateCombinations(ref List<string>[] allCombos, int numLandmarks) // generates all possible pairwise comparisons of landmarks
        {
            string currentMode = Properties.Settings.Default.currentMode;
            switch (currentMode)
            {
                case "basic":
                    for (int i = 0; i < numLandmarks; i++)
                    {
                        int j = i;
                        while (j < numLandmarks)
                        {
                            if (i != j)
                            {
                                allCombos[0].Add(Convert.ToString(i));
                                allCombos[1].Add(Convert.ToString(j));
                            }
                            j++;
                        }
                    }
                    break;
                case "advanced":
                    for (int i = 0; i < numLandmarks - 8; i++)
                    {
                        for (int j = (i/8 + 1) * 8; j < numLandmarks; j++)
                        {
                            allCombos[0].Add(Convert.ToString(i));
                            allCombos[1].Add(Convert.ToString(j));
                        }                 
                    }
                    int stop = 8;
                    for (int i = 0; i < numLandmarks; i++) // 0 - 31
                    {
                        if (i % 8 == 0 && i !=0) { stop += 8; }
                        for (int j = i+1; j < stop; j++)
                        {
                            allBoxCombos[0].Add(Convert.ToString(i));
                            allBoxCombos[1].Add(Convert.ToString(j));
                        }
                        
                    }
                  /*  outfile = new StreamWriter(Application.StartupPath + "//Data//debug_" + cb_coordsFiles.SelectedItem + "_" +
                       ".csv");
                    for (int i = 0; i < allCombos[0].Count; i++)
                    {
                        outfile.WriteLine(allCombos[0][i].ToString() + ", " + allCombos[1][i].ToString());
                    }
                    outfile.Close();*/
                  //  MessageBox.Show(allCombos[0].ToString() + ", " + allCombos[1].ToString());
                    break;
            }
        }

        private void generateCanonicalJudgments(ref List<string>[] allCombos, ref List<double>[] actualMapCoordinates) // generate canonical judgments for actual map
        {
            for (int i = 0; i < allCombos[0].Count; i++)
            {
                double lm1_X = Convert.ToDouble(actualMapCoordinates[1][Convert.ToInt32(allCombos[0][i])]);
                double lm2_X = Convert.ToDouble(actualMapCoordinates[1][Convert.ToInt32(allCombos[1][i])]);
                double lm1_Y = Convert.ToDouble(actualMapCoordinates[2][Convert.ToInt32(allCombos[0][i])]);
                double lm2_Y = Convert.ToDouble(actualMapCoordinates[2][Convert.ToInt32(allCombos[1][i])]);
                string NSjudgment = "";
                string EWjudgment = "";
                // N/S Jugment
                if (lm1_Y <= lm2_Y)
                {
                    if (lm1_Y == lm2_Y) { NSjudgment = "F"; }
                    else { NSjudgment = "S"; }
                }
                else { NSjudgment = "N"; }
                // E/W Judgment
                if (lm1_X <= lm2_X)
                {
                    if (lm1_X == lm2_X) { EWjudgment = "F"; }
                    else { EWjudgment = "W"; }
                }
                else { EWjudgment = "E"; }
                // update allCombos
                allCombos[2].Add(NSjudgment);
                allCombos[3].Add(EWjudgment);
                
            }
        }

        private void calculateDistancesAndAngles(ref List<string>[] allCombos, ref List<double>[] actualMapCoordinates) // calculate distance ratios & angles for actual map
        {
            double currentMax = 0.0;
            // calculate distances and angles between landmark pairs and find max distance
            for (int i = 0; i < allCombos[0].Count; i++)
            {
                // calculate distance
                double lm1_X = Convert.ToDouble(actualMapCoordinates[1][Convert.ToInt32(allCombos[0][i])]);
                double lm2_X = Convert.ToDouble(actualMapCoordinates[1][Convert.ToInt32(allCombos[1][i])]);
                double lm1_Y = Convert.ToDouble(actualMapCoordinates[2][Convert.ToInt32(allCombos[0][i])]);
                double lm2_Y = Convert.ToDouble(actualMapCoordinates[2][Convert.ToInt32(allCombos[1][i])]);
                double dbl_distance = Math.Sqrt(Math.Pow((lm2_X - lm1_X), 2) + Math.Pow((lm2_Y - lm1_Y), 2));
                if (dbl_distance > currentMax) { currentMax = dbl_distance; }
                string distance = Convert.ToString(dbl_distance);
                allCombos[4].Add(distance);
                // calculate angles
                double dblAngle = Math.Atan2(lm2_X - lm1_X, lm2_Y - lm1_Y);
                dblAngle = (180.0 / Math.PI) * dblAngle;// convert rads to degrees
                allCombos[6].Add(Convert.ToString(dblAngle));
            }
            // calculate distance ratios and for landmark pairs
            for (int i = 0; i < allCombos[0].Count; i++)
            {
                double distanceRatio = Convert.ToDouble(allCombos[4][i]) / currentMax;
                if (distanceRatio > 1.0) { distanceRatio = 1.0; } // correct any double division error that may occur
                allCombos[5].Add(Convert.ToString(distanceRatio));
            }
        }

        private double[] computeBDR(List<double>[] actualMapCoordinates, List<string>[] participantsMapCoordinates)
        {
            // Coded adapted from Friedman and Kohler (2003) Psych Methods VBA macro
            // http://www.psych.ualberta.ca/~alinda/PDFs/Friedman%20Kohler%20%5b03-Psych%20Methods%5d.xls

            double[] BDR_parameters = new double[11];

            // Statistical Variables
            double mean_Of_X = 0.0, mean_Of_Y = 0.0, mean_Of_A = 0.0, mean_Of_B = 0.0;
            double sum_Of_Sq_Of_X = 0.0, sum_Of_Sq_Of_Y = 0.0, sum_Of_Sq_Of_A = 0.0, sum_Of_Sq_Of_B = 0.0;
            double sum_Of_Sq_Of_A_Prime = 0.0, sum_Of_Sq_Of_B_Prime = 0.0;
            double sum_Of_Sq_Of_X_And_Y = 0.0;
            double sum_Of_Sq_Of_A_And_B = 0.0;
            double sum_Of_Sq_Of_A_Prime_And_B_Prime = 0.0;
            double sum_Of_Products_Of_X_And_A = 0.0, sum_Of_Products_Of_Y_And_B = 0.0;
            double sum_Of_Products_Of_X_And_B = 0.0, sum_Of_Products_Of_Y_And_A = 0.0;

            // Euclidian bidimensional regression parameters

            double beta1, beta2;
            double scaleFactor, theta;
            double alpha1, alpha2;
            double r;

            // Distortion parameters

            double dMax, d, di;

            // Local Lists (Copy)
            List<double>[] actualMap = new List<double>[3];
            List<string>[] participantsMap = new List<string>[3];
            for (int i = 0; i < actualMap.Length; i++)
            {
                actualMap[i] = new List<double>(actualMapCoordinates[i]);
                participantsMap[i] = new List<string>(participantsMapCoordinates[i]);
            }

            List<double>[] participantsMapCoordinatesDBL = new List<double>[3];
            for (int i = 0; i < participantsMap.Length; i++) { participantsMapCoordinatesDBL[i] = new List<double>(); }

            BDR_parameters[0] = 0.0; // set successful return to FALSE

            // remove missing landmarks from lists

            for (int i = 0; i < participantsMap[0].Count; i++)
            {
                // Case: landmark is missing, so remove it
                if (participantsMap[1][i].Equals("M"))
                {
                    for (int j = 0; j < participantsMap.Length; j++)
                    {
                        participantsMap[j].RemoveAt(i);
                        actualMap[j].RemoveAt(i);
                    }
                    i = -1;
                }
            }

            if (participantsMap[0].Count == 0) { return BDR_parameters; } // if all landmarks missing

            // convert participant's coordinates to doubles

            for (int i = 0; i < participantsMap[0].Count; i++)
            {
                for (int j = 0; j < participantsMap.Length; j++)
                {
                    participantsMapCoordinatesDBL[j].Add(Convert.ToDouble(participantsMap[j][i]));
                }
            }

            string currentBDR_mode = Properties.Settings.Default.BDR_IV;
            if (currentBDR_mode.Equals("participant's"))
            {
                // swap the two lists
                List<double>[] swap = new List<double>[3];
                for (int i = 0; i < participantsMap.Length; i++) { swap[i] = new List<double>(); }

                // copy participantsMapCoordinatesDBL into swap, overwrite participantsMapCoordinatesDBL with actual
                for (int i = 0; i < participantsMapCoordinatesDBL[0].Count; i++)
                {
                    for (int j = 0; j < participantsMapCoordinatesDBL.Length; j++)
                    {
                        swap[j].Add(participantsMapCoordinatesDBL[j][i]);
                        participantsMapCoordinatesDBL[j][i] = actualMap[j][i];
                    }
                }

                // overwrite actual map with swap
                for (int i = 0; i < participantsMapCoordinatesDBL[0].Count; i++)
                {
                    for (int j = 0; j < participantsMapCoordinatesDBL.Length; j++)
                    {
                        actualMap[j][i] = swap[j][i];
                    }
                }
            }

            // Compute the bidimensional regression parameters

            // compute averages

            mean_Of_X = actualMap[1].Average();
            mean_Of_Y = actualMap[2].Average();
            mean_Of_A = participantsMapCoordinatesDBL[1].Average();
            mean_Of_B = participantsMapCoordinatesDBL[2].Average();

            // compute sum of squares: Sum(x - mean(x))^2

            for (int i = 0; i < participantsMapCoordinatesDBL[0].Count; i++)
            {
                double x = actualMap[1][i];
                double y = actualMap[2][i];
                double a = participantsMapCoordinatesDBL[1][i];
                double b = participantsMapCoordinatesDBL[2][i];

                sum_Of_Sq_Of_X += Math.Pow(x - mean_Of_X, 2);
                sum_Of_Sq_Of_Y += Math.Pow(y - mean_Of_Y, 2);
                sum_Of_Sq_Of_A += Math.Pow(a - mean_Of_A, 2);
                sum_Of_Sq_Of_B += Math.Pow(b - mean_Of_B, 2);
            }

            sum_Of_Sq_Of_X_And_Y = sum_Of_Sq_Of_X + sum_Of_Sq_Of_Y;
            sum_Of_Sq_Of_A_And_B = sum_Of_Sq_Of_A + sum_Of_Sq_Of_B;

            // compute Sum of Products: Sum((X-Mean(X)) * (Y-Mean(Y))

            for (int i = 0; i < participantsMapCoordinatesDBL[0].Count; i++)
            {
                double x = actualMap[1][i];
                double y = actualMap[2][i];
                double a = participantsMapCoordinatesDBL[1][i];
                double b = participantsMapCoordinatesDBL[2][i];

                sum_Of_Products_Of_X_And_A += (x - mean_Of_X) * (a - mean_Of_A);
                sum_Of_Products_Of_Y_And_B += (y - mean_Of_Y) * (b - mean_Of_B);
                sum_Of_Products_Of_X_And_B += (x - mean_Of_X) * (b - mean_Of_B);
                sum_Of_Products_Of_Y_And_A += (y - mean_Of_Y) * (a - mean_Of_A);
            }

            if (sum_Of_Sq_Of_X == 0.0 || sum_Of_Sq_Of_Y == 0.0 || sum_Of_Sq_Of_A == 0.0 || sum_Of_Sq_Of_B == 0.0)
            {
                MessageBox.Show("Error: Cannot calculate bidimensional regression parameters. One or more of the variances is zero.", "Message");
                return BDR_parameters;
            }
            // compute euclidean bidimensional regression parameters
            beta1 = (sum_Of_Products_Of_X_And_A + sum_Of_Products_Of_Y_And_B) / sum_Of_Sq_Of_X_And_Y;
            beta2 = (sum_Of_Products_Of_X_And_B - sum_Of_Products_Of_Y_And_A) / sum_Of_Sq_Of_X_And_Y;
            scaleFactor = Math.Sqrt(Math.Pow(beta1, 2) + Math.Pow(beta2, 2));
            theta = Math.Atan(beta2 / beta1);

            // adjust theta for four quadrants (-90 to +270 degrees)
            if (beta1 < 0) { theta = theta + Math.PI; }
            theta = theta *(180.0 / Math.PI); // convert from radians to degrees
            alpha1 = mean_Of_A - beta1 * mean_Of_X + beta2 * mean_Of_Y;
            alpha2 = mean_Of_B - beta2 * mean_Of_X - beta1 * mean_Of_Y;

            // compute predicted pairs (primes), their sums, and sums of squares using equation (4) on page ???13

            // create List container for A' and B' values
            A_prime_B_prime = new List<double>[participantsMapCoordinatesDBL.Length];
            for (int i = 0; i < A_prime_B_prime.Length; i++)
            {
                A_prime_B_prime[i] = new List<double>();
                for (int j = 0; j < participantsMapCoordinatesDBL[0].Count; j++)
                {
                    A_prime_B_prime[i].Add(actualMap[i][j]);
                }
            }


            for (int i = 0; i < participantsMapCoordinatesDBL[0].Count; i++)
            {
                double ap = 0.0;
                double bp = 0.0;
                double x = actualMap[1][i];
                double y = actualMap[2][i];

                // compute A prime and B prime

                ap = alpha1 + beta1 * x - beta2 * y;
                bp = alpha2 + beta2 * x + beta1 * y;
                A_prime_B_prime[1][i] = ap;
                A_prime_B_prime[2][i] = bp;

                // compute the sum of squares (note: the means for A' and B' are always the same as for A and B, so means for A' and B' are not computed separately here

                sum_Of_Sq_Of_A_Prime += Math.Pow((ap - mean_Of_A), 2);
                sum_Of_Sq_Of_B_Prime += Math.Pow((bp - mean_Of_B), 2);
            }

            sum_Of_Sq_Of_A_Prime_And_B_Prime = sum_Of_Sq_Of_A_Prime + sum_Of_Sq_Of_B_Prime;

            // compute r

            r = Math.Sqrt(sum_Of_Sq_Of_A_Prime_And_B_Prime / sum_Of_Sq_Of_A_And_B);

            // compute distortion paramters: DMax, D, and DI

            dMax = Math.Sqrt(sum_Of_Sq_Of_A_And_B);
            d = Math.Sqrt(sum_Of_Sq_Of_A_And_B - sum_Of_Sq_Of_A_Prime_And_B_Prime);
            di = 100.0 * (d / dMax);

            // package results

            BDR_parameters[0] = 1.0;
            BDR_parameters[1] = r;
            BDR_parameters[2] = alpha1;
            BDR_parameters[3] = alpha2;
            BDR_parameters[4] = beta1;
            BDR_parameters[5] = beta2;
            BDR_parameters[6] = scaleFactor;
            BDR_parameters[7] = theta;
            BDR_parameters[8] = dMax;
            BDR_parameters[9] = d;
            BDR_parameters[10] = di;
            /*    MessageBox.Show("r = " + Convert.ToString(r) + "\n" +
                                "alpha 1 = " + Convert.ToString(alpha1) + "\n" +
                                "alpha 2 = " + Convert.ToString(alpha2) + "\n" +
                                "beta 1 = " + Convert.ToString(beta1) + "\n" +
                                "beta 2 = " + Convert.ToString(beta2) + "\n" +
                                "scale = " + Convert.ToString(scaleFactor) + "\n" +
                                "theta = " + Convert.ToString(theta) + "\n" +
                                "DMax = " + Convert.ToString(dMax) + "\n" +
                                "D = " + Convert.ToString(d) + "\n" +
                                "DI = " + Convert.ToString(di));*/


            return BDR_parameters;
        } // computes bidimensional regression parameters for participant's map

        private List<PointD> advancedRotationAdjustment(List<PointD> coords) // figure out a good comment for this
        {
            List<PointD> retList = new List<PointD>();
            int start = 0;
            switch (currentRot)
            {
                case 0:
                    retList = coords;
                    break;
                case 1:
                    start = 2;
                    for (int j = start; j != (start - 1); j++)
                    {
                        if (j == coords.Count) { j = 0; }
                        retList.Add(coords[j]);
                    }
                    retList.Add(coords[start - 1]);
                    break;
                case 2:
                    start = 4;
                    for (int j = start; j != (start - 1); j++)
                    {
                        if (j == coords.Count) { j = 0; }
                        retList.Add(coords[j]);
                    }
                    retList.Add(coords[start - 1]);
                    break;
                case 3:
                    start = 6;
                    for (int j = start; j != (start - 1); j++)
                    {
                        if (j == coords.Count) { j = 0; }
                        retList.Add(coords[j]);
                    }
                    retList.Add(coords[start - 1]);
                    break;
            }
            return retList;
        }

        // Event Handlers

        private void Form1_Load(object sender, EventArgs e) // need comment
        {
            loadForm();
        }

        // these allow the landmark labels/boxes to be moved

        private void picturebox_MouseDown(object sender, MouseEventArgs e) // need comment
        {
            if (e.Button == MouseButtons.Left)
            {
                x = e.X;
                y = e.Y;
            }
        }

        private void picturebox_MouseMove(object sender, MouseEventArgs e) // need comment
        {
            if (e.Button == MouseButtons.Left)
            {
                string currentMode = Properties.Settings.Default.currentMode;
                string picBoxName = "";
                picBoxName = ((PictureBox)sender).Name;
                picBoxName = picBoxName.Remove(0, 15);
                int nodeIndex = Convert.ToInt32(picBoxName) - 1;
                // change this maybe
                int nextTop = ((PictureBox)sender).Top + (e.Y - y);
                int nextLeft = ((PictureBox)sender).Left + (e.X - x);
                // move landmark label
                if (nodesMissing[nodeIndex].Equals(false))
                {
                    if (nextTop < picturebox_map.Location.Y + picturebox_map.Size.Height && nextTop > picturebox_map.Location.Y && nextLeft > picturebox_map.Location.X)
                    {
                        ((PictureBox)sender).Left += (e.X - x);
                        ((PictureBox)sender).Top += (e.Y - y);  
                    }
                }
                else
                {
                    ((PictureBox)sender).Left += (e.X - x);
                    ((PictureBox)sender).Top += (e.Y - y);  
                    if (nextTop < picturebox_map.Location.Y + picturebox_map.Size.Height && nextTop > picturebox_map.Location.Y && nextLeft > picturebox_map.Location.X)
                    {
                        nodesMissing[nodeIndex] = false;
                    }
                }
                ((PictureBox)sender).BringToFront();
                ((PictureBox)sender).Focus();
                lastMovedNode = ((PictureBox)sender);
                updateZoomWindow(lastMovedNode.Left - picturebox_map.Location.X, lastMovedNode.Top - picturebox_map.Location.Y);
                if (Properties.Settings.Default.movableZoomWindow && currentMode.Equals("basic")) { picZoom.Location = ((PictureBox)sender).Location; }
                this.Update(); // prevents trail during landmark label movement
            }
        }

        private void picturebox_DoubleClick(object sender, EventArgs e)
        {
            string picBoxName = "";
            picBoxName = ((PictureBox)sender).Name;
            picBoxName = picBoxName.Remove(0, 15);
            int nodeIndex = Convert.ToInt32(picBoxName) - 1;
            Point currentPos = nodes[nodeIndex].Location;
            Point newPos = missingDefaultPosition(nodeIndex);
            if (newPos.Equals(currentPos)) // landmark is already missing when double clicked, then move it back to initial location
            {
                defaultPosition(nodeIndex);
                nodesMissing[nodeIndex] = false;
            }
            nodesMissing[nodeIndex] = true;
        } // moved landmark label to missing box when double clicked

        private void picturebox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)// arrows keys can move landmark labels for fine tuning
        {
            bool canMoveLeft = true;
            bool canMoveDown = true;
            bool canMoveUp = true;
            if (((PictureBox)sender).Top <= picturebox_map.Location.Y) { canMoveUp = false; }
            if (((PictureBox)sender).Top >= picturebox_map.Location.Y + picturebox_map.Size.Height) { canMoveDown = false; }
            if (((PictureBox)sender).Left <= picturebox_map.Location.X) { canMoveLeft = false; }
            if (e.KeyCode == Keys.Up && canMoveUp) { ((PictureBox)sender).Top -= 1; }
            else if (e.KeyCode == Keys.Down && canMoveDown) { ((PictureBox)sender).Top += 1; }
            else if (e.KeyCode == Keys.Left && canMoveLeft) { ((PictureBox)sender).Left -= 1; }
            else if (e.KeyCode == Keys.Right) { ((PictureBox)sender).Left += 1; }
            ((PictureBox)sender).Focus();
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right) { updateZoomWindow(((PictureBox)sender).Left - picturebox_map.Location.X, ((PictureBox)sender).Top - picturebox_map.Location.Y); }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((keyData == Keys.Right) || (keyData == Keys.Left) ||
                (keyData == Keys.Up) || (keyData == Keys.Down))
            {
                string currentMode = Properties.Settings.Default.currentMode;
                if (currentMode.Equals("advanced"))
                {
                    if (lastMovedUserRect != null)
                    {
                        if (keyData == Keys.Up)
                        {
                            if (lastMovedUserRect.rect.Y > 0) { lastMovedUserRect.rect.Y -= 1; }
                        }
                        else if (keyData == Keys.Down)
                        {
                            if (lastMovedUserRect.rect.Y < (picturebox_map.Size.Height - (lastMovedUserRect.rect.Size.Height + 2))) { lastMovedUserRect.rect.Y += 1; }
                        }
                        else if (keyData == Keys.Left)
                        {
                            if (lastMovedUserRect.rect.X > 0) { lastMovedUserRect.rect.X -= 1; }
                        }
                        else if (keyData == Keys.Right)
                        {
                            if (lastMovedUserRect.rect.X < (picturebox_map.Size.Width - (lastMovedUserRect.rect.Size.Width + 2))) { lastMovedUserRect.rect.X += 1; }
                        }
                    }
                    picturebox_map.Invalidate();
                }
                return true;
            }
            else
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        protected override bool ProcessDialogKey(Keys keyData) // allows arrow keys to be processed
        {
            if (keyData.Equals(Keys.Left) || keyData.Equals(Keys.Right) || keyData.Equals(Keys.Up) || keyData.Equals(Keys.Down))
            {
                return false;
            }
            else
            {
                // Pass it on to the base class for processing
                return base.ProcessDialogKey(keyData);
            }
        }

        private void picturebox_map_MouseMove(object sender, MouseEventArgs e) // update zoom window on cursor move
        {
            string currentMode = Properties.Settings.Default.currentMode;
            if (e.Location != oldMouseLocation)
            {
                oldMouseLocation = e.Location;
                updateZoomWindow(e.X, e.Y);
                if(currentMode.Equals("advanced"))
                {
                    bool normalCursor= true;
                    foreach(UserRect ur in allRects)
                    {
                        if(ur.rect.Contains(oldMouseLocation)){normalCursor = false;}
                    }
                    if (normalCursor) { picturebox_map.Cursor = Cursors.Default; }
                    if (e.Button == MouseButtons.Left)
                    {
                        if (allRects.Count > 0)
                        {
                            for (int i = 0; i < allRects.Count; i++)
                            {
                                if (allRects[i].getlastMoved() == true)
                                {
                                    lastMovedUserRect = allRects[i];
                                    allRects[i].setLastMoved(false);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void picturebox_MouseUp(object sender, MouseEventArgs e)
        {
            if (allLabelsMoved() && picturebox_map.Tag.Equals("map"))
            {
                button_calculate.Enabled = true;
                button_preview.Enabled = true;
                saveConfigurationToolStripMenuItem.Enabled = true;
            }
            picZoom.Location = picZoomLoc;
        }

        // menu item events

        private void coordinatesFileToolStripMenuItem_Click(object sender, EventArgs e) // New Coordinates File
        {
            newCoordinatesFile();
        }

        private void loadMapImageToolStripMenuItem_Click(object sender, EventArgs e) // Open Map Image
        {
            openMapImage();
        }

        private void loadConfigurationToolStripMenuItem_Click(object sender, EventArgs e) // Load Configuration
        {
            if (picturebox_map.Tag.Equals("homeScreen"))
            {
                MessageBox.Show("To load a configuration first open the map image that corresponds to the configuration file.");
                bool mapOpened = openMapImage();
                if (mapOpened) 
                {
                    MessageBox.Show("Now select the configuration file.");
                    loadConfigOpenDialog(false); 
                }
            }
            else { loadConfigOpenDialog(false); }
        }

        private void saveConfigurationToolStripMenuItem_Click(object sender, EventArgs e) // Save Configuration
        {
            saveConfiguration();
        }

        private void saveMapImageToolStripMenuItem_Click(object sender, EventArgs e) // Save Map Image
        {
            takeScreenshot();
        }

        private void batchReanalysisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            batchReanalysis();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e) // Reset
        {
            reset();
        }

        private void restoreDefaultsToolStripMenuItem_Click(object sender, EventArgs e) // Restore Defaults
        {
            restoreDefaults();
        }

        // Buttons

        private void b_rotate_ccw_Click(object sender, EventArgs e)
        {
            if (picturebox_map.Tag.ToString().Equals("map")) { rotateWorkspace(true, "ccw"); }
        }

        private void b_rotate_cw_Click(object sender, EventArgs e)
        {
            if (picturebox_map.Tag.ToString().Equals("map")) { rotateWorkspace(true, "cw"); }
        }

        private void button_preview_Click(object sender, EventArgs e) // Preview
        {
            calculate(true);
        }

        private void button_calculate_Click(object sender, EventArgs e) // Calculate
        {
            calculate(false);
        }

        // Track Bar

        private void trackBar_zoomLevel_ValueChanged(object sender, EventArgs e) // Zoom track bar
        {
            Properties.Settings.Default.prevZoomLevel = trackBar_zoomLevel.Value;
            Properties.Settings.Default.Save();
        }

        // Combo Box

        private void cb_coordsFiles_SelectionChangeCommitted(object sender, EventArgs e) // load new coordinates file from combo box
        {
            // update prev coordinates file setting
            string currentMode = Properties.Settings.Default.currentMode;
            updatePrevCoordsFile();
            loadCoordinatesFile(cb_coordsFiles.SelectedItem.ToString());
            this.Refresh();
            if(currentMode.Equals("basic")){highlightPanel(panel_highlight_labels);}
        }

        private void CheckKeys(object sender, System.Windows.Forms.KeyPressEventArgs e) // handles Enter key for combo box, loads new coordinates file
        {

            if (sender == cb_coordsFiles)
            {
                if (e.KeyChar == (char)13) // Enter key
                {

                    if (cb_coordsFiles.Text == "") // if invalid input or blank
                    {
                        button_calculate.Enabled = false;
                        button_preview.Enabled = false;
                        tb_legend.Enabled = false;
                        b_rotate_ccw.Enabled = false;
                        b_rotate_cw.Enabled = false;
                        tb_legend.Text = "";
                    }
                    else
                    { // if good input
                        try
                        {
                            cb_coordsFiles.SelectedItem = cb_coordsFiles.Text;
                            if (cb_coordsFiles.SelectedItem == null) { throw new System.ArgumentException("Coordinates file does not exist!"); }
                            loadCoordinatesFile(cb_coordsFiles.Text);
                        }
                        catch
                        {
                            MessageBox.Show("Unable to load coordinates file. \n\nDoes it exist in " + Application.StartupPath + "\\Resources?", "Message");
                        }
                    }
                }
            }
        }

        // Textboxes

        private void tb_legend_Enter(object sender, EventArgs e)// prevent blinking caret in legend texbox (tb_legend)
        {
            mainMenu.Focus();
        }

        private void tb_legend_Leave(object sender, EventArgs e) // prevent blinking caret in legend texbox (tb_legend)
        {
            Cursor = Cursors.Default;
        }

        private void tb_legend_MouseClick(object sender, MouseEventArgs e)
        {
            string currentMode = Properties.Settings.Default.currentMode;
            if (allLabelsBoxesLoaded && currentMode.Equals("basic")) { highlightPanel(this.panel_highlight_labels); }// bring attention to landmark labels
        }

        private void lb_basicMode_Click(object sender, EventArgs e) // need comment
        {
            string currentMode = Properties.Settings.Default.currentMode;
            if (currentMode.Equals("advanced"))
            {
                updateCurrentMode("basic");
                Properties.Settings.Default.prevCoordsFile = "";
                Properties.Settings.Default.Save();
                cb_coordsFiles.Text = "";
                toggleMode();
                reset();
            }
        }

        private void lb_advanced_Click(object sender, EventArgs e) // need comment
        {
            string currentMode = Properties.Settings.Default.currentMode;
            if (currentMode.Equals("basic"))
            {
                updateCurrentMode("advanced");
                Properties.Settings.Default.prevCoordsFile = "";
                Properties.Settings.Default.Save();
                cb_coordsFiles.Text = "";
                toggleMode();
                reset();
            }
        }

        // picturebox paint methods

        private void picturebox_map_Paint(object sender, PaintEventArgs e) // need comment
        {
            string currentMode = Properties.Settings.Default.currentMode;
            if (allRects != null && currentMode.Equals("advanced"))
            {
                foreach (UserRect ur in allRects)
                {
                    try
                    {
                        if (ur.getMissing() == false) { ur.Draw(e.Graphics); }
                    }
                    catch (Exception exp)
                    {
                        System.Console.WriteLine(exp.Message);
                    }
                }
            }
        }

        private void picturebox_missingLandmarks_Paint(object sender, PaintEventArgs e) // need comment
        {
            string currentMode = Properties.Settings.Default.currentMode;
            if (allRects != null && currentMode.Equals("advanced"))
            {
                foreach (UserRect ur in allRects)
                {
                    try
                    {
                        if (ur.getMissing() == true) { ur.Draw(e.Graphics); }
                    }
                    catch (Exception exp)
                    {
                        System.Console.WriteLine(exp.Message);
                    }
                }
            }

        }

        // color palette controls

        private void pb_rectCol_Click(object sender, EventArgs e) // need comment
        {
            updateBoxBorderColor(((PictureBox)(sender)));
        }

        private void pb_rectCenCol_Click(object sender, EventArgs e) // need comment
        {
            updateBoxCenterColor(((PictureBox)(sender)));
        }

        private void actualMapIVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.BDR_IV = "actual";
            this.actualMapIVToolStripMenuItem.Checked = true;
            this.actualMapDVToolStripMenuItem.Checked = false;
            Properties.Settings.Default.Save();
        }

        private void actualMapDVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.BDR_IV = "participant's";
            this.actualMapIVToolStripMenuItem.Checked = false;
            this.actualMapDVToolStripMenuItem.Checked = true;
            Properties.Settings.Default.Save();
        }

        private void movableZoomWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.movableZoomWindowToolStripMenuItem.Checked == true)
            {
                Properties.Settings.Default.movableZoomWindow = true;
            }
            if (this.movableZoomWindowToolStripMenuItem.Checked == false)
            {
                Properties.Settings.Default.movableZoomWindow = false; 
            }
            Properties.Settings.Default.Save();
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NetSparkleAppCastItem _frm = new NetSparkleAppCastItem();
            _sparkle.GetApplicationConfig().SetVersionToSkip("");
            if (_sparkle.IsUpdateRequired(_sparkle.GetApplicationConfig(), out _frm))
            {
                
                _sparkle.ShowUpdateNeededUI(_frm);
            }
            else { MessageBox.Show("Your software version is up to date!","Check for Updates"); }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string currentVersion = _sparkle.GetApplicationConfig().InstalledVersion;
            string[] versionArray = currentVersion.Split('.');
            currentVersion = versionArray[0] + "." + versionArray[1];
            MessageBox.Show("Gardony Map Drawing Analyzer (GMDA): v. " + currentVersion + "\n\n" +
                "Copyright 2013 Aaron Gardony" + "\n\n" +
                "http://www.aarongardony.com" + "\n\n" + 
                "This program is free software: you can redistribute it and/or modify " +
                "it under the terms of the GNU General Public License as published by " +
                "the Free Software Foundation, either version 3 of the License, or " +
                "(at your option) any later version." + "\n\n" +
                "This program is distributed in the hope that it will be useful, " +
                "but WITHOUT ANY WARRANTY; without even the implied warranty of " +
                "MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the " +
                "GNU General Public License for more details." + "\n\n" +
                "You should have received a copy of the GNU General Public License " +
                "along with this program.  If not, see <http://www.gnu.org/licenses/>."
                );
        }        
    }
}
