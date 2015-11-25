﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Maps.Tiled;

namespace VideoGame.Classes
{
    public class ContentLoader
    {
        public static ContentManager Content;
        public static Texture2D GronkeyFront, GronkeyBack, GronkeyParty;
        public static Texture2D GrassyBackground;
        public static Texture2D Christman;
        public static Texture2D Button;
        public static Texture2D LeafBandage, MagicStone, AntiPoison, BucketOfWater, Salt, AirHorn, SpinalCord, RoosVicee;
        public static TiledMap Map;
        public static SpriteFont Arial;

        public static void SetContent(ContentManager content)
        {
            Content = content;
        }

        //Add all Textures in here
        public void LoadContent()
        {
            #region Monsters
            GronkeyFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Gronkey");
            GronkeyBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/Gronkey");
            GronkeyParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/Gronkey");
            #endregion

            #region Characters
            Christman = Content.Load<Texture2D>(@"Sprites/Characters/World/Christman");
            #endregion

            #region battle
            GrassyBackground = Content.Load<Texture2D>(@"Sprites/Battle/Backgrounds/Grassy");
            Button = Content.Load<Texture2D>(@"Sprites/Buttons/Button");
            #endregion

            #region Maps
            Map = Content.Load<TiledMap>("map");
            #endregion

            #region Fonts

            Arial = Content.Load<SpriteFont>(@"Fonts/Arial");

            #endregion

            #region Items
            LeafBandage = Content.Load<Texture2D>(@"Sprites/Items/Medicine/LeafBandage");
            MagicStone = Content.Load<Texture2D>(@"Sprites/Items/Medicine/MagicStone");
            AntiPoison = Content.Load<Texture2D>(@"Sprites/Items/Medicine/AntiPoison");
            BucketOfWater = Content.Load<Texture2D>(@"Sprites/Items/Medicine/BucketOfWater");
            Salt = Content.Load<Texture2D>(@"Sprites/Items/Medicine/Salt");
            AirHorn = Content.Load<Texture2D>(@"Sprites/Items/Medicine/AirHorn");
            RoosVicee = Content.Load<Texture2D>(@"Sprites/Items/Medicine/RoosVicee");
            

            #endregion
        }
    }
}

