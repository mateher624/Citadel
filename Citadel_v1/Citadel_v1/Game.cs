using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Citadel_v1
{
    public class Game
    {
        public Round Round { get; private set; }
        public List<Player> Players { get; set; } = new List<Player>();   // lista graczy (max 6)
        public Decks Deck { get; private set; }
        public SynchronizationController SynchronizationController { get; private set; }

        public Game(int playersAmount, IUserAdapter userAdapter, Decks deck, SynchronizationController synchronizationController)
        {
            Deck = deck;
            SynchronizationController = synchronizationController;
            Round = new Round(Players, Deck, synchronizationController, userAdapter);
            //pętla odpowiedzialna za dodanie graczy o nazwach kolejno 1, 2, 3, ...
            for (int i=0;i<playersAmount;i++)
            {
                Players.Add(new Player(userAdapter, i.ToString(), Players, Deck, Deck.CharacterDeck.ToList()));
            }
        }

        public void StartNewRound()
        {
            Round.DoTurns();
        }

       
    }
}
