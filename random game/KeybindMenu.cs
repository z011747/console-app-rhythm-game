using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace random_game
{
    class KeybindMenu : BaseSelectionMenu
    {
        Object _text;
        public KeybindMenu() : base()
        {
            _text = new Object(2, 1, null);
            _text.text = "----------------------------------------------------";
            _text.text += "\nKeybinds";
            _text.text += "\n----------------------------------------------------";

            

            setupMenu();
            objects.Add(_text);
            startUpdateLoop();
        }

        public override void onSelect(int selection)
        {
            base.onSelect(selection);
            switch (optionList[selection])
            {
                case "Play":
                    changeRoom(new SongSelectionMenu());
                    break;

                case "Options":
                    changeRoom(new OptionsMenu());
                    break;

                case "Note Skins":
                    changeRoom(new NoteSkinMenu());
                    break;

                case "Exit":
                    running = false;
                    break;
            }
        }
    }

}
