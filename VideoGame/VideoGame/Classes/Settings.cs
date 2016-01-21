using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace VideoGame.Classes
{
    public static class Settings
    {
        public static int ResolutionHeight = 480;
        public static int ResolutionWidth = 800;
        public static Keys moveUp = Keys.W;
        public static Keys moveDown = Keys.S;
        public static Keys moveRight = Keys.D;
        public static Keys moveLeft = Keys.A;
        public static Keys conversation = Keys.Space;
        public static string DatabaseName = "ripoff";
        public static string ServerName = "localhost";
        public static string Password = "";
        public static string Username = "root";
    }
}
