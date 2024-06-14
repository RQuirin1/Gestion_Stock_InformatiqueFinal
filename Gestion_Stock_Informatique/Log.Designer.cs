namespace Gestion_Stock_Informatique
{
    partial class Log
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Log));
            buttonSubmit = new Button();
            buttonCancel = new Button();
            labelLog = new Label();
            labelMdp = new Label();
            textBoxLog = new TextBox();
            textBoxMdp = new TextBox();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // buttonSubmit
            // 
            buttonSubmit.Enabled = false;
            buttonSubmit.Location = new Point(350, 279);
            buttonSubmit.Name = "buttonSubmit";
            buttonSubmit.Size = new Size(112, 34);
            buttonSubmit.TabIndex = 0;
            buttonSubmit.Text = "Valider";
            buttonSubmit.UseVisualStyleBackColor = true;
            buttonSubmit.Click += buttonSubmit_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(232, 279);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(112, 34);
            buttonCancel.TabIndex = 1;
            buttonCancel.Text = "Annuler";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // labelLog
            // 
            labelLog.AutoSize = true;
            labelLog.Location = new Point(123, 131);
            labelLog.Name = "labelLog";
            labelLog.Size = new Size(65, 25);
            labelLog.TabIndex = 2;
            labelLog.Text = "Login :";
            // 
            // labelMdp
            // 
            labelMdp.AutoSize = true;
            labelMdp.Location = new Point(59, 197);
            labelMdp.Name = "labelMdp";
            labelMdp.Size = new Size(129, 25);
            labelMdp.TabIndex = 3;
            labelMdp.Text = "Mot de passe :";
            // 
            // textBoxLog
            // 
            textBoxLog.BackColor = Color.Gainsboro;
            textBoxLog.Location = new Point(194, 128);
            textBoxLog.Name = "textBoxLog";
            textBoxLog.Size = new Size(150, 31);
            textBoxLog.TabIndex = 4;
            textBoxLog.TextChanged += textBoxLog_TextChanged;
            // 
            // textBoxMdp
            // 
            textBoxMdp.BackColor = Color.Gainsboro;
            textBoxMdp.Location = new Point(194, 194);
            textBoxMdp.Name = "textBoxMdp";
            textBoxMdp.Size = new Size(150, 31);
            textBoxMdp.TabIndex = 5;
            textBoxMdp.UseSystemPasswordChar = true;
            textBoxMdp.TextChanged += textBoxMdp_TextChanged;
            textBoxMdp.KeyDown += textBoxMdp_KeyDown;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(161, 13);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(183, 94);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            // 
            // Log
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DarkGray;
            ClientSize = new Size(476, 330);
            Controls.Add(pictureBox1);
            Controls.Add(textBoxMdp);
            Controls.Add(textBoxLog);
            Controls.Add(labelMdp);
            Controls.Add(labelLog);
            Controls.Add(buttonCancel);
            Controls.Add(buttonSubmit);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "Log";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gestion des stocks";
            Load += Log_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonSubmit;
        private Button buttonCancel;
        private Label labelLog;
        private Label labelMdp;
        private TextBox textBoxLog;
        private TextBox textBoxMdp;
        private PictureBox pictureBox1;
    }
}