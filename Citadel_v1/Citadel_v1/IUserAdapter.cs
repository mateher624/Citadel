using System.Collections.Generic;
using System.Dynamic;

namespace Citadel_v1
{
    public interface IUserAdapter
    {
        DistrictCard PickOneOfTwo(List<DistrictCard> oneToPick);   //zwraca wybraną kartę z dwóch

        CharacterCard ChooseCharacterCard(List<CharacterCard> available);           //zwraca wybraną kartę ze wszystkich możliwych

        PlayerAction.OneAction ChooseOneFromTwoPlayerActions(params PlayerAction.OneAction[] availableActions);      // dobry typ???

        MagicianPlayerAction.MagicianActionChoice MagicianActionChoice(params MagicianPlayerAction.MagicianActionChoice[] availableActions);

        Player ChoosePlayerToExchangeCardsWith(List<Player> players);

        List<DistrictCard> ChooseCardsToDiscard(List<DistrictCard> availableCards);

        DistrictCard ChooseDistrictToBuild(List<DistrictCard> currentPlayerHand);

        bool DecideToBuildDistrict();      // zwraca true jeżeli użytkownik chce budować

        bool DecideToDestroyDistrict();

        WarlordPlayerAction.DistrictCardToDestroy ChooseDistrictCardToDestroy(List<Player> players);

        void NextPlayerChosesCard(int playerIndex);

        void NextPlayerMakeTurn(int playerIndex, string charName);
    }
}