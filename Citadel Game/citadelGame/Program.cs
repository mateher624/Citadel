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
            
            bool returnMenu = true;
            while (returnMenu == true)
            {
                returnMenu = false;
                SceneMenu myMenu = new SceneMenu();
                bool proceed = myMenu.Run();
                if (proceed)
                {
                    SceneRPG myGame = new SceneRPG();
                    returnMenu = myGame.Run();
                    myGame.Abort();
                }
            }
            
        }
    }
}
