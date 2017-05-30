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

        public CharacterCard ChooseCharacterCard(List<CharacterCard> available)
        {
            whatThePhase = "ChooseCharacterCard";
            chosenCardOrDilema = false;      // error protection
            eventDenture.ChooseCharacterCard(available);
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

        public DistrictCard ChooseDistrictToBuild(List<DistrictCard> currentPlayerHand)
        {
            whatThePhase = "ChooseDistrictToBuild";
            chosenCardOrDilema = false;
            eventDenture.ChooseDistrictToBuild(currentPlayerHand);
            synchronizationController.ResetEventController.Set();
            synchronizationController.ResetEventModel.WaitOne();
            if (chosenCardOrDilema) return currentPlayerHand[chosenCardOrDilemaIndex];
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

            throw new NotImplementedException();
        }
    }
}
