using System.Collections.Generic;
using System.Dynamic;

namespace Citadel_v1
{
    public interface IUserAdapter
    {
        DistrictCard PickOneOfTwo(List<DistrictCard> oneToPick);   //zwraca wybraną kartę z dwóch

        CharacterCard ChooseCharacterCard(List<CharacterCard> available, int type, Player currentPlayer);           //zwraca wybraną kartę ze wszystkich możliwych

        PlayerAction.OneAction ChooseOneFromTwoPlayerActions(params PlayerAction.OneAction[] availableActions);      // dobry typ???

        MagicianPlayerAction.MagicianActionChoice MagicianActionChoice(params MagicianPlayerAction.MagicianActionChoice[] availableActions);

        Player ChoosePlayerToExchangeCardsWith(List<Player> players);

        List<DistrictCard> ChooseCardsToDiscard(List<DistrictCard> availableCards);

        DistrictCard ChooseDistrictToBuild(Player currentPlayer);

        bool DecideToBuildDistrict();      // zwraca true jeżeli użytkownik chce budować

        bool DecideToDestroyDistrict();

        WarlordPlayerAction.DistrictCardToDestroy ChooseDistrictCardToDestroy(List<Player> players);

        void NextPlayerChosesCard(int playerIndex);

        void NextPlayerMakeTurn(Player currentPlayer);

        void NextPlayerIsDead(Player currentPlayer);

        void ResetPanels();

        void UpdatePanels(List<Player> players);

        void UpdateCurrentPanel(Player player);

        void DrawCardFromDeck(DistrictCard card, Player currentPlayer);

        void AddCardToDeck(DistrictCard card);

        void HandToPlayground(DistrictCard card, Player currentPlayer);

        void PlaygroundToHand(DistrictCard card, Player currentPlayer);

        void HandDiscard(DistrictCard card, Player currentPlayer);

        void PlaygroundDiscard(DistrictCard card, Player currentPlayer);

        void HandsExchange(Player player1, Player player2);
    }
}