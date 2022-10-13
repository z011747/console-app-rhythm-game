using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace random_game
{
    class Note : Object
    {
        public float time { get; set; }
        public int lane { get; }

        public Note(float time, int lane, GameData _gameData) : base(0, 0, _gameData)
        {
            this.x = 2 + (lane * 8);
            text = Constants.NOTETEXT;
            this.lane = lane;
            this.time = time;
        }

        public override void update(float dt)
        {
            base.update(dt);
            updatePosition();
            //if (_gameData.songTime > time)
                //time += _gameData.beatTime;
        }

        public void updatePosition()
        {
            float targetY = 3;
            y = targetY - (float)((_gameData.songTime - time)*_gameData.scrollSpeed*0.1);
        }
    }
}
