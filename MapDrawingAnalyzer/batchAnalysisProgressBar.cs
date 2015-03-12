using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
// Copyright 2014 Aaron Gardony
// This program is distributed under the terms of the GNU General Public License
// last update: 4/17/14 by Aaron Gardony
namespace MapDrawingAnalyzer
{
    public partial class batchAnalysisProgressBar : Form
    {
        private readonly Form1 _form1;
        public batchAnalysisProgressBar(Form1 form1)
        {
            _form1 = form1;
            InitializeComponent();
        }

        public void setPB(int val)
        {
            if (val != 100) { val = val + 1; }
            this.pb_batchReanalysis.Value = val;
            if (val != 100) { this.pb_batchReanalysis.Value--; }
        }

        public void clearPB()
        {
            pb_batchReanalysis.Maximum = 1000;
            pb_batchReanalysis.Value = 1000;
            pb_batchReanalysis.Value = 999;
            pb_batchReanalysis.Maximum = 100;
        }
    }
}
