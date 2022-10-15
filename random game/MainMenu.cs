using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace random_game
{
    class MainMenu : BaseGameClass
    {
        Object _text;
        public MainMenu() : base()
        {
            _text = new Object(2, 2, null);
            _text.text = "press Enter To Start";
            objects.Add(_text);
            startUpdateLoop();
        }

        public override void update(float dt)
        {
            base.update(dt);
            if (Keyboard.IsKeyDown(Key.Enter))
                changeRoom(new Game());
        }
    }
}
