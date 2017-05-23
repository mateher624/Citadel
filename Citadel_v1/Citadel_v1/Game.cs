using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citadel_v1
{
    public class Game
    {
        public Round Round { get; private set; }
        public List<Player> Players { get; set; } = new List<Player>();   // lista graczy (max 6)
        public Decks Deck { get; private set; }

        public Game(int playersAmount, IUserAdapter userAdapter, Decks deck)
        {
            Deck = deck;
            Round = new Round(Players, Deck);
            //pętla odpowiedzialna za dodanie graczy o nazwach kolejno 1, 2, 3, ...
            for (int i=0;i<playersAmount;i++)
            {
                Players.Add(new Player(userAdapter, i.ToString(), Players, Deck, Deck.CharacterDeck.ToList()));
            }
            //dorobić ładowanie kart z bazy danych
        }

        public void StartNewRound()
        {
            Round.DoTurns();
        }

       
    }
}
