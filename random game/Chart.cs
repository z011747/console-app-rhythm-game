using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace random_game
{
    class ChartNote
    {
        public float time;
        public int lane;
        public float sustainLength;
        public ChartNote()
        {

        }
    };
    class Chart
    {
        public List<ChartNote> notes = new List<ChartNote>();
        public Chart(string chartName, GameData _gameData)
        {
            string chartPath = System.IO.Directory.GetCurrentDirectory() + "/assets/charts/" + chartName + ".chart"; //load chart

            using (StreamReader sr = File.OpenText(chartPath))
            {
                int lineNumber = 0;
                string s;
                //read lines
                while ((s = sr.ReadLine()) != null)
                {
                    switch (lineNumber)
                    {
                        case 0: //song name

                            break;
                        case 1: //bpm
                            float bpm = 100;
                            float.TryParse(s, out bpm);
                            _gameData.bpm = bpm;
                            break;
                        default: //notes
                            ChartNote note = new ChartNote();
                            string[] noteData = s.Split(':');

                            float.TryParse(noteData[0], out note.time); //note time
                            Int32.TryParse(noteData[1], out note.lane); //note lane
                            float.TryParse(noteData[2], out note.sustainLength); //sustain length

                            //note.time = (note.time * 1000);

                            notes.Add(note);

                            break;
                    }
                    //Console.WriteLine(s);
                    lineNumber++;
                }
            }
        }
    }
}
