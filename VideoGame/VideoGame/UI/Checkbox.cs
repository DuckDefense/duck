using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameUI
{
    class Checkbox
    {
        //Image
        public Texture2D UncheckedTexture { get; set; }
        public Texture2D CheckedTexture { get; set; }
        private Texture2D HoverTexture { get; set; }
        private Texture2D ClickTexture { get; set; }
        //Misc
        public Rectangle Position { get; set; }
        public Vector2 VectorPosition { get; set; }
        public Vector2 Origin { get; set; }

        //Drawing
        private bool DrawSourceImage = false;
        private bool DrawHoverTexture = false;
        private bool DrawClickTexture = false;

        public bool Visible = true;
        public bool Enabled = true;
        public Vector2 Scale = new Vector2(1, 1);
        private Vector2 DefaultScale = new Vector2(1, 1);
        public float Rotation = 0.00f;
        public float DefaultRotation = 0.00f;
        #region Constructors

        public Checkbox(Texture2D uncheckedTexture, Texture2D checkedTexture, Texture2D hoverTexture, Texture2D clickTexture,
            Rectangle position, Vector2 origin)
        {
            UncheckedTexture = uncheckedTexture;
            CheckedTexture = checkedTexture;
            HoverTexture = hoverTexture;
            ClickTexture = clickTexture;
            Position = position;
            Origin = origin;
            VectorPosition = new Vector2(position.X, position.Y);
        }
        #endregion
    }
}
