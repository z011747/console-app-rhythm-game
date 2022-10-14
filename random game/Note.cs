using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace random_game
{
    class Note : Object
    {
        public float time { get; set; }
        public int lane { get; }
        public float sustainLength { get; set; }

        public bool shouldRemove = false;

        public bool hitNote = false;
        public bool canHitNote = false;

        public Note(float time, int lane, float sustainLength, GameData _gameData) : base(0, 0, _gameData)
        {
            this.x = 2 + (lane * 8);
            text = Constants.NOTETEXT;
            this.lane = lane;
            this.time = time;
            this.sustainLength = sustainLength;
            BGColor = ConsoleColor.Gray;
            FGColor = ConsoleColor.Gray;
        }

        public override void update(float dt)
        {
            base.update(dt);
            updatePosition();
            //if (_gameData.songTime > time)
            //doDraw = false;

            canHitNote = false;
            if (_gameData.songTime < time + Constants.EARLYHITTIMING &&
                _gameData.songTime > time - Constants.LATEHITTIMING)
            {
                canHitNote = true;
            }

            if (_gameData.songTime > time+_gameData.beatTime+sustainLength)
                shouldRemove = true;
        }
        public override void draw()
        {
            base.draw();
            
            if (sustainLength > 0) //long note rendering
            {
                Console.BackgroundColor = BGColor;
                Console.ForegroundColor = FGColor;
                string longNote = "";

                float targetY = getTargetY();
                float scroll = getScrollDirection();

                float longNoteTop = y;
                float longNoteBottom = y;

                if (_gameData.downscroll)
                {
                    longNoteTop = targetY - (float)(((_gameData.songTime - (time+sustainLength)) * _gameData.scrollSpeed * 0.05) * scroll);
                    if (longNoteBottom > targetY)
                        longNoteBottom = targetY;
                }
                else
                {
                    longNoteBottom = targetY - (float)(((_gameData.songTime - (time + sustainLength)) * _gameData.scrollSpeed * 0.05) * scroll);
                    longNoteTop += 4;
                    if (longNoteTop < targetY+4)
                        longNoteTop = targetY+4;
                }

                for (int i = (int)Math.Round(longNoteTop); i < longNoteBottom; i++)
                {
                    longNote += Constants.LONGNOTETEXT+'\n';
                }
                


                string[] lines = longNote.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    if (longNoteTop+i >= 0 && longNoteTop+i < 30)
                    {
                        Console.SetCursorPosition((int)Math.Round(x + offsetX)+2, (int)Math.Round(longNoteTop) + i);
                        Console.Write(lines[i]); //make sure it goes to next line properly
                    }
                }
            }
        }

        public void updatePosition()
        {
            float targetY = getTargetY();
            float scroll = getScrollDirection();

            y = targetY - (float)(((_gameData.songTime - time) * _gameData.scrollSpeed * 0.05)*scroll);
        }

        public float getTargetY()
        {
            if (_gameData.checkLane(lane))
            {
                return _gameData.receptors[lane].y;
            }
            return 0;
        }
        public float getScrollDirection()
        {
            float scroll = 1;
            if (_gameData.downscroll)
                scroll *= -1;
            return scroll;
        }
    }
}
