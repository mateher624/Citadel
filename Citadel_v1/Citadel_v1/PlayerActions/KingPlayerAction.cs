using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citadel_v1
{
    class KingPlayerAction : PlayerAction
    {
        protected override void DoCharacterAction(List<Player> players, Player currentPlayer)
        {
            int yellowDistrictAmount = YellowDistrictCount(currentPlayer);
            AddGoldForYellowDistricts(yellowDistrictAmount, currentPlayer);
            if (UserAdapter.DecideToBuildDistrict())
            {
                BuildDistricts(currentPlayer);
            }
        }

        private void AddGoldForYellowDistricts(int yellowDistrictAmount, Player currentPlayer)
        {
            currentPlayer.Gold += yellowDistrictAmount;
        }

        private int YellowDistrictCount(Player currentPlayer)
        {
            return currentPlayer.Table.Count(card => card.Color == Color.Yellow);
        }

        public KingPlayerAction(IUserAdapter userAdapter, Decks deck, List<CharacterCard> fullCharacterCardList) : base(userAdapter, deck, fullCharacterCardList)
        {
        }
    }
}
