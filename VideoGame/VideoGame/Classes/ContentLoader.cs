using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Maps.Tiled;

namespace VideoGame.Classes {
    public class ContentLoader {
        public static ContentManager Content;
        public static GraphicsDevice GraphicsDevice;
        public static Texture2D Health;

        #region Monsters
        public static Texture2D MonsterViewer;
        public static Texture2D GronkeyFront, GronkeyBack, GronkeyParty;
        public static Texture2D ArmlerFront, ArmlerBack, ArmlerParty;
        public static Texture2D BrassFront, BrassBack, BrassParty;
        public static Texture2D HuffsteinFront, HuffsteinBack, HuffsteinParty;
        public static Texture2D FesterFront;
        #endregion
        
        public static Texture2D NormalType, FightType, FireType, WaterType, GrassType, PoisonType, PsychType, GhostType, IceType, RockType, FlyingType, SoundType;

        public static Texture2D GrassyBackground;
        public static Texture2D Christman;
        public static Texture2D Button;
        public static Texture2D AirHorn, AntiPoison, BucketOfWater, LeafBandage, MagicStone, RoosVicee, Salt;
        public static Texture2D RottenNet, RegularNet, GreatNet;
        public static TiledMap Map, City;
        public static SpriteFont Arial;

        public static void SetContent(ContentManager content, GraphicsDeviceManager graphicsDevice) {
            GraphicsDevice = graphicsDevice.GraphicsDevice;
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
            FesterFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Fester");
            MonsterViewer = Content.Load<Texture2D>(@"Sprites/Monsters/Viewer");
            #endregion

            #region Characters
            Christman = Content.Load<Texture2D>(@"Sprites/Characters/World/Christman");
            #endregion

            #region Battle
            GrassyBackground = Content.Load<Texture2D>(@"Sprites/Battle/Backgrounds/Grassy");
            Button = Content.Load<Texture2D>(@"Sprites/Buttons/Button");
            Health = new Texture2D(GraphicsDevice, 5, 5, false, SurfaceFormat.Color);
            #endregion

            #region Types
            NormalType = Content.Load<Texture2D>(@"Sprites/Type/Normal");
            FightType = Content.Load<Texture2D>(@"Sprites/Type/Fight");
            FireType = Content.Load<Texture2D>(@"Sprites/Type/Fire");
            WaterType = Content.Load<Texture2D>(@"Sprites/Type/Water");
            GrassType = Content.Load<Texture2D>(@"Sprites/Type/Grass");
            PoisonType = Content.Load<Texture2D>(@"Sprites/Type/Poison");
            PsychType = Content.Load<Texture2D>(@"Sprites/Type/Psych");
            GhostType = Content.Load<Texture2D>(@"Sprites/Type/Ghost");
            IceType = Content.Load<Texture2D>(@"Sprites/Type/Ice");
            RockType = Content.Load<Texture2D>(@"Sprites/Type/Rock");
            FlyingType = Content.Load<Texture2D>(@"Sprites/Type/Flying");
            SoundType = Content.Load<Texture2D>(@"Sprites/Type/Sound");
            #endregion

            #region Maps
            Map = Content.Load<TiledMap>("map");
            City = Content.Load<TiledMap>("city");
            #endregion

            #region Fonts

            Arial = Content.Load<SpriteFont>(@"Fonts/Arial");

            #endregion

            #region Items

            #region Capture

            RottenNet = Content.Load<Texture2D>(@"Sprites\Items\Capture\Rotten Net");
            RegularNet = Content.Load<Texture2D>(@"Sprites\Items\Capture\Regular Net");
            GreatNet = Content.Load<Texture2D>(@"Sprites\Items\Capture\Great Net");
            #endregion

            #region Medicine
            AirHorn = Content.Load<Texture2D>(@"Sprites/Items/Medicine/AirHorn");
            AntiPoison = Content.Load<Texture2D>(@"Sprites/Items/Medicine/AntiPoison");
            BucketOfWater = Content.Load<Texture2D>(@"Sprites/Items/Medicine/BucketOfWater");
            LeafBandage = Content.Load<Texture2D>(@"Sprites/Items/Medicine/LeafBandage");
            MagicStone = Content.Load<Texture2D>(@"Sprites/Items/Medicine/MagicStone");
            RoosVicee = Content.Load<Texture2D>(@"Sprites/Items/Medicine/RoosVicee");
            Salt = Content.Load<Texture2D>(@"Sprites/Items/Medicine/Salt");
            #endregion


            #endregion
        }

        public void UnloadContent() {
            GronkeyFront.Dispose();  
            GronkeyBack.Dispose();  
            GronkeyParty.Dispose();  
            ArmlerFront.Dispose();
            ArmlerBack.Dispose(); 
            ArmlerParty.Dispose(); 
            BrassFront.Dispose();
            BrassBack.Dispose(); 
            BrassParty.Dispose();
            HuffsteinFront.Dispose();  
            HuffsteinBack.Dispose();  
            HuffsteinParty.Dispose(); 
            MonsterViewer.Dispose();
            
            Christman.Dispose();
            
            GrassyBackground.Dispose();
            Button.Dispose();  
            Health.Dispose();  
            
            Map.Dispose(); 

            RottenNet.Dispose(); 
            RegularNet.Dispose();  
            GreatNet.Dispose();  
      
            AirHorn.Dispose(); 
            AntiPoison.Dispose(); 
            BucketOfWater.Dispose();  
            LeafBandage.Dispose();  
            MagicStone.Dispose(); 
            RoosVicee.Dispose();
        }
    }
}

