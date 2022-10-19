using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace random_game
{
    class BarObject : Object
    {
        public float minValue;
        public float maxValue;
        float currentValue;
        float barWidth;
        public BarObject(float x, float y, GameData _gameData, float min, float max, float barWidth) : base(x,y,_gameData)
        {
            minValue = min;
            maxValue = max;
            currentValue = min;
            this.barWidth = barWidth;
            BGColor = ConsoleColor.White;
            FGColor = ConsoleColor.White;
        }
        public override void update(float dt)
        {
            base.update(dt);  
        }
        public void updateBarValue(float val)
        {
            currentValue = MathUtil.scaleNumberToRange(val, minValue, maxValue, 0, barWidth);
            text = "";
            for (int i = 0; i < Math.Round(currentValue); i++)
            {
                text += "#";
            }
        }
    }
}
