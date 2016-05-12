using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Globalization;
using UserRectDemo;
using PointDClass;
// Copyright 2014 Aaron Gardony
// This program is distributed under the terms of the GNU General Public License
// last update: 5/11/16 by Aaron Gardony
namespace MapDrawingAnalyzer
{
    public partial class Form2 : Form
    {
        private readonly Form1 _form1;
        public Form2(Form1 form1)
        {
            _form1 = form1;
            InitializeComponent();
        }

        private int x;
        private int y;
        int numLandmarks = 0;
        bool numLandmarksEntered = false;
        private StreamWriter outfile;
        private string enterMode = "";
        private List<string> landmarkNames = new List<string>();
        private PictureBox[] nodes;
        private Point[] labelStartPos;
        private PictureBox lastMovedNode;

        // Initializing, Arranging, Updating

        private void showForm()
        {
            if (landmarkNames != null) { landmarkNames.Clear(); }
            if (nodes != null) { nodes = null; }
            dgv_coords.Visible = false;
            picturebox_map.Location = new Point(5, 24);
            picturebox_map.SendToBack();
            tb_numLandmarks.Focus();
            foreach (DataGridViewColumn column in dgv_coords.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            tb_numLandmarks.Text = "";
            MessageBoxManager.Yes = "Enter Manually";
            MessageBoxManager.No = "Arrange Labels";
            MessageBoxManager.Register();
            DialogResult result = MessageBox.Show("To make a coordinates file you have two options.\n\n" +
                "You can enter the landmark coordinates of the target environment manually.\n\n" +
                "Note: This is the better option if you already have the coordinates (ex. virtual environment landmarks)\n\nOR\n\n" +
                "You can arrange landmark labels on a target sketch map.\n\n" +
                "Note: This is the better option if you have a target sketch map to which you want to compare other maps (e.g. a perfect map).", "Options", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes) { enterMode = "manual"; }
            else if (result == DialogResult.No) { enterMode = "GUI"; }
            MessageBoxManager.Unregister();
        }

        private bool createLandmarkLabels(int numLandmarks) // create & arrange numLandmarks landmark labels
        {
            string currentMode = Properties.Settings.Default.currentMode;
            bool retBool = false;
            removeNodes();
            // dynamically create landmark labels
            // if labels already exist remove them

            nodes = new PictureBox[numLandmarks];
            labelStartPos = new Point[numLandmarks];
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            for (int i = 0; i < nodes.Count(); i++)
            {
                nodes[i] = new PictureBox();

                try { nodes[i].Image = GetImage("_" + (i + 1)); }
                catch { return false; }

                nodes[i].InitialImage = null;
                nodes[i].Name = "picturebox_node" + Convert.ToString(i + 1, CultureInfo.InvariantCulture);
                nodes[i].Size = new System.Drawing.Size(20, 20);
                defaultPosition(i);
                nodes[i].MouseDown += new System.Windows.Forms.MouseEventHandler(this.picturebox_MouseDown);
                nodes[i].MouseMove += new System.Windows.Forms.MouseEventHandler(this.picturebox_MouseMove);
                nodes[i].PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.picturebox_PreviewKeyDown);
                nodes[i].MouseUp += new System.Windows.Forms.MouseEventHandler(this.picturebox_MouseUp);
                nodes[i].BringToFront();
                nodes[i].Visible = true;
                ToolTip tempTT = new ToolTip();
                tempTT.AutomaticDelay = 50;
                tempTT.IsBalloon = true;
                tempTT.AutoPopDelay = 2000;
                tempTT.SetToolTip(this.nodes[i], landmarkNames[i]);
                this.Controls.Add(nodes[i]);
            }
            retBool = true;
            return retBool;
        }

        private void defaultPosition(int nodeIndex) // places landmark labels in their default position as calculated by their index
        {
            int nodeXPos = 0;
            int nodeYPos = 0;
            nodeYPos = ((nodeIndex / 24) * 25) + 730;
            if (nodeIndex < 24)
            {
                nodeXPos = 5 + (nodeIndex * 25);
            }
            else
            {
                nodeXPos = 5 + (nodeIndex % 24) * 25;
            }
            nodes[nodeIndex].Location = new System.Drawing.Point(nodeXPos, nodeYPos);
            labelStartPos[nodeIndex].X = nodeXPos;
            labelStartPos[nodeIndex].Y = nodeYPos;
        }

        private void removeNodes() // clears and diposes landmark labels from form
        {

            if (nodes != null)
            {
                for (int j = 0; j < nodes.Length; j++)
                {
                    this.Controls.Remove(nodes[j]);
                    nodes[j] = null;
                }
                nodes = null;
                labelStartPos = null;
            }
        }
        
        // Core Functions

        private bool openMapImage()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = "Open Map Image";
            openFile.Filter = "JPG files (*.jpg)|*.jpg|JPEG files (*.jpeg)|*.jpeg|PNG files (*.png)|*.png|GIF files (*.gif)|*.gif|BMP files (*.bmp)|*.bmp|All files (*.*)|*.*";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
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
                        newHeight = Convert.ToInt32(downscale * tempImage.Height, CultureInfo.InvariantCulture);
                        finalImg = new Bitmap(tempImage, picturebox_map.Width, newHeight);
                    }
                    else if (tempImage.Height > tempImage.Width) // Case 1b: the height of the loaded image exceeds its width
                    {
                        downscale = (double)picturebox_map.Height / (double)tempImage.Height;
                        newWidth = Convert.ToInt32(downscale * tempImage.Width, CultureInfo.InvariantCulture);
                        finalImg = new Bitmap(tempImage, newWidth, picturebox_map.Height);
                    }
                }
                else if (tempImage.Width < picturebox_map.Width || tempImage.Height < picturebox_map.Height)// Case 2: loaded image is smaller than picturebox_map.size one one or more dimensions
                {
                    double upscale = 1.0;
                    if (tempImage.Width >= tempImage.Height) // Case 2a: the width of the loaded image exceeds or is is equal to its height
                    {
                        upscale = (double)picturebox_map.Width / (double)tempImage.Width;
                        newHeight = Convert.ToInt32(upscale * tempImage.Height, CultureInfo.InvariantCulture);
                        finalImg = new Bitmap(tempImage, picturebox_map.Width, newHeight);
                    }
                    else if (tempImage.Height > tempImage.Width) // Case 2b: the height of the loaded image exceeds its width
                    {
                        upscale = (double)picturebox_map.Height / (double)tempImage.Height;
                        newWidth = Convert.ToInt32(upscale * tempImage.Width, CultureInfo.InvariantCulture);
                        finalImg = new Bitmap(tempImage, newWidth, picturebox_map.Height);
                    }
                }
                // create graphics object and insert it centered into picturebox_map
                Graphics gfx = Graphics.FromImage(finalImg);
                gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                picturebox_map.Image = null;
                picturebox_map.SizeMode = PictureBoxSizeMode.CenterImage;
                picturebox_map.Image = finalImg;
                toolStripStatusLabel.Text = "Currently opened image: " + System.IO.Path.GetFileName(openFile.FileName);
                return true;
            }
            else return false;
        }

        private void reset()
        {
            if (numLandmarks > 0)
            {
                // remove rows from table

                for (int i = (numLandmarks - 1); i >= 1; i--)
                {
                    dgv_coords.Rows.Remove(dgv_coords.Rows[i]);
                }
                for(int i = 0; i < 4; i++) { dgv_coords.Rows[0].Cells[i].Value = "";}
                numLandmarks = 0;
                dgv_coords.Visible = false;
                tb_numLandmarks.Focus();
            }
            // clear picture box and send to back
            picturebox_map.Image = null;
            picturebox_map.SendToBack();
            b_saveFile.Text = "Save File";
            b_saveFile.Enabled = false;
            tb_legend.Text = "";
            toolStripStatusLabel.Text = "Coordinates File Builder";
            tb_legend.Visible = false;
            removeNodes();
            lastMovedNode = null;
            landmarkNames = new List<string>();
            b_enter.Enabled = true;
            pb_nextStep.Image = Properties.Resources.save;
            showForm();
        }

        private void backToMainForm(int handleCode)
        {
            if (handleCode == 1) { this.Close(); }
            else
            {
                foreach (Form form in Application.OpenForms)
                {
                    if (form is Form1)
                    {
                        form.Show();
                        _form1.loadForm();
                        break;
                    }
                }
            }   
        }

        private void writeCoordsFile(string saveName)
        {
            try
            {
                outfile = new StreamWriter(Application.StartupPath + "//Resources//xycoords_map_" + saveName + ".csv");
                string writeString = "landmarkName,landmarkNum,X,Y\n";

                switch (enterMode)
                {
                    case "manual":
                        for (int i = 0; i < dgv_coords.RowCount; i++)
                        {
                            for (int j = 0; j < dgv_coords.ColumnCount; j++)
                            {
                                if (j < dgv_coords.ColumnCount - 1) { writeString += Convert.ToString(dgv_coords[j, i].Value, CultureInfo.InvariantCulture).Trim() + ","; }
                                else { writeString += Convert.ToString(dgv_coords[j, i].Value, CultureInfo.InvariantCulture).Trim() + "\n"; }
                            }
                        }
                        break;
                    case "GUI":
                        for (int i = 0; i < dgv_coords.RowCount; i++)
                        {
                            PointD cartCoords = getCartesianCoords(i + 1);
                            writeString += Convert.ToString(dgv_coords[0, i].Value, CultureInfo.InvariantCulture).Trim() + "," + Convert.ToString(dgv_coords[1, i].Value, CultureInfo.InvariantCulture).Trim() + "," + Convert.ToString(cartCoords.X, CultureInfo.InvariantCulture) + "," + Convert.ToString(cartCoords.Y, CultureInfo.InvariantCulture) + "\n";
                        }
                        break;
                }
                outfile.Write(writeString);
                outfile.Close();

                MessageBox.Show("Coordinates file sucessfully written!\n\n" +
                                 "File Location: " + Application.StartupPath +
                                 "\\Resources\\xycoords_map_" + saveName +
                                 ".csv");
            }
            catch
            {
                MessageBox.Show("Unable to write coordinates file: Does the Resources folder exist?");
            }
        }

        private void buildSpreadsheet()
        {
            if (numLandmarks > 0 && numLandmarks <= 48) { reset(); }
            try
            {
                numLandmarks = Convert.ToInt32(tb_numLandmarks.Text, CultureInfo.InvariantCulture);
                if (numLandmarks <= 1) { throw new System.ArgumentException("Numbers less than 2 are not allowed."); }
                if (numLandmarks > 48) { throw new System.ArgumentException("Numbers greater than 48 are not allowed."); }
                if (enterMode.Equals("GUI"))
                {
                    dgv_coords.Columns["X"].Visible = false;
                    dgv_coords.Columns["Y"].Visible = false;
                }
                if (enterMode.Equals("manual"))
                {
                    dgv_coords.Columns["X"].Visible = true;
                    dgv_coords.Columns["Y"].Visible = true;
                }
                for (int i = 1; i < numLandmarks + 1; i++)
                {
                    if (i == 1 && numLandmarksEntered)
                    {
                        dgv_coords.Rows[0].Cells[0].Value = "";
                        dgv_coords.Rows[0].Cells[1].Value = i;
                        dgv_coords.Rows[0].Cells[2].Value = "";
                        dgv_coords.Rows[0].Cells[3].Value = "";
                        continue;
                    }
                    else
                    {
                        DataGridViewRow row = (DataGridViewRow)dgv_coords.Rows[0].Clone();
                        row.Cells[1].Value = i;
                        dgv_coords.Rows.Add(row);
                    }
                }
                dgv_coords.CurrentCell = dgv_coords[0, 0];
                dgv_coords.CurrentCell.Selected = true;
                dgv_coords.AllowUserToAddRows = false;
                dgv_coords.Visible = true;
                dgv_coords.Focus();
                numLandmarksEntered = true;
                b_enter.Enabled = false;
                if (enterMode.Equals("GUI")) { 
                    b_saveFile.Text = "Next Step";
                    pb_nextStep.Image = Properties.Resources.forward;
                }
            }
            catch
            {
                MessageBox.Show("Error: Did you enter a positive integer [2 - 48] for the number of landmarks?");
                tb_numLandmarks.Text = "";
                b_enter.Enabled = true;
            }
        }

        private void saveFile()
        {
            tb_legend.Text = "";
            string saveName = "";
            // test for blank fields
            bool pass = dgvIsFull();
            if (!pass)
            {
                landmarkNames.Clear();
                switch (enterMode)
                {
                    case "manual":
                        MessageBox.Show("One of the fields is missing a value. Coordinates file has not been saved", "Error");
                        break;
                    case "GUI":
                        MessageBox.Show("One of the landmark names is missing. Enter all the landmark names before moving on to the next step.", "Error");
                        break;
                }

            }
            else
            {
                switch (enterMode)
                {
                    case "manual":
                        DialogResult result;
                        result = MessageBox.Show("Are you sure you want to save the coordinates?", "Confirm", MessageBoxButtons.YesNo);
                        if (result == DialogResult.No) { return; }
                        else
                        {
                            saveName = Microsoft.VisualBasic.Interaction.InputBox("Enter the name of the environment", "Enter Name");
                            if (!saveName.Equals("")) { writeCoordsFile(saveName); }
                        }
                        break;
                    case "GUI":
                        if (b_saveFile.Text.Equals("Next Step"))
                        {
                            tb_legend.Visible = true;
                            picturebox_map.BringToFront();
                            MessageBox.Show("Next, select the map image of the target map (the map all other maps will be compared to).", "Message");
                            if (openMapImage())
                            {
                                labelStartPos = new Point[numLandmarks];
                                createLandmarkLabels(numLandmarks);
                                // add tool tips
                                MessageBox.Show("Lastly, move the landmark labels (located at the bottom of the screen) to each landmark on the map, positioning the top left corner of the label at the landmark's location.\n\n" +
                                    "When you are done click Save File to save the coordinates file.", "Message");
                                pb_nextStep.Image = Properties.Resources.save;
                                b_saveFile.Text = "Save File";
                                b_saveFile.Enabled = false;
                                highlightPanel(this.panel_highlight_labels);
                            }
                        }
                        else if (b_saveFile.Text.Equals("Save File"))
                        {
                            bool alm = allLabelsMoved();
                            if (alm == true)
                            {
                                result = MessageBox.Show("Are you sure you want to save the coordinates?", "Confirm", MessageBoxButtons.YesNo);
                                if (result == DialogResult.No) { return; }
                                else
                                {
                                    saveName = Microsoft.VisualBasic.Interaction.InputBox("Enter the file name", "Save Coordinates File");
                                    if (!saveName.Equals("")) { writeCoordsFile(saveName); }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Unable to save coordinates. Have you moved all the landmark labels?");
                            }
                        }
                        break;
                }
            }
        }

        // Helper Functions

        private bool dgvIsFull()
        {
            bool pass = true;
            if (landmarkNames != null) { landmarkNames.Clear(); }
         //   if (b_saveFile.Text.Equals("Save File") && enterMode.Equals("GUI")) { return true; }
  int max = 0;
                switch (enterMode)
                {
                    case "manual":
                        max = dgv_coords.ColumnCount;
                        break;
                    case "GUI":
                        max = dgv_coords.ColumnCount - 2;
                        break;
                }
                for (int i = 0; i < dgv_coords.RowCount; i++)
                {
                    for (int j = 0; j < max; j++)
                    {
                        if (dgv_coords[j, i].Value == null || Convert.ToString(dgv_coords[j, i].Value, CultureInfo.InvariantCulture).Equals(""))
                        {
                            pass = false;
                        }
                        else
                        {
                            if (j == 0)
                            {
                                landmarkNames.Add(Convert.ToString(dgv_coords[j, i].Value, CultureInfo.InvariantCulture).Trim());
                                tb_legend.Text += Convert.ToString(i + 1, CultureInfo.InvariantCulture) + ". " + landmarkNames[landmarkNames.Count - 1] + "\r\n";
                            }
                        }
                    }
                }
            
            return pass;
        }

        public static Image GetImage(string name) // returns landmark label image from Application's Resources
        {
            return (Image)typeof(MapDrawingAnalyzer.Properties.Resources).GetProperty(name, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).GetValue(null, null);
        }

        private bool allLabelsMoved() // checks if all the landmark labels were moved by user, yes = returns true
        {
            string currentMode = Properties.Settings.Default.currentMode;
            bool allLabelsMoved = true;
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
            return allLabelsMoved;
        }

        private PointD getCartesianCoords(int landmarkNum) // converts c# coordinate system to cartesian plane
        {
            PointD retPoint = new PointD(0.0, 0.0);
            retPoint.X = (0 - ((350 + picturebox_map.Location.X) - nodes[landmarkNum - 1].Left)); // picturebox location is 5,24 
            retPoint.Y = (0 + ((350 + picturebox_map.Location.Y) - nodes[landmarkNum - 1].Top)); 
            return retPoint;
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

        // Event Handlers

        private void coordsFileBuilder_Shown(object sender, EventArgs e)
        {
            showForm();
        }

        private void coordsFileBuilder_FormClosed(object sender, FormClosedEventArgs e)
        {
            backToMainForm(0);
        }

        private void numLandmarks_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void b_reset_Click(object sender, EventArgs e)
        {
            reset();
            tb_numLandmarks.Text = "";
        }

        private void b_saveFile_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void b_goback_Click(object sender, EventArgs e)
        {
            backToMainForm(1);
        }

        private void cut_copyText(int code)
        {
            DataObject d = dgv_coords.GetClipboardContent();
            Clipboard.SetDataObject(d);
            if (code == 1)
            {
                foreach (DataGridViewCell dgvc in dgv_coords.SelectedCells)
                {
                    dgvc.Value = "";
                }
            }
        }

        private void pasteText()
        {
            string s = Clipboard.GetText();
            string[] lines = s.Split('\n');
            int row = dgv_coords.CurrentCell.RowIndex;
            int col = dgv_coords.CurrentCell.ColumnIndex;
            foreach (string line in lines)
            {
                if (row < dgv_coords.RowCount && line.Length > 0)
                {
                    string[] cells = line.Split('\t');
                    for (int i = 0; i < cells.GetLength(0); ++i)
                    {
                        if (col + i < this.dgv_coords.ColumnCount)
                        {
                            if (dgv_coords.Columns[col + i].ReadOnly == false)
                            {
                                dgv_coords[col + i, row].Value = Convert.ChangeType(cells[i], dgv_coords[col + i, row].ValueType);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    row++;
                }
                else
                {
                    break;
                }
            }
          //  dgv_coords.ClearSelection();
        }

        private void dgv_coords_KeyDown(object sender, KeyEventArgs e)
        {
            // if copy (CTRL + C)
            if (e.Control && e.KeyCode == Keys.C)
            {
                cut_copyText(0);
                e.Handled = true;
            }

            // if cut (CTRL + X)

            else if (e.Control && e.KeyCode == Keys.X)
            {
                cut_copyText(1);
                e.Handled = true;
            }
            // if paste (CTRL + V)
            else if (e.Control && e.KeyCode == Keys.V)
            {
                pasteText();
                e.Handled = true;
            }

        }

        private void picturebox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                x = e.X;
                y = e.Y;
            }
        }

        private void picturebox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // move landmark label
                // change this maybe
                int nextTop = ((PictureBox)sender).Top + (e.Y - y);
                int nextLeft = ((PictureBox)sender).Left + (e.X - x);
                // move landmark label
                if (nextTop < picturebox_map.Location.Y + picturebox_map.Size.Height && nextTop > picturebox_map.Location.Y && nextLeft > picturebox_map.Location.X)
                {
                    ((PictureBox)sender).Left += (e.X - x);
                    ((PictureBox)sender).Top += (e.Y - y);
                }
                ((PictureBox)sender).BringToFront();
                ((PictureBox)sender).Focus();
                lastMovedNode = ((PictureBox)sender);
                this.Update(); // prevents trail during landmark label movement
            }
        }

        private void picturebox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
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
        } // arrows keys can move landmark labels for fine tuning

        private void Form1_KeyDown(object sender, KeyEventArgs e) // handle key events in main form
        {
            e.Handled = true;
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

        private void tb_legend_Enter(object sender, EventArgs e)// prevent blinking caret in legend texbox (tb_legend)
        {
            picturebox_map.Focus();
        }

        private void tb_legend_Leave(object sender, EventArgs e)// prevent blinking caret in legend texbox (tb_legend)
        {
            Cursor = Cursors.Default;
        }

        private void picturebox_MouseUp(object sender, MouseEventArgs e)
        {
            if (enterMode.Equals("GUI"))
            {
                if (allLabelsMoved())
                {
                    b_saveFile.Enabled = true;
                }
            }
        }

        private void dgv_coords_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvIsFull())
            {
                b_saveFile.Enabled = true;
            }
            else
            {
                b_saveFile.Enabled = false; 
            }
        }

        private void tb_legend_Click(object sender, EventArgs e)
        {
            string currentMode = Properties.Settings.Default.currentMode;
            if (currentMode.Equals("basic")) { highlightPanel(this.panel_highlight_labels); }// bring attention to landmark labels
        }

        private void button1_Click(object sender, EventArgs e)
        {
            buildSpreadsheet();
        }

        private void cut_Click(object sender, EventArgs e)
        {
            cut_copyText(1);
        }

        private void copy_Click(object sender, EventArgs e)
        {
            cut_copyText(0);
        }

        private void paste_Click(object sender, EventArgs e)
        {
            pasteText();
        }
    }
}
