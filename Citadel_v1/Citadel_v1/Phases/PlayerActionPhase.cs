using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citadel_v1
{
    class PlayerActionPhase : Phase
    {
        private const int BasicCharacterCardAmount = 8;

        public PlayerActionPhase(List<Player> players, Phase phase, Decks deck, List<CharacterCard> fullCharacterCardList, SynchronizationController synchronizationController) :this(players, phase.Round, deck, fullCharacterCardList, synchronizationController)
        {

        }

        public PlayerActionPhase(List<Player> players, Round round, Decks deck, List<CharacterCard> fullCharacterCardList, SynchronizationController synchronizationController) : base(players, deck, fullCharacterCardList, synchronizationController)
        {
            Round = round;
        }

        public override void DoPhase()
        {
            MakeTheRightPlayerDoAction();
            UpdatePhase();
        }

        public override void UpdatePhase()
        {
            Round.CurrentPhase = new ResultPhase(Players, this, Deck, FullCharacterCardList, SynchronizationController);
        }

        private void MakeTheRightPlayerDoAction()   // odnalezienie gracza z odpowiednią kartą postaci i wymuszenie na nim wykonania akcji
        {
            for (int i = 1; i <= BasicCharacterCardAmount; i++)
            {
                if (!Deck.Found(Deck.DiscardedCharacterDeck, i))   // sprawdzenie, czy karta nie została odrzucona
                {
                    var playerToDoAction = Players.Find(player => player.CharacterCard.Id == i);  // nie została odrzucona, więc któryś z graczy ją ma na ręce
                    if (playerToDoAction.CharacterCard.Active)
                    {
                        playerToDoAction.DoAction(Players);  // jeżeli karta aktywna, to wykonaj ruch
                    }
                }
            }
        }

        //public Player NextPlayerToDoAction(int cardIndex) // zwraca następnego gracza w kolejce lub null jeżeli nie ma żadnego do wykonania ruchu
        //{
        //    if(cardIndex>basicCharacterCardAmount)
        //    {
        //        return null;
        //    }
        //    if(!Decks.Found(Decks.DiscardedCharacterDeck, cardIndex))   // sprawdzenie, czy karta nie została odrzucona
        //    {
        //        var playerToDoAction = Players.Find(player => player.CharacterCard.Id == cardIndex);     // nie została odrzucona, więc któryś z graczy ją ma na ręce
        //        if (playerToDoAction.CharacterCard.Active)
        //        {
        //            return playerToDoAction;  // jeżeli karta aktywna, to znaleźliśmy gracza do wykonania ruchu
        //        }
        //        else
        //        {
        //            return NextPlayerToDoAction(++cardIndex);   // jeżeli karta nieaktywna to szukaj następnego gracza
        //        }
        //    }
        //    else
        //    {
        //        return NextPlayerToDoAction(++cardIndex);       // jeżeli karta odrzucona to szukaj następnego gracza
        //    }
        //}

    }
}
