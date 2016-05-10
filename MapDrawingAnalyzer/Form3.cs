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
// last update: 5/09/16 by Aaron Gardony
namespace MapDrawingAnalyzer
{
    public partial class advancedCoordsFileBuilder : Form
    {
        private readonly Form1 _form1;
        public advancedCoordsFileBuilder(Form1 form1)
        {
            _form1 = form1;
            InitializeComponent();
        }

        int numLandmarks = 0;
        bool numLandmarksEntered = false;
        private StreamWriter outfile;
        private List<UserRect> allRects = new List<UserRect>();
        private List<string> landmarkNames = new List<string>();
        private Point[] boxStartPos;
        private Point oldMouseLocation = Point.Empty;
        private UserRect lastMovedUserRect;

        // Initializing, Arranging, Updating

        private void showForm()
        {
            if (landmarkNames != null) { landmarkNames.Clear(); }
            if (allRects != null) { allRects.Clear(); }
            toolStripStatusLabel.Text = "Coordinates File Builder";
            tb_numLandmarks.Focus();
            foreach (DataGridViewColumn column in dgv_coords.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            picturebox_map.Location = new Point(5, 24);
            picturebox_map.SendToBack();
            dgv_coords.Visible = false;
        }

        private void defaultPosition(int nodeIndex) // places landmark labels in their default position as calculated by their index
        {
            int nodeXPosRect = 0;
            int nodeYPosRect = 0;
            string currentMode = Properties.Settings.Default.currentMode;
            nodeYPosRect = 20 + (nodeIndex / 8) * 80;
            if (nodeIndex < 8)
            {
                nodeXPosRect = 30 + (nodeIndex * 80);
            }
            else
            {
                nodeXPosRect = 30 + (nodeIndex % 8) * 80;
            }
            UserRect ur = new UserRect(new Rectangle(new System.Drawing.Point(nodeXPosRect, nodeYPosRect), new Size(30, 30)), landmarkNames[nodeIndex]);
            ur.buildingCoordsFile = true;
            allRects.Add(ur);
            boxStartPos[nodeIndex].X = nodeXPosRect;
            boxStartPos[nodeIndex].Y = nodeYPosRect;
        }

        private void removeNodes() // clears and diposes landmark labels from form
        {
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
            }
        }

        // Core Functions

        private void reset()
        {
            if (numLandmarks > 0)
            {
                // remove rows from table

                for (int i = (numLandmarks - 1); i >= 1; i--)
                {
                    dgv_coords.Rows.Remove(dgv_coords.Rows[i]);
                }
                for (int i = 0; i < dgv_coords.ColumnCount; i++) { dgv_coords.Rows[0].Cells[i].Value = ""; }
                numLandmarks = 0;
                dgv_coords.Visible = false;
                tb_numLandmarks.Focus();

            }
            b_step2.Text = "Next Step";
            toolStripStatusLabel.Text = "Coordinates File Builder";
            b_step2.Enabled = false;
            landmarkNames = new List<string>();
            removeNodes();
            // clear picture box and send to back
            picturebox_map.Refresh();
            picturebox_map.Image = null;
            picturebox_map.SendToBack();
            b_enter.Enabled = true;
            pb_nextStep.Image = Properties.Resources.forward;
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
                outfile = new StreamWriter(Application.StartupPath + "//Resources//xycoords_mapAdvanced_" + saveName + ".csv");
                string writeString = "landmarkName,rectangleVertexNum,X,Y\n";

                int counter = 0;
                for (int i = 0; i < dgv_coords.RowCount; i++)
                {
                    List<PointD> rectBordCartCoords = allRects[i].getRectBorderCartesianCoordinates();
                    for (int j = 0; j < rectBordCartCoords.Count; j++)
                    {
                        writeString += dgv_coords[0, i].Value.ToString().Trim() + ",";
                        writeString += Convert.ToString(counter) + ",";
                        writeString += Convert.ToString(rectBordCartCoords[j].X) + "," + Convert.ToString(rectBordCartCoords[j].Y);
                        writeString += "\n";
                        counter++;
                    }
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

        private void step2()
        {
            // test for blank fields, if not blank
            if (b_step2.Text.Equals("Next Step"))
            {
                bool pass = dgvIsFull();
                if (!pass)
                {
                    landmarkNames.Clear();
                    MessageBox.Show("One of the landmark names is missing. Enter all the landmark names before moving on to the next step.", "Error");
                }
                else
                {
                    picturebox_map.BringToFront();
                    MessageBox.Show("Next, select the map image of the target map (the map all other maps will be compared to).", "Message");
                    if (openMapImage())
                    {
                        boxStartPos = new Point[numLandmarks];
                        for (int i = 0; i < numLandmarks; i++)
                        {
                            defaultPosition(i);
                            allRects[i].SetPictureBox(picturebox_map, picturebox_map);
                            allRects[i].setRectColor(getRectColor(Properties.Settings.Default.rectColor));
                            allRects[i].setRectCenterColor(getRectColor(Properties.Settings.Default.rectCentColor));
                        }
                        picturebox_map.Invalidate();
                        MessageBox.Show("Lastly, resize and move the landmark boxes so they surround the landmarks on the target map.\n\n" +
                            "When you are done click Save File to save the coordinates file.", "Message");
                        b_step2.Text = "Save File";
                        b_step2.Enabled = false;
                        pb_nextStep.Image = Properties.Resources.save;
                    }
                }
            }
            else if (b_step2.Text.Equals("Save File"))
            {
                string saveName = "";
                bool alm = allLabelsMoved();
                if (alm == true)
                {
                    DialogResult result;
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
                    MessageBox.Show("Unable to save coordinates. Have you moved all the landmark boxes?");
                }
            }
        }

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



        // Helper Functions

        private bool dgvIsFull()
        {
            bool pass = true;
            if (landmarkNames != null) { landmarkNames.Clear(); }
            for (int i = 0; i < dgv_coords.RowCount; i++)
            {
                if (dgv_coords[0, i].Value == null || dgv_coords[0, i].Value.ToString().Equals(""))
                {
                    pass = false;
                }
                else
                {
                    landmarkNames.Add(dgv_coords[0, i].Value.ToString().Trim());
                }
            }
            return pass;
        }

        private bool allLabelsMoved() // checks if all the landmark labels were moved by user, yes = returns true
        {
            string currentMode = Properties.Settings.Default.currentMode;
            bool allLabelsMoved = true;
            if (allRects != null)
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

        // Event Handlers

        private void advancedCoordsFileBuilder_Shown(object sender, EventArgs e)
        {
            showForm();
        }

        private void numLandmarks_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                
            }
        }

        private void b_reset_Click(object sender, EventArgs e)
        {
            reset();
            tb_numLandmarks.Text = "";
        }

        private void b_goback_Click(object sender, EventArgs e)
        {
            backToMainForm(1);
        }       

        private void advancedCoordsFileBuilder_FormClosed(object sender, FormClosedEventArgs e)
        {
            backToMainForm(0);
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

        private void b_step2_Click(object sender, EventArgs e)
        {
            step2();
        }   

        private void picturebox_map_Paint(object sender, PaintEventArgs e)
        {
            string currentMode = Properties.Settings.Default.currentMode;
            if (allRects != null && currentMode.Equals("advanced"))
            {
                foreach (UserRect ur in allRects)
                {
                    try
                    {
                        ur.Draw(e.Graphics);
                    }
                    catch (Exception exp)
                    {
                        System.Console.WriteLine(exp.Message);
                    }
                }
            }
        }

        private void picturebox_map_MouseUp(object sender, MouseEventArgs e)
        {
            if (allLabelsMoved())
            {
                b_step2.Enabled = true;
            }
        }

        private void dgv_coords_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvIsFull())
            {
                b_step2.Enabled = true;
            }
            else
            {
                b_step2.Enabled = false;
            }
        }

        private void picturebox_map_MouseMove(object sender, MouseEventArgs e)
        {
            string currentMode = Properties.Settings.Default.currentMode;
            if (e.Location != oldMouseLocation)
            {
                oldMouseLocation = e.Location;
                if (currentMode.Equals("advanced"))
                {
                    bool normalCursor = true;
                    foreach (UserRect ur in allRects)
                    {
                        if (ur.rect.Contains(oldMouseLocation)) { normalCursor = false; }
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

        private void b_enter_Click(object sender, EventArgs e)
        {
            if (numLandmarks > 0 && numLandmarks <= 48) { reset(); }
            try
            {
                numLandmarks = Convert.ToInt32(tb_numLandmarks.Text, CultureInfo.InvariantCulture);
                if (numLandmarks <= 1) { throw new System.ArgumentException("Negative less than 2 are not allowed."); }
                if (numLandmarks > 48) { throw new System.ArgumentException("Numbers greater than 48 are not allowed."); }

                for (int i = 1; i < numLandmarks + 1; i++)
                {
                    if (i == 1 && numLandmarksEntered)
                    {
                        dgv_coords.Rows[0].Cells[0].Value = "";
                        dgv_coords.Rows[0].Cells[1].Value = i;
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
            }
            catch
            {
                MessageBox.Show("Error: Did you enter a positive integer [2 - 48] for the number of landmarks?");
                tb_numLandmarks.Text = "";
                b_enter.Enabled = true;
            }
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
                            if (lastMovedUserRect.rect.Y < (picturebox_map.Size.Height - lastMovedUserRect.rect.Size.Height)) { lastMovedUserRect.rect.Y += 1; }
                        }
                        else if (keyData == Keys.Left)
                        {
                            if (lastMovedUserRect.rect.X > 0) { lastMovedUserRect.rect.X -= 1; }
                        }
                        else if (keyData == Keys.Right)
                        {
                            if (lastMovedUserRect.rect.X < (picturebox_map.Size.Width - lastMovedUserRect.rect.Size.Width)) { lastMovedUserRect.rect.X += 1; }
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
