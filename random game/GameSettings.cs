using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Input;
using System.Reflection;
namespace random_game
{
    class SettingsJson
    {
        public float scrollSpeed;
        public bool downscroll;
        public bool noteQuants;
        public bool autoPlay;
        public string noteSkin;
        public SettingsJson() { }
    }
    class GameSettings
    {
        public static float scrollSpeed = 1.0f;
        public static bool downscroll = true;
        public static bool noteQuants = false;
        public static bool autoPlay = false;
        public static string noteSkin = "Default";


        public static List<List<Key>> keyBinds = new List<List<Key>>();
        /*{
            new Key[]{ Key.Space}.ToList(),
            new Key[]{ Key.D, Key.K}.ToList(),
            new Key[]{ Key.D, Key.Space, Key.K}.ToList(),
            new Key[]{ Key.D, Key.F, Key.J, Key.K}.ToList(),
            new Key[]{ Key.D, Key.F, Key.Space, Key.J, Key.K}.ToList(),
            new Key[]{ Key.S, Key.D, Key.F, Key.J, Key.K, Key.L}.ToList(),
            new Key[]{ Key.S, Key.D, Key.F, Key.Space, Key.J, Key.K, Key.L}.ToList(),
            new Key[]{ Key.A, Key.S, Key.D, Key.F, Key.H, Key.J, Key.K, Key.L}.ToList(),
            new Key[]{ Key.A, Key.S, Key.D, Key.F, Key.Space, Key.H, Key.J, Key.K, Key.L}.ToList()
        };*/


        //not saved
        public static float songSpeed = 1.0f;

        public static void loadSettings()
        {
            string path = System.IO.Directory.GetCurrentDirectory() + "/saveData.json";
            if (File.Exists(path))
            {
                string saveDataStr = "";
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = sr.ReadToEnd();
                    saveDataStr += s;
                }
                SettingsJson saveDataJson = JsonConvert.DeserializeObject<SettingsJson>(saveDataStr);
                scrollSpeed = saveDataJson.scrollSpeed;
                downscroll = saveDataJson.downscroll;
                noteQuants = saveDataJson.noteQuants;
                autoPlay = saveDataJson.autoPlay;
                noteSkin = saveDataJson.noteSkin;
            }

            string keyBindPath = System.IO.Directory.GetCurrentDirectory() + "/keyBinds.txt";
            if (!File.Exists(keyBindPath))
            {
                keyBindPath = System.IO.Directory.GetCurrentDirectory() + "/defaultBinds.txt";
            }

            if (File.Exists(keyBindPath))
            {
                string keybindsStr = "";
                using (StreamReader sr = File.OpenText(keyBindPath))
                {
                    string s = sr.ReadToEnd();
                    keybindsStr += s;
                }
                string[] bindsArrayStr = keybindsStr.Split(';');
                int keyCount = 1;
                foreach (string keyCountStr in bindsArrayStr)
                {
                    keyCount++;
                    string[] keysStr = keyCountStr.Split(':');
                    for (int i = 0; i < keysStr.Length; i++)
                    {
                        Key key = (Key)Enum.Parse(typeof(Key), keysStr[i]);
                    
                    }
                }

            }
        }

        public static void saveSettings()
        {
            string path = System.IO.Directory.GetCurrentDirectory() + "/saveData.json";
            SettingsJson saveDataJson = new SettingsJson();
            saveDataJson.scrollSpeed = scrollSpeed;
            saveDataJson.downscroll = downscroll;
            saveDataJson.noteQuants = noteQuants;
            saveDataJson.autoPlay = autoPlay;
            saveDataJson.noteSkin = noteSkin;
            string jsonString = JsonConvert.SerializeObject(saveDataJson);
            File.WriteAllText(path, jsonString);
        }
    }
}
