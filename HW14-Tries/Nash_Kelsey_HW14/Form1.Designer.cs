namespace Nash_Kelsey_HW14
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
            this.inputBox = new System.Windows.Forms.GroupBox();
            this.resultsBox = new System.Windows.Forms.GroupBox();
            this.userInput = new System.Windows.Forms.TextBox();
            this.autoCompleteResults = new System.Windows.Forms.TextBox();
            this.inputBox.SuspendLayout();
            this.resultsBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // inputBox
            // 
            this.inputBox.Controls.Add(this.userInput);
            this.inputBox.Location = new System.Drawing.Point(22, 21);
            this.inputBox.Name = "inputBox";
            this.inputBox.Size = new System.Drawing.Size(613, 66);
            this.inputBox.TabIndex = 0;
            this.inputBox.TabStop = false;
            this.inputBox.Text = "Prefix Input";
            // 
            // resultsBox
            // 
            this.resultsBox.Controls.Add(this.autoCompleteResults);
            this.resultsBox.Location = new System.Drawing.Point(22, 109);
            this.resultsBox.Name = "resultsBox";
            this.resultsBox.Size = new System.Drawing.Size(613, 669);
            this.resultsBox.TabIndex = 1;
            this.resultsBox.TabStop = false;
            this.resultsBox.Text = "Results";
            // 
            // userInput
            // 
            this.userInput.Location = new System.Drawing.Point(6, 25);
            this.userInput.Name = "userInput";
            this.userInput.Size = new System.Drawing.Size(601, 26);
            this.userInput.TabIndex = 0;
            this.userInput.TextChanged += new System.EventHandler(this.userInput_TextChanged);
            // 
            // autoCompleteResults
            // 
            this.autoCompleteResults.Location = new System.Drawing.Point(6, 25);
            this.autoCompleteResults.Multiline = true;
            this.autoCompleteResults.Name = "autoCompleteResults";
            this.autoCompleteResults.Size = new System.Drawing.Size(601, 638);
            this.autoCompleteResults.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 799);
            this.Controls.Add(this.resultsBox);
            this.Controls.Add(this.inputBox);
            this.Name = "Form1";
            this.Text = "Trie Prefix Auto-Complete - Kelsey Nash - 11093115";
            this.inputBox.ResumeLayout(false);
            this.inputBox.PerformLayout();
            this.resultsBox.ResumeLayout(false);
            this.resultsBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox inputBox;
        private System.Windows.Forms.GroupBox resultsBox;
        private System.Windows.Forms.TextBox userInput;
        private System.Windows.Forms.TextBox autoCompleteResults;
    }
}

