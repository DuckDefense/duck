using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Maps.Tiled;

namespace VideoGame.Classes {
    public class ContentLoader {
        public static ContentManager Content;
        public static GraphicsDevice GraphicsDevice;
        public static Texture2D Health, HealthBorder, HealthBorderUser;

        #region Monsters
        public static Texture2D MonsterViewer;
        public static Texture2D GronkeyFront, GronkeyBack, GronkeyParty;
        public static Texture2D ArmlerFront, ArmlerBack, ArmlerParty;
        public static Texture2D BrassFront, BrassBack, BrassParty;
        public static Texture2D HuffsteinFront, HuffsteinBack, HuffsteinParty;
        public static Texture2D FesterFront, FesterBack, FesterParty;
        public static Texture2D BonsantaiFront, BonsantaiBack, BonsantaiParty;
        public static Texture2D DrumbyFront, DrumbyBack, DrumbyParty;
        public static Texture2D DvallalinFront, DvallalinBack, DvallalinParty;
        public static Texture2D GladkeyFront, GladkeyBack, GladkeyParty;
        public static Texture2D HippolaciousFront, HippolaciousBack, HippolaciousParty;
        public static Texture2D JoiantlerFront, JoiantlerBack, JoiantlerParty;
        public static Texture2D KolfieFront, KolfieBack, KolfieParty;
        public static Texture2D KrabbleFront, KrabbleBack, KrabbleParty;
        public static Texture2D MimirdFront, MimirdBack, MimirdParty;
        public static Texture2D NjortorFront, NjortorBack, NjortorParty;
        public static Texture2D PantslerFront, PantslerBack, PantslerParty;
        public static Texture2D PrestlerFront, PrestlerBack, PrestlerParty;
        public static Texture2D RasionFront, RasionBack, RasionParty;
        #endregion

        public static Texture2D NormalType, FightType, FireType, WaterType, GrassType, PoisonType, PsychType, GhostType, IceType, RockType, FlyingType, SoundType;
        public static Texture2D SLP, PSN, BRN, DZL, FRZ, FZD, FNT;

        public static Texture2D Grid;

        public static Texture2D GrassyBackground;
        public static Texture2D Christman;
        public static Texture2D MCGirl;
        public static Texture2D Button, ButtonHover, ButtonClicked;
        public static Texture2D AirHorn, AntiPoison, BucketOfWater, LeafBandage, MagicStone, RoosVicee, Salt;
        public static Texture2D RottenNet, RegularNet, GreatNet;
        public static TiledMap Map, City, Route2, Shop;
        public static SpriteFont Arial;

        public static SoundEffect TownSong;
        public static SoundEffect RouteSong;

        public static void SetContent(ContentManager content, GraphicsDeviceManager graphicsDevice) {
            GraphicsDevice = graphicsDevice.GraphicsDevice;
            Content = content;
        }

        //Add all Textures in here
        public void LoadContent() {
            Grid = Content.Load<Texture2D>(@"Sprites/Debug/32grid");

            #region Button
            Button = Content.Load<Texture2D>(@"Sprites/Buttons/Button");
            ButtonHover = Content.Load<Texture2D>(@"Sprites/Buttons/ButtonHover");
            ButtonClicked = Content.Load<Texture2D>(@"Sprites/Buttons/ButtonClicked");
            #endregion

            #region Monsters
            MonsterViewer = Content.Load<Texture2D>(@"Sprites/Monsters/Viewer");

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
            FesterBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/Fester");
            FesterParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/Fester");

            BonsantaiFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Bonsantai");
            BonsantaiBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/Bonsantai");
            BonsantaiParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/Bonsantai");

            DrumbyFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Drumby");
            //DrumbyBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/");
            //DrumbyParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/");

            DvallalinFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Dvallalin");
            DvallalinBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/Dvallalin");
            DvallalinParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/Dvallalin");

            GladkeyFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Gladkey");
            GladkeyBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/Gladkey");
            GladkeyParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/Gladkey");

            HippolaciousFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Hippolacious");
            //HippolaciousBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/");
            //HippolaciousParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/");

            JoiantlerFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Joiantler");
            JoiantlerBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/Joiantler");
            JoiantlerParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/Joiantler");

            KolfieFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Kolfie");
            //KolfieBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/");
            //KolfieParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/");

            KrabbleFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Krabble");
            //KrabbleBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/");
            //KrabbleParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/");

            MimirdFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Mimird");
            MimirdBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/Mimird");
            MimirdParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/Mimird");

            NjortorFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Njortor");
            NjortorBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/Njortor");
            NjortorParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/Njortor");

            PantslerFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Pantsler");
            PantslerBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/Pantsler");
            PantslerParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/Pantsler");

            PrestlerFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Prestler");
            PrestlerBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/Prestler");
            PrestlerParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/Prestler");

            RasionFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Rasion");
            //RasionBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/");
            //RasionParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/");

            #endregion

            #region Characters
            Christman = Content.Load<Texture2D>(@"Sprites/Characters/World/Christman");
            MCGirl = Content.Load<Texture2D>(@"Sprites/Characters/World/MC Girl");
            #endregion

            #region Battle
            GrassyBackground = Content.Load<Texture2D>(@"Sprites/Battle/Backgrounds/Grassy");
            Health = Content.Load<Texture2D>(@"Sprites/Battle/Health");
            HealthBorder = Content.Load<Texture2D>(@"Sprites/Battle/HealthBorder");
            HealthBorderUser = Content.Load<Texture2D>(@"Sprites/Battle/HealthBorderUser");
            #endregion

            #region Types
            NormalType = Content.Load<Texture2D>(@"Sprites/Types/Normal");
            FightType = Content.Load<Texture2D>(@"Sprites/Types/Fight");
            FireType = Content.Load<Texture2D>(@"Sprites/Types/Fire");
            WaterType = Content.Load<Texture2D>(@"Sprites/Types/Water");
            GrassType = Content.Load<Texture2D>(@"Sprites/Types/Grass");
            PoisonType = Content.Load<Texture2D>(@"Sprites/Types/Poison");
            PsychType = Content.Load<Texture2D>(@"Sprites/Types/Psych");
            GhostType = Content.Load<Texture2D>(@"Sprites/Types/Ghost");
            IceType = Content.Load<Texture2D>(@"Sprites/Types/Ice");
            RockType = Content.Load<Texture2D>(@"Sprites/Types/Rock");
            FlyingType = Content.Load<Texture2D>(@"Sprites/Types/Flying");
            SoundType = Content.Load<Texture2D>(@"Sprites/Types/Sound");
            #endregion

            #region Ailments

            SLP = Content.Load<Texture2D>(@"Sprites/Ailments/SLP");
            PSN = Content.Load<Texture2D>(@"Sprites/Ailments/PSN");
            BRN = Content.Load<Texture2D>(@"Sprites/Ailments/BRN");
            DZL = Content.Load<Texture2D>(@"Sprites/Ailments/DZL");
            FRZ = Content.Load<Texture2D>(@"Sprites/Ailments/FRZ");
            FZD = Content.Load<Texture2D>(@"Sprites/Ailments/FZD");
            FNT = Content.Load<Texture2D>(@"Sprites/Ailments/FNT");
            #endregion

            #region Maps
            Map = Content.Load<TiledMap>("map");
            City = Content.Load<TiledMap>("city");
            Route2 = Content.Load<TiledMap>("nicemap");
            Shop = Content.Load<TiledMap>("shop");

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

            #region Music

            TownSong = Content.Load<SoundEffect>(@"Music/rustige stad");
            RouteSong = Content.Load<SoundEffect>(@"Music/seasong");

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

