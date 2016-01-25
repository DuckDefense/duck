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

namespace VideoGame.Forms {
    public partial class Settings : Form {
        public bool up;
        public bool down;
        public bool right;
        public bool left;
        public bool conversation;

        public Settings() {
            KeyPreview = true;
            InitializeComponent();
        }

        private void Settings_KeyDown(object sender, KeyEventArgs e) {
            if (up == true) {
                if (Classes.Settings.moveRight != (Keys)e.KeyCode && Classes.Settings.moveDown != (Keys)e.KeyCode &&
                    Classes.Settings.moveLeft != (Keys)e.KeyCode && Classes.Settings.conversation != (Keys)e.KeyCode) {
                    Classes.Settings.moveUp = (Keys)e.KeyCode;
                    lblUp.Text = Convert.ToString(e.KeyCode);
                    up = false;
                }
                else {
                    MessageBox.Show("This button was used before");
                }
            }
            if (down == true) {
                if (Classes.Settings.moveRight != (Keys)e.KeyCode && Classes.Settings.moveUp != (Keys)e.KeyCode &&
                    Classes.Settings.moveLeft != (Keys)e.KeyCode && Classes.Settings.conversation != (Keys)e.KeyCode) {
                    Classes.Settings.moveDown = (Keys)e.KeyCode;
                    lblDown.Text = Convert.ToString(e.KeyCode);
                    down = false;
                }
                else {
                    MessageBox.Show("This button was used before");
                }
            }
            if (right == true) {
                if (Classes.Settings.moveUp != (Keys)e.KeyCode && Classes.Settings.moveDown != (Keys)e.KeyCode &&
                    Classes.Settings.moveLeft != (Keys)e.KeyCode && Classes.Settings.conversation != (Keys)e.KeyCode) {
                    Classes.Settings.moveRight = (Keys)e.KeyCode;
                    lblRight.Text = Convert.ToString(e.KeyCode);
                    right = false;
                }
                else {
                    MessageBox.Show("This button was used before");
                }
            }
            if (left == true) {
                if (Classes.Settings.moveRight != (Keys)e.KeyCode && Classes.Settings.moveDown != (Keys)e.KeyCode &&
                    Classes.Settings.moveUp != (Keys)e.KeyCode && Classes.Settings.conversation != (Keys)e.KeyCode) {
                    Classes.Settings.moveLeft = (Keys)e.KeyCode;
                    lblLeft.Text = Convert.ToString(e.KeyCode);
                    left = false;
                }
                else {
                    MessageBox.Show("This button was used before");
                }
            }
            if (conversation == true) {
                if (Classes.Settings.moveRight != (Keys)e.KeyCode && Classes.Settings.moveDown != (Keys)e.KeyCode &&
                    Classes.Settings.moveUp != (Keys)e.KeyCode && Classes.Settings.moveLeft != (Keys)e.KeyCode) {
                    Classes.Settings.conversation = (Keys)e.KeyCode;
                    lblConversation.Text = Convert.ToString(e.KeyCode);
                    conversation = false;
                }
                else {
                    MessageBox.Show("This button was used before");
                }
            }

        }

        private void btnMoveUp_Click(object sender, EventArgs e) {
            up = true;
        }

        private void btnMoveDown_Click(object sender, EventArgs e) {
            down = true;
        }

        private void btnMoveRight_Click(object sender, EventArgs e) {
            right = true;
        }

        private void btnMoveLeft_Click(object sender, EventArgs e) {
            left = true;
        }

        private void btnApply_Click(object sender, EventArgs e) {
            Classes.Settings.ServerName = tbServerName.Text;
            Classes.Settings.DatabaseName = tbDatabaseName.Text;
            Classes.Settings.Username = tbUsername.Text;
            Classes.Settings.Password = tbPassword.Text;
            DatabaseConnector.SaveSettings(Login.PlayerId);
            this.Close();
        }

        private void btnConversation_Click(object sender, EventArgs e) {
            conversation = true;
        }
    }
}
