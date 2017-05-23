using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citadel_v1
{
    class BishopPlayerAction : PlayerAction
    {
        
        protected override void DoCharacterAction(List<Player> players, Player currentPlayer)
        {
            int blueDistrictAmount = BlueDistrictCount(currentPlayer);
            AddGoldForBlueDistricts(blueDistrictAmount, currentPlayer);
            if (UserAdapter.DecideToBuildDistrict())
            {
                BuildDistricts(currentPlayer);
            }
        }

        private void AddGoldForBlueDistricts(int blueDistrictAmount, Player currentPlayer)
        {
            currentPlayer.Gold += blueDistrictAmount;
        }

        private int BlueDistrictCount(Player currentPlayer)
        {
            return currentPlayer.Table.Count(card => card.Color == Color.Blue);
        }

        public BishopPlayerAction(IUserAdapter userAdapter, Decks deck, List<CharacterCard> fullCharacterCardList) : base(userAdapter, deck, fullCharacterCardList)
        {
        }
    }
}
