using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace random_game
{
    class GameData
    {
        public float songTime { get; set; }
        public bool downscroll { get; }

        public float bpm { get; set; }
        public float beatTime { get; set; }
        
        public float scrollSpeed { get; }
        public GameData()
        {
            downscroll = false;
            songTime = 0;
            scrollSpeed = 1;
        }
    }
}
