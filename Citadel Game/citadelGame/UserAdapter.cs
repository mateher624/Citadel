using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Citadel_v1;

namespace citadelGame
{  
    class UserAdapter : IUserAdapter
    {
        public string whatThePhase;

        public int chosenCardOrDilemaIndex;
        public bool choice;

        public bool chosenCardOrDilema { get; set; }

        public WarlordPlayerAction.DistrictCardToDestroy chosenCardToDestroy;

        private SynchronizationController synchronizationController { get; }
        private EventDenture eventDenture { get; }

        public UserAdapter(SynchronizationController synchronizationController, EventDenture eventDenture)
        {
            this.synchronizationController = synchronizationController;
            this.eventDenture = eventDenture;
            this.eventDenture.userAdapter = this;
        }

        public DistrictCard PickOneOfTwo(List<DistrictCard> oneToPick)
        {
            whatThePhase = "PickOneOfTwo";
            chosenCardOrDilema = false;      // error protection
            eventDenture.PickOneOfTwo(oneToPick);
            synchronizationController.ResetEventController.Set();
            synchronizationController.ResetEventModel.WaitOne();
            if (chosenCardOrDilema) return oneToPick[chosenCardOrDilemaIndex];
            throw new NotImplementedException();
        }

        public CharacterCard ChooseCharacterCard(List<CharacterCard> available, int type)
        {
            whatThePhase = "ChooseCharacterCard";
            chosenCardOrDilema = false;      // error protection
            eventDenture.ChooseCharacterCard(available, type);
            synchronizationController.ResetEventController.Set();
            synchronizationController.ResetEventModel.WaitOne();
            if (chosenCardOrDilema) return available[chosenCardOrDilemaIndex];
            throw new NotImplementedException();
        }

        public PlayerAction.OneAction ChooseOneFromTwoPlayerActions(params PlayerAction.OneAction[] availableActions)
        {
            whatThePhase = "ChooseOneFromTwoPlayerActions";
            chosenCardOrDilema = false;
            eventDenture.ChooseActionCard(availableActions);
            synchronizationController.ResetEventController.Set();
            synchronizationController.ResetEventModel.WaitOne();
            if (chosenCardOrDilema) return availableActions[chosenCardOrDilemaIndex];
            throw new NotImplementedException();
        }

        public MagicianPlayerAction.MagicianActionChoice MagicianActionChoice(params MagicianPlayerAction.MagicianActionChoice[] availableActions)
        {
            whatThePhase = "MagicianActionChoice";
            chosenCardOrDilema = false;
            eventDenture.MagicianActionChoice(availableActions);
            synchronizationController.ResetEventController.Set();
            synchronizationController.ResetEventModel.WaitOne();
            if (chosenCardOrDilema) return availableActions[chosenCardOrDilemaIndex];
            throw new NotImplementedException();
        }

        public Player ChoosePlayerToExchangeCardsWith(List<Player> players)
        {
            whatThePhase = "ChoosePlayerToExchangeCardsWith";
            chosenCardOrDilema = false;
            eventDenture.ChoosePlayerToExchangeCardsWith(players);
            synchronizationController.ResetEventController.Set();
            synchronizationController.ResetEventModel.WaitOne();
            if (chosenCardOrDilema) return players[chosenCardOrDilemaIndex];
            throw new NotImplementedException();
        }

        // nie działa można odrzucić tlko jedną kartę!

        public List<DistrictCard> ChooseCardsToDiscard(List<DistrictCard> availableCards)
        {
            whatThePhase = "ChooseCardsToDiscard";
            chosenCardOrDilema = false;
            eventDenture.ChooseCardsToDiscard(availableCards);
            synchronizationController.ResetEventController.Set();
            synchronizationController.ResetEventModel.WaitOne();
            if (chosenCardOrDilema)
            {
                availableCards.Remove(availableCards[chosenCardOrDilemaIndex]);
                return availableCards;
            }
            throw new NotImplementedException();
        }

        public DistrictCard ChooseDistrictToBuild(Player currentPlayer)
        {
            whatThePhase = "ChooseDistrictToBuild";
            chosenCardOrDilema = false;
            eventDenture.ChooseDistrictToBuild(currentPlayer);
            synchronizationController.ResetEventController.Set();
            synchronizationController.ResetEventModel.WaitOne();
            if (chosenCardOrDilema) return currentPlayer.Hand[chosenCardOrDilemaIndex];
            throw new NotImplementedException();
        }

        public bool DecideToBuildDistrict()
        {
            whatThePhase = "DecideToBuildDistrict";
            chosenCardOrDilema = false;
            eventDenture.DecideToBuildDistrict();
            synchronizationController.ResetEventController.Set();
            synchronizationController.ResetEventModel.WaitOne();
            if (chosenCardOrDilema) return choice;
            throw new NotImplementedException();
        }

        public bool DecideToDestroyDistrict()
        {
            whatThePhase = "DecideToDestroyDistrict";
            chosenCardOrDilema = false;
            eventDenture.DecideToDestroyDistrict();
            synchronizationController.ResetEventController.Set();
            synchronizationController.ResetEventModel.WaitOne();
            if (chosenCardOrDilema) return choice;
            throw new NotImplementedException();
        }

        public WarlordPlayerAction.DistrictCardToDestroy ChooseDistrictCardToDestroy(List<Player> players)
        {
            whatThePhase = "ChooseDistrictCardToDestroy";
            chosenCardOrDilema = false;
            eventDenture.ChooseDistrictCardToDestroy(players);
            synchronizationController.ResetEventController.Set();
            synchronizationController.ResetEventModel.WaitOne();
            if (chosenCardOrDilema) return chosenCardToDestroy;
            throw new NotImplementedException();
        }

        public void NextPlayerChosesCard(int playerIndex)
        {
            whatThePhase = "NextPlayerChosesCard";
            eventDenture.NextPlayerChosesCard(playerIndex);
            synchronizationController.ResetEventController.Set();
            synchronizationController.ResetEventModel.WaitOne();
        }

        public void NextPlayerMakeTurn(Player currentPlayer)
        {
            whatThePhase = "NextPlayerMakeTurn";
            eventDenture.NextPlayerMakeTurn(currentPlayer);
            synchronizationController.ResetEventController.Set();
            synchronizationController.ResetEventModel.WaitOne();
        }

        public void NextPlayerIsDead(Player currentPlayer)
        {
            whatThePhase = "NextPlayerIsDead";
            eventDenture.NextPlayerIsDead(currentPlayer);
            synchronizationController.ResetEventController.Set();
            synchronizationController.ResetEventModel.WaitOne();
        }

        public void NextPlayerInfo(int playerIndex)
        {
            throw new NotImplementedException();
        }

        public void ResetPanels()
        {
            eventDenture.ResetPanels();
            synchronizationController.ResetEventController.Set();
            synchronizationController.ResetEventModel.WaitOne();
        }

        public void UpdatePanels(List<Player> players)
        {
            eventDenture.UpdatePanels(players);
            synchronizationController.ResetEventController.Set();
            synchronizationController.ResetEventModel.WaitOne();
        }

        public void UpdateCurrentPanel(Player player)
        {
            eventDenture.UpdateCurrentPanel(player);
            synchronizationController.ResetEventController.Set();
            synchronizationController.ResetEventModel.WaitOne();
        }

        public void DrawCardFromDeck(DistrictCard card, Player currentPlayer)
        {
            eventDenture.DrawCardFromDeck(card, currentPlayer);
            synchronizationController.ResetEventController.Set();
            synchronizationController.ResetEventModel.WaitOne();
        }

        public void AddCardToDeck(DistrictCard card)
        {
            eventDenture.AddCardToDeck(card);
            synchronizationController.ResetEventController.Set();
            synchronizationController.ResetEventModel.WaitOne();
        }

        public void HandToPlayground(DistrictCard card, Player currentPlayer)
        {
            eventDenture.HandToPlayground(card, currentPlayer);
            synchronizationController.ResetEventController.Set();
            synchronizationController.ResetEventModel.WaitOne();
        }

        public void PlaygroundToHand(DistrictCard card, Player currentPlayer)
        {
            eventDenture.PlaygroundToHand(card, currentPlayer);
            synchronizationController.ResetEventController.Set();
            synchronizationController.ResetEventModel.WaitOne();
        }

        public void HandDiscard(DistrictCard card, Player currentPlayer)
        {
            eventDenture.HandDiscard(card, currentPlayer);
            synchronizationController.ResetEventController.Set();
            synchronizationController.ResetEventModel.WaitOne();
        }

        public void PlaygroundDiscard(DistrictCard card, Player currentPlayer)
        {
            eventDenture.PlaygroundDiscard(card, currentPlayer);
            synchronizationController.ResetEventController.Set();
            synchronizationController.ResetEventModel.WaitOne();
        }

        public void HandsExchange(Player player1, Player player2)
        {
            eventDenture.HandsExchange(player1, player2);
            synchronizationController.ResetEventController.Set();
            synchronizationController.ResetEventModel.WaitOne();
        }
    }
}
