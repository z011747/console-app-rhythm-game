using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;

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
        GameData _gameData;
        public Chart(string chartPath, GameData _gameData)
        {
            this._gameData = _gameData;
            //string chartPath = System.IO.Directory.GetCurrentDirectory() + "/assets/charts/" + chartName + ".chart"; //load chart
            if (!File.Exists(chartPath))
            {
                MessageBox.Show(chartPath + " does not exist");
                return;
            }

            try
            {
                if (chartPath.EndsWith(".json")) //fnf chart
                {
                    loadFNFChart(chartPath);
                }
                else //assume its a .chart file
                {
                    loadChart(chartPath);
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Error Loading Chart: " + e.Message);
                return;
            }





        }


        void loadChart(string chartPath)
        {
            using (StreamReader sr = File.OpenText(chartPath))
            {
                int lineNumber = 0;
                string s;
                //read lines
                while ((s = sr.ReadLine()) != null)
                {
                    switch (s[0])
                    {
                        case '#': //song name
                                  //s = s.Remove(0,1);
                                  //_gameData.songName = s;
                            break;
                        case '~': //bpm
                            s = s.Remove(0, 1);
                            float bpm = 100;
                            float.TryParse(s, out bpm);
                            _gameData.bpm = bpm;
                            break;
                        case '*': //key count
                            s = s.Remove(0, 1);
                            int keyCount = 100;
                            Int32.TryParse(s, out keyCount);
                            _gameData.keyCount = keyCount;
                            break;
                        default: //notes
                            ChartNote note = new ChartNote();
                            string[] noteData = s.Split(':');

                            float.TryParse(noteData[0], out note.time); //note time
                            Int32.TryParse(noteData[1], out note.lane); //note lane
                            float.TryParse(noteData[2], out note.sustainLength); //sustain length

                            //note.time = (note.time * 1000);

                            if (note.lane >= 0)
                                notes.Add(note);

                            break;
                    }
                    //Console.WriteLine(s);
                    lineNumber++;
                }
            }
        }

        void loadFNFChart(string chartPath)
        {
            string chartStr = "";
            using (StreamReader sr = File.OpenText(chartPath))
            {
                string s = sr.ReadToEnd();
                chartStr += s;
            }
            JObject fnfJson = JObject.Parse(chartStr);

            //get bpm
            float bpm = 100;
            float.TryParse(fnfJson["song"]["bpm"].ToString(), out bpm);
            _gameData.bpm = bpm;

            foreach (var section in fnfJson["song"]["notes"]) //loop through sections
            {
                foreach (var note in section["sectionNotes"]) //loop through notes
                {
                    ChartNote n = new ChartNote();

                    float.TryParse(note[0].ToString(), out n.time); //note time
                    Int32.TryParse(note[1].ToString(), out n.lane); //note lane
                    float.TryParse(note[2].ToString(), out n.sustainLength); //sustain length

                    if (n.lane >= 0)
                        notes.Add(n);
                }
            }
        }

        void loadCHCHart(string chartPath)
        {

        }
    }
}
