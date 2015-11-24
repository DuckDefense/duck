namespace VideoGame.Forms
{
    partial class Settings
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpKeybinds = new System.Windows.Forms.TabPage();
            this.btnMoveLeft = new System.Windows.Forms.Button();
            this.btnMoveRight = new System.Windows.Forms.Button();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.lblLeft = new System.Windows.Forms.Label();
            this.lblRight = new System.Windows.Forms.Label();
            this.lblDown = new System.Windows.Forms.Label();
            this.lblUp = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tpGraphics = new System.Windows.Forms.TabPage();
            this.cbb16to9 = new System.Windows.Forms.ComboBox();
            this.cbb4to3 = new System.Windows.Forms.ComboBox();
            this.cbbRatio = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tpKeybinds.SuspendLayout();
            this.tpGraphics.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabControl1.Controls.Add(this.tpKeybinds);
            this.tabControl1.Controls.Add(this.tpGraphics);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(310, 155);
            this.tabControl1.TabIndex = 0;
            // 
            // tpKeybinds
            // 
            this.tpKeybinds.Controls.Add(this.btnMoveLeft);
            this.tpKeybinds.Controls.Add(this.btnMoveRight);
            this.tpKeybinds.Controls.Add(this.btnMoveDown);
            this.tpKeybinds.Controls.Add(this.btnMoveUp);
            this.tpKeybinds.Controls.Add(this.lblLeft);
            this.tpKeybinds.Controls.Add(this.lblRight);
            this.tpKeybinds.Controls.Add(this.lblDown);
            this.tpKeybinds.Controls.Add(this.lblUp);
            this.tpKeybinds.Controls.Add(this.label4);
            this.tpKeybinds.Controls.Add(this.label3);
            this.tpKeybinds.Controls.Add(this.label2);
            this.tpKeybinds.Controls.Add(this.label1);
            this.tpKeybinds.Location = new System.Drawing.Point(4, 25);
            this.tpKeybinds.Name = "tpKeybinds";
            this.tpKeybinds.Padding = new System.Windows.Forms.Padding(3);
            this.tpKeybinds.Size = new System.Drawing.Size(302, 126);
            this.tpKeybinds.TabIndex = 0;
            this.tpKeybinds.Text = "Keybinds";
            this.tpKeybinds.UseVisualStyleBackColor = true;
            // 
            // btnMoveLeft
            // 
            this.btnMoveLeft.Location = new System.Drawing.Point(187, 93);
            this.btnMoveLeft.Name = "btnMoveLeft";
            this.btnMoveLeft.Size = new System.Drawing.Size(75, 23);
            this.btnMoveLeft.TabIndex = 11;
            this.btnMoveLeft.Text = "Change";
            this.btnMoveLeft.UseVisualStyleBackColor = true;
            this.btnMoveLeft.Click += new System.EventHandler(this.btnMoveLeft_Click);
            // 
            // btnMoveRight
            // 
            this.btnMoveRight.Location = new System.Drawing.Point(187, 64);
            this.btnMoveRight.Name = "btnMoveRight";
            this.btnMoveRight.Size = new System.Drawing.Size(75, 23);
            this.btnMoveRight.TabIndex = 10;
            this.btnMoveRight.Text = "Change";
            this.btnMoveRight.UseVisualStyleBackColor = true;
            this.btnMoveRight.Click += new System.EventHandler(this.btnMoveRight_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Location = new System.Drawing.Point(187, 35);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(75, 23);
            this.btnMoveDown.TabIndex = 9;
            this.btnMoveDown.Text = "Change";
            this.btnMoveDown.UseVisualStyleBackColor = true;
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Location = new System.Drawing.Point(187, 6);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(75, 23);
            this.btnMoveUp.TabIndex = 8;
            this.btnMoveUp.Text = "Change";
            this.btnMoveUp.UseVisualStyleBackColor = true;
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // lblLeft
            // 
            this.lblLeft.AutoSize = true;
            this.lblLeft.Location = new System.Drawing.Point(108, 98);
            this.lblLeft.Name = "lblLeft";
            this.lblLeft.Size = new System.Drawing.Size(14, 13);
            this.lblLeft.TabIndex = 7;
            this.lblLeft.Text = "A";
            // 
            // lblRight
            // 
            this.lblRight.AutoSize = true;
            this.lblRight.Location = new System.Drawing.Point(108, 69);
            this.lblRight.Name = "lblRight";
            this.lblRight.Size = new System.Drawing.Size(15, 13);
            this.lblRight.TabIndex = 6;
            this.lblRight.Text = "D";
            // 
            // lblDown
            // 
            this.lblDown.AutoSize = true;
            this.lblDown.Location = new System.Drawing.Point(109, 40);
            this.lblDown.Name = "lblDown";
            this.lblDown.Size = new System.Drawing.Size(14, 13);
            this.lblDown.TabIndex = 5;
            this.lblDown.Text = "S";
            // 
            // lblUp
            // 
            this.lblUp.AutoSize = true;
            this.lblUp.Location = new System.Drawing.Point(109, 11);
            this.lblUp.Name = "lblUp";
            this.lblUp.Size = new System.Drawing.Size(18, 13);
            this.lblUp.TabIndex = 4;
            this.lblUp.Text = "W";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Move left";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Move right";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Move down";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Move up";
            // 
            // tpGraphics
            // 
            this.tpGraphics.Controls.Add(this.label6);
            this.tpGraphics.Controls.Add(this.label5);
            this.tpGraphics.Controls.Add(this.cbbRatio);
            this.tpGraphics.Controls.Add(this.cbb4to3);
            this.tpGraphics.Controls.Add(this.cbb16to9);
            this.tpGraphics.Location = new System.Drawing.Point(4, 25);
            this.tpGraphics.Name = "tpGraphics";
            this.tpGraphics.Padding = new System.Windows.Forms.Padding(3);
            this.tpGraphics.Size = new System.Drawing.Size(302, 146);
            this.tpGraphics.TabIndex = 1;
            this.tpGraphics.Text = "Graphics";
            this.tpGraphics.UseVisualStyleBackColor = true;
            // 
            // cbb16to9
            // 
            this.cbb16to9.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbb16to9.FormattingEnabled = true;
            this.cbb16to9.Items.AddRange(new object[] {
            "854x480",
            "960x540",
            "1280x720",
            "1600x900",
            "1920x1080",
            "2560x1440",
            "3840x2160"});
            this.cbb16to9.Location = new System.Drawing.Point(74, 39);
            this.cbb16to9.Name = "cbb16to9";
            this.cbb16to9.Size = new System.Drawing.Size(121, 21);
            this.cbb16to9.TabIndex = 0;
            this.cbb16to9.SelectedIndexChanged += new System.EventHandler(this.cbb16to9_SelectedIndexChanged);
            // 
            // cbb4to3
            // 
            this.cbb4to3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbb4to3.FormattingEnabled = true;
            this.cbb4to3.Items.AddRange(new object[] {
            "640x480",
            "800x600",
            "960x720",
            "1280x960",
            "1440x1080",
            "1600x1200",
            "1920x1440"});
            this.cbb4to3.Location = new System.Drawing.Point(74, 39);
            this.cbb4to3.Name = "cbb4to3";
            this.cbb4to3.Size = new System.Drawing.Size(121, 21);
            this.cbb4to3.TabIndex = 1;
            this.cbb4to3.Visible = false;
            this.cbb4to3.SelectedIndexChanged += new System.EventHandler(this.cbb4to3_SelectedIndexChanged);
            // 
            // cbbRatio
            // 
            this.cbbRatio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbRatio.FormattingEnabled = true;
            this.cbbRatio.Items.AddRange(new object[] {
            "16:9",
            "4:3"});
            this.cbbRatio.Location = new System.Drawing.Point(74, 12);
            this.cbbRatio.Name = "cbbRatio";
            this.cbbRatio.Size = new System.Drawing.Size(121, 21);
            this.cbbRatio.TabIndex = 2;
            this.cbbRatio.SelectedIndexChanged += new System.EventHandler(this.cbbRatio_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Ratio";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 42);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Resolution";
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(28, 169);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(115, 23);
            this.btnApply.TabIndex = 1;
            this.btnApply.Text = "Apply changes";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 201);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.tabControl1);
            this.Name = "Settings";
            this.Text = "Settings";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Settings_KeyDown);
            this.tabControl1.ResumeLayout(false);
            this.tpKeybinds.ResumeLayout(false);
            this.tpKeybinds.PerformLayout();
            this.tpGraphics.ResumeLayout(false);
            this.tpGraphics.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpKeybinds;
        private System.Windows.Forms.Button btnMoveLeft;
        private System.Windows.Forms.Button btnMoveRight;
        private System.Windows.Forms.Button btnMoveDown;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.Label lblLeft;
        private System.Windows.Forms.Label lblRight;
        private System.Windows.Forms.Label lblDown;
        private System.Windows.Forms.Label lblUp;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tpGraphics;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbbRatio;
        private System.Windows.Forms.ComboBox cbb4to3;
        private System.Windows.Forms.ComboBox cbb16to9;
        private System.Windows.Forms.Button btnApply;
    }
}