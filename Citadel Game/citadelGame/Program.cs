﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace citadelGame
{
    enum Origin { nowhere, hand, deck, playground };

    class Program
    {
        static void Main(string[] args)
        {
            _test_RPG myGame = new _test_RPG();
            myGame.Run();
        }
    }
}
