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
            Console.CursorVisible = false;
            Console.Title = "Simple Console App Rhythm Game";
            
            GameSettings.loadSettings();
            new MainMenu(); //load main menu
        }
    }
}
