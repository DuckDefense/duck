using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VideoGame.Classes {
    public interface ITimer {
        /// <summary>
        /// Interval at which the animation should update
        /// </summary>
        float Interval { get; set; }
        /// <summary>
        /// Timer that keeps getting updated until the Interval is reached
        /// </summary>
        float Timer { get; set; }
    }
}
