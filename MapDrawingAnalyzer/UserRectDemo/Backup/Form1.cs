using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UserRectDemo
{
    public partial class Form1 : Form
    {
        UserRect rect;
        public Form1()
        {
            InitializeComponent();
            rect = new UserRect(new Rectangle(10, 10, 100, 100));
            rect.SetPictureBox(this.pictureBox1);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            rect.allowDeformingDuringMovement = checkBox1.Checked;
        }      
    }
}