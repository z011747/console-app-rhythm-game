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
                else if (chartPath.EndsWith(".sm"))
                {
                    loadSMFile(chartPath);
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

        void loadCHChart(string chartPath)
        {
            _gameData.keyCount = 5;
        }

        void loadSMFile(string chartPath)
        {
            _gameData.keyCount = 4;
            string chartStr = "";
            using (StreamReader sr = File.OpenText(chartPath))
            {
                string s = sr.ReadToEnd();
                chartStr += s;
            }
            string[] chartSections = chartStr.Split(';');
            bool loadedNotes = false;
            float bpm = 100;
            float currentParsingTime = 0.0f;
            ChartNote[] holdNoteStorage = { null, null, null, null };
            foreach (string sec in chartSections)
            {
                string[] data = sec.Split(':');

                if (data[0].Contains("#BPMS"))
                {
                    string[] bpms = data[1].Split(',');
                    if (bpms.Length > 1)
                    {
                        MessageBox.Show("Cannot load songs with bpm changes."); //will sort this out later
                    }
                    string[] bpmData = bpms[0].Split('=');

                    float.TryParse(bpmData[1], out bpm);
                    _gameData.bpm = bpm;
                    
                }
                else if (data[0].Contains("#OFFSET"))
                {
                    float offset = 0;
                    float.TryParse(data[1], out offset);
                    //currentParsingTime -= (offset * 1000);
                    _gameData.songOffset = -(offset * 1000);
                }
                else if(data[0].Contains("#NOTES"))
                {
                    if (!loadedNotes) //only use first diff in song, ignore the rest
                    {
                        loadedNotes = true;
                        string[] measures = data[6].Split(',');
                        

                        foreach (string measure in measures)
                        {
                            string[] rows = measure.Trim().Split('\n');
                            float measureTime = (((60 / bpm) * 1000) * 4); //assume its a 4/4 song
                            for (int i = 0; i < rows.Length; i++)
                            {
                                string row = rows[i];
                                
                                float noteTime = currentParsingTime + ((measureTime / rows.Length) * i);
                                for (int n = 0; n < row.Length; n++)
                                {
                                    
                                    switch (row[n])
                                    {
                                        case '1': //regular note
                                            ChartNote note = new ChartNote();
                                            note.time = noteTime;
                                            note.lane = n;
                                            note.sustainLength = 0;
                                            notes.Add(note);
                                            break;

                                        case '4': //rolls (not added, just act the same as hold note)
                                        case '2': //hold note start
                                            ChartNote holdNote = new ChartNote();
                                            holdNote.time = noteTime;
                                            holdNote.lane = n;
                                            holdNote.sustainLength = 0;
                                            notes.Add(holdNote);
                                            holdNoteStorage[n] = holdNote;
                                            break;

                                        case '3': //hold note end
                                            if (holdNoteStorage[n] != null)
                                            {
                                                holdNoteStorage[n].sustainLength = noteTime - holdNoteStorage[n].time;
                                                holdNoteStorage[n] = null;
                                            }
                                            break;
                                    }

                                }
                            }


                            currentParsingTime += measureTime;
                        }
                    }
                }

                switch (data[0])
                {
                    case "#BPMS":

                        
                        break;
                    case "#NOTES":

                        break;
                }

            }
        }
    }
}
