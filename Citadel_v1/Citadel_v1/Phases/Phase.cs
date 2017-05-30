using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Citadel_v1
{
    public abstract class Phase
    {
        public Round Round { get; set; }

        protected Decks Deck { get; private set; }

        protected List<CharacterCard> FullCharacterCardList { get; private set; }

        protected readonly List<Player> Players;

        public SynchronizationController SynchronizationController { get; private set; }

        protected Phase(List<Player> players, Decks deck, List<CharacterCard> fullCharacterCardList, SynchronizationController synchronizationController)
        {
            Players = players;
            Deck = deck;
            FullCharacterCardList = fullCharacterCardList;
            SynchronizationController = synchronizationController;
        }

        public abstract void DoPhase();

        public abstract void UpdatePhase();
    }
}
