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
using System.IO;

namespace random_game
{
    class Game : BaseGameClass
    {
        private int notesHit = 0;
        private int notesMissed = 0;
        private int combo = 0;
        private float accuracy = 0.0f;

        private int perfects = 0;
        private int greats = 0;
        private int oks = 0;
        private int bads = 0;

        private MediaPlayer _mediaPlayer;
        public GameData _gameData;
        private Chart _chart;
        private Object _displayText;
        private Object _FPSDisplay;
        private Object _ratingDisplay;
        private BarObject _healthBar;


        private int health = 50;
        private bool failedSong = false;
        public bool paused = false;

        public static Game instance; //i know these arent great but i need it for lua

        private string chartName;

        public Object _debugText; //used for testing in lua

        public Game(string songName, string chartName) : base()
        {
            instance = this;

            

            string audioName = getAudioName(songName);
            this.chartName = chartName; //need song restarts


            _gameData = new GameData(songName, audioName);
            _debugText = new Object(0, 5, _gameData);
            objects.Add(_debugText);
            _debugText.text = "";
            _mediaPlayer = new MediaPlayer();
            
            _chart = new Chart(_gameData.getSongPath()+"/"+chartName, _gameData);
            try
            {
                _gameData.noteSkinData = new NoteSkinData(_gameData, GameSettings.noteSkin);
            }
            catch(Exception e)
            {
                Constants.errorPopup("Error loading Noteskin.");
                _gameData.noteSkinData = new NoteSkinData(_gameData, "Default");
                GameSettings.noteSkin = "Default";
            }
            
            _gameData.recalculateBeats();
            


            initSong();
            initReceptors();
            initNotes();
            _gameData.script = new LuaScript(_gameData.getSongPath() + "/script.lua", _gameData); //do after notes/receptors are made
            setupBinds();
            _displayText = new Object(_gameData.receptors[_gameData.keyCount-1].x+10, 5, _gameData);
            objects.Add(_displayText);

            _FPSDisplay = new Object(0,0, _gameData);
            objects.Add(_FPSDisplay);

            _ratingDisplay = new Object(0, (_gameData.downscroll ? 29 : 0), _gameData);
            objects.Add(_ratingDisplay);
            _ratingDisplay.text = "";

            _healthBar = new BarObject(_displayText.x, _displayText.y + 10, _gameData, 0, 100, 20);
            objects.Add(_healthBar);

            startUpdateLoop(); //run after everything else
        }

        string getAudioName(string songName)
        {
            string path = System.IO.Directory.GetCurrentDirectory() + "/songs/" + songName + "/info.txt";
            string audioName = "";
            using (StreamReader sr = File.OpenText(path))
            {
                string s;
                //read lines
                while ((s = sr.ReadLine()) != null)
                {
                    string[] line = s.Split(':');

                    switch (line[0])
                    {
                        case "audio":
                            audioName =  line[1];
                            break;
                    }
                }
            }
            return audioName;
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
            
            Uri audioPath = new Uri(_gameData.getAudioFilePath());
            _mediaPlayer.Open(audioPath);
            _mediaPlayer.SpeedRatio = _gameData.songSpeed;
            _mediaPlayer.Volume = 0;
            //_mediaPlayer.Position = TimeSpan.Zero;
        }


        float timeElapsed = 0;
        bool startedSong = false;
        public override void update(float dt)
        {
            timeElapsed += dt;
            _FPSDisplay.text = (int)(1000/(dt*1000))+" FPS";
            if (!paused)
            {
                input(); //run input code
                CheckIfNotesNeedAdding();
                _gameData.script.update(dt);

                _gameData.songTime += (dt) * 1000 * _gameData.songSpeed; //increase song time (in milliseconds), seperate from audio time to match for song speed
                base.update(dt); //update objects/notes
                if (_gameData.autoPlay)
                    autoPlayNotes(); //auto hit notes

                CheckIfNotesNeedRemoving(); //remove any notes when needed

                if (!startedSong && _gameData.songTime > 0 && _mediaPlayer.Position > TimeSpan.Zero) //start song
                {
                    _mediaPlayer.Volume = 1;
                    _mediaPlayer.Position = TimeSpan.Zero;
                    startedSong = true;
                    _gameData.songTime = 0;
                }
                updateDisplayText();
                checkForSongEnd();
            }
            if (Keyboard.IsKeyDown(Key.Escape))
            {
                pause();
            }
            checkforDesync();

        }

        private List<Key> Keybinds = new List<Key>();
        private List<bool> KeysHeld = new List<bool>();
        private List<float> LongNoteTimes = new List<float>();
        private List<Note> HeldNL = new List<Note>();

        void setupBinds()
        {
            //fill binds
            for (int i = 0; i < GameSettings.keyBinds[_gameData.keyCount-1].Count; i++)
            {
                Keybinds.Add(GameSettings.keyBinds[_gameData.keyCount - 1][i]);
            }

            for (int i = 0; i < Keybinds.Count; i++)
            {
                KeysHeld.Add(false);
                LongNoteTimes.Add(-10000); //do -10000 just in case
                HeldNL.Add(null);
            }
        }

        public void input()
        {
            if (!WindowsUtil.isWindowFocused())
                return;

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
                _gameData.receptors[lane].text = _gameData.noteSkinData.pressedNoteTexts[lane];
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
                        _gameData.receptors[lane].text = _gameData.noteSkinData.noteTexts[lane];
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
                _gameData.receptors[lane].text = _gameData.noteSkinData.noteTexts[lane];
                _gameData.receptors[lane].BGColor = _gameData.noteSkinData.receptorColors[lane];
                _gameData.receptors[lane].FGColor = _gameData.receptors[lane].BGColor;
                if (_gameData.songTime < LongNoteTimes[lane]-Constants.EARLYHITTIMING && startedSong)
                {
                    notesMissed++;
                    combo = 0;
                    addHealth(-15);
                    updateAccuracy();
                    if (HeldNL[lane] != null)
                        HeldNL[lane].missedNote = true;
                }
                LongNoteTimes[lane] = 0;
                HeldNL[lane] = null;
            }
        }

        void onHitNote(Note n)
        {
            notesHit++;
            combo++;
            if (n.sustainLength == 0) //regular notes
                n.shouldRemove = true;
            else
            {
                LongNoteTimes[n.lane] = n.getTime() + n.sustainLength;
                HeldNL[n.lane] = n;
            }
                
            n.doDraw = false;
            n.hitNote = true;
            _gameData.receptors[n.lane].BGColor = n.BGColor;
            _gameData.receptors[n.lane].FGColor = _gameData.receptors[n.lane].BGColor;

            checkNoteRating(n);
        }


        void checkNoteRating(Note n)
        {
            float msDiff = n.getTime() - _gameData.songTime;
            int[] timings = { Constants.PERFECTTIMING, Constants.GREATTIMING, Constants.OKTIMING };
            int ratingID = 3; //bad
            for (int i = 0; i < timings.Length; i++)
            {
                if (Math.Abs(msDiff) < timings[i])
                {
                    ratingID = i;
                    break;
                }
            }
            switch(ratingID)
            {
                case 0: //perfect
                    perfects++;
                    _ratingDisplay.text = "Perfect";
                    _ratingDisplay.FGColor = ConsoleColor.Cyan;
                    addHealth(5);
                    break;
                case 1: //great 
                    greats++;
                    _ratingDisplay.text = "Great";
                    _ratingDisplay.FGColor = ConsoleColor.Green;
                    addHealth(2);
                    break;
                case 2: //ok
                    oks++;
                    _ratingDisplay.text = "Ok";
                    _ratingDisplay.FGColor = ConsoleColor.DarkGreen;
                    break;
                default: //bad
                    bads++;
                    _ratingDisplay.text = "Bad";
                    _ratingDisplay.FGColor = ConsoleColor.DarkRed;
                    break;
            }
            _ratingDisplay.text += " " + combo + " "+ Math.Round(msDiff) + "ms";
            _ratingDisplay.x = (float)((Constants.BUFFERWIDTH * 0.5) - (_ratingDisplay.getWidth() * 0.5)-(_gameData.noteSkinData.spacing*0.5));


            updateAccuracy();
        }
        void updateAccuracy()
        {
            accuracy = (float)((perfects + (greats * 0.85) + (oks * 0.65) + (bads * 0.35)) / (notesHit + notesMissed));
            accuracy *= 10000;
            accuracy = (float)Math.Round(accuracy);
            accuracy = (float)(accuracy * 0.01);
        }




        void updateDisplayText()
        {
            _displayText.text = _gameData.songName;
            if (_gameData.songSpeed != 1.0f)
                _displayText.text += " (" + _gameData.songSpeed + "x)"; //speed multiplier display
            _displayText.text += "\nAccuracy: " + accuracy+"%\nPerfect: " +perfects + "\nGreat: " + greats + "\nOk: " + oks + "\nBad: " + bads + "\nMisses: " + notesMissed;
            if (startedSong)
            {
                TimeSpan currentTime = TimeSpan.FromMilliseconds(_gameData.songTime);
                if (currentTime > _mediaPlayer.NaturalDuration.TimeSpan)
                    currentTime = _mediaPlayer.NaturalDuration.TimeSpan; //if song time is above max then it should just be max

                //_songTimeBar.maxValue = (float)_mediaPlayer.NaturalDuration.TimeSpan.TotalMilliseconds;
                //_songTimeBar.updateBarValue((float)currentTime.TotalMilliseconds);

                _displayText.text += "\n" + ((float)currentTime.Minutes) + ":" + //current minutes
                    (((float)currentTime.Seconds) < 10 ? "0" : "") + //add a 0 in front to make it look better
                    ((float)currentTime.Seconds) + " / " + //current seconds

                    ((float)_mediaPlayer.NaturalDuration.TimeSpan.Minutes) + ":" + //total time minutes
                    (((float)_mediaPlayer.NaturalDuration.TimeSpan.Seconds) < 10 ? "0" : "") +
                    ((float)_mediaPlayer.NaturalDuration.TimeSpan.Seconds); //total time seconds
            }
            _displayText.text += "\nHP: ";
            _healthBar.x = _displayText.x + 4;
            _healthBar.y = _displayText.y + _displayText.getHeight()-1;
            _healthBar.updateBarValue(health);
            if (_gameData.autoPlay)
                _displayText.text += "\nAUTOPLAY";
            if (failedSong)
                _displayText.text += "\nSONG FAILED";


        }

        void addHealth(int change)
        {

            health += change;
            health = MathUtil.bound(health, -1, 100);
            if (health < 0)
            {
                //die
                failedSong = true;
            }
            if (failedSong)
                health = 0;
        }




        void CheckIfNotesNeedRemoving()
        {
            int noteIdx = 0;
            while (noteIdx < _gameData.renderedNotes.Count) //remove notes that need removing
            {
                if (_gameData.renderedNotes[noteIdx].shouldRemove)
                {
                    Note n = _gameData.renderedNotes[noteIdx];
                    if (!n.hitNote && n.lane < _gameData.keyCount) //stop broken notes from causing misses
                    {
                        notesMissed++;
                        combo = 0;
                        addHealth(-15);
                        updateAccuracy();
                    }
                        
                    objects.RemoveAt(objects.IndexOf(n));
                    _gameData.renderedNotes.RemoveAt(noteIdx);
                    n = null; //no longer needed
                }
                else
                    noteIdx++;
            }
        }


        void CheckIfNotesNeedAdding()
        {
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
                    if (n.getTime() < _gameData.songTime + ((_gameData.beatTime * 4) / _gameData.scrollSpeed))
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
        }



        void autoPlayNotes()
        {
            for (int i = 0; i < _gameData.renderedNotes.Count; i++)
            {
                Note n = _gameData.renderedNotes[i];
                if (_gameData.songTime > n.getTime() && !n.hitNote)
                {
                    onHitNote(n);
                    _gameData.receptors[n.lane].autoPlayReset = 0.1f + (n.sustainLength * 0.001f / _gameData.songSpeed);
                }
            }
        }


        void checkForSongEnd()
        {
            if (startedSong)
            {
                if (_gameData.songTime > _mediaPlayer.NaturalDuration.TimeSpan.TotalMilliseconds + 1000)
                {
                    changeRoom(new MainMenu());
                }
            }
        }


        public override void changeRoom(BaseGameClass room)
        {
            _gameData.script.killScript();
            base.changeRoom(room);
        }
        private PauseMenu _pauseMenu;
        private float timeDiff = -int.MaxValue;
        public void pause()
        {
            paused = true;
            if (startedSong)
            {
                if (timeDiff == -int.MaxValue)
                    timeDiff = (float)_mediaPlayer.Position.TotalMilliseconds- _gameData.songTime;
            }
            _mediaPlayer.Pause();
            _pauseMenu = new PauseMenu(this);
            
        }
        public void unpause()
        {
            if (startedSong)
            {

                //_mediaPlayer.Position = TimeSpan.FromMilliseconds(_gameData.songTime+ timeDiff); //automatically unpauses
                _gameData.songTime = (float)_mediaPlayer.Position.TotalMilliseconds;
                _mediaPlayer.Play();

            }
            resetDrawThread();
            paused = false;
        }

        public void checkforDesync()
        {
            float diff = (float)Math.Abs(_mediaPlayer.Position.TotalMilliseconds - _gameData.songTime);
            if (diff > 100)
            {
                _gameData.songTime = (float)_mediaPlayer.Position.TotalMilliseconds;
            }
        }

        public void restartSong()
        {
            changeRoom(new Game(_gameData.songName, chartName));
        }
    }
}
