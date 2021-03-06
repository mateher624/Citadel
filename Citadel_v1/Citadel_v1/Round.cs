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

        public SynchronizationController SynchronizationController { get; private set; }
        public IUserAdapter UserAdapter { get; private set; }

        public Round(List<Player> players, Decks deck, SynchronizationController synchronizationController, IUserAdapter userAdapter)
        {
            SynchronizationController = synchronizationController;
            this.UserAdapter = userAdapter;
            CurrentPhase = new InitialPhase(players, this, deck, deck.CharacterDeck.ToList(), synchronizationController, userAdapter);           
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
