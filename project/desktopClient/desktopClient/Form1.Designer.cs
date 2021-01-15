namespace desktopClient
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnSend = new System.Windows.Forms.Button();
            this.messageLog = new System.Windows.Forms.TextBox();
            this.messageInput = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.radioButtonNormal = new System.Windows.Forms.RadioButton();
            this.radioButtonAngry = new System.Windows.Forms.RadioButton();
            this.radioButtonDrunk = new System.Windows.Forms.RadioButton();
            this.radioButtonSarc = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(351, 334);
            this.btnSend.Margin = new System.Windows.Forms.Padding(6, 2, 6, 2);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(53, 26);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // messageLog
            // 
            this.messageLog.Location = new System.Drawing.Point(27, 119);
            this.messageLog.Margin = new System.Windows.Forms.Padding(6, 2, 6, 2);
            this.messageLog.Multiline = true;
            this.messageLog.Name = "messageLog";
            this.messageLog.ReadOnly = true;
            this.messageLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.messageLog.Size = new System.Drawing.Size(391, 308);
            this.messageLog.TabIndex = 1;
            this.messageLog.TextChanged += new System.EventHandler(this.messageLog_TextChanged);
            // 
            // messageInput
            // 
            this.messageInput.Location = new System.Drawing.Point(12, 336);
            this.messageInput.Margin = new System.Windows.Forms.Padding(6, 2, 6, 2);
            this.messageInput.Name = "messageInput";
            this.messageInput.Size = new System.Drawing.Size(327, 19);
            this.messageInput.TabIndex = 2;
            this.messageInput.Text = "Input";
            this.messageInput.TextChanged += new System.EventHandler(this.messageInput_TextChanged);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 367);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(6, 2, 6, 2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(392, 23);
            this.progressBar1.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.btnSend);
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Controls.Add(this.messageInput);
            this.groupBox1.Location = new System.Drawing.Point(14, 96);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(6, 2, 6, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6, 2, 6, 2);
            this.groupBox1.Size = new System.Drawing.Size(416, 407);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Message Log";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(243, 14);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(6, 2, 6, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(110, 65);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("OCR A Extended", 25.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 37);
            this.label1.TabIndex = 6;
            this.label1.Text = "ChattApp";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButtonSarc);
            this.groupBox2.Controls.Add(this.radioButtonDrunk);
            this.groupBox2.Controls.Add(this.radioButtonAngry);
            this.groupBox2.Controls.Add(this.radioButtonNormal);
            this.groupBox2.Location = new System.Drawing.Point(439, 96);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(150, 407);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mode";
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Location = new System.Drawing.Point(484, 23);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(75, 23);
            this.buttonUpdate.TabIndex = 8;
            this.buttonUpdate.Text = "Update";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // radioButtonNormal
            // 
            this.radioButtonNormal.AutoSize = true;
            this.radioButtonNormal.Checked = true;
            this.radioButtonNormal.Location = new System.Drawing.Point(7, 19);
            this.radioButtonNormal.Name = "radioButtonNormal";
            this.radioButtonNormal.Size = new System.Drawing.Size(58, 17);
            this.radioButtonNormal.TabIndex = 2;
            this.radioButtonNormal.TabStop = true;
            this.radioButtonNormal.Text = "Normal";
            this.radioButtonNormal.UseVisualStyleBackColor = true;
            // 
            // radioButtonAngry
            // 
            this.radioButtonAngry.AutoSize = true;
            this.radioButtonAngry.Location = new System.Drawing.Point(7, 42);
            this.radioButtonAngry.Name = "radioButtonAngry";
            this.radioButtonAngry.Size = new System.Drawing.Size(52, 17);
            this.radioButtonAngry.TabIndex = 3;
            this.radioButtonAngry.Text = "Angry";
            this.radioButtonAngry.UseVisualStyleBackColor = true;
            // 
            // radioButtonDrunk
            // 
            this.radioButtonDrunk.AutoSize = true;
            this.radioButtonDrunk.Location = new System.Drawing.Point(7, 65);
            this.radioButtonDrunk.Name = "radioButtonDrunk";
            this.radioButtonDrunk.Size = new System.Drawing.Size(54, 17);
            this.radioButtonDrunk.TabIndex = 4;
            this.radioButtonDrunk.Text = "Drunk";
            this.radioButtonDrunk.UseVisualStyleBackColor = true;
            // 
            // radioButtonSarc
            // 
            this.radioButtonSarc.AutoSize = true;
            this.radioButtonSarc.Location = new System.Drawing.Point(7, 88);
            this.radioButtonSarc.Name = "radioButtonSarc";
            this.radioButtonSarc.Size = new System.Drawing.Size(69, 17);
            this.radioButtonSarc.TabIndex = 5;
            this.radioButtonSarc.Text = "Sarcastic";
            this.radioButtonSarc.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 521);
            this.Controls.Add(this.buttonUpdate);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.messageLog);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(6, 2, 6, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSend;
        public System.Windows.Forms.TextBox messageLog;
        private System.Windows.Forms.TextBox messageInput;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.RadioButton radioButtonDrunk;
        private System.Windows.Forms.RadioButton radioButtonAngry;
        private System.Windows.Forms.RadioButton radioButtonNormal;
        private System.Windows.Forms.RadioButton radioButtonSarc;
    }
}

