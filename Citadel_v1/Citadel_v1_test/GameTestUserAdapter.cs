using System;
using System.Collections.Generic;
using System.Linq;
using Citadel_v1;

namespace Citadel_v1_test
{
    class GameTestUserAdapter : IUserAdapter
    {
        public DistrictCard PickOneOfTwo(List<DistrictCard> oneToPick)
        {
            return oneToPick.FirstOrDefault();
        }

        public CharacterCard ChooseCharacterCard(List<CharacterCard> available)
        {
            return available.FirstOrDefault();
        }

        public PlayerAction.OneAction ChooseOneFromTwoPlayerActions(params PlayerAction.OneAction[] availableActions)
        {
            //var a = default(PlayerAction.OneAction);
            return availableActions.FirstOrDefault();
        }

        public MagicianPlayerAction.MagicianActionChoice MagicianActionChoice(params MagicianPlayerAction.MagicianActionChoice[] availableActions)
        {
            return availableActions.FirstOrDefault();
        }

        public Player ChoosePlayerToExchangeCardsWith(List<Player> players)
        {
            return players.FirstOrDefault();
        }

        public List<DistrictCard> ChooseCardsToDiscard(List<DistrictCard> availableCards)
        {
            var cardsToDiscard = new List<DistrictCard> {availableCards[0], availableCards[1]};
            return cardsToDiscard;
        }

        public DistrictCard ChooseDistrictToBuild(List<DistrictCard> currentPlayerHand)
        {
            return currentPlayerHand.FirstOrDefault();
        }

        public bool DecideToBuildDistrict()
        {
            Random rnd = new Random();
            return rnd.Next(0, 2) != 0;
        }

        public bool DecideToDestroyDistrict()
        {
            Random rnd = new Random();
            return rnd.Next(0, 2) != 0;
        }

        public WarlordPlayerAction.DistrictCardToDestroy ChooseDistrictCardToDestroy(List<Player> players)
        {
            var rnd=new Random();
            int randomPlayerId = rnd.Next(0, players.Count());
            var districtCardToDestroy =
                new WarlordPlayerAction.DistrictCardToDestroy
                {
                    Player = players[randomPlayerId],
                    DistrictCard = players[randomPlayerId].Table.FirstOrDefault()
                };
            return districtCardToDestroy;
        }
    }
}