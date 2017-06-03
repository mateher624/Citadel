using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citadel_v1
{
    class CharacterChoicePhase : Phase
    {
        const int FirstPlayerId = 0;

        public CharacterChoicePhase(List<Player> players, Phase phase, Decks deck, List<CharacterCard> fullCharacterCardList, SynchronizationController synchronizationController, IUserAdapter userAdapter) : this(players, phase.Round, deck, fullCharacterCardList, synchronizationController, userAdapter)
        {

        }

        public CharacterChoicePhase(List<Player> players, Round round, Decks deck, List<CharacterCard> fullCharacterCardList, SynchronizationController synchronizationController, IUserAdapter userAdapter) : base(players, deck, fullCharacterCardList, synchronizationController, userAdapter)
        {
            Round = round;
        }

        public override void DoPhase()
        {
            SetAllCharacterCardsActive();
            SetAllDistrictBuildPerTourAmount();
            Deck.Shuffle<CharacterCard>(Deck.CharacterDeck, new Random());     // tasowanie talii kart postaci
            DiscardCharacterCards(1 /*EvaluateCardsToRemove()*/);        // odrzucenie wymaganej ilości kart postaci
            PickCharacterCard();         // wybór karty postaci przez każdego z graczy
            DiscardRest();                      // odrzucenie pozostałych kart postaci
            UpdatePhase();
        }

        private void SetAllDistrictBuildPerTourAmount()
        {
            foreach (var player in Players)
            {
                player.CanBuild = 1;
            }
        }

        public override void UpdatePhase()
        {
            Round.CurrentPhase = new PlayerActionPhase(Players, this, Deck, FullCharacterCardList, SynchronizationController, _userAdapter);
        }

        private void SetAllCharacterCardsActive()       // ustawienie wszystkich kart postaci, jako aktywnych
        {
            foreach (var characterCard in Deck.CharacterDeck)
            {
                characterCard.Active = true;
            }
        }

        private void DiscardRest()      // dodanie pozostałych kart postaci do talii kart odrzuconych (dla kart postaci)
        {
            DiscardCharacterCards(Deck.CharacterDeck.Count());
        }

        private void PickCharacterCard()   // zezwolenie każdemu użytkownikowi na wybranie karty postaci
        {
            int playerId = FindFirstPlayerInPreviousRound();
            for(int i = playerId; i < Players.Count(); i++)  // wybór karty postaci rozpoczynany jest od gracza ze znacznikiem króla
            {
                _userAdapter.NextPlayerChosesCard(i+1);
                Players[i].PickCharacterCard();
            }
            for(int i = 0; i < playerId; i++)       // wybór kart postaci przez resztę graczy
            {
                _userAdapter.NextPlayerChosesCard(i+1);
                Players[i].PickCharacterCard();
            }
        }

        private int FindFirstPlayerInPreviousRound()    // odnalezienie gracza, który rozpoczynał poprzednią rundę
        {
            int playerId = FirstPlayerId;
            foreach (var player in Players)
            {
                if (player.IsKing)
                {
                    playerId = player.PlayerId;
                }
            }
            return playerId-1;
        }

        private void DiscardCharacterCards(int amount)      // odrzuca zadaną ilość kart postaci na stos kart odrzuconych postaci
        {
            for (int i = 0; i < amount; i++)
            {
                var cardToDiscard = Deck.CharacterDeck.First();
                Deck.CharacterDeck.Remove(cardToDiscard);
                Deck.DiscardedCharacterDeck.Add(cardToDiscard);
            }
        }

    }
}
