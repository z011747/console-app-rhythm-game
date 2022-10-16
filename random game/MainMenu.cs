using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace random_game
{
    class MainMenu : BaseSelectionMenu
    {
        Object _text;

        public MainMenu() : base()
        {
            _text = new Object(2, 1, null);
            _text.text = "----------------------------------------------------";
            _text.text += "\nSimple Console App Rhythm Game";
            _text.text += "\n----------------------------------------------------";
            

            optionList.Add("Play");
            optionList.Add("Options");
            optionList.Add("Note Skins");
            optionList.Add("Exit");

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
