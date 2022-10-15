﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace random_game
{
    class Constants
    {
        public const string NOTETEXT =
            "  ###\n" +
            "#######\n" +
            "#######\n" +
            "  ###";

        public const string NOTEPRESSEDTEXT =
            "   #\n" +
            " #####\n" +
            " #####\n" +
            "   #";

        public const string LONGNOTETEXT =
            "###";

        public const int NOTEWIDTH = 8;
        public const int LNOFFSET = 2;


        public const int EARLYHITTIMING = 150;
        public const int LATEHITTIMING = 150;
        public const int PERFECTTIMING = 35;
        public const int GREATTIMING = 60;
        public const int OKTIMING = 100;
        //another other timing is bad

        public static List<List<Key>> defaultBinds = new List<List<Key>>
        {
            new Key[]{ Key.Space}.ToList(),
            new Key[]{ Key.D, Key.K}.ToList(),
            new Key[]{ Key.D, Key.Space, Key.K}.ToList(),
            new Key[]{ Key.D, Key.F, Key.J, Key.K}.ToList(),
            new Key[]{ Key.D, Key.F, Key.Space, Key.J, Key.K}.ToList(),
            new Key[]{ Key.S, Key.D, Key.F, Key.J, Key.K, Key.L}.ToList(),
            new Key[]{ Key.S, Key.D, Key.F, Key.Space, Key.J, Key.K, Key.L}.ToList(),
            new Key[]{ Key.A, Key.S, Key.D, Key.F, Key.H, Key.J, Key.K, Key.L}.ToList(),
            new Key[]{ Key.A, Key.S, Key.D, Key.F, Key.Space, Key.H, Key.J, Key.K, Key.L}.ToList()
        };

        public const int BUFFERWIDTH = 120;
        public const int BUFFERHEIGHT = 30;
        
    }
}
