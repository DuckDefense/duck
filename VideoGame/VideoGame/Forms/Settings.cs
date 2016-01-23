using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sandbox.Classes;
using Sandbox.Forms;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace VideoGame.Forms
{
    public partial class Settings : Form
    {
        public bool up;
        public bool down;
        public bool right;
        public bool left;
        public bool conversation;

        public Settings()
        {
            KeyPreview = true;
            InitializeComponent();
            double value = (double)Classes.Settings.ResolutionWidth / Classes.Settings.ResolutionHeight;
            if (value > 1.66) {
                cbbRatio.SelectedText = "16:9";
                cbb4to3.Visible = false;
                cbb16to9.Visible = true;
                cbb16to9.SelectedText = $"{Classes.Settings.ResolutionWidth}x{Classes.Settings.ResolutionHeight}";
            }
            else {
                cbbRatio.SelectedText = "4:3";
                cbb16to9.Visible = false;
                cbb4to3.Visible = true;
                cbb4to3.SelectedText = $"{Classes.Settings.ResolutionWidth}x{Classes.Settings.ResolutionHeight}";
            }
        }

        private void Settings_KeyDown(object sender, KeyEventArgs e)
        {
            if (up == true)
            { 
                if (Classes.Settings.moveRight != (Keys) e.KeyCode && Classes.Settings.moveDown != (Keys) e.KeyCode &&
                    Classes.Settings.moveLeft != (Keys) e.KeyCode && Classes.Settings.conversation != (Keys)e.KeyCode)
                {
                    Classes.Settings.moveUp = (Keys) e.KeyCode;
                    lblUp.Text = Convert.ToString(e.KeyCode);
                    up = false;
                }
                else
                {
                    MessageBox.Show("This button was used before");
                }
            }
            if (down == true)
            {
                if (Classes.Settings.moveRight != (Keys) e.KeyCode && Classes.Settings.moveUp != (Keys) e.KeyCode &&
                    Classes.Settings.moveLeft != (Keys) e.KeyCode && Classes.Settings.conversation != (Keys)e.KeyCode)
                {
                    Classes.Settings.moveDown = (Keys) e.KeyCode;
                    lblDown.Text = Convert.ToString(e.KeyCode);
                    down = false;
                }
                else
                {
                    MessageBox.Show("This button was used before");
                }
            }
            if (right == true)
            {
                if (Classes.Settings.moveUp != (Keys) e.KeyCode && Classes.Settings.moveDown != (Keys) e.KeyCode &&
                    Classes.Settings.moveLeft != (Keys) e.KeyCode && Classes.Settings.conversation != (Keys)e.KeyCode)
                {
                    Classes.Settings.moveRight = (Keys) e.KeyCode;
                    lblRight.Text = Convert.ToString(e.KeyCode);
                    right = false;
                }
                else
                {
                    MessageBox.Show("This button was used before");
                }
            }
            if (left == true)
            {
                if (Classes.Settings.moveRight != (Keys) e.KeyCode && Classes.Settings.moveDown != (Keys) e.KeyCode &&
                    Classes.Settings.moveUp != (Keys) e.KeyCode && Classes.Settings.conversation != (Keys)e.KeyCode)
                {
                    Classes.Settings.moveLeft = (Keys) e.KeyCode;
                    lblLeft.Text = Convert.ToString(e.KeyCode);
                    left = false;
                }
                else
                {
                    MessageBox.Show("This button was used before");
                }
            }
            if (conversation == true)
            {
                if (Classes.Settings.moveRight != (Keys)e.KeyCode && Classes.Settings.moveDown != (Keys)e.KeyCode &&
                    Classes.Settings.moveUp != (Keys)e.KeyCode && Classes.Settings.moveLeft != (Keys)e.KeyCode)
                {
                    Classes.Settings.conversation = (Keys)e.KeyCode;
                    lblConversation.Text = Convert.ToString(e.KeyCode);
                    conversation = false;
                }
                else
                {
                    MessageBox.Show("This button was used before");
                }
            }

        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            up = true;
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            down = true;
        }

        private void btnMoveRight_Click(object sender, EventArgs e)
        {
            right = true;
        }

        private void btnMoveLeft_Click(object sender, EventArgs e)
        {
            left = true;
        }

        private void cbbRatio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbRatio.Text == "16:9")
            {
                cbb16to9.Visible = true;
                cbb4to3.Visible = false;
            }else if (cbbRatio.Text == "4:3")
            {
                cbb16to9.Visible = false;
                cbb4to3.Visible = true;
            }
        }

        private void cbb16to9_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbb16to9.Text)
            {
                case "854x480":
                    Classes.Settings.ResolutionHeight = 480;
                    Classes.Settings.ResolutionWidth = 854;
                    break;
                case "960x540":
                    Classes.Settings.ResolutionHeight = 540;
                    Classes.Settings.ResolutionWidth = 960;
                    break;
                case "1280x720":
                    Classes.Settings.ResolutionHeight = 720;
                    Classes.Settings.ResolutionWidth = 1280;
                    break;
                case "1600x900":
                    Classes.Settings.ResolutionHeight = 900;
                    Classes.Settings.ResolutionWidth = 1600;
                    break;
                case "1920x1080":
                    Classes.Settings.ResolutionHeight = 1080;
                    Classes.Settings.ResolutionWidth = 1920;
                    break;
                case "2560x1440":
                    Classes.Settings.ResolutionHeight = 1440;
                    Classes.Settings.ResolutionWidth = 2560;
                    break;
                case "3840x2160":
                    Classes.Settings.ResolutionHeight = 2160;
                    Classes.Settings.ResolutionWidth = 3840;
                    break;
            }
        }

        private void cbb4to3_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbb4to3.Text)
            {
                case "640x480":
                    Classes.Settings.ResolutionHeight = 480;
                    Classes.Settings.ResolutionWidth = 640;
                    break;
                case "800x600":
                    Classes.Settings.ResolutionHeight = 600;
                    Classes.Settings.ResolutionWidth = 800;
                    break;
                case "960x720":
                    Classes.Settings.ResolutionHeight = 720;
                    Classes.Settings.ResolutionWidth = 960;
                    break;
                case "1280x960":
                    Classes.Settings.ResolutionHeight = 960;
                    Classes.Settings.ResolutionWidth = 1280;
                    break;
                case "1440x1080":
                    Classes.Settings.ResolutionHeight = 1080;
                    Classes.Settings.ResolutionWidth = 1440;
                    break;
                case "1600x1200":
                    Classes.Settings.ResolutionHeight = 1200;
                    Classes.Settings.ResolutionWidth = 1600;
                    break;
                case "1920x1440":
                    Classes.Settings.ResolutionHeight = 1440;
                    Classes.Settings.ResolutionWidth = 1920;
                    break;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            Classes.Settings.ServerName = tbServerName.Text;
            Classes.Settings.DatabaseName = tbDatabaseName.Text;
            Classes.Settings.Username = tbUsername.Text;
            Classes.Settings.Password = tbPassword.Text;
            DatabaseConnector.SaveSettings(Login.PlayerId);
            this.Close();
        }

        private void btnConversation_Click(object sender, EventArgs e)
        {
            conversation = true;
        }
    }
}
