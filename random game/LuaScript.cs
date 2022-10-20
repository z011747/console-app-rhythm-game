using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NLua;
using System.Windows.Forms;
using System.Reflection;
using Betwixt;

namespace random_game
{
    class LuaScript
    {
        Lua state;
        public bool running = true;
        GameData _gameData;
        //public Tweener<T> tweener;
        public List<LuaTween> tweens = new List<LuaTween>();
        public LuaScript(string path, GameData _gameData)
        {
            state = new Lua();
            this._gameData = _gameData;
            if (!File.Exists(path))
            {
                running = false;
                killScript();
                return;
            }
            try
            {
                var res = state.DoFile(path);
                initLua();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                running = false;
                killScript();
            }
        }
        
        void initLua()
        {
            //for (int i = 0; i < _gameData.receptors.Count; i++)
            //{
            //state["receptor" + i] = _gameData.receptors[i];
            //state["receptorX" + i] = _gameData.receptors[i].x;
            //state["receptorY" + i] = _gameData.receptors[i].y;
            //}
            /*string[] funcsToAdd = {
                "setReceptorX",
                "setReceptorY",
                "getReceptorX",
                "getReceptorY",
                "getSongTime",
                "getDefaultScrollSpeed",
                "getDefaultDownscroll",
                "setScrollSpeed",
                "setDownscroll",
                "centerScreen",
                "setWindowPos"
            };
            foreach (string funcStr in funcsToAdd)
            {
                MethodBase funcToAddToLua = typeof(LuaScript).GetMethod(funcStr);
                state.RegisterFunction(funcStr, funcToAddToLua);
            }*/

            //tweenManager = new Tweener();

            

            foreach (var func in typeof(LuaScriptFunctions).GetMethods()) //auto load all functions
            {
                MethodBase funcToAddToLua = func;
                state.RegisterFunction(func.Name, funcToAddToLua);
            }

            call("create", new string[] { });

        }


        public float currentBeat = 0;
        public int flooredBeat = 0;
        int lastFlooredBeatCalled = -1;
        
        public void update(float dt)
        {
            if (!running)
                return;

            //tweenManager.Update(dt);
            if (tweens.Count > 0)
            {
                for(int i = tweens.Count-1; i >= 0; i--)
                {
                    LuaTween t = tweens[i];
                    t.updateValue(dt);
                    if (t.shouldRemove)
                        tweens.Remove(t);
                }
                
            }


            currentBeat = (float)((_gameData.songTime+_gameData.songOffset) / _gameData.beatTime);
            flooredBeat = (int)Math.Floor(currentBeat);
            if (lastFlooredBeatCalled != flooredBeat)
            {
                lastFlooredBeatCalled = flooredBeat;
                call("onBeat" + flooredBeat, new string[] { });
                call("onBeat", new string[] { });
            }

            call("update", new string[] { dt + "" });
        }
        public void call(string func, dynamic args)
        {
            if (!running)
                return;
            try
            {
                var luaFunc = state[func] as LuaFunction;
                if (luaFunc != null)
                {
                    var res = luaFunc.Call(args);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                running = false;
            }
        }
        public void killScript()
        {
            state.Close();
            state.Dispose();
            state = null;
        }
    }


    class LuaScriptFunctions
    {
        static public int getBeat()
        {
            return Game.instance._gameData.script.flooredBeat;
        }

        static public void setDebugText(string text)
        {
            Game.instance._debugText.text = text;
        }
        static public int getKeyCount()
        {
            return Game.instance._gameData.keyCount;
        }
        static public void setReceptorX(int i, float pos)
        {
            Game.instance._gameData.receptors[i].x = pos;
        }
        static public void setReceptorY(int i, float pos)
        {
            Game.instance._gameData.receptors[i].y = pos;
        }
        static public float getReceptorX(int i)
        {
            return Game.instance._gameData.receptors[i].x;
        }
        static public float getReceptorY(int i)
        {
            return Game.instance._gameData.receptors[i].y;
        }
        static public float getSongTime()
        {
            return Game.instance._gameData.songTime;
        }
        static public float getDefaultScrollSpeed()
        {
            return GameSettings.scrollSpeed;
        }
        static public bool getDefaultDownscroll()
        {
            return GameSettings.downscroll;
        }
        static public void setScrollSpeed(float a)
        {
            Game.instance._gameData.scrollSpeed = a;
        }
        static public void setDownscroll(bool a)
        {
            Game.instance._gameData.downscroll = a;
        }
        static public void centerScreen()
        {
            WindowsUtil.centerScreen();
        }
        static public void setWindowPos(int x, int y)
        {
            WindowsUtil.setWindowPos(x, y);
        }
        static public void setWindowAlpha(float alpha)
        {
            WindowsUtil.setWindowAlpha(alpha);
        }
        static public float getBeatTime()
        {
            return Game.instance._gameData.beatTime;
        }
        static public float getBpm()
        {
            return Game.instance._gameData.bpm;
        }

        static public int getScreenResolutionWidth()
        {
            return WindowsUtil.getScreenResolutionWidth();
        }
        static public int getScreenResolutionHeight()
        {
            return WindowsUtil.getScreenResolutionHeight();
        }
        static public int getWindowWidth()
        {
            return WindowsUtil.getWindowWidth();
        }
        static public int getWindowHeight()
        {
            return WindowsUtil.getWindowHeight();
        }

        static public int getWindowX()
        {
            return WindowsUtil.windowX;
        }
        static public int getWindowY()
        {
            return WindowsUtil.windowY;
        }

        static public void tweenReceptorX(int i, float pos, float time, string ease)
        {
            Object receptor = Game.instance._gameData.receptors[i];
            //Game.instance._gameData.script.tweenManager.Tween(receptor, new { x = pos }, time).Ease(getEaseFromString(ease));
            Tweener<float> tween = new Tweener<float>(receptor.x, pos, time, Ease.Cubic.InOut);
            PropertyInfo prop = receptor.GetType().GetProperty("x");
            LuaTween luaTween = new LuaTween(tween, prop, receptor);
            Game.instance._gameData.script.tweens.Add(luaTween);

        }
        static public void tweenReceptorY(int i, float pos, float time, string ease)
        {
            Object receptor = Game.instance._gameData.receptors[i];
            //Game.instance._gameData.script.tweenManager.Tween(receptor, new { y = pos }, time).Ease(getEaseFromString(ease));
        }



        static void addTween(Tweener<float> tween)
        {
            //Game.instance._gameData.script.tweens.Add(tween);
        }

        /*static Func<float, float> getEaseFromString(string ease) //seems like the best way to do this
        {
            switch (ease.ToLower())
            {
                case "backin": return Ease.BackIn; break;
                case "backinout": return Ease.BackInOut; break;
                case "backout": return Ease.BackOut; break;
                case "bouncein": return Ease.BounceIn; break;
                case "bounceinout": return Ease.BounceInOut; break;
                case "bounceout": return Ease.BounceOut; break;

                case "circin": return Ease.CircIn; break;
                case "circinout": return Ease.CircInOut; break;
                case "circout": return Ease.CircOut; break;

                case "cubein": return Ease.CubeIn; break;
                case "cubeinout": return Ease.CubeInOut; break;
                case "cubeout": return Ease.CubeOut; break;

            }

            return Ease.BackIn;
        }*/
    }
}


class LuaTween
{
    Tweener<float> tween;
    dynamic instance;
    PropertyInfo property;
    public bool shouldRemove = false;
    public LuaTween(Tweener<float> tween, PropertyInfo property, dynamic instance)
    {
        this.tween = tween;
        this.property = property;
        this.instance = instance;
    }
    public void updateValue(float dt)
    {
        if (tween.Running)
        {
            tween.Update(dt); //update tween
            property.SetValue(instance, tween.Value); //use reflection to set the value
        }
        else
        {
            shouldRemove = true; //remove when tween is finished
        }
    }
}