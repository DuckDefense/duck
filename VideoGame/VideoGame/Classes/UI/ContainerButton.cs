﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using Microsoft.Win32.SafeHandles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VideoGame.Classes;

namespace Sandbox.Classes.UI {
    public class ContainerButton {
        private Vector2 middlePos;
        public Button Button;
        public Move Move;
        public Monster Monster;
        public Medicine Medicine;
        public Capture Capture;
        public BoxedMonster BoxedMonster;

        public ContainerButton(Button button, Move move) {
            Button = button;
            Move = move;
            Monster = null;
            Medicine = null;
            Capture = null;
            BoxedMonster = null;
        }

        public ContainerButton(Button button, Monster monster) {
            Button = button;
            Move = null;
            Monster = monster;
            Medicine = null;
            Capture = null;
            BoxedMonster = null;
        }
        public ContainerButton(Button button, Medicine medicine) {
            Button = button;
            Move = null;
            Monster = null;
            Medicine = medicine;
            Capture = null;
            BoxedMonster = null;
        }
        public ContainerButton(Button button, Capture capture) {
            Button = button;
            Move = null;
            Monster = null;
            Medicine = null;
            Capture = capture;
            BoxedMonster = null;
        }

        public ContainerButton(Button button, BoxedMonster boxedMonster) {
            Button = button;
            Move = null;
            Monster = null;
            Medicine = null;
            Capture = null;
            BoxedMonster = boxedMonster;
        }

        public void Update(MouseState cur, MouseState prev) {
            middlePos = new Vector2(Button.Position.X + (Button.SourceTexture.Width  / 2), Button.Position.Y + (Button.SourceTexture.Height / 2));
            Button.Update(cur, prev);
        }

        /// <summary>
        /// Draw button with the texture added as parameter
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="texture">Texture that should be drawn. ex. ContainerButton.Monster.PartySprite</param>
        public void Draw(SpriteBatch batch, Texture2D texture) {
            if (texture != null) {
                Button.Draw(batch);
                batch.Draw(texture, middlePos);
            }
        }

        /// <summary>
        /// Draw button with the added parameter string
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="display">String that should be drawn. ex. ContainerButton.Monster.Name</param>
        public void Draw(SpriteBatch batch, string display) {
            Button.Text = display;
            Button.Draw(batch);
        }
    }
}
