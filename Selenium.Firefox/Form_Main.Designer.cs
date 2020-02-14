namespace Valloon.Selenium
{
    partial class Form_Main
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
            this.button_StartPublishing = new System.Windows.Forms.Button();
            this.textBox_RootDirectory = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_Email = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_DataFilename = new System.Windows.Forms.TextBox();
            this.textBox_Password = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_StartPublishing
            // 
            this.button_StartPublishing.Location = new System.Drawing.Point(356, 89);
            this.button_StartPublishing.Margin = new System.Windows.Forms.Padding(4);
            this.button_StartPublishing.Name = "button_StartPublishing";
            this.button_StartPublishing.Size = new System.Drawing.Size(153, 65);
            this.button_StartPublishing.TabIndex = 0;
            this.button_StartPublishing.Text = "Start Publishing";
            this.button_StartPublishing.UseVisualStyleBackColor = true;
            this.button_StartPublishing.Click += new System.EventHandler(this.button_StartPublshing_Click);
            // 
            // textBox_RootDirectory
            // 
            this.textBox_RootDirectory.Location = new System.Drawing.Point(109, 12);
            this.textBox_RootDirectory.Name = "textBox_RootDirectory";
            this.textBox_RootDirectory.Size = new System.Drawing.Size(400, 27);
            this.textBox_RootDirectory.TabIndex = 1;
            this.textBox_RootDirectory.Text = "D:\\_temp\\Cars";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Location = new System.Drawing.Point(10, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = "Email";
            // 
            // textBox_Email
            // 
            this.textBox_Email.Location = new System.Drawing.Point(109, 89);
            this.textBox_Email.Name = "textBox_Email";
            this.textBox_Email.Size = new System.Drawing.Size(237, 27);
            this.textBox_Email.TabIndex = 5;
            this.textBox_Email.Text = "lovrenachette491@laserdog.nl";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label3.Location = new System.Drawing.Point(10, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 19);
            this.label3.TabIndex = 6;
            this.label3.Text = "Root Folder";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(10, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 19);
            this.label1.TabIndex = 8;
            this.label1.Text = "Data File";
            // 
            // textBox_DataFilename
            // 
            this.textBox_DataFilename.Location = new System.Drawing.Point(109, 51);
            this.textBox_DataFilename.Name = "textBox_DataFilename";
            this.textBox_DataFilename.Size = new System.Drawing.Size(400, 27);
            this.textBox_DataFilename.TabIndex = 7;
            this.textBox_DataFilename.Text = "D:\\_temp\\Cars\\data.txt";
            // 
            // textBox_Password
            // 
            this.textBox_Password.Location = new System.Drawing.Point(109, 127);
            this.textBox_Password.Name = "textBox_Password";
            this.textBox_Password.PasswordChar = '●';
            this.textBox_Password.Size = new System.Drawing.Size(237, 27);
            this.textBox_Password.TabIndex = 10;
            this.textBox_Password.Text = "Dadadaxx901!!";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label4.Location = new System.Drawing.Point(10, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 19);
            this.label4.TabIndex = 9;
            this.label4.Text = "Password";
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(521, 165);
            this.Controls.Add(this.textBox_Password);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_DataFilename);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_Email);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_RootDirectory);
            this.Controls.Add(this.button_StartPublishing);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Form_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "leboncoin.fr";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_Main_FormClosed);
            this.Load += new System.EventHandler(this.Form_Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button_StartPublishing;
        private System.Windows.Forms.TextBox textBox_RootDirectory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_Email;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_DataFilename;
        private System.Windows.Forms.TextBox textBox_Password;
        private System.Windows.Forms.Label label4;
    }
}

