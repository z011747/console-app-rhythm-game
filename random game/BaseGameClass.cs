using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace random_game
{
    class BaseGameClass
    {
        public List<Object> objects = new List<Object>();
        public bool running = true;
        public bool drawing = true;
        static Thread drawThread = null;
        public BaseGameClass()
        {
        }
        public void startUpdateLoop()
        {
            //startDrawThread();
            DateTime _previousGameTime = DateTime.Now;

            if (drawThread != null)
            {
                drawThread.Abort();
            }
            drawThread = new Thread(drawingOnThread);
            drawThread.Start();

            while (running)
            {
                //update loop
                TimeSpan GameTime = DateTime.Now - _previousGameTime;
                _previousGameTime = _previousGameTime + GameTime;

                //sw.Stop();
                //input();
                update((float)(GameTime.TotalSeconds));
                //draw();

                Thread.Sleep(1);
            }
        }
        public void drawingOnThread()
        {
            while (drawing)
            {
                draw();
                Thread.Sleep(1);
            }
        }
        public virtual void update(float dt)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].update(dt);
            }
        }
        static string fillInSpace = "";
        public virtual void draw()
        {
            if (!running)
                return; 

            if (fillInSpace == "")
            {
                for (int i = 0; i < Constants.BUFFERHEIGHT; i++)
                {

                    fillInSpace += "                                                                                                                       \n";
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




        public virtual void changeRoom(BaseGameClass room)
        {
            drawing = false;
            running = false;
            while (objects.Count > 0)
            {
                objects[0] = null;
                objects.RemoveAt(0);
            }
        }
    }
}
