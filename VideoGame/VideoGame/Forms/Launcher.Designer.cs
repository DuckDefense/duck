namespace VideoGame.Forms
{
    partial class Launcher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Launcher));
            this.pbLaunchBackground = new System.Windows.Forms.PictureBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbLaunchBackground)).BeginInit();
            this.SuspendLayout();
            // 
            // pbLaunchBackground
            // 
            this.pbLaunchBackground.Image = ((System.Drawing.Image)(resources.GetObject("pbLaunchBackground.Image")));
            this.pbLaunchBackground.Location = new System.Drawing.Point(-2, -1);
            this.pbLaunchBackground.Name = "pbLaunchBackground";
            this.pbLaunchBackground.Size = new System.Drawing.Size(477, 239);
            this.pbLaunchBackground.TabIndex = 0;
            this.pbLaunchBackground.TabStop = false;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(381, 12);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 1;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(12, 200);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(75, 23);
            this.btnPlay.TabIndex = 2;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // Launcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 235);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.pbLaunchBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Launcher";
            this.Text = "Launcher";
            ((System.ComponentModel.ISupportInitialize)(this.pbLaunchBackground)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbLaunchBackground;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnPlay;
    }
}