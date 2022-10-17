using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace random_game
{
    class GameSettings
    {
        public static float scrollSpeed = 1.0f;
        public static bool downscroll = true;
        public static bool noteQuants = false;
        public static bool autoPlay = false;
        public static string noteSkin = "Default";

        //not saved
        public static float songSpeed = 1.0f;
        
        public static void loadSettings()
        {
            string path = System.IO.Directory.GetCurrentDirectory() + "saveData.json";
            if (File.Exists(path))
            {
                string saveDataStr = "";
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = sr.ReadToEnd();
                    saveDataStr += s;
                }
                JObject saveDataJson = JObject.Parse(saveDataStr);
            }
        }
    }
}
