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

            
            string[] lines = text.Split('\n'); //split for each line
            for (int i = 0; i < lines.Length; i++)
            {
                int roundedX = (int)Math.Round(x + offsetX); //get x
                int roundedY = (int)Math.Round(y + offsetY) + i; //get y pos for line
                /*string[] spaceSplit = lines[i].Split(' '); //split for space so the bg stays black on spaces
                int drawOffset = 0;
                for (int j = 0; j < spaceSplit.Length; j++)
                {
                    string drawString = spaceSplit[j];

                    drawOffset += drawString.Length;
                }*/

                if (roundedY >= 0 && roundedY < Constants.BUFFERHEIGHT) //if within bounds
                {
                    int drawOffset = 0;
                    while (lines[i][0] == ' ')
                    {
                        lines[i] = lines[i].Remove(0, 1);
                        drawOffset++;
                    }
                    Console.SetCursorPosition(roundedX+drawOffset, roundedY); //set the cursor correctly
                    Console.BackgroundColor = BGColor; //set colors
                    Console.ForegroundColor = FGColor;
                    Console.Write(lines[i]); //draw the string
                }
            }
        }


        public int getWidth()
        {
            int highestWidth = 0;

            string[] lines = text.Split('\n'); //split for each line
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Length > highestWidth)
                    highestWidth = lines[i].Length;
            }
            return highestWidth;
        }
        public int getHeight()
        {
            string[] lines = text.Split('\n'); //split for each line
            return lines.Length;
        }
    }
}
