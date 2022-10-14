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

        public float bpm { get; set; }
        public float beatTime { get; set; }
        
        public float scrollSpeed { get; }

        public bool autoPlay { get; set; }

        public List<Note> notes = new List<Note>();
        public List<Note> renderedNotes = new List<Note>();
        public List<Receptor> receptors = new List<Receptor>();
        public GameData()
        {
            downscroll = true;
            songTime = 0;
            scrollSpeed = 0.9f;
            autoPlay = true;
        }

        public bool checkLane(int lane)
        {
            return lane >= 0 && lane < receptors.Count - 1;
        }
    }
}
