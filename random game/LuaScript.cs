using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NLua;
using System.Windows.Forms;
using System.Reflection;

namespace random_game
{
    class LuaScript
    {
        Lua state;
        public bool running = true;
        GameData _gameData;
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
            catch(Exception e)
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
            string[] funcsToAdd = { 
                "setReceptorX",
                "setReceptorY",
                "getReceptorX",
                "getReceptorY"
            };
            foreach(string funcStr in funcsToAdd)
            {
                MethodBase funcToAddToLua = typeof(LuaScript).GetMethod(funcStr);
                state.RegisterFunction(funcStr, funcToAddToLua);
            }

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

        public void update(float dt)
        {
            if (!running)
                return;

            call("update", new string[] { });
        }
        public void call(string func, string[] args)
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
}
