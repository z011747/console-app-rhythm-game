using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace random_game
{
    class GameData
    {
        public float songTime { get; set; }
        public bool downscroll { get; private set; }        
        public float scrollSpeed { get; private set; }

        public bool autoPlay { get; private set; }
        public int keyCount { get; set; }
        public string songName { get; private set; }
        public float bpm { get; set; }
        public float beatTime { get; private set; }

        public float songSpeed { get; private set; }
        private string audioName;

        public NoteSkinData noteSkinData { get; set; }

        public List<Note> notes = new List<Note>();
        public List<Note> renderedNotes = new List<Note>();
        public List<Receptor> receptors = new List<Receptor>();
        public GameData(string songName, string audioName)
        {
            this.songName = songName;
            this.audioName = audioName;
            downscroll = GameSettings.downscroll;
            songSpeed = 1.0f;
            songTime = -1000*Math.Abs(songSpeed); //1 sec before song start
            keyCount = 4;
            scrollSpeed = GameSettings.scrollSpeed/songSpeed;
            autoPlay = GameSettings.autoPlay;
        }
        public void recalculateBeats()
        {
           beatTime = ((60 / bpm) * 1000); //bpm is from the chart so need to do after loading chart
        }
        public void changeScrollSpeed(float newSpeed)
        {
            scrollSpeed = newSpeed;
        }

        public bool checkLane(int lane)
        {
            return lane >= 0 && lane < receptors.Count;
        }

        public string getSongPath()
        {
            return System.IO.Directory.GetCurrentDirectory() + "/songs/" + songName;
        }
        public string getAudioFilePath()
        {
            return getSongPath() + "/" + audioName;
        }
    }
}
