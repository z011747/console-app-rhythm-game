using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace random_game
{
    internal class Program
    {
        [STAThread] //fix isKeyDown from crashing
        static void Main(string[] args)
        {
            //Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.CursorVisible = false;
            Console.Title = "Test Game";
            //Console.SetBufferSize(1280, 720);
            //Console.LargestWindowWidth = 1280;
            //Console.LargestWindowHeight = 720;
            //Console.SetWindowSize(Console.WindowWidth, Console.WindowHeight);

            new Game();
        }
    }
}
