using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VideoGame.Classes {
    public interface IAnimatable {
        void AnimateWorld(GameTime gametime);
        void AnimateFront(GameTime gametime);
        void AnimateBack(GameTime gametime);
        void AnimateParty(GameTime gametime);
    }
}
