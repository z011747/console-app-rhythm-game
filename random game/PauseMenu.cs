using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace random_game
{
    class PauseMenu : BaseSelectionMenu
    {
        Object _text;
        Game _game;
        public PauseMenu(Game _game) : base()
        {
            this._game = _game;

            _text = new Object(2, 1, null);
            _text.text = "----------------------";
            _text.text += "\n        Paused"; 
            _text.text += "\n----------------------";


            optionList.Add("Resume Song");
            optionList.Add("Restart");
            optionList.Add("Exit To Main Menu");

            setupMenu();
            objects.Add(_text);

            //put in screen center
            _text.x = (float)((Constants.BUFFERWIDTH * 0.5) - (_text.getWidth() * 0.5));
            _text.y = (float)((Constants.BUFFERHEIGHT * 0.5) - (_text.getHeight() * 0.5));
            startUpdateLoop();
        }

        public override void onSelect(int selection)
        {
            base.onSelect(selection);
            switch (optionList[selection])
            {
                case "Resume Song":
                    running = false;
                    _game.unpause();
                    break;

                case "Restart":
                    _game.restartSong();
                    break;
                case "Exit To Main Menu":
                    running = false;
                    _game.running = false;
                    changeRoom(new MainMenu());
                    break;
            }

        }

        /*public override void draw()
        {
            //base.draw();
            if (!running)
                return;

            //dont fill in bg
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].draw();
            }
        }*/
    }
}
