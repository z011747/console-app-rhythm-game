using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;

namespace random_game
{
    class BaseSelectionMenu : BaseGameClass
    {
        protected List<string> optionList = new List<string>();
        protected int selectedOption = 0;
        protected List<Object> options = new List<Object>();

        public BaseSelectionMenu() : base()
        {

        }

        //have this in seperate function so the list can be setup first
        public void setupMenu()
        {
            for (int i = 0; i < optionList.Count; i++)
            {
                LerpedObject obj = new LerpedObject(2, 5 + i, null);
                obj.lerpSpeed = 10;
                obj.text = optionList[i];
                objects.Add(obj);
                options.Add(obj);
            }
            onChangeSelection(0);
        }

        //for overriding
        public virtual void onSelect(int selection)
        {

        }

        bool _controlEnter, _controlUp, _controlDown,_controlLeft,_controlRight = true;
        float delay = 0.15f;
        public override void update(float dt)
        {
            base.update(dt);
            if (!WindowsUtil.isWindowFocused())
                return;

            if (delay > 0)
            {
                delay -= dt;
            }

            if (Keyboard.IsKeyDown(Key.Enter) && !_controlEnter && delay < 0) //delay so you dont press in the next menu
                onSelect(selectedOption);
            if (Keyboard.IsKeyDown(Key.Up) && !_controlUp) //only 1 frame
                onChangeSelection(-1);
            if (Keyboard.IsKeyDown(Key.Down) && !_controlDown) //only 1 frame
                onChangeSelection(1);

            if (Keyboard.IsKeyDown(Key.Left) && !_controlLeft) //only 1 frame
                onSideSelection(-1);
            if (Keyboard.IsKeyDown(Key.Right) && !_controlRight) //only 1 frame
                onSideSelection(1);

            _controlEnter = Keyboard.IsKeyDown(Key.Enter);
            _controlUp = Keyboard.IsKeyDown(Key.Up);
            _controlDown = Keyboard.IsKeyDown(Key.Down);
            _controlLeft = Keyboard.IsKeyDown(Key.Left);
            _controlRight = Keyboard.IsKeyDown(Key.Right);
        }

        public virtual void onChangeSelection(int change)
        {
            selectedOption += change;
            if (selectedOption < 0)
                selectedOption = optionList.Count - 1; //-1 because its an index
            else if (selectedOption >= optionList.Count)
                selectedOption = 0;
            regenerateMenu();
        }

        public virtual void onSideSelection(int change)
        {
            
            regenerateMenu();
        }

        public virtual void regenerateMenu()
        {
            for (int i = 0; i < optionList.Count; i++)
            {
                Object obj = options[i];
                obj.text = optionList[i];
                obj.x = 2;
                if (i == selectedOption)
                {
                    obj.text += " <-----";
                    obj.x = 5;
                }
                


                //scroll down
                if (selectedOption + 10 > Constants.BUFFERHEIGHT)
                {
                    obj.y = ((5 + i) - (selectedOption + 10)) + Constants.BUFFERHEIGHT;
                }
                else
                {
                    obj.y = 5 + i;
                }
            }
        }
    }
}
