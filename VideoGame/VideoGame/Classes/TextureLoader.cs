using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace VideoGame.Classes {
    public class TextureLoader {
        public static Texture2D GronkeyFront, GronkeyBack, GronkeyParty;


        //Add all Textures in here
        public void LoadTextures(ContentManager content) {
            GronkeyFront = content.Load<Texture2D>(@"Sprites/Monsters/Front/Gronkey");
            GronkeyBack = content.Load<Texture2D>(@"Sprites/Monsters/Back/Gronkey");
            GronkeyParty = content.Load<Texture2D>(@"Sprites/Monsters/Party/Gronkey");
        }
    }
}
