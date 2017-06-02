using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citadel_v1
{
    class ArchitectPlayerAction : PlayerAction
    {
        protected override void DoCharacterAction(List<Player> players, Player currentPlayer)
        {
            GrantBonusBuildAmount(currentPlayer);
            while(UserAdapter.DecideToBuildDistrict())
            {
                BuildDistricts(currentPlayer);
            }
            DrawTwoExtraDistrictCards(currentPlayer);
        }

        private void DrawTwoExtraDistrictCards(Player currentPlayer)
        {
            for (int i = 0; i < 2; i++)
            {
                DistrictCard cardToDraw = Deck.DistrictDeck.First();
                Deck.DistrictDeck.Remove(cardToDraw);
                currentPlayer.Hand.Add(cardToDraw);
                // draw card from deck
                UserAdapter.DrawCardFromDeck(cardToDraw, currentPlayer);
            }
        }

        private void GrantBonusBuildAmount(Player currentPlayer)
        {
            currentPlayer.CanBuild = 3;
        }

        public ArchitectPlayerAction(IUserAdapter userAdapter, Decks deck, List<CharacterCard> fullCharacterCardList) : base(userAdapter, deck, fullCharacterCardList)
        {
        }
    }
}
