using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace VideoGame.Classes {
    public class TextureLoader {
        public static Texture2D GronkeyFront, GronkeyBack, GronkeyParty;
        public static Texture2D TileSet;

        //Add all Textures in here
        public void LoadTextures(ContentManager content) {
            #region Monsters
            GronkeyFront = content.Load<Texture2D>(@"Sprites/Monsters/Front/Gronkey");
            GronkeyBack = content.Load<Texture2D>(@"Sprites/Monsters/Back/Gronkey");
            GronkeyParty = content.Load<Texture2D>(@"Sprites/Monsters/Party/Gronkey");
            #endregion

            #region Characters
            #endregion

            #region Maps

            TileSet = content.Load<Texture2D>(@"Sprites/TileSet/tileset");

            #endregion

            #region Items

            #endregion
        }
    }
}

