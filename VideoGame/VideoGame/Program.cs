﻿using System;
using Sandbox.Forms;
using VideoGame.Forms;

namespace VideoGame {
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            //using (var game = new Game1())
            //    game.Run();
            Login l = new Login();
            l.ShowDialog();
        }
    }
#endif
}
