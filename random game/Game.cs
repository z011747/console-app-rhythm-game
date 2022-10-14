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
        public Game()
        {
            _gameData = new GameData();
            _soundPlayer = new SoundPlayer();
            _chart = new Chart("testChart", _gameData);


            _gameData.beatTime = ((60 / _gameData.bpm) * 1000);

            initSong();
            initReceptors();
            initNotes();

            DateTime _previousGameTime = DateTime.Now;

            bool running = true;
            while (running)
            {
                //update loop
                TimeSpan GameTime = DateTime.Now - _previousGameTime;
                _previousGameTime = _previousGameTime + GameTime;

                //sw.Stop();
                update((float)(GameTime.TotalSeconds));
                draw();

                Thread.Sleep(8);
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




            if (startedSong)
            {
                _gameData.songTime += (dt)*1000;
            }
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].update(dt);
            }
            int noteIdx = 0;
            while (noteIdx < _gameData.notes.Count)
            {
                if (_gameData.notes[noteIdx].shouldRemove)
                {
                    Note n = _gameData.notes[noteIdx];
                    objects.RemoveAt(objects.IndexOf(n));
                    _gameData.notes.RemoveAt(noteIdx);
                    n = null;
                }
                else
                    noteIdx++;
            }
            //objects[0].y = (float)(5 + (Math.Cos(timeElapsed*0.02) * 3));
            //objects[0].x = (float)(5 + (Math.Sin(timeElapsed * 0.01) * 3));
            if (!startedSong && _soundPlayer.IsLoadCompleted)
            {
                _soundPlayer.Play();
                startedSong = true;
            }


            if (Console.KeyAvailable)
            {
                var k = Console.ReadKey();
                switch (k.Key)
                {

                }
            }
        }
        string fillInSpace = "";
        void draw()
        {
            if (fillInSpace == "")
            {
                for (int i = 0; i < 30; i++)
                {
                    fillInSpace += "                                                                                      \n";
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
    }
}
