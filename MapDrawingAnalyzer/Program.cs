using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
// Copyright 2013 Aaron Gardony
// This program is distributed under the terms of the GNU General Public License
namespace MapDrawingAnalyzer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
