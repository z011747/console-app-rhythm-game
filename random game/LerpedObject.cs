using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace random_game
{
    class LerpedObject : Object
    {
        float lerpedX = 0.0f;
        float lerpedY = 0.0f;
        public float lerpSpeed = 1.0f;
        public LerpedObject(float x, float y, GameData _gameData) : base(x,y,_gameData)
        {
            lerpedX = x;
            lerpedY = y;
            
        }
        public override void update(float dt)
        {
            base.update(dt);
            lerpedX = MathUtil.lerp(lerpedX, x, lerpSpeed * dt);
            lerpedY = MathUtil.lerp(lerpedY, y, lerpSpeed * dt);
        }

        public override void draw()
        {
            if (!doDraw)
                return;

            
            string[] lines = text.Split('\n'); //split for each line
            for (int i = 0; i < lines.Length; i++)
            {
                int roundedX = (int)Math.Round(lerpedX + offsetX); //get x
                int roundedY = (int)Math.Round(lerpedY + offsetY) + i; //get y pos for line
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
    }
}
