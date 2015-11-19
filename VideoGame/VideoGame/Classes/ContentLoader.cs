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
        public static Texture2D Christman;
        public static TiledMap Map;

        public static void SetContent(ContentManager content) {
            Content = content;
        }

        //Add all Textures in here
        public void LoadContent() {
            #region Monsters
            GronkeyFront = Content.Load<Texture2D>(@"Sprites/Monsters/Front/Gronkey");
            GronkeyBack = Content.Load<Texture2D>(@"Sprites/Monsters/Back/Gronkey");
            GronkeyParty = Content.Load<Texture2D>(@"Sprites/Monsters/Party/Gronkey");
            #endregion

            #region Characters

            Christman = Content.Load<Texture2D>(@"Sprites/Characters/World/Christman");
            #endregion

            #region Maps
            Map = Content.Load<TiledMap>("map");

            #endregion

            #region Items

            #endregion
        }
    }
}

