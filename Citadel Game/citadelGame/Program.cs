using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace citadelGame
{
    enum Origin { Nowhere, Hand, Deck, Playground };
    enum PhaseType { CharacterChoice, InitialPhase, PlayerAction, Result }

    class Program
    {
        static void Main(string[] args)
        {
            TestRpg myGame = new TestRpg();
            myGame.Run();
        }
    }
}
