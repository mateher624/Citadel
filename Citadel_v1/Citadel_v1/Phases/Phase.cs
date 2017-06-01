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
        protected IUserAdapter _userAdapter;

        public Round Round { get; set; }

        protected Decks Deck { get; private set; }

        protected List<CharacterCard> FullCharacterCardList { get; private set; }

        protected readonly List<Player> Players;

        public SynchronizationController SynchronizationController { get; private set; }

        protected Phase(List<Player> players, Decks deck, List<CharacterCard> fullCharacterCardList, SynchronizationController synchronizationController, IUserAdapter userAdapter)
        {
            Players = players;
            Deck = deck;
            FullCharacterCardList = fullCharacterCardList;
            SynchronizationController = synchronizationController;
            _userAdapter = userAdapter;
        }

        public abstract void DoPhase();

        public abstract void UpdatePhase();
    }
}
