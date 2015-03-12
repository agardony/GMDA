using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
// Copyright 2014 Aaron Gardony
// This program is distributed under the terms of the GNU General Public License
// last update: 4/17/14 by Aaron Gardony
namespace Plexiglass
{

    public partial class Plexiglass : Form
    {

        public Plexiglass(Control tocover, Form containingForm)
        {
            var frm = new Form();
            this.BackColor = Color.Yellow;
            this.Opacity = 0.30;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ControlBox = false;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.AutoScaleMode = AutoScaleMode.None;
            this.AutoSize = false;
            Point loc = new Point(tocover.Location.X + containingForm.Location.X, tocover.Location.Y + containingForm.Location.Y + 29);
            this.Location = loc;
            this.Size = tocover.Size;
            this.MinimumSize = tocover.Size;
            this.MaximumSize = tocover.Size;
        }

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }
    }
}
