using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace random_game
{
    class OptionsMenu : BaseSelectionMenu
    {
        Object _text;

        List<Receptor> receptors = new List<Receptor>();
        Object note;
        GameData _gameData;

        float reverse = 1; //smooth scroll flip
        float lerpedReverse = 0;

        public OptionsMenu() : base()
        {
            _text = new Object(2, 1, null);
            _text.text = "----------------------------------------------------";
            _text.text += "\nOptions";
            _text.text += "\n----------------------------------------------------";


            optionList.Add("Scroll Speed");
            optionList.Add("Scroll Direction");
            optionList.Add("Auto Play");
            optionList.Add("Note Quants");
            optionList.Add("Key Binds");
            optionList.Add("Go Back");

            setupMenu();
            objects.Add(_text);
            _gameData = new GameData("", "");
            _gameData.noteSkinData = new NoteSkinData(_gameData, GameSettings.noteSkin);
            for (int i = 0; i < 4; i++)
            {
                Receptor r = new Receptor(i, _gameData);
                receptors.Add(r);
                objects.Add(r);
                r.x = (i * _gameData.noteSkinData.spacing) + Constants.BUFFERWIDTH-(_gameData.noteSkinData.spacing*4)-5;
            }
            reverse = (GameSettings.downscroll ? 1 : 0);
            lerpedReverse = reverse;
            note = new Object(0, 0, _gameData);
            objects.Add(note);
            note.text = _gameData.noteSkinData.noteTexts[0];
            note.BGColor = _gameData.noteSkinData.noteColors[0];
            note.FGColor = note.BGColor;

            startUpdateLoop();
        }
        float timeElapsed = 0;
        public override void update(float dt)
        {
            timeElapsed += dt * 1000;
            base.update(dt);
            for (int i = 0; i < receptors.Count; i++)
            {
                Receptor r = receptors[i];
                lerpedReverse = MathUtil.lerp(lerpedReverse, reverse, dt * 4);
                float targetY = 1 + (Constants.BUFFERHEIGHT - r.getHeight()-2) * lerpedReverse;
                r.y = targetY;
            }
            float scroll = 1 * (1 - (lerpedReverse * 2));
            note.x = receptors[0].x;
            note.y = receptors[0].y - (float)(((((timeElapsed%1000) - 1000)) * GameSettings.scrollSpeed * 0.05) * scroll);
        }

        public override void onSideSelection(int change)
        {
            switch (optionList[selectedOption])
            {
                case "Scroll Speed":
                    //float newScrollSpeed = (float)(Math.Round()*10)/10); //make sure its rounded
                    float newScrollSpeed = MathUtil.roundToDecimalPlace((float)(GameSettings.scrollSpeed + (0.1 * change)), 1);
                    newScrollSpeed = MathUtil.bound(newScrollSpeed, 0.1f, 10.0f); //cant go below 0.1, cant go above 10

                    GameSettings.scrollSpeed = newScrollSpeed;
                    break;
                case "Scroll Direction":
                    GameSettings.downscroll = !GameSettings.downscroll;
                    reverse = (GameSettings.downscroll ? 1 : 0);
                    break;
                case "Auto Play":
                    GameSettings.autoPlay = !GameSettings.autoPlay;
                    break;
                case "Note Quants":
                    GameSettings.noteQuants = !GameSettings.noteQuants;
                    break;
            }
            base.onSideSelection(change);
        }


        public override void regenerateMenu()
        {
            for (int i = 0; i < optionList.Count; i++)
            {
                Object obj = options[i];
                obj.text = optionList[i];
                switch (optionList[i])
                {
                    case "Scroll Speed":
                        obj.text += ": <"+GameSettings.scrollSpeed+">";
                        break;
                    case "Scroll Direction":
                        obj.text += ": <" + (GameSettings.downscroll ? "Downscroll" : "Upscroll") + ">";
                        break;
                    case "Auto Play":
                        obj.text += ": <" + GameSettings.autoPlay + ">";
                        break;
                    case "Note Quants":
                        obj.text += ": <" + GameSettings.noteQuants + ">";
                        break;
                }
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
        }

        public override void onSelect(int selection)
        {
            base.onSelect(selection);
            switch (optionList[selection])
            {
                case "Go Back":
                    GameSettings.saveSettings();
                    changeRoom(new MainMenu());
                    break;

                case "Key Binds":
                    changeRoom(new KeybindMenu());
                    break;
                default:

                    break;
            }

        }
    }
}
