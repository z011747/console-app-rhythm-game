using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace random_game
{
    class NoteSkinData
    {
        string path = "";
        public int spacing { get; private set; }
        public int longNoteOffset { get; private set; }
        public bool noteQuants { get; private set; }

        public List<string> noteTexts = new List<string>();
        public List<string> pressedNoteTexts = new List<string>();
        public List<string> longNoteTexts = new List<string>();

        public List<ConsoleColor> receptorColors = new List<ConsoleColor>();
        public List<ConsoleColor> noteColors = new List<ConsoleColor>();

        GameData _gameData;
        public NoteSkinData(GameData _gameData, string skin = "default")
        {
            this._gameData = _gameData;
            path = System.IO.Directory.GetCurrentDirectory() + "/noteskins/" + skin + "/";

            using (StreamReader sr = File.OpenText(path+"info.txt"))
            {
                string s;
                bool readingNotes = false;
                //read lines
                while ((s = sr.ReadLine()) != null)
                {
                    if (!readingNotes)
                    {
                        int keyCountCheck = 0;
                        Int32.TryParse(s, out keyCountCheck);
                        if (keyCountCheck == _gameData.keyCount)
                        {
                            readingNotes = true;
                        }
                        else
                        {
                            string[] data = s.Split(':');
                            switch (data[0])
                            {
                                case "spacing":
                                    Int32 space = 0;
                                    Int32.TryParse(data[1], out space);
                                    spacing = space;
                                    break;
                                case "longNoteOffset":
                                    Int32 lnoff = 0;
                                    Int32.TryParse(data[1], out lnoff);
                                    longNoteOffset = lnoff;
                                    break;
                                /*case "noteQuants":
                                    Int32 quants = 0;
                                    Int32.TryParse(data[1], out quants);
                                    noteQuants = quants == 1;
                                    break;*/
                            }
                        }
                    }
                    else
                    {
                        if (s == "#")
                        {
                            readingNotes = false; //stop from reading notes
                        }
                        else
                        {
                            string[] data = s.Split(':');

                            string noteText = getTextFromPath(path + data[0] + ".txt");
                            noteTexts.Add(noteText);

                            string pressedNoteText = getTextFromPath(path + data[1] + ".txt");
                            pressedNoteTexts.Add(pressedNoteText);

                            string longNoteText = getTextFromPath(path + data[2] + ".txt");
                            longNoteTexts.Add(longNoteText);

                            ConsoleColor receptorColor = getColorFromString(data[3]);
                            receptorColors.Add(receptorColor);

                            ConsoleColor noteColor = getColorFromString(data[4]);
                            noteColors.Add(noteColor);
                        }
                    }

                }

            }

        }

        string getTextFromPath(string path)
        {
            string str = "";
            using (StreamReader sr = File.OpenText(path))
            {
                string s = sr.ReadToEnd();
                str += s;
            }
            return str;
        }

        ConsoleColor getColorFromString(string col)
        {
            ConsoleColor color = ConsoleColor.Black;
            switch (col.ToLower())
            {
                case "white":
                    color = ConsoleColor.White;
                    break;
                case "darkgray":
                    color = ConsoleColor.DarkGray;
                    break;
                case "gray":
                    color = ConsoleColor.Gray;
                    break;
                case "yellow":
                    color = ConsoleColor.Yellow;
                    break;
                case "red":
                    color = ConsoleColor.Red;
                    break;
                case "green":
                    color = ConsoleColor.Green;
                    break;
                case "blue":
                    color = ConsoleColor.Blue;
                    break;
                case "cyan":
                    color = ConsoleColor.Cyan;
                    break;
                case "darkblue":
                    color = ConsoleColor.DarkBlue;
                    break;
                case "darkcyan":
                    color = ConsoleColor.DarkCyan;
                    break;
                case "darkmagenta":
                    color = ConsoleColor.DarkMagenta;
                    break;
                case "darkgreen":
                    color = ConsoleColor.DarkGreen;
                    break;
                case "darkred":
                    color = ConsoleColor.DarkRed;
                    break;
                case "darkyellow":
                    color = ConsoleColor.DarkYellow;
                    break;
                case "magenta":
                    color = ConsoleColor.Magenta;
                    break;
            }
            return color;
        }

        //https://step-mania.fandom.com/wiki/Notes //most colors come from here
        int[] beats = { 4, 8, 12, 16, 24, 32, 48, 64, 96, 128, 192 };
        //tried my best to match
        ConsoleColor[] beatColor = { ConsoleColor.Red, ConsoleColor.Blue, ConsoleColor.Magenta, ConsoleColor.Green, ConsoleColor.DarkMagenta, 
            ConsoleColor.DarkRed, ConsoleColor.Cyan, ConsoleColor.DarkGreen, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.DarkGreen };

        public ConsoleColor getNoteQuantColor(Note n)
        {
            double noteBeat = Math.Floor(((n.time / (_gameData.beatTime)) * 48) + 0.5);
            ConsoleColor noteCol = ConsoleColor.DarkGray;
            for (int i = 0; i < beats.Length; i++)
            {
                if (noteBeat % (192 / beats[i]) == 0)
                {
                    noteBeat = beats[i];
                    noteCol = beatColor[i];
                    break;
                }
            }


            return noteCol;
        }
    }
}
