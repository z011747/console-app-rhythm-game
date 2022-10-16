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

        public SongSelectionMenu() : base()
        {
            _text = new Object(2, 1, null);
            _text.text = "----------------------------------------------------";
            _text.text += "\nSong Selection";
            _text.text += "\n----------------------------------------------------";

            string[] songFolders = Directory.GetDirectories(System.IO.Directory.GetCurrentDirectory() + "/songs/");
            foreach (string file in songFolders)
            {
                string newfile = file.Replace(System.IO.Directory.GetCurrentDirectory() + "/songs/", "");
                optionList.Add(newfile);
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
                    changeRoom(new MainMenu());
                    break;
                default:
                    changeRoom(new SongDiffSelection(optionList[selection].Trim()));
                    break;
            }

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


            string[] fileExts = { ".chart", ".json" };

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
                default:
                    changeRoom(new Game(songName, chartWithFileExt[selection].Trim()));
                    break;
            }

        }
    }
}
