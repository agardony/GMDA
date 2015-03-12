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

namespace MapDrawingAnalyzer
{
    public partial class Form1 : Form
    {
        
        private int x;
        private int y;
        private double maxDist = 0.0;
        private int clickCounter = 0;
        private double[] maxDistClickArray = new double[4];
        private bool maxDistClick = false;
        private List<string>[] combinations = new List<string>[6];
        PictureBox[] pictureArray = new PictureBox[16];
        private StreamWriter outfile;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)// Open map picture, initialize variables
        {
            String[] tempArray = new string[2];
            String thisLine;

            combinations[0] = new List<string>();
            combinations[1] = new List<string>();
            combinations[2] = new List<string>();
            combinations[3] = new List<string>();
            combinations[4] = new List<string>();
            combinations[5] = new List<string>();

            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "JPG files (*.jpg)|*.jpg|JPEG files (*.jpeg)|*.jpeg|All files (*.*)|*.*";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(openFile.OpenFile());

                Size imgSize = new Size(700,700);

                //create a new Bitmap with the proper dimensions
                Bitmap finalImg = new Bitmap(pictureBox1.Image, imgSize.Width, imgSize.Height);
     
                //create a new Graphics object from the image
                Graphics gfx = Graphics.FromImage(pictureBox1.Image);
   
                //clean up the image (take care of any image loss from resizing)
                gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
    
                //empty the PictureBox
                pictureBox1.Image = null;
    
                //center the new image
                pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
    
                //set the new image
                pictureBox1.Image = finalImg;

            }
            // load landmark combinations
            StreamReader infile = new StreamReader(Application.StartupPath + "//Resources//landmarkCombinations.csv");
            while (!infile.EndOfStream)
            {
                thisLine = infile.ReadLine();
                tempArray = thisLine.Split(',');
                combinations[0].Add(tempArray[0]);
                combinations[1].Add(tempArray[1]);
            }

            // load picture Array
            pictureArray[0] = pictureBox2;
            pictureArray[1] = pictureBox3;
            pictureArray[2] = pictureBox4;
            pictureArray[3] = pictureBox5;
            pictureArray[4] = pictureBox6;
            pictureArray[5] = pictureBox7;
            pictureArray[6] = pictureBox8;
            pictureArray[7] = pictureBox9;
            pictureArray[8] = pictureBox10;
            pictureArray[9] = pictureBox11;
            pictureArray[10] = pictureBox12;
            pictureArray[11] = pictureBox13;
            pictureArray[12] = pictureBox14;
            pictureArray[13] = pictureBox15;
            pictureArray[14] = pictureBox16;
            pictureArray[15] = pictureBox17;

            textBox1.Enabled = true;

        }

        private string[] compareLandmarks(int L1, int L2)// this function compares two landmarks using categorical NSEW measures and quantitative distance and angle measures
        {
            string[] NSEW = new string[4];
            // Canonical Comparison
            if (pictureArray[L1-1].Top < 710 & pictureArray[L2-1].Top < 710)// If the landmarks labels are not in the missing landmark zone...
            {
                if (pictureArray[L2-1].Top - pictureArray[L1-1].Top > 0) { NSEW[0] = "S"; }
                if (pictureArray[L2-1].Top - pictureArray[L1-1].Top < 0) { NSEW[0] = "N"; }
                if (pictureArray[L2-1].Top - pictureArray[L1-1].Top == 0) { NSEW[0] = "F"; }// If on the same Y axis, code as wrong
                if (pictureArray[L2-1].Left - pictureArray[L1-1].Left > 0) { NSEW[1] = "E"; }
                if (pictureArray[L2-1].Left - pictureArray[L1-1].Left < 0) { NSEW[1] = "W"; }
                if (pictureArray[L2 - 1].Left - pictureArray[L1 - 1].Left == 0) { NSEW[1] = "F"; }// If on the same X axis, code as wrong
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

            return NSEW;
        }

        private string getDistanceRatio(int L1, int L2)// L1 = Source, L2 = Target, get's distance between L1 and L2 relative to max distance
        {
            string distanceRatio = "";
            double dblDistanceRatio = 0.0;
            // convert c# coordinate system to standard cartesian plane
            double x1 = (0 - (350 - pictureArray[L1-1].Left));
            double x2 = (0 - (350 - pictureArray[L2-1].Left));
            double y1 = (0 + (350 - pictureArray[L1-1].Top));
            double y2 = (0 + (350 - pictureArray[L2-1].Top));

            dblDistanceRatio = Math.Sqrt(Math.Pow((x2 - x1),2) + Math.Pow((y2 - y1),2))/maxDist;
            distanceRatio = dblDistanceRatio.ToString();
            return distanceRatio;
        }

        private string getAngle(int L1, int L2)// L1 = Source, L2 = Target, gets angle between L1 and L2
        {
            string angle = "";
            double dblAngle = 0.0;
            // convert c# coordinate system to standard cartesian plane
            double x1 = (0 - (350 - pictureArray[L1 - 1].Left));
            double x2 = (0 - (350 - pictureArray[L2 - 1].Left));
            double y1 = (0 + (350 - pictureArray[L1 - 1].Top));
            double y2 = (0 + (350 - pictureArray[L2 - 1].Top));

            dblAngle = Math.Atan2(x2 - x1, y2 - y1);
            dblAngle = (180.0 / Math.PI) * dblAngle;// convert rads to degrees
            angle = dblAngle.ToString();
            return angle;
        }

        
        private void button2_Click(object sender, EventArgs e)// Calculate and output data
        {
            outfile = new StreamWriter(Application.StartupPath + "//Data//mapDrawingAnalysis.csv");
            int numCorrect = 0;
            int numMissing = 0;
            int numDistanceCorrect = 0;
            double distanceSum = 0.0;
            double angleSum = 0.0;
            double ABSdistanceSum = 0.0;
            double ABSangleSum = 0.0;
            double percentCorrect = 0;
            string[] tempArray;
            string thisLine;
            outfile.WriteLine("Source,Target,Actual(N/S),Actual(E/W),Observed(N/S),Observed(E/W),Actual D ratio,Actual Angle,Observed D ratio,Observed Angle,O-A Distance,O-A Angle,ABS(O-A) Distance, ABS(O-A) Angle");// header of output file
            StreamReader infile = new StreamReader(Application.StartupPath + "//Resources//map"+textBox1.Text+".csv");
            double OminusADistance = 0.0;
            double OminusAAngle = 0.0;
            double ABSOminusADistance = 0.0;
            double ABSOminusAAngle = 0.0;
            while (!infile.EndOfStream)// load NSEW info, distance ratios, and angle for specified map #
            {
                thisLine = infile.ReadLine();
                tempArray = thisLine.Split(',');
                combinations[2].Add(tempArray[2]);
                combinations[3].Add(tempArray[3]);
                combinations[4].Add(tempArray[4]);
                combinations[5].Add(tempArray[5]);
            }
            for (int i = 0; i < 120; i++)// make comparisons, update counters
            {
                tempArray = compareLandmarks(Convert.ToInt32(combinations[0][i]), Convert.ToInt32(combinations[1][i]));
                if(tempArray[0].Equals(combinations[2][i])){numCorrect++;}
                if(tempArray[1].Equals(combinations[3][i])){numCorrect++;}
                if(tempArray[0].Equals("M")) 
                { 
                    numMissing+=2;
                    OminusADistance = 0.0;
                    ABSOminusADistance = 0.0;
                    OminusAAngle = 0.0;
                    ABSOminusAAngle = 0.0;
                }
                if(!tempArray[0].Equals("M"))
                {
                    OminusADistance = Convert.ToDouble(tempArray[2]) - Convert.ToDouble(combinations[4][i]);
                    ABSOminusADistance = Math.Abs(OminusADistance);

                    distanceSum += OminusADistance;
                    ABSdistanceSum += ABSOminusADistance;

                    OminusAAngle = Convert.ToDouble(tempArray[3]) - Convert.ToDouble(combinations[5][i]);
                    ABSOminusAAngle = Math.Abs(OminusAAngle);

                    angleSum += OminusAAngle;
                    ABSangleSum += ABSOminusAAngle;
                    numDistanceCorrect++;
                }
                outfile.WriteLine(combinations[0][i].ToString() + "," // Source
                    + combinations[1][i].ToString() + "," // Target
                    + combinations[2][i].ToString() + "," // Actual(N/S)
                    + combinations[3][i].ToString() + "," // Actual(E/W)
                    + tempArray[0] + "," // Observed(N/S)
                    + tempArray[1] // Observed(E/W)
                    + "," + combinations[4][i].ToString() // Actual D ratio
                    + "," + combinations[5][i].ToString() // Actual Angle
                    + "," + tempArray[2] // Observed D ratio
                    + "," + tempArray[3] // Observed Angle
                    + "," + OminusADistance.ToString() // O-A Distance
                    + "," + OminusAAngle.ToString() // O-A Angle
                    + "," + ABSOminusADistance.ToString() // ABS(O-A) Distance
                    + "," + ABSOminusAAngle.ToString()); // ABS(O-A) Angle
            }
            // Info at end of output file
            outfile.WriteLine("Total Correct = ,"+numCorrect);
            percentCorrect = numCorrect / 240.0;
            outfile.WriteLine("Total % Correct = ," + percentCorrect);
            percentCorrect = numCorrect / (240.0 - numMissing);
            outfile.WriteLine("Observed % Correct = ," + percentCorrect);
            numMissing = numMissing / 16;
            outfile.WriteLine("Num Landmarks Missing = ," + numMissing);
            outfile.WriteLine("O-A distance mean = ," + distanceSum / numDistanceCorrect);
            outfile.WriteLine("O-A actual mean = ," + angleSum / numDistanceCorrect);
            outfile.WriteLine("O-A distance mean (ABS) = ," + ABSdistanceSum/numDistanceCorrect);
            outfile.WriteLine("O-A angle mean (ABS) = ," + ABSangleSum/numDistanceCorrect);
            outfile.WriteLine("O-A distance proportion = ," + (1 - (ABSdistanceSum / numDistanceCorrect)).ToString());
            outfile.WriteLine("O-A angle proportion = ," + (1 - ((ABSangleSum / numDistanceCorrect)/180.0)).ToString());
            outfile.Close();
            MessageBox.Show("Observed % Correct = " + percentCorrect + "\nO-A angle proportion = " + (1 - ((ABSangleSum / numDistanceCorrect) / 180.0)).ToString());
        }

        private void rotateWorkspace()
        {
           // Image oldImage = pictureBox1.Image;
            pictureBox1.Image = rotateImage(pictureBox1.Image,new PointF(350,350),90);
            foreach (PictureBox P in pictureArray) 
            {
                if (P.Top < 710)
                {
                    int i = P.Left;
                    P.Left = (700 - P.Location.Y) + 10;
                    P.Top = i;
                }
                //((700 - P.Location.Y) - P.Location.X, P.Location.X);
               // P.Location.Y = 
            }
          /*  if (oldImage != null)
            {
                oldImage.Dispose();
            }*/
           // pictureBox1 .Image = Uti
        }
        
        public static Bitmap rotateImage(Image image, PointF offset, float angle)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            //create a new empty bitmap to hold rotated image
            Bitmap rotatedBmp = new Bitmap(image.Width, image.Height);
            rotatedBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            //make a graphics object from the empty bitmap
            Graphics g = Graphics.FromImage(rotatedBmp);

            //Put the rotation point in the center of the image
            g.TranslateTransform(offset.X, offset.Y);

            //rotate the image
            g.RotateTransform(angle);

            //move the image back
            g.TranslateTransform(-offset.X, -offset.Y);

            //draw passed in image onto graphics object
            g.DrawImage(image, new PointF(0, 0));

            return rotatedBmp;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)// map # textbox
        {
            if (textBox1.Text.Equals("") || Convert.ToInt32(textBox1.Text) > 4 || Convert.ToInt32(textBox1.Text) < 1)
            {
                button2.Enabled = false;
                textBox2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                textBox2.Text = "";    
            }
            else{
                textBox2.Enabled = true;
                button3.Enabled = true;
                StreamReader infile = new StreamReader(Application.StartupPath + "//Resources//map" + textBox1.Text + ".txt");
                while (!infile.EndOfStream)
                { textBox2.Text += infile.ReadToEnd(); }
            }
        }

        private void button3_Click(object sender, EventArgs e)// max Dist button, click it, then click two location representing the farthest landmarks from eachother to get max distance
        {
            maxDistClick = true;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            textBox3.Enabled = false;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (maxDistClick)
            {
                if (clickCounter == 0)
                {
                    maxDistClickArray[0] = 0-(350-e.X);
                    maxDistClickArray[1] = 0+(350-e.Y);
                    clickCounter++;
                }
                else
                {
                    maxDistClickArray[2] = 0 - (350 - e.X);
                    maxDistClickArray[3] = 0 + (350 - e.Y);
                    maxDist = Math.Sqrt(Math.Pow((maxDistClickArray[2] - maxDistClickArray[0]),2) + Math.Pow((maxDistClickArray[3] - maxDistClickArray[1]),2));
                    textBox3.Text = "Max d = " + Math.Round(maxDist,2).ToString();
                    textBox3.Enabled = true;
                    maxDistClick = false;
                    clickCounter = 0;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                }
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            rotateWorkspace();
        }     
        // the following methods allow the landmark labels to be moved
        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                x = e.X;
                y = e.Y;
            }
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                pictureBox2.Left += (e.X - x);
                pictureBox2.Top += (e.Y - y);
            }
        }

        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                x = e.X;
                y = e.Y;
            }
        }

        private void pictureBox3_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                pictureBox3.Left += (e.X - x);
                pictureBox3.Top += (e.Y - y);
            }
        }

        private void pictureBox4_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                x = e.X;
                y = e.Y;
            }
        }

        private void pictureBox4_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                pictureBox4.Left += (e.X - x);
                pictureBox4.Top += (e.Y - y);
            }
        }

        private void pictureBox5_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                x = e.X;
                y = e.Y;
            }
        }

        private void pictureBox5_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                pictureBox5.Left += (e.X - x);
                pictureBox5.Top += (e.Y - y);
            }
        }

        private void pictureBox6_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                x = e.X;
                y = e.Y;
            }
        }

        private void pictureBox6_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                pictureBox6.Left += (e.X - x);
                pictureBox6.Top += (e.Y - y);
            }
        }

        private void pictureBox7_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                x = e.X;
                y = e.Y;
            }
        }

        private void pictureBox7_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                pictureBox7.Left += (e.X - x);
                pictureBox7.Top += (e.Y - y);
            }
        }

        private void pictureBox8_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                x = e.X;
                y = e.Y;
            }
        }

        private void pictureBox8_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                pictureBox8.Left += (e.X - x);
                pictureBox8.Top += (e.Y - y);
            }
        }

        private void pictureBox9_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                x = e.X;
                y = e.Y;
            }
        }

        private void pictureBox9_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                pictureBox9.Left += (e.X - x);
                pictureBox9.Top += (e.Y - y);
            }
        }

        private void pictureBox10_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                x = e.X;
                y = e.Y;
            }
        }

        private void pictureBox10_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                pictureBox10.Left += (e.X - x);
                pictureBox10.Top += (e.Y - y);
            }
        }

        private void pictureBox11_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                x = e.X;
                y = e.Y;
            }
        }

        private void pictureBox11_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                pictureBox11.Left += (e.X - x);
                pictureBox11.Top += (e.Y - y);
            }
        }

        private void pictureBox12_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                x = e.X;
                y = e.Y;
            }
        }

        private void pictureBox12_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                pictureBox12.Left += (e.X - x);
                pictureBox12.Top += (e.Y - y);
            }
        }

        private void pictureBox13_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                x = e.X;
                y = e.Y;
            }
        }

        private void pictureBox13_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                pictureBox13.Left += (e.X - x);
                pictureBox13.Top += (e.Y - y);
            }
        }

        private void pictureBox14_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                x = e.X;
                y = e.Y;
            }
        }

        private void pictureBox14_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                pictureBox14.Left += (e.X - x);
                pictureBox14.Top += (e.Y - y);
            }
        }

        private void pictureBox15_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                x = e.X;
                y = e.Y;
            }
        }

        private void pictureBox15_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                pictureBox15.Left += (e.X - x);
                pictureBox15.Top += (e.Y - y);
            }

        }

        private void pictureBox16_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                x = e.X;
                y = e.Y;
            }
        }

        private void pictureBox16_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                pictureBox16.Left += (e.X - x);
                pictureBox16.Top += (e.Y - y);
            }

        }

        private void pictureBox17_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                x = e.X;
                y = e.Y;
            }
        }

        private void pictureBox17_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                pictureBox17.Left += (e.X - x);
                pictureBox17.Top += (e.Y - y);
            }

        }
    }
}
