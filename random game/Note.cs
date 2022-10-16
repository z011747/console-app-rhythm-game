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
        public bool missedNote = false;



        public Note(float time, int lane, float sustainLength, GameData _gameData) : base(0, 0, _gameData)
        {
            this.x = 2 + (lane * _gameData.noteSkinData.spacing); 
            text = _gameData.noteSkinData.noteTexts[lane];
            this.lane = lane;
            this.time = time;
            this.sustainLength = sustainLength;

            //get noteskin color
            BGColor = _gameData.noteSkinData.noteColors[lane];
            if (GameSettings.noteQuants)
                BGColor = _gameData.noteSkinData.getNoteQuantColor(this);
            FGColor = BGColor;

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

            if (missedNote)
            {
                BGColor = ConsoleColor.DarkGray;
                FGColor = BGColor;
            }
        }
        public override void draw()
        {
            base.draw();
            
            if (sustainLength > 0) //long note rendering
            {
                //BGColor = _gameData.noteSkinData.noteColors[lane];
                //FGColor = _gameData.noteSkinData.noteColors[lane];
                Console.BackgroundColor = BGColor; //set colors
                Console.ForegroundColor = FGColor;
                string longNote = "";

                float targetY = getTargetY();
                float scroll = getScrollDirection();

                float longNoteTop = y;
                float longNoteBottom = y;

                int offsetToCenter = (int)Math.Round(getHeight()*0.5); //go into the center of the note

                if (_gameData.downscroll) //on downscroll, top is the end of the long note, bottom is the start of the long note, its the otherway around on upscroll
                {
                    longNoteTop = targetY - (float)(((_gameData.songTime - (time+sustainLength)) * _gameData.scrollSpeed * 0.05) * scroll);
                    longNoteBottom += offsetToCenter;
                    if (longNoteBottom > targetY+ offsetToCenter) //clip to receptor
                        longNoteBottom = targetY+ offsetToCenter;
                }
                else
                {
                    longNoteBottom = targetY - (float)(((_gameData.songTime - (time + sustainLength)) * _gameData.scrollSpeed * 0.05) * scroll);
                    longNoteTop += offsetToCenter;
                    if (longNoteTop < targetY+ offsetToCenter) //clip to receptor
                        longNoteTop = targetY+ offsetToCenter;

                    
                }

                for (int i = (int)Math.Round(longNoteTop); i < longNoteBottom; i++)
                {
                    longNote += _gameData.noteSkinData.longNoteTexts[lane] + '\n';
                }

                string[] lines = longNote.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    int roundedTop = (int)Math.Round(longNoteTop) + i;
                    if (roundedTop >= 0 && roundedTop < Constants.BUFFERHEIGHT)
                    {
                        Console.SetCursorPosition((int)Math.Round(x + offsetX)+ _gameData.noteSkinData.longNoteOffset, roundedTop);
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
            x = getTargetX();
        }

        public float getTargetY()
        {
            if (_gameData.checkLane(lane))
            {
                return _gameData.receptors[lane].y;
            }
            return 0;
        }
        public float getTargetX()
        {
            if (_gameData.checkLane(lane))
            {
                return _gameData.receptors[lane].x;
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
