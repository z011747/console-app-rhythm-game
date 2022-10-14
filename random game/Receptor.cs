using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace random_game
{
    class Receptor : Object
    {
        public int lane { get; }
        public Receptor(int lane, GameData _gameData) : base(0, 0, _gameData)
        {
            this.x = 2 + (lane * 8);
            text = Constants.NOTETEXT;
            this.lane = lane;

            y = 1;
            if (_gameData.downscroll)
                y = 25;
        }
    }
}
