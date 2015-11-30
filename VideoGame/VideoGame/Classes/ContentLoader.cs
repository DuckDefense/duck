using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Maps.Tiled;

namespace VideoGame.Classes {
    public class ContentLoader {
        public static ContentManager Content;
        public static Texture2D GronkeyFront, GronkeyBack, GronkeyParty;
        public static Texture2D ArmlerFront, ArmlerBack, ArmlerParty;
        public static Texture2D BrassFront, BrassBack, BrassParty;
        public static Texture2D HuffsteinFront, HuffsteinBack, HuffsteinParty;
        public static Texture2D GrassyBackground;
        public static Texture2D Christman;
        public static Texture2D Button;
        public static Texture2D AirHorn, AntiPoison, BucketOfWater, LeafBandage, MagicStone, RoosVicee, Salt, SpinalCord;
        public static TiledMap Map;
        public static SpriteFont Arial;

        public static void SetContent(ContentManager content) {
            Content = content;
        }

        //Add all Textures in here
        public void LoadContent() {
            #region Monsters
            GronkeyFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Gronkey");
            GronkeyBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/Gronkey");
            GronkeyParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/Gronkey");
            ArmlerFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Armler");
            ArmlerBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/Armler");
            ArmlerParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/Armler");
            BrassFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Brass");
            BrassBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/Brass");
            BrassParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/Brass");
            HuffsteinFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Huffstein");
            HuffsteinBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/Huffstein");
            HuffsteinParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/Huffstein");
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
            AirHorn = Content.Load<Texture2D>(@"Sprites/Items/Medicine/AirHorn");
            AntiPoison = Content.Load<Texture2D>(@"Sprites/Items/Medicine/AntiPoison");
            BucketOfWater = Content.Load<Texture2D>(@"Sprites/Items/Medicine/BucketOfWater");
            LeafBandage = Content.Load<Texture2D>(@"Sprites/Items/Medicine/LeafBandage");
            MagicStone = Content.Load<Texture2D>(@"Sprites/Items/Medicine/MagicStone");
            RoosVicee = Content.Load<Texture2D>(@"Sprites/Items/Medicine/RoosVicee");
            Salt = Content.Load<Texture2D>(@"Sprites/Items/Medicine/Salt");
            SpinalCord = Content.Load<Texture2D>(@"Sprites/Items/Medicine/Salt");


            #endregion
        }
    }
}

