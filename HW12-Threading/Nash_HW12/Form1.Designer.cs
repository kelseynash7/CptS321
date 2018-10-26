namespace Nash_HW12
{
    partial class Form1
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
            this.URLBox = new System.Windows.Forms.GroupBox();
            this.URLTextBox = new System.Windows.Forms.TextBox();
            this.DownloadButton = new System.Windows.Forms.Button();
            this.ResultGroupBox = new System.Windows.Forms.GroupBox();
            this.resultsBox = new System.Windows.Forms.TextBox();
            this.sortingButton = new System.Windows.Forms.Button();
            this.timeResults = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.URLBox.SuspendLayout();
            this.ResultGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // URLBox
            // 
            this.URLBox.AutoSize = true;
            this.URLBox.Controls.Add(this.URLTextBox);
            this.URLBox.Location = new System.Drawing.Point(43, 24);
            this.URLBox.Name = "URLBox";
            this.URLBox.Size = new System.Drawing.Size(349, 100);
            this.URLBox.TabIndex = 0;
            this.URLBox.TabStop = false;
            this.URLBox.Text = "URL";
            // 
            // URLTextBox
            // 
            this.URLTextBox.Location = new System.Drawing.Point(20, 43);
            this.URLTextBox.Name = "URLTextBox";
            this.URLTextBox.Size = new System.Drawing.Size(306, 26);
            this.URLTextBox.TabIndex = 0;
            this.URLTextBox.TextChanged += new System.EventHandler(this.URLTextBox_TextChanged);
            // 
            // DownloadButton
            // 
            this.DownloadButton.Location = new System.Drawing.Point(43, 160);
            this.DownloadButton.Name = "DownloadButton";
            this.DownloadButton.Size = new System.Drawing.Size(349, 38);
            this.DownloadButton.TabIndex = 1;
            this.DownloadButton.Text = "Go (Download string from URL)";
            this.DownloadButton.UseVisualStyleBackColor = true;
            this.DownloadButton.Click += new System.EventHandler(this.DownloadButton_Click);
            // 
            // ResultGroupBox
            // 
            this.ResultGroupBox.Controls.Add(this.resultsBox);
            this.ResultGroupBox.Location = new System.Drawing.Point(23, 237);
            this.ResultGroupBox.Name = "ResultGroupBox";
            this.ResultGroupBox.Size = new System.Drawing.Size(390, 381);
            this.ResultGroupBox.TabIndex = 2;
            this.ResultGroupBox.TabStop = false;
            this.ResultGroupBox.Text = "Download Result (as a string)";
            // 
            // resultsBox
            // 
            this.resultsBox.Location = new System.Drawing.Point(20, 37);
            this.resultsBox.Multiline = true;
            this.resultsBox.Name = "resultsBox";
            this.resultsBox.Size = new System.Drawing.Size(353, 320);
            this.resultsBox.TabIndex = 0;
            // 
            // sortingButton
            // 
            this.sortingButton.Location = new System.Drawing.Point(19, 60);
            this.sortingButton.Name = "sortingButton";
            this.sortingButton.Size = new System.Drawing.Size(327, 44);
            this.sortingButton.TabIndex = 3;
            this.sortingButton.Text = "Go (sorting!)";
            this.sortingButton.UseVisualStyleBackColor = true;
            this.sortingButton.Click += new System.EventHandler(this.sortingButton_Click);
            // 
            // timeResults
            // 
            this.timeResults.Location = new System.Drawing.Point(19, 134);
            this.timeResults.Multiline = true;
            this.timeResults.Name = "timeResults";
            this.timeResults.Size = new System.Drawing.Size(327, 263);
            this.timeResults.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.sortingButton);
            this.groupBox1.Controls.Add(this.timeResults);
            this.groupBox1.Location = new System.Drawing.Point(450, 105);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(369, 419);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "List Sorting (Part 2)";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 630);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ResultGroupBox);
            this.Controls.Add(this.DownloadButton);
            this.Controls.Add(this.URLBox);
            this.Name = "Form1";
            this.Text = "HW 12 - Threading - Kelsey Nash - 11093115";
            this.URLBox.ResumeLayout(false);
            this.URLBox.PerformLayout();
            this.ResultGroupBox.ResumeLayout(false);
            this.ResultGroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox URLBox;
        private System.Windows.Forms.TextBox URLTextBox;
        private System.Windows.Forms.Button DownloadButton;
        private System.Windows.Forms.GroupBox ResultGroupBox;
        private System.Windows.Forms.TextBox resultsBox;
        private System.Windows.Forms.Button sortingButton;
        private System.Windows.Forms.TextBox timeResults;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

