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
            this.x = 1 + (lane * _gameData.noteSkinData.spacing);
            text = _gameData.noteSkinData.noteTexts[lane];
            this.lane = lane;

            y = 1;
            if (_gameData.downscroll)
                y = Constants.BUFFERHEIGHT-getHeight()-1;



            x += (float)Math.Floor(Constants.BUFFERWIDTH * 0.5)-(float)(_gameData.noteSkinData.spacing * (_gameData.keyCount+1)*0.5);

            BGColor = _gameData.noteSkinData.receptorColors[lane];
            FGColor = BGColor;
        }

        /*public override void draw()
        {
            //base.draw();

            Console.SetCursorPosition((int)Math.Round(x + offsetX)+2, (int)Math.Round(y + offsetY));
            Console.BackgroundColor = BGColor;
            Console.Write("###");
            Console.SetCursorPosition((int)Math.Round(x + offsetX), (int)Math.Round(y + offsetY)+1);
            Console.Write("#######");
            Console.SetCursorPosition((int)Math.Round(x + offsetX), (int)Math.Round(y + offsetY) + 2);
            Console.Write("#######");
            Console.SetCursorPosition((int)Math.Round(x + offsetX)+2, (int)Math.Round(y + offsetY) + 3);
            Console.Write("###");
        }*/

        public float autoPlayReset = 0;
        public override void update(float dt)
        {
            base.update(dt);
            if (_gameData.autoPlay)
            {
                if (autoPlayReset > 0)
                {
                    autoPlayReset -= dt;
                    if (autoPlayReset < 0)
                    {
                        BGColor = _gameData.noteSkinData.receptorColors[lane];
                        FGColor = BGColor;
                    }
                }
            }
        }
    }
}
