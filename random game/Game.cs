using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Windows.Input;
using System.Media;
using System.Windows.Media;

namespace random_game
{
    public class Game
    {
        //Stopwatch sw = new Stopwatch();
        List<Object> objects = new List<Object>();
        private MediaPlayer _mediaPlayer;
        private GameData _gameData;
        private Chart _chart;
        private Object _displayText;
        private int notesHit = 0;
        private int notesMissed = 0;
        public Game()
        {
           
            _gameData = new GameData();
            _mediaPlayer = new MediaPlayer();
            
            _chart = new Chart("testChart", _gameData);


            _gameData.beatTime = ((60 / _gameData.bpm) * 1000);

            initSong();
            initReceptors();
            initNotes();
            setupBinds();
            _displayText = new Object(_gameData.receptors[_gameData.keyCount-1].x+10, 5, _gameData);
            objects.Add(_displayText);

            DateTime _previousGameTime = DateTime.Now;

            bool running = true;
            while (running)
            {
                //update loop
                TimeSpan GameTime = DateTime.Now - _previousGameTime;
                _previousGameTime = _previousGameTime + GameTime;

                //sw.Stop();
                input();
                update((float)(GameTime.TotalSeconds));
                draw();

                Thread.Sleep(1);
            }
        }
        public void initNotes()
        {
            for (int i = 0; i < _chart.notes.Count; i++)
            {
                if (_chart.notes[i].lane < _gameData.keyCount)
                {
                    Note n = new Note(_chart.notes[i].time, _chart.notes[i].lane, _chart.notes[i].sustainLength, _gameData);
                    _gameData.notes.Add(n);
                    //objects.Add(n);
                }
            }
            List<Note> sortedNotes = _gameData.notes.OrderBy(a => a.time).ToList();
            _gameData.notes = sortedNotes;

        }
        public void initReceptors()
        {
            for (int i = 0; i < _gameData.keyCount; i++)
            {
                Receptor r = new Receptor(i, _gameData);
                _gameData.receptors.Add(r);
                objects.Add(r);
            }
                
        }
        public void initSong()
        {
            
            Uri audioPath = new Uri(System.IO.Directory.GetCurrentDirectory()+"/assets/songs/"+_gameData.songName.ToLower()+".wav");
            // _soundPlayer.LoadCompleted
            //string thing = System.IO.Directory.GetCurrentDirectory() + "assets/songs/testSong.wav";
            
            _mediaPlayer.Open(audioPath);
            _mediaPlayer.SpeedRatio = _gameData.songSpeed;
            _mediaPlayer.Volume = 0;
            //_mediaPlayer.Position = TimeSpan.Zero;



            //_soundPlayer.Play();


        }


        float timeElapsed = 0;
        bool startedSong = false;
        void update(float dt)
        {
            timeElapsed += dt;


            if (_gameData.notes.Count > 0)
            {
                bool checkIfNoteNeedsAdding = true;
                while (checkIfNoteNeedsAdding)
                {
                    if (_gameData.notes.Count <= 0)
                    {
                        checkIfNoteNeedsAdding = false;
                        return;
                    }
                        

                    Note n = _gameData.notes[0];
                    if (n.time < _gameData.songTime+((_gameData.beatTime*4)/_gameData.scrollSpeed))
                    {
                        _gameData.renderedNotes.Add(n);
                        objects.Add(n);
                        _gameData.notes.RemoveAt(0);
                    }
                    else
                    {
                        checkIfNoteNeedsAdding = false;
                    }
                }
            }



            //if (_mediaPlayer.e)
                _gameData.songTime += (dt) * 1000* _gameData.songSpeed;

            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].update(dt);
            }

            if (_gameData.autoPlay)
            {
                for (int i = 0; i < _gameData.renderedNotes.Count; i++)
                {
                    Note n = _gameData.renderedNotes[i];
                    if (_gameData.songTime > n.time && !n.hitNote)
                    {
                        onHitNote(n);
                        _gameData.receptors[n.lane].autoPlayReset = 0.1f + (n.sustainLength*0.001f);
                    }
                }
            }

            
            int noteIdx = 0;
            while (noteIdx < _gameData.renderedNotes.Count) //remove notes that need removing
            {
                if (_gameData.renderedNotes[noteIdx].shouldRemove)
                {
                    Note n = _gameData.renderedNotes[noteIdx];
                    if (!n.hitNote && n.lane < _gameData.keyCount)
                        notesMissed++;
                    objects.RemoveAt(objects.IndexOf(n));
                    _gameData.renderedNotes.RemoveAt(noteIdx);
                    n = null;
                }
                else
                    noteIdx++;
            }
            if (!startedSong && _gameData.songTime > 0 && _mediaPlayer.Position > TimeSpan.Zero)
            {
                
                _mediaPlayer.Volume = 1;
                _mediaPlayer.Position = TimeSpan.Zero;
                //
                startedSong = true;
                _gameData.songTime = 0;
            }

            _displayText.text = _gameData.songName;
            if (_gameData.songSpeed != 1.0f)
                _displayText.text += " (" + _gameData.songSpeed + "x)";
            _displayText.text += "\n" +"Notes Hit: " + notesHit+ "\nNotes Missed: " + notesMissed+"\nNotes Loaded: " + _gameData.renderedNotes.Count;
            if (startedSong)
            {
                TimeSpan currentTime = TimeSpan.FromMilliseconds(_gameData.songTime);
                if (currentTime > _mediaPlayer.NaturalDuration.TimeSpan)
                    currentTime = _mediaPlayer.NaturalDuration.TimeSpan;
                _displayText.text += "\n"+((float)currentTime.Minutes) + ":" +
                    (((float)currentTime.Seconds) < 10 ? "0" : "") +
                    ((float)currentTime.Seconds) + " / "+
                
                    ((float)_mediaPlayer.NaturalDuration.TimeSpan.Minutes) + ":" + ((float)_mediaPlayer.NaturalDuration.TimeSpan.Seconds);
            }
            if (_gameData.autoPlay)
                _displayText.text += "\nAUTOPLAY";
        }
        string fillInSpace = "";
        void draw()
        {
            
            if (fillInSpace == "")
            {
                for (int i = 0; i < 30; i++)
                {
                    fillInSpace += "                                                                                \n";
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, 0);
            Console.Write(fillInSpace);
            //Console.Clear();
            Console.SetCursorPosition(0, 0);

            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].draw();
            }
            Console.SetCursorPosition(0, 0);

        }

        private List<Key> Keybinds = new List<Key>();
        private List<bool> KeysHeld = new List<bool>();
        private List<float> LongNoteTimes = new List<float>();

        void setupBinds()
        {
            //fill binds
            for (int i = 0; i < Constants.defaultBinds[_gameData.keyCount-1].Count; i++)
            {
                Keybinds.Add(Constants.defaultBinds[_gameData.keyCount - 1][i]);
            }

            for (int i = 0; i < Keybinds.Count; i++)
            {
                KeysHeld.Add(false);
                LongNoteTimes.Add(-10000); //do -10000 just in case
            }
        }

        public void input()
        {
            for (int i = 0; i < Keybinds.Count; i++)
            {
                bool checkPress = false;
                bool checkRelease = false;
                if (!KeysHeld[i])
                    checkPress = true;
                else
                    checkRelease = true;
                
                if (Keyboard.IsKeyDown(Keybinds[i]))
                    KeysHeld[i] = true;
                else 
                    KeysHeld[i] = false;

                if (KeysHeld[i] && checkPress)
                    onKeyPress(i);
                else if (!KeysHeld[i] && checkRelease)
                    onKeyRelease(i);
            }
        }

        void onKeyPress(int lane)
        {
            //notesHit++;
            if (_gameData.checkLane(lane))
            {
                _gameData.receptors[lane].text = Constants.NOTEPRESSEDTEXT;
            }
            List<Note> sortedNotes = _gameData.renderedNotes.OrderBy(a=>a.time).ToList();
            _gameData.renderedNotes = sortedNotes;
            for (int i = 0; i < _gameData.renderedNotes.Count; i++)
            {
                Note n = _gameData.renderedNotes[i];
                if (n.lane == lane)
                {
                    
                    if (n.canHitNote && !n.hitNote)
                    {
                        _gameData.receptors[lane].text = Constants.NOTETEXT;
                        onHitNote(n);
                        break;
                    }
                }
            }
        }
        void onKeyRelease(int lane)
        {
            if (_gameData.checkLane(lane))
            {
                _gameData.receptors[lane].text = Constants.NOTETEXT;
                _gameData.receptors[lane].BGColor = ConsoleColor.DarkGray;
                _gameData.receptors[lane].FGColor = ConsoleColor.DarkGray;
                if (_gameData.songTime < LongNoteTimes[lane] && startedSong)
                {
                    notesMissed++;
                }
                LongNoteTimes[lane] = 0;
            }
        }

        void onHitNote(Note n)
        {
            notesHit++;
            if (n.sustainLength == 0) //regular notes
                n.shouldRemove = true;
            else
                LongNoteTimes[n.lane] = n.time + n.sustainLength;
            n.doDraw = false;
            n.hitNote = true;
            _gameData.receptors[n.lane].BGColor = ConsoleColor.White;
            _gameData.receptors[n.lane].FGColor = ConsoleColor.White;
        }
    }
}
