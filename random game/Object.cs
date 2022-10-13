using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace random_game
{

    public class Object
    {
        public float x { get; set; }
        public float y { get; set; }
        public Object(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
