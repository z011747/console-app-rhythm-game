using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace random_game
{
    class SongSelectionMenu : BaseSelectionMenu
    {
        Object _text;
        Object _songSpeedtext;

        public SongSelectionMenu() : base()
        {
            _text = new Object(2, 1, null);
            _text.text = "----------------------------------------------------";
            _text.text += "\nSong Selection";
            _text.text += "\n----------------------------------------------------";
            _songSpeedtext = new Object(30, 4, null);
            _songSpeedtext.text = "Song Speed: " + GameSettings.songSpeed + "x";

            try
            {
                string[] songFolders = Directory.GetDirectories(System.IO.Directory.GetCurrentDirectory() + "/songs/");
                foreach (string file in songFolders)
                {
                    string newfile = file.Replace(System.IO.Directory.GetCurrentDirectory() + "/songs/", "");
                    optionList.Add(newfile);
                }
            }
            catch(Exception e)
            {
                //no songs found
            }

            if (optionList.Count == 0)
                optionList.Add("No Songs Found.");

            optionList.Add("Go Back");

            setupMenu();
            objects.Add(_text);
            objects.Add(_songSpeedtext);
            startUpdateLoop();
        }

        public override void onSelect(int selection)
        {
            base.onSelect(selection);
            switch (optionList[selection])
            {
                case "Go Back":
                    changeRoom(new MainMenu());
                    break;
                case "No Songs Found.":
                    break;
                default:
                    changeRoom(new SongDiffSelection(optionList[selection].Trim()));
                    break;
            }

        }

        public override void onSideSelection(int change)
        {
            base.onSideSelection(change);

            GameSettings.songSpeed = MathUtil.roundToDecimalPlace((float)(GameSettings.songSpeed + (0.05 * change)), 2);
            GameSettings.songSpeed = MathUtil.bound(GameSettings.songSpeed, 0.1f, 8.0f);

            _songSpeedtext.text = "Song Speed: " + GameSettings.songSpeed + "x";
        }
    }


    class SongDiffSelection : BaseSelectionMenu
    {
        Object _text;
        string songName;
        List<string> chartWithFileExt = new List<string>();
        public SongDiffSelection(string songName) : base()
        {
            this.songName = songName;
            _text = new Object(2, 1, null);
            _text.text = "----------------------------------------------------";
            _text.text += "\nSelect Chart";
            _text.text += "\n----------------------------------------------------";


            string[] fileExts = { ".chart", ".json", ".sm" };

            foreach (string fileExt in fileExts)
            {
                string[] songFolders = Directory.GetFiles(System.IO.Directory.GetCurrentDirectory() + "/songs/" + songName + "/", "*"+fileExt);
                foreach (string file in songFolders)
                {
                    string newfile = file.Replace(System.IO.Directory.GetCurrentDirectory() + "/songs/" + songName + "/", "");
                    chartWithFileExt.Add(newfile); //add before it gets removed
                    newfile = newfile.Replace(fileExt, "");
                    optionList.Add(newfile);
                }
            }
            if (optionList.Count == 0)
                optionList.Add("No Charts Found.");




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
                    changeRoom(new MainMenu());
                    break;
                case "No Charts Found.":
                    break;
                default:
                    changeRoom(new Game(songName, chartWithFileExt[selection].Trim()));
                    break;
            }

        }
    }
}
