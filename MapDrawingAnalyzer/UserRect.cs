using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Globalization;
using PointDClass;
// Copyright 2014 Aaron Gardony
// This program is distributed under the terms of the GNU General Public License
// ADAPTED FROM http://www.codeproject.com/Articles/30434/A-Resizable-Graphical-Rectangle
// last update: 4/17/14 by Aaron Gardony
namespace UserRectDemo
{
        // STRUCT FOR double point, taken from http://stackoverflow.com/questions/5366659/converting-double-values-for-making-a-point-type

    public class UserRect 
    {        
        private PictureBox mPictureBox;
        private PictureBox missingPBox;
        public Rectangle rect;
        public bool allowDeformingDuringMovement=false ;
        private bool mIsClick=false;
        private bool mMove=false;        
        private int oldX;
        private int oldY;
        private int sizeNodeRect= 8;
        private Bitmap mBmp=null;
        private PosSizableRect nodeSelected = PosSizableRect.None;
        private string landmarkName = "";
        private bool isMissing = false;
        public Color rectColor = Color.Red;
        public Color backupRectColor = Color.Red;
        public Color rectCenterColor = Color.Red;
        public bool buildingCoordsFile = false;
        private Rectangle currentMouseOverRect = new Rectangle();
        private bool lastMoved = false;

        private enum PosSizableRect
        {   
            LeftUp,
            UpMiddle,
            RightUp,
            RightMiddle,
            RightBottom,
            BottomMiddle,
            LeftBottom,
            LeftMiddle,
            None
        };

        public UserRect(Rectangle r, string landmarkName)
        {
            rect = r;
            mIsClick = false;
            isMissing = false;
            rectColor = Color.Red;
            this.landmarkName = landmarkName;
        }

        public void setRect(Point loc, Size s)
        {
            rect.Location = loc;
            rect.Size = s;
        }
        public Rectangle getRect()
        {
            Rectangle returnRect = new Rectangle();
            returnRect.Location = rect.Location;
            returnRect.Size = rect.Size;
            return returnRect;
        }

        public void setRectColor(Color c)
        {
            rectColor = c;
            backupRectColor = c;
        }

        public void setRectCenterColor(Color c)
        {
            rectCenterColor = c;
        }

        public List<PointD> getRectBorderCartesianCoordinates()
        {
            
            List<PointD> retCoords = new List<PointD>();
            foreach (PosSizableRect r in Enum.GetValues(typeof(PosSizableRect)))
            {
                if (!r.ToString().Equals("None")) // exclude none node
                {
                    PointD retPoint = new PointD(0.0, 0.0);
                    Rectangle currRect = GetRect(r);
                    retPoint.X = (0 - (350 - currRect.Location.X));
                    retPoint.Y = (0 + (350 - currRect.Location.Y));
                    retCoords.Add(retPoint);
                }
            }
            return retCoords;
        }

        public bool getMissing()
        {
            return isMissing;
        }

        public bool getlastMoved()
        {
            return this.lastMoved;
        }

        public void setLastMoved(bool b)
        {
            this.lastMoved = b;
        }

        public void setMissing(bool im)
        {
            if (!buildingCoordsFile)
            {
                isMissing = im;
                mPictureBox.Refresh();
                missingPBox.Refresh();
                if (isMissing) { switchPicBoxes(); }

            }
        }

        public void switchPicBoxes()
        {
            mPictureBox.MouseDown -= new MouseEventHandler(mPictureBox_MouseDown);
            mPictureBox.MouseUp -= new MouseEventHandler(mPictureBox_MouseUp);
            mPictureBox.MouseMove -= new MouseEventHandler(mPictureBox_MouseMove);
            mPictureBox.MouseDoubleClick -= new MouseEventHandler(mPictureBox_MouseDoubleClick);
            SetPictureBox(missingPBox, mPictureBox);
            if (isMissing)
            {
                setRect(new Point(Convert.ToInt32(rect.X / 2.33, CultureInfo.InvariantCulture), rect.Y / 50), new Size(30, 30));
            }
            else
            {
                rect.X = Convert.ToInt32(rect.X * 2.33, CultureInfo.InvariantCulture);
                rect.Y = rect.Y * 50;
            }
            this.mMove = false;
            mPictureBox.Refresh();
            missingPBox.Refresh();
        }

        public void Draw(Graphics g)
        {
            SizeF stringSize = g.MeasureString(landmarkName, new Font("Microsoft Sans Serif", 12, FontStyle.Regular, GraphicsUnit.Pixel));
            Rectangle rectCenter = new Rectangle(new Point(rect.X + rect.Width / 2 - 8, rect.Y + rect.Height / 2 - 8), new Size(16, 16)); // center of rect is 16 x 16 pixels
            g.DrawRectangle(new Pen(rectColor),rect);
            g.DrawRectangle(new Pen(rectCenterColor),rectCenter);
            if (this.isMissing) { rectColor = Color.Black; }
            SolidBrush textBrush = new SolidBrush(rectColor);
            g.DrawString(landmarkName, new Font("Microsoft Sans Serif", 12, FontStyle.Regular, GraphicsUnit.Pixel), textBrush, new PointF((float)(rect.X + rect.Size.Width / 2 - stringSize.Width / 2), (float)(rect.Y - (stringSize.Height + 6))));
            
            foreach (PosSizableRect pos in Enum.GetValues(typeof(PosSizableRect)))
            {
              g.DrawRectangle(new Pen(rectColor),GetRect(pos));
            }
        }

        public void SetBitmapFile(string filename)
        {
            this.mBmp = new Bitmap(filename);
        }

        public void SetBitmap(Bitmap bmp)
        {
            this.mBmp = bmp;
        }

        public void SetPictureBox(PictureBox p, PictureBox m)
        {
            this.mPictureBox = p;
            this.missingPBox = m;
            mPictureBox.MouseDown +=new MouseEventHandler(mPictureBox_MouseDown);
            mPictureBox.MouseUp += new MouseEventHandler(mPictureBox_MouseUp);
            mPictureBox.MouseMove += new MouseEventHandler(mPictureBox_MouseMove);
            mPictureBox.MouseDoubleClick += new MouseEventHandler(mPictureBox_MouseDoubleClick);
            mPictureBox.Refresh();
            missingPBox.Refresh();
        }

        public void DiposePictureBox()
        {
            mPictureBox.MouseDown -= new MouseEventHandler(mPictureBox_MouseDown);
            mPictureBox.MouseUp -= new MouseEventHandler(mPictureBox_MouseUp);
            mPictureBox.MouseMove -= new MouseEventHandler(mPictureBox_MouseMove);
            mPictureBox.MouseDoubleClick -= new MouseEventHandler(mPictureBox_MouseDoubleClick);
            mPictureBox.Refresh();
            missingPBox.Refresh();
            this.mPictureBox = null;
            this.missingPBox = null;
        }



        private void mPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            mIsClick = true;
            Point mouseClick = new Point(e.X,e.Y);
            nodeSelected = PosSizableRect.None;
            nodeSelected = GetNodeSelectable(e.Location);
                
            if (isClickInCenter(rect, mouseClick))
            {
                this.lastMoved = true; 
                mMove = true;      
            }
            oldX = e.X;
            oldY = e.Y;
        }

        private void mPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            mIsClick = false;
            mMove = false;            
        }

        private void mPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            GetNodeSelectable(e.Location);
            ChangeCursor(e.Location);
            if (mIsClick == false)
            {
                return;
            }

            Rectangle backupRect = rect;
            switch (nodeSelected)
            {
                case PosSizableRect.LeftUp:
                    rect.X += e.X - oldX;
                    rect.Width -= e.X - oldX;
                    rect.Y += e.Y - oldY;
                    rect.Height -= e.Y - oldY;
                    break;
                case PosSizableRect.LeftMiddle:
                    rect.X += e.X - oldX;
                    rect.Width -= e.X - oldX;
                    break;
                case PosSizableRect.LeftBottom:
                    rect.Width -= e.X - oldX;
                    rect.X += e.X - oldX;
                    rect.Height += e.Y - oldY;
                    break;
                case PosSizableRect.BottomMiddle:
                    rect.Height += e.Y - oldY;
                    break;
                case PosSizableRect.RightUp:
                    rect.Width += e.X - oldX;
                    rect.Y += e.Y - oldY;
                    rect.Height -= e.Y - oldY;
                    break;
                case PosSizableRect.RightBottom:
                    rect.Width += e.X - oldX;
                    rect.Height += e.Y - oldY;
                    break;
                case PosSizableRect.RightMiddle:
                    rect.Width += e.X - oldX;
                    break;

                case PosSizableRect.UpMiddle:
                    rect.Y += e.Y - oldY;
                    rect.Height -= e.Y - oldY;
                    break;

                default:
                    if (mMove)
                    {
                        rect.X = rect.X + e.X - oldX;
                        rect.Y = rect.Y + e.Y - oldY;
                        this.lastMoved = true; 
                    }
                    break;
            }
            oldX = e.X;
            oldY = e.Y;

            if (rect.Width < 5 || rect.Height < 5)
            {
                rect = backupRect;
            }
            TestIfRectInsideArea();
            mPictureBox.Invalidate();
        }

        private void mPictureBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!buildingCoordsFile)
            {
                Point mouseClick = new Point(e.X, e.Y);
                if (isClickInCenter(rect, mouseClick))
                {
                    toggleMissing();
                }
                mIsClick = false;
            }
        }

        public void toggleMissing()
        {
            isMissing = !isMissing;
            if (!rectColor.Equals(Color.Black)) { rectColor = Color.Black; }
            else if (rectColor.Equals(Color.Black)) { rectColor = backupRectColor; }
            mPictureBox.Refresh();
            missingPBox.Refresh();
            switchPicBoxes();
        }

        private bool isClickInCenter(Rectangle r, Point click)
        {
            bool retBool = false;
            // define 20 x 20 pixel center area
            Rectangle tempRect = new Rectangle(new Point(r.X + r.Width / 2 - 5, r.Y + r.Height / 2 - 5), new Size(10, 10));
            if (tempRect.Contains(click))
            {
                retBool = true;
            }
            return retBool;
        }

        private void TestIfRectInsideArea()
        {
            // Test if rectangle still inside the area.
            if (rect.X < 0) rect.X = 0;
            if (rect.Y < 0) rect.Y = 0;
            if (rect.Width <= 0) rect.Width = 1;
            if (rect.Height <= 0) rect.Height = 1;

            if (rect.X + rect.Width > mPictureBox.Width)
            {
                rect.Width = mPictureBox.Width - rect.X - 1; // -1 to be still show 
                if (allowDeformingDuringMovement == false)
                {
                    mIsClick = false;
                }
            }
            if (rect.Y + rect.Height > mPictureBox.Height)
            {
                rect.Height = mPictureBox.Height - rect.Y - 1;// -1 to be still show 
                if (allowDeformingDuringMovement == false)
                {
                    mIsClick = false;
                }
            }
        }        

        private Rectangle CreateRectSizableNode(int x, int y)
        {
            return new Rectangle(x - sizeNodeRect / 2, y - sizeNodeRect / 2, sizeNodeRect, sizeNodeRect);   
        }

        private Rectangle GetRect(PosSizableRect p)
        {
            switch (p)
            {
                case PosSizableRect.LeftUp:
                    return CreateRectSizableNode(rect.X, rect.Y);
                 
                case PosSizableRect.LeftMiddle:
                    return CreateRectSizableNode(rect.X, rect.Y + +rect.Height / 2);                    

                case PosSizableRect.LeftBottom:
                    return CreateRectSizableNode(rect.X, rect.Y +rect.Height);                                   

                case PosSizableRect.BottomMiddle:
                    return CreateRectSizableNode(rect.X  + rect.Width / 2,rect.Y + rect.Height);

                case PosSizableRect.RightUp:
                    return CreateRectSizableNode(rect.X + rect.Width,rect.Y );

                case PosSizableRect.RightBottom:
                    return CreateRectSizableNode(rect.X  + rect.Width,rect.Y  + rect.Height);

                case PosSizableRect.RightMiddle:
                    return CreateRectSizableNode(rect.X  + rect.Width, rect.Y  + rect.Height / 2);

                case PosSizableRect.UpMiddle:
                    return CreateRectSizableNode(rect.X + rect.Width/2, rect.Y);
                default :
                    return new Rectangle();
            }
        }

        private PosSizableRect GetNodeSelectable(Point p)
        {
           foreach (PosSizableRect r in Enum.GetValues(typeof(PosSizableRect)))
            {
                if (GetRect(r).Contains(p))
                {
                    currentMouseOverRect = GetRect(r);
                    return r;                    
                }
            }
            // mouse is in middle
            currentMouseOverRect = this.rect;
            return PosSizableRect.None;
        }

        private void ChangeCursor(Point p)
        {
            if (currentMouseOverRect.Contains(p))
            {
                mPictureBox.Cursor = GetCursor(GetNodeSelectable(p));
            }
        }

        /// <summary>
        /// Get cursor for the handle
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private Cursor GetCursor(PosSizableRect p)
        {
            switch (p)
            {
                case PosSizableRect.LeftUp:
                    return Cursors.SizeNWSE;               

                case PosSizableRect.LeftMiddle:
                    return Cursors.SizeWE;

                case PosSizableRect.LeftBottom:
                    return Cursors.SizeNESW;

                case PosSizableRect.BottomMiddle:
                    return Cursors.SizeNS;

                case PosSizableRect.RightUp:
                    return Cursors.SizeNESW;

                case PosSizableRect.RightBottom:
                    return Cursors.SizeNWSE;

                case PosSizableRect.RightMiddle:
                    return Cursors.SizeWE;

                case PosSizableRect.UpMiddle:
                    return Cursors.SizeNS;
                default:
                    return Cursors.Default;
            }
        }

    }
}
