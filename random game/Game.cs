using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Windows.Input;
using System.Media;

namespace random_game
{
    public class Game
    {
        //Stopwatch sw = new Stopwatch();
        List<Object> objects = new List<Object>();
        private SoundPlayer _soundPlayer;
        private GameData _gameData;
        private Chart _chart;
        private Object _displayText;
        private int notesHit = 0;
        private int notesMissed = 0;
        public Game()
        {
           
            _gameData = new GameData();
            _soundPlayer = new SoundPlayer();
            _chart = new Chart("testChart", _gameData);


            _gameData.beatTime = ((60 / _gameData.bpm) * 1000);

            //initSong();
            initReceptors();
            initNotes();
            setupBinds();
            _displayText = new Object(100, 5, _gameData);
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

                Thread.Sleep(16);
            }
        }
        public void initNotes()
        {
            for (int i = 0; i < _chart.notes.Count; i++)
            {
                Note n = new Note(_chart.notes[i].time, _chart.notes[i].lane, _chart.notes[i].sustainLength, _gameData);
                _gameData.notes.Add(n);
                objects.Add(n);
            }
                
        }
        public void initReceptors()
        {
            for (int i = 0; i < 8; i++)
            {
                Receptor r = new Receptor(i, _gameData);
                _gameData.receptors.Add(r);
                objects.Add(r);
            }
                
        }
        public void initSong()
        {
            
            _soundPlayer.SoundLocation = System.IO.Directory.GetCurrentDirectory()+"/assets/songs/testSong.wav";
            // _soundPlayer.LoadCompleted
            //string thing = System.IO.Directory.GetCurrentDirectory() + "assets/songs/testSong.wav";
            _soundPlayer.LoadAsync();

            
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
                while (checkIfNoteNeedsAdding && _gameData.notes.Count > 0)
                {
                    Note n = _gameData.notes[0];
                    if (n.time > _gameData.songTime-(_gameData.beatTime*4))
                    {
                        
                        _gameData.renderedNotes.Add(n);
                        _gameData.notes.RemoveAt(0);
                    }
                    else
                    {
                        checkIfNoteNeedsAdding = false;
                    }
                }
            }




            if (startedSong)
            {
                _gameData.songTime += (dt)*1000;
            }
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].update(dt);
            }
            int noteIdx = 0;
            while (noteIdx < _gameData.renderedNotes.Count) //remove notes that need removing
            {
                if (_gameData.renderedNotes[noteIdx].shouldRemove)
                {
                    Note n = _gameData.renderedNotes[noteIdx];
                    if (!n.hitNote)
                        notesMissed++;
                    objects.RemoveAt(objects.IndexOf(n));
                    _gameData.renderedNotes.RemoveAt(noteIdx);
                    n = null;
                }
                else
                    noteIdx++;
            }
            //if (!startedSong && _soundPlayer.IsLoadCompleted)
            //{
            //    _soundPlayer.Play();
                startedSong = true;
            //}

            _displayText.text = "Notes Hit: " + notesHit+ "\nNotes Missed: " + notesMissed;

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

        void setupBinds()
        {
            //fill binds
            Keybinds.Add(Key.D);
            Keybinds.Add(Key.F);
            Keybinds.Add(Key.J);
            Keybinds.Add(Key.K);
            for (int i = 0; i < Keybinds.Count; i++)
            {
                KeysHeld.Add(false);
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
                        notesHit++;
                        n.shouldRemove = true;
                        n.doDraw = false;
                        n.hitNote = true;
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
            }
        }
    }
}
