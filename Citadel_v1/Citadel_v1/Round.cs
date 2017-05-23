﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citadel_v1
{
    public class Round
    {
        public Phase CurrentPhase { get; set; }

        public Round(List<Player> players, Decks deck)
        {
          CurrentPhase = new InitialPhase(players, this, deck);           
        }


        public void DoTurns()      // wykonanie całej rundy (wszystkich tur w rundzie)
        {
            while (CanRunPhasePolicy.IsSatisfied(CurrentPhase))
            {
                CurrentPhase.DoPhase();
            }
        }
    }
}
