using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace random_game
{

    class NoteSkinMenu : BaseSelectionMenu
    {
        Object _text;
        GameData _gameData;
        public NoteSkinMenu() : base()
        {
            _text = new Object(2, 1, null);
            _text.text = "----------------------------------------------------";
            _text.text += "\nNote Skin Selection";
            _text.text += "\n----------------------------------------------------";

            string[] folders = Directory.GetDirectories(System.IO.Directory.GetCurrentDirectory() + "/noteskins/");
            foreach (string file in folders)
            {
                string newfile = file.Replace(System.IO.Directory.GetCurrentDirectory() + "/noteskins/", "");
                optionList.Add(newfile);
            }

            optionList.Add("Go Back");

            _gameData = new GameData("", "");
            _gameData.recalculateBeats();
            _gameData.changeScrollSpeed(1.0f); //force to match on menu

            regenNotesAndReceptors();

            setupMenu();
            objects.Add(_text);
            startUpdateLoop();
        }

        public override void regenerateMenu()
        {
            for (int i = 0; i < optionList.Count; i++)
            {
                Object obj = options[i];
                obj.text = optionList[i];
                if (optionList[i] == GameSettings.noteSkin)
                    obj.text += " (Selected)";
                obj.x = 2;
                if (i == selectedOption)
                {
                    obj.text += " <-----";
                    obj.x = 5;
                }
                if (selectedOption + 10 > Constants.BUFFERHEIGHT)
                {
                    obj.y = ((5 + i) - (selectedOption + 10)) + Constants.BUFFERHEIGHT;
                }
                else
                {
                    obj.y = 5 + i;
                }
            }
            //remove old receptors
            while(_gameData.receptors.Count > 0)
            {
                Receptor r = _gameData.receptors[0];
                objects.Remove(r);
                _gameData.receptors.Remove(r);
            }
            //remove old notes
            while (_gameData.notes.Count > 0)
            {
                Note n = _gameData.notes[0];
                objects.Remove(n);
                _gameData.notes.Remove(n);
            }
            regenNotesAndReceptors();
        }
        void regenNotesAndReceptors()
        {
            //refresh noteskin
            try
            {
                _gameData.noteSkinData = new NoteSkinData(_gameData, GameSettings.noteSkin);
            }
            catch(Exception e)
            {
                _gameData.noteSkinData = new NoteSkinData(_gameData, "Default");
                Constants.errorPopup("Error loading Noteskin.");
                GameSettings.noteSkin = "Default";
            }
            
            //regenerate receptors
            for (int i = 0; i < 4; i++)
            {
                Receptor r = new Receptor(i, _gameData);
                _gameData.receptors.Add(r);
                objects.Add(r);
                r.x = (i * _gameData.noteSkinData.spacing) + Constants.BUFFERWIDTH - (_gameData.noteSkinData.spacing * 4) - 5;

                Note n = new Note(-900, i, 0, _gameData);
                n.updatePosition();
                _gameData.notes.Add(n);
                objects.Add(n);

                Note n2 = new Note(-800, i, 50*(i+1), _gameData);
                n2.updatePosition();
                _gameData.notes.Add(n2);
                objects.Add(n2);
            }
        }

        public override void onSelect(int selection)
        {
            base.onSelect(selection);
            switch (optionList[selection])
            {
                case "Go Back":
                    changeRoom(new MainMenu());
                    break;
                default:
                    GameSettings.noteSkin = optionList[selection];
                    break;
            }
            regenerateMenu();



        }
    }
}
