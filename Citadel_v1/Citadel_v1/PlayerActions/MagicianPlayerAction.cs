using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citadel_v1
{
    public class MagicianPlayerAction : PlayerAction
    {
        public delegate void MagicianActionChoice(List<Player> players, Player currentPlayer);

        protected override void DoCharacterAction(List<Player> players, Player currentPlayer)
        {
            MagicianActionChoice decision = UserAdapter.MagicianActionChoice(ExchangeCardsWithOtherPlayer, DiscardAndDrawCards);
            decision.Invoke(players, currentPlayer);

            if (UserAdapter.DecideToBuildDistrict())
            {
                BuildDistricts(currentPlayer);
                UserAdapter.UpdateCurrentPanel(currentPlayer);
            }
        }

        private void ExchangeCardsWithOtherPlayer(List<Player> players, Player currentPlayer)
        {
            Player playerToExchangeCardsWith = UserAdapter.ChoosePlayerToExchangeCardsWith(players);
            playerToExchangeCardsWith.ExchangeCards(currentPlayer);
        }

        private void DiscardAndDrawCards(List<Player> players, Player currentPlayer)
        {
            currentPlayer.DiscardAndDrawCards();
        }

        public MagicianPlayerAction(IUserAdapter userAdapter, Decks deck, List<CharacterCard> fullCharacterCardList) : base(userAdapter, deck, fullCharacterCardList)
        {
        }
    }
}
