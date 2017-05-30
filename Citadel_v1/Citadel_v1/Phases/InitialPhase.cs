using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citadel_v1
{
    class InitialPhase : Phase
    {
        const int CardsAmount = 4;

        public InitialPhase(List<Player> players, Phase phase, Decks deck, List<CharacterCard> fullCharacterCardList, SynchronizationController synchronizationController) : this(players, phase.Round, deck, fullCharacterCardList, synchronizationController)
        {

        }

        public InitialPhase(List<Player> players, Round round, Decks deck, List<CharacterCard> fullCharacterCardList, SynchronizationController synchronizationController) : base(players, deck, fullCharacterCardList, synchronizationController)
        {
            Round = round;
        }

        public override void DoPhase()
        {
            Deck.Shuffle<DistrictCard>(Deck.DistrictDeck, new Random());   // potasowanie talii kart dzielnic
            HandOutDistrictCards(Players);     // rozdanie 4 kart każdemu graczowi
            GiveAwayGold(Players);              // rozdanie złota graczom
            Players.First().IsKing = true;     // nadanie pierwszemu graczowi znacznika króla
            UpdatePhase();
        }

        public override void UpdatePhase()
        {
            Round.CurrentPhase = new CharacterChoicePhase(Players, this, Deck, FullCharacterCardList, SynchronizationController);
        }

        private void GiveAwayGold(List<Player> players)    // rozdanie 2 szt. złota każdemu z graczy
        {
            foreach (var player in players)
            {
                player.Gold += 2;
            }
        }

        private void HandOutDistrictCards(List<Player> players)     // rozdanie każdemu graczowi dowolnej ilości kart dzielnic
        {
            foreach(var player in players)
            {
                player.AddCardsToHand(CardsAmount);
            }
        }
    }
}
