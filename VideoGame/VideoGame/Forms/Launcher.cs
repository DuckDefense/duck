using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sandbox.Classes;
using Sandbox.Forms;

namespace VideoGame.Forms {
    public partial class Launcher : Form {
        public static bool isIngelogd;
        public Launcher() {
            InitializeComponent();
            if (isIngelogd) {
                btnPlay.Text = "Play";
                btnLogin.Visible = false;
            }
            else {
                btnPlay.Enabled = false;
                btnSettings.Enabled = false;
                //btnPlay.Text = "Play offline";
            }
            //ShowLogin();
        }

        private void btnLogin_Click(object sender, EventArgs e) {
            if (isIngelogd) {
                isIngelogd = false;
                btnPlay.Enabled = false;
                btnSettings.Enabled = false;
            }
            else ShowLogin();
        }

        private void btnPlay_Click(object sender, EventArgs e) {
            this.Hide();
            DatabaseConnector.GetSettings(Login.PlayerId);
            using (var game = new Game1())
                game.Run();

        }

        private void btnSettings_Click(object sender, EventArgs e) {
            using (var setting = new Settings())
                setting.ShowDialog();
        }

        private void ShowLogin() {
            using (var login = new Login())
                login.ShowDialog();
            btnPlay.Enabled = isIngelogd;
            btnSettings.Enabled = isIngelogd;
            if (isIngelogd) {
                isIngelogd = false;
            }
            else {
                isIngelogd = true;
                btnPlay.Text = "Play";
                btnLogin.Text = "Logout";
            }
        }
    }
}
