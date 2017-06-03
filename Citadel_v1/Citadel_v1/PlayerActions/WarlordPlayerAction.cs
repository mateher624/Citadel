using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Citadel_v1.Policies;

namespace Citadel_v1
{
    public class WarlordPlayerAction : PlayerAction
    {
        public struct DistrictCardToDestroy
        {
            public Player Player { get; set; }
            public DistrictCard DistrictCard { get; set; }
        }

        protected override void DoCharacterAction(List<Player> players, Player currentPlayer)
        {
            int redDistrictAmount = RedDistrictCount(currentPlayer);
            AddGoldForRedDistricts(redDistrictAmount, currentPlayer);
            UserAdapter.UpdateCurrentPanel(currentPlayer);
            if (UserAdapter.DecideToDestroyDistrict())
            {
                DistrictCardToDestroy districtToDestroy = UserAdapter.ChooseDistrictCardToDestroy(players);
                DestroyDistrictIfPossible(districtToDestroy, currentPlayer);
                UserAdapter.UpdatePanels(players);
            }
            if (UserAdapter.DecideToBuildDistrict())
            {
                BuildDistricts(currentPlayer);
                UserAdapter.UpdateCurrentPanel(currentPlayer);
            }
        }

        private void DestroyDistrictIfPossible(DistrictCardToDestroy districtToDestroy, Player currentPlayer)
        {
            if (CanDestroyDistrictPolicy.IsSatisfied(districtToDestroy, currentPlayer))
            {
                int costOfDestroyingDistrict = districtToDestroy.DistrictCard.Cost - 1;
                currentPlayer.Gold -= costOfDestroyingDistrict;
                districtToDestroy.Player.LooseDistrict(districtToDestroy.DistrictCard);
            }
        }

        private void AddGoldForRedDistricts(int redDistrictAmount, Player currentplayer)
        {
            currentplayer.Gold += redDistrictAmount;
        }

        private int RedDistrictCount(Player currentplayer)
        {
            return currentplayer.Table.Count(card => card.Color == Color.Red);
        }

        public WarlordPlayerAction(IUserAdapter userAdapter, Decks deck, List<CharacterCard> fullCharacterCardList) : base(userAdapter, deck, fullCharacterCardList)
        {
        }
    }
}
