using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Windows.Input;

namespace random_game
{
    public class Game
    {
        Stopwatch sw = new Stopwatch();
        List<Object> objects = new List<Object>();
        public Game()
        {
            objects.Add(new Object(2, 2));
            bool running = true;
            while (running)
            {
                sw.Reset();
                sw.Start();
                Thread.Sleep(17);
                sw.Stop();
                update(sw.ElapsedMilliseconds);
                draw();
            }

        }
        void update(float dt)
        {
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
            Console.Clear();
            for (int i = 0; i < 50; i++)
            {
                //Console.WriteLine("");
            }
            for (int i = 0; i < objects.Count; i++)
            {

                Console.SetCursorPosition((int)Math.Round(objects[i].x), (int)Math.Round(objects[i].y));
                Console.Write("#");
            }
        }
    }
}
