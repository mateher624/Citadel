using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citadel_v1
{
    class MerchantPlayerAction : PlayerAction
    {
        protected override void DoCharacterAction(List<Player> players, Player currentPlayer)
        {
            int greenDistrictAmount = GreenDistrictCount(currentPlayer);
            AddGoldForGreenDistricts(greenDistrictAmount, currentPlayer);
            UserAdapter.UpdateCurrentPanel(currentPlayer);
            if (UserAdapter.DecideToBuildDistrict())
            {
                BuildDistricts(currentPlayer);
                UserAdapter.UpdateCurrentPanel(currentPlayer);
            }
            AddOneExtraGold(currentPlayer);
            UserAdapter.UpdateCurrentPanel(currentPlayer);
        }

        private void AddOneExtraGold(Player currentPlayer)
        {
            currentPlayer.Gold++;
        }

        private void AddGoldForGreenDistricts(int greenDistrictAmount, Player currentPlayer)
        {
            currentPlayer.Gold += greenDistrictAmount;
        }

        private int GreenDistrictCount(Player currentPlayer)
        {
            return currentPlayer.Table.Count(card => card.Color == Color.Green);
        }

        public MerchantPlayerAction(IUserAdapter userAdapter, Decks deck, List<CharacterCard> fullCharacterCardList) : base(userAdapter, deck, fullCharacterCardList)
        {
        }
    }
}
