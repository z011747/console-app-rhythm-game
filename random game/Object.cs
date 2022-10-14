using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace random_game
{

    class Object
    {


        public float x { get; set; }
        public float y { get; set; }

        public float offsetX { get; set; }
        public float offsetY { get; set; }
        public string text { get; set; }

        public bool doDraw = true;

        public ConsoleColor BGColor = 0; //black
        public ConsoleColor FGColor = ConsoleColor.Gray; 

        protected GameData _gameData;
        public Object(float x, float y, GameData _gameData)
        {
            this._gameData = _gameData;
            this.x = x;
            this.y = y;
            offsetX = 0;
            offsetY = 0;
            text = "#";

            


        }

        public virtual void update(float dt)
        {
            //text = _gameData.songTime+"";
        }
        
        public virtual void draw()
        {
            if (!doDraw)
                return;

            
            string[] lines = text.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                if (y+offsetY+i >= 0 && y + offsetY+i < 30)
                {
                    int spaceRemoveOffset = 0;
                    while (lines[i][0] == ' ')
                    {
                        lines[i] = lines[i].Remove(0, 1);
                        spaceRemoveOffset++;
                    }

                    Console.SetCursorPosition((int)Math.Round(x + offsetX)+spaceRemoveOffset, (int)Math.Round(y + offsetY) + i);
                    Console.BackgroundColor = BGColor;
                    Console.ForegroundColor = FGColor;
                    Console.Write(lines[i]); //make sure it goes to next line properly
                }
            }
        }
    }
}
