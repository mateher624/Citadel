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
            var rnd=new Random();
            var index = rnd.Next(0, available.Count());
            return available[index];
        }

        public PlayerAction.OneAction ChooseOneFromTwoPlayerActions(params PlayerAction.OneAction[] availableActions)
        {
            var rnd = new Random();
            var index = rnd.Next(0, 2);
            return availableActions.FirstOrDefault();
        }

        public MagicianPlayerAction.MagicianActionChoice MagicianActionChoice(params MagicianPlayerAction.MagicianActionChoice[] availableActions)
        {
            var rnd = new Random();
            var index = rnd.Next(0, 2);
            return availableActions[index];
        }

        public Player ChoosePlayerToExchangeCardsWith(List<Player> players)
        {
            var rnd = new Random();
            var index = rnd.Next(0, players.Count());
            return players[index];
        }

        public List<DistrictCard> ChooseCardsToDiscard(List<DistrictCard> availableCards)
        {
            var cardsToDiscard = new List<DistrictCard> {availableCards[0], availableCards[1]};
            return cardsToDiscard;
        }

        public DistrictCard ChooseDistrictToBuild(List<DistrictCard> currentPlayerHand)
        {
            var rnd=new Random();
            var index = rnd.Next(0, currentPlayerHand.Count());
            return currentPlayerHand[index];
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

        public void NextPlayerChosesCard(int playerIndex)
        {
            throw new NotImplementedException();
        }

        public void NextPlayerMakeTurn(int playerIndex, string charName)
        {
            throw new NotImplementedException();
        }
    }
}