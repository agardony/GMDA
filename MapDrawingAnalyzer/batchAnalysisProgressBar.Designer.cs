namespace MapDrawingAnalyzer
{
    partial class batchAnalysisProgressBar
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
            this.pb_batchReanalysis = new System.Windows.Forms.ProgressBar();
            this.lb_pbBA = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pb_batchReanalysis
            // 
            this.pb_batchReanalysis.Location = new System.Drawing.Point(50, 46);
            this.pb_batchReanalysis.Name = "pb_batchReanalysis";
            this.pb_batchReanalysis.Size = new System.Drawing.Size(200, 18);
            this.pb_batchReanalysis.Step = 1;
            this.pb_batchReanalysis.TabIndex = 39;
            // 
            // lb_pbBA
            // 
            this.lb_pbBA.AutoSize = true;
            this.lb_pbBA.Location = new System.Drawing.Point(56, 30);
            this.lb_pbBA.Name = "lb_pbBA";
            this.lb_pbBA.Size = new System.Drawing.Size(188, 13);
            this.lb_pbBA.TabIndex = 40;
            this.lb_pbBA.Text = "Please Wait... Batch Analysis Running";
            // 
            // batchAnalysisProgressBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 90);
            this.Controls.Add(this.lb_pbBA);
            this.Controls.Add(this.pb_batchReanalysis);
            this.Name = "batchAnalysisProgressBar";
            this.Text = "Batch Analysis Running...";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar pb_batchReanalysis;
        private System.Windows.Forms.Label lb_pbBA;

    }
}