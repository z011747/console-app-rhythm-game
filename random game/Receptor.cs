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

            FGColor = ConsoleColor.DarkGray;
            BGColor = ConsoleColor.DarkGray;
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
                        FGColor = ConsoleColor.DarkGray;
                        BGColor = ConsoleColor.DarkGray;
                    }
                }
            }
        }
    }
}
