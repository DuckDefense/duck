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
    public enum TextureFace {
        Front,
        Back,
        World
    }

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
        public static Texture2D KirkFront, KirkBack, KirkParty;
        public static Texture2D BaggleFront, BaggleBack, BaggleParty;
        public static Texture2D GuilailFront, GuilailBack, GuilailParty;
        public static Texture2D GulagaFront, GulagaBack, GulagaParty;
        public static Texture2D DagaFront, DagaBack, DagaParty;

        #endregion

        public static Texture2D NormalType, FightType, FireType, WaterType, GrassType, PoisonType, PsychType, GhostType, IceType, RockType, FlyingType, SoundType;
        public static Texture2D PhysicalAttack, SpecialAttack, NonDamageAttack;
        public static Texture2D SLP, PSN, BRN, DZL, FRZ, FZD, FNT;

        public static Texture2D Grid;
        public static Texture2D Move;

        public static Texture2D GrassyBackground;
        public static Texture2D Koak;
        public static Texture2D HealLady;
        public static Texture2D ChristmanWorld;
        public static Texture2D MCGirlWorld;
        public static Texture2D MCBoyWorld;
        public static Texture2D MessageBox, BattleMessage;
        public static Texture2D Button, ButtonHover, ButtonClicked;
        public static Texture2D AirHorn, AntiPoison, BucketOfWater, LeafBandage, MagicStone, RoosVicee, Salt, NastySpoon;
        public static Texture2D RottenNet, RegularNet, GreatNet, MasterNet;
        public static TiledMap City, Route1, Route2, Route3, Route4, Route5, Route6, Route7, Route8, Route9, Shop, SecretTunnel, SecretCity, EasterCity;
        public static SpriteFont Arial;

        public static SoundEffect TownSong;
        public static SoundEffect RouteSong;

        public static void SetContent(ContentManager content, GraphicsDeviceManager graphicsDevice) {
            GraphicsDevice = graphicsDevice.GraphicsDevice;
            Content = content;
        }

        //Add all Textures in here
        public void LoadContent() {
            Move = Content.Load<Texture2D>(@"Sprites/Move");
            Grid = Content.Load<Texture2D>(@"Sprites/Debug/32grid");
            MessageBox = Content.Load<Texture2D>(@"Sprites/Conversation/MessageBox");
            BattleMessage = Content.Load<Texture2D>(@"Sprites/Conversation/BattleMessage");

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
            DrumbyBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/Drumby");
            DrumbyParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/Drumby");

            DvallalinFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Dvallalin");
            DvallalinBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/Dvallalin");
            DvallalinParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/Dvallalin");

            GladkeyFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Gladkey");
            GladkeyBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/Gladkey");
            GladkeyParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/Gladkey");

            HippolaciousFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Hippolacious");
            HippolaciousBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/Hippolacious");
            HippolaciousParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/Hippolacious");

            JoiantlerFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Joiantler");
            JoiantlerBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/Joiantler");
            JoiantlerParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/Joiantler");

            KolfieFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Kolfie");
            KolfieBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/Kolfie");
            KolfieParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/Kolfie");

            KrabbleFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Krabble");
            KrabbleBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/Krabble");
            KrabbleParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/Krabble");

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
            RasionBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/Rasion");
            RasionParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/Rasion");

            KirkFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Kirk");
            KirkBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/Kirk");
            KirkParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/Kirk");

            BaggleFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Baggle");
            BaggleBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/Baggle");
            BaggleParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/Baggle");

            GuilailFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Guilail");
            GuilailBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/Guilail");
            GuilailParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/Guilail");

            GulagaFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Gulaga");
            GulagaBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/Gulaga");
            GulagaParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/Gulaga");

            DagaFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Daga");
            DagaBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/Daga");
            DagaParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/Daga");

            #endregion

            #region Characters
            Koak = Content.Load<Texture2D>(@"Sprites/Characters/Front/Professor Koak");
            ChristmanWorld = Content.Load<Texture2D>(@"Sprites/Characters/World/Christman");
            MCGirlWorld = Content.Load<Texture2D>(@"Sprites/Characters/World/MC Girl");
            MCBoyWorld = Content.Load<Texture2D>(@"Sprites/Characters/World/MC Boy");
            HealLady = Content.Load<Texture2D>(@"Sprites/Characters/World/Heal mevrouw");

            #endregion

            #region Battle
            GrassyBackground = Content.Load<Texture2D>(@"Sprites/Battle/Backgrounds/Grassy");
            Health = Content.Load<Texture2D>(@"Sprites/Battle/Health");
            PhysicalAttack = Content.Load<Texture2D>(@"Sprites/Battle/Physical");
            SpecialAttack = Content.Load<Texture2D>(@"Sprites/Battle/Special");
            NonDamageAttack = Content.Load<Texture2D>(@"Sprites/Battle/NonDamage");
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
            City = Content.Load<TiledMap>("city");
            Route1 = Content.Load<TiledMap>("nicemap");
            Route2 = Content.Load<TiledMap>("Route2");
            Route3 = Content.Load<TiledMap>("Route3");
            Route4 = Content.Load<TiledMap>("Route4");
            Route5 = Content.Load<TiledMap>("Route5");
            Route6 = Content.Load<TiledMap>("Route6");
            Route7 = Content.Load<TiledMap>("Route7");
            Route8 = Content.Load<TiledMap>("Route8");
            Route9 = Content.Load<TiledMap>("Route9");
            Shop = Content.Load<TiledMap>("shop");
            SecretTunnel = Content.Load<TiledMap>("SecretTunnel");
            SecretCity = Content.Load<TiledMap>("SecretCity");
            EasterCity = Content.Load<TiledMap>("cityeasteregg");


            #endregion

            #region Fonts

            Arial = Content.Load<SpriteFont>(@"Fonts/Arial");

            #endregion

            #region Items

            #region Capture

            RottenNet = Content.Load<Texture2D>(@"Sprites\Items\Capture\Rotten Net");
            RegularNet = Content.Load<Texture2D>(@"Sprites\Items\Capture\Regular Net");
            GreatNet = Content.Load<Texture2D>(@"Sprites\Items\Capture\Great Net");
            MasterNet = Content.Load<Texture2D>(@"Sprites\Items\Capture\Master Net");
            #endregion

            #region Medicine
            AirHorn = Content.Load<Texture2D>(@"Sprites/Items/Medicine/AirHorn");
            AntiPoison = Content.Load<Texture2D>(@"Sprites/Items/Medicine/AntiPoison");
            BucketOfWater = Content.Load<Texture2D>(@"Sprites/Items/Medicine/BucketOfWater");
            LeafBandage = Content.Load<Texture2D>(@"Sprites/Items/Medicine/LeafBandage");
            MagicStone = Content.Load<Texture2D>(@"Sprites/Items/Medicine/MagicStone");
            RoosVicee = Content.Load<Texture2D>(@"Sprites/Items/Medicine/RoosVicee");
            Salt = Content.Load<Texture2D>(@"Sprites/Items/Medicine/Salt");
            NastySpoon = Content.Load<Texture2D>(@"Sprites/Items/Medicine/Nasty Spoon");
            #endregion

            #region Music

            TownSong = Content.Load<SoundEffect>(@"Music/rustige stad");
            RouteSong = Content.Load<SoundEffect>(@"Music/seasong");

            #endregion

            #endregion
        }

        public void UnloadContent() {
            //TODO: Add all textures here
        }

        #region TextureReturns

        public static Texture2D GetTextureFromMonsterId(int id, TextureFace face) {
            switch (id) {
            case 1:
                switch (face) {
                case TextureFace.Front: return ArmlerFront;
                case TextureFace.Back: return ArmlerBack;
                case TextureFace.World: return ArmlerParty;
                }
                break;
            case 2:
                switch (face) {
                case TextureFace.Front: return PantslerFront;
                case TextureFace.Back: return PantslerBack;
                case TextureFace.World: return PantslerParty;
                }
                break;
            case 3:
                switch (face) {
                case TextureFace.Front: return PrestlerFront;
                case TextureFace.Back: return PrestlerBack;
                case TextureFace.World: return PrestlerParty;
                }
                break;
            case 4:
                switch (face) {
                case TextureFace.Front: return MimirdFront;
                case TextureFace.Back: return MimirdBack;
                case TextureFace.World: return MimirdParty;
                }
                break;
            case 5:
                switch (face) {
                case TextureFace.Front: return NjortorFront;
                case TextureFace.Back: return NjortorBack;
                case TextureFace.World: return NjortorParty;
                }
                break;
            case 6:
                switch (face) {
                case TextureFace.Front: return DvallalinFront;
                case TextureFace.Back: return DvallalinBack;
                case TextureFace.World: return DvallalinParty;
                }
                break;
            case 7:
                switch (face) {
                case TextureFace.Front: return GuilailFront;
                case TextureFace.Back: return GuilailBack;
                case TextureFace.World: return GuilailParty;
                }
                break;
            case 8:
                switch (face) {
                case TextureFace.Front: return GulagaFront;
                case TextureFace.Back: return GulagaBack;
                case TextureFace.World: return GulagaParty;
                }
                break;
            case 9:
                switch (face) {
                case TextureFace.Front: return DagaFront;
                case TextureFace.Back: return DagaBack;
                case TextureFace.World: return DagaParty;
                }
                break;
            case 10:
                switch (face) {
                case TextureFace.Front: return GronkeyFront;
                case TextureFace.Back: return GronkeyBack;
                case TextureFace.World: return GronkeyParty;
                }
                break;
            case 11:
                switch (face) {
                case TextureFace.Front: return GladkeyFront;
                case TextureFace.Back: return GladkeyBack;
                case TextureFace.World: return GladkeyParty;
                }
                break;
            case 12:
                switch (face) {
                case TextureFace.Front: return BrassFront;
                case TextureFace.Back: return BrassBack;
                case TextureFace.World: return BrassParty;
                }
                break;
            case 13:
                switch (face) {
                case TextureFace.Front: return BonsantaiFront;
                case TextureFace.Back: return BonsantaiBack;
                case TextureFace.World: return BonsantaiParty;
                }
                break;
            case 14:
                switch (face) {
                case TextureFace.Front: return HuffsteinFront;
                case TextureFace.Back: return HuffsteinBack;
                case TextureFace.World: return HuffsteinParty;
                }
                break;
            case 15:
                switch (face) {
                case TextureFace.Front: return FesterFront;
                case TextureFace.Back: return FesterBack;
                case TextureFace.World: return FesterParty;
                }
                break;
            case 16:
                switch (face) {
                case TextureFace.Front: return JoiantlerFront;
                case TextureFace.Back: return JoiantlerBack;
                case TextureFace.World: return JoiantlerParty;
                }
                break;
            case 17:
                switch (face) {
                case TextureFace.Front: return RasionFront;
                case TextureFace.Back: return RasionBack;
                case TextureFace.World: return RasionParty;
                }
                break;
            case 18:
                switch (face) {
                case TextureFace.Front: return HippolaciousFront;
                case TextureFace.Back: return HippolaciousBack;
                case TextureFace.World: return HippolaciousParty;
                }
                break;
            case 19:
                switch (face) {
                case TextureFace.Front: return BaggleFront;
                case TextureFace.Back: return BaggleBack;
                case TextureFace.World: return BaggleParty;
                }
                break;
            case 20:
                switch (face) {
                case TextureFace.Front: return KrabbleFront;
                case TextureFace.Back: return KrabbleBack;
                case TextureFace.World: return KrabbleParty;
                }
                break;
            case 21:
                switch (face) {
                case TextureFace.Front: return DrumbyFront;
                case TextureFace.Back: return DrumbyBack;
                case TextureFace.World: return DrumbyParty;
                }
                break;
            case 22:
                switch (face) {
                case TextureFace.Front: return KolfieFront;
                case TextureFace.Back: return KolfieBack;
                case TextureFace.World: return KolfieParty;
                }
                break;
            case 23:
                switch (face) {
                case TextureFace.Front: return KirkFront;
                case TextureFace.Back: return KirkBack;
                case TextureFace.World: return KirkParty;
                }
                break;
            }

            return null;
        }

        public static Texture2D GetTextureFromCapture(int id) {
            switch (id) {
            case 1: return RottenNet;
            case 2: return RegularNet;
            case 3: return GreatNet;
            case 4: return MasterNet;
            }
            return null;
        }

        public static Texture2D GetTextureFromMedicine(int id) {
            switch (id) {
            case 1: return LeafBandage;
            case 2: return MagicStone;
            case 3: return AntiPoison;
            case 4: return BucketOfWater;
            case 5: return Salt;
            case 6: return AirHorn;
            case 7: return RoosVicee;
            case 8: return NastySpoon;
            }
            return null;
        }

        public static Texture2D GetTextureFromPlayer(int id) {
            switch (id) {
            case 1: return MCGirlWorld;
            case 2: return MCBoyWorld;
            case 3: return ChristmanWorld;
            }
            return null;
        }

        public static int GetIdFromCharacterWorldTexture(Texture tex) {
            if (tex == MCGirlWorld) return 1;
            if (tex == MCBoyWorld) return 2;
            if (tex == ChristmanWorld) return 3;
            return 0;
        }
        #endregion
    }
}

