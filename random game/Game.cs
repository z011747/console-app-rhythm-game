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
        Stopwatch sw = new Stopwatch();
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
            initNotes();

            bool running = true;
            while (running)
            {
                //update loop
                sw.Reset();
                sw.Start();
                Thread.Sleep(16);
                sw.Stop();
                update(sw.ElapsedMilliseconds);
                draw();
            }
        }
        public void initNotes()
        {
            for (int i = 0; i < _chart.notes.Count; i++)
                objects.Add(new Note(_chart.notes[i].time, _chart.notes[i].lane, _gameData));
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

            if (!startedSong && _soundPlayer.IsLoadCompleted)
            {
                _soundPlayer.Play();
                startedSong = true;
            }


            if (startedSong)
            {
                _gameData.songTime += dt*3;
            }
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].update(dt);
            }
            //objects[0].y = (float)(5 + (Math.Cos(timeElapsed*0.02) * 3));
            //objects[0].x = (float)(5 + (Math.Sin(timeElapsed * 0.01) * 3));


            if (Console.KeyAvailable)
            {
                var k = Console.ReadKey();
                switch (k.Key)
                {

                }
            }
        }

        void draw()
        {
            //Console.Clear();
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < 30; i++)
            {
                //Console.WriteLine("\n");
                for (int j = 0; j < 100; j++)
                {
                    Console.SetCursorPosition(j, i);
                    Console.Write(" ");
                }
            }
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].draw();
            }
            Console.SetCursorPosition(0, 0);
        }
    }
}
