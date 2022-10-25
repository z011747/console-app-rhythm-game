using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

            for (int i = 0; i < GameSettings.keyBinds.Count; i++)
            {
                optionList.Add((i+1)+"K");
            }

            optionList.Add("Go Back");
            

            setupMenu();
            objects.Add(_text);
            startUpdateLoop();
        }

        public override void onSelect(int selection)
        {
            base.onSelect(selection);
            switch (optionList[selection])
            {
                case "Go Back":
                    changeRoom(new OptionsMenu());
                    break;
                default:
                    changeRoom(new ControlPickerMenu(selection+1));
                    break;
            }
        }
    }


    class ControlPickerMenu : BaseSelectionMenu
    {
        Object _text;
        int keyCount;
        GameData _gameData;
        bool waitingForInput = false;
        public ControlPickerMenu(int keyCount, int optionSelected = 0) : base()
        {
            this.selectedOption = optionSelected;
            _text = new Object(2, 1, null);
            _text.text = "----------------------------------------------------";
            _text.text += "\nControls";
            _text.text += "\n----------------------------------------------------";

            this.keyCount = keyCount;

            _gameData = new GameData("", "");
            _gameData.keyCount = keyCount;
            try
            {
                _gameData.noteSkinData = new NoteSkinData(_gameData, GameSettings.noteSkin);
            }
            catch (Exception e)
            {
                _gameData.noteSkinData = new NoteSkinData(_gameData, "Default");
                Constants.errorPopup("Error loading Noteskin.");
                GameSettings.noteSkin = "Default";
            }

            for (int i = 0; i < keyCount; i++)
            {
                Receptor r = new Receptor(i, _gameData);
                _gameData.receptors.Add(r);
                optionList.Add(GameSettings.keyBinds[keyCount - 1][i].ToString());
                objects.Add(r);

                r.y = 10;
            }

            optionList.Add("Go Back");
            setupMenu();
            objects.Add(_text);
            startUpdateLoop();
        }

        public override void onSelect(int selection)
        {
            if (waitingForInput)
                return; 

            base.onSelect(selection);
            switch (optionList[selection])
            {
                case "Go Back":
                    changeRoom(new KeybindMenu());
                    break;
                default:
                    waitingForInput = true;
                    options[selection].text = "?";
                    break;
            }
        }

        public override void onChangeSelection(int change)
        {
            if (waitingForInput)
                return;
            regenerateMenu();
        }

        public override void onSideSelection(int change) //swap which
        {
            if (waitingForInput)
                return;
            selectedOption += change;
            if (selectedOption < 0)
                selectedOption = optionList.Count - 1; //-1 because its an index
            else if (selectedOption >= optionList.Count)
                selectedOption = 0;
            regenerateMenu();
        }


        public override void regenerateMenu()
        {
            for (int i = 0; i < optionList.Count; i++)
            {
                Object obj = options[i];
                obj.text = optionList[i];
                obj.x = 1 + (i * _gameData.noteSkinData.spacing);
                obj.x += (float)Math.Floor(Constants.BUFFERWIDTH * 0.5) - (float)(_gameData.noteSkinData.spacing * (_gameData.keyCount + 1) * 0.5);
                obj.x += (float)Math.Floor(_gameData.noteSkinData.spacing * 0.5);
                obj.y = 10 + 4;
                if (i == selectedOption)
                {
                    obj.text += "\n\n^\n|\n|";
                    obj.y = 11+ 4;
                }
            }
        }


        public override void update(float dt)
        {
            base.update(dt);
            if (waitingForInput)
            {
                if (Keyboard.IsKeyDown(Key.Escape))
                {
                    waitingForInput = false;
                    return;
                }
                var keys = Enum.GetNames(typeof(Key));
                
                for (int i = 0; i < keys.Length; i++)
                {
                    Key key = (Key)Enum.Parse(typeof(Key), keys[i]);
                    if (key != 0)
                    {
                        if (Keyboard.IsKeyDown(key) && key != Key.Return) //bloc enter
                        {
                            GameSettings.keyBinds[keyCount - 1][selectedOption] = key;
                            GameSettings.saveKeybinds();
                            changeRoom(new ControlPickerMenu(keyCount, selectedOption)); //refresh
                            break;
                        }
                    }

                }
            }

        }
    }
}
