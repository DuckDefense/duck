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
            if (isIngelogd == true) {
                btnPlay.Text = "Play";
            }
            else {
                btnPlay.Enabled = false;
                btnSettings.Enabled = false;
                //btnPlay.Text = "Play offline";
            }
        }

        private void btnLogin_Click(object sender, EventArgs e) {
            using (var login = new Login())
                login.ShowDialog();
            btnPlay.Enabled = isIngelogd;
            btnSettings.Enabled = isIngelogd;
            if (isIngelogd == true) {
                isIngelogd = false;
                //btnPlay.Text = "Play offline";
                //btnLogin.Text = "Login";
            }
            else {
                isIngelogd = true;
                btnPlay.Text = "Play";
                //btnLogin.Text = "Logout";
            }

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
    }
}
