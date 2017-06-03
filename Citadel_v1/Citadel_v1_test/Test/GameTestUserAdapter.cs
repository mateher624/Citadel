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

        public CharacterCard ChooseCharacterCard(List<CharacterCard> available, int type, Player currentPlayer)
        {
            var rnd=new Random();
            var index = rnd.Next(0, available.Count());
            return available[index];
        }

        public PlayerAction.OneAction ChooseOneFromTwoPlayerActions(Player currentPlayer, params PlayerAction.OneAction[] availableActions)
        {
            var rnd = new Random();
            var index = rnd.Next(0, 2);
            return availableActions.FirstOrDefault();
        }

        public MagicianPlayerAction.MagicianActionChoice MagicianActionChoice(Player currentPlayer, params MagicianPlayerAction.MagicianActionChoice[] availableActions)
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

        public DistrictCard ChooseDistrictToBuild(Player currentPlayer)
        {
            var rnd=new Random();
            var index = rnd.Next(0, currentPlayer.Hand.Count());
            return currentPlayer.Hand[index];
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

        public void NextPlayerMakeTurn(Player currentPlayer)
        {
            throw new NotImplementedException();
        }

        public void NextPlayerIsDead(Player currentPlayer)
        {
            throw new NotImplementedException();
        }

        public void ResetPanels()
        {
            throw new NotImplementedException();
        }

        public void UpdatePanels(List<Player> players)
        {
            throw new NotImplementedException();
        }

        public void UpdateCurrentPanel(Player player)
        {
            throw new NotImplementedException();
        }

        public void DrawCardFromDeck(DistrictCard card, Player currentPlayer)
        {
            throw new NotImplementedException();
        }

        public void AddCardToDeck(DistrictCard card)
        {
            throw new NotImplementedException();
        }

        public void HandToPlayground(DistrictCard card, Player currentPlayer)
        {
            throw new NotImplementedException();
        }

        public void PlaygroundToHand(DistrictCard card, Player currentPlayer)
        {
            throw new NotImplementedException();
        }

        public void HandDiscard(DistrictCard card, Player currentPlayer)
        {
            throw new NotImplementedException();
        }

        public void PlaygroundDiscard(DistrictCard card, Player currentPlayer)
        {
            throw new NotImplementedException();
        }

        public void HandsExchange(Player player1, Player player2)
        {
            throw new NotImplementedException();
        }
    }
}