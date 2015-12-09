using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VideoGame.Forms
{
    public partial class Launcher : Form
    {
        public bool isIngelogd;
        public Launcher()
        {
            InitializeComponent();
            if (isIngelogd == true)
            {
                btnPlay.Text = "Play";
            }
            else
            {
                btnPlay.Text = "Play offline";
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (isIngelogd == true)
            {
                isIngelogd = false;
                btnPlay.Text = "Play offline";
                btnLogin.Text = "Login";
            }
            else
            {
                isIngelogd = true;
                btnPlay.Text = "Play";
                btnLogin.Text = "Logout";
            }
            
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (var game = new Game1())
                game.Run();
            
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            using (var setting = new Settings())
                setting.ShowDialog();
        }
    }
}
