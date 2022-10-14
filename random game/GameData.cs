using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace random_game
{
    class GameData
    {
        public float songTime { get; set; }
        public bool downscroll { get; }


        
        public float scrollSpeed { get; }

        public bool autoPlay { get; set; }
        public int keyCount { get; set; }
        public string songName { get; set; }
        public float bpm { get; set; }
        public float beatTime { get; set; }

        public float songSpeed { get; set; }

        public List<Note> notes = new List<Note>();
        public List<Note> renderedNotes = new List<Note>();
        public List<Receptor> receptors = new List<Receptor>();
        public GameData()
        {
            downscroll = true;
            songSpeed = 1.0f;
            songTime = -1000*Math.Abs(songSpeed); //1 sec before song start
            keyCount = 4;
            scrollSpeed = 1.1f/songSpeed;
            
            autoPlay = false;
        }

        public bool checkLane(int lane)
        {
            return lane >= 0 && lane < receptors.Count;
        }
    }
}
