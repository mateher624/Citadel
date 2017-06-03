using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Citadel_v1
{

    public abstract class PlayerAction
    {
        protected const int PlayerAmount = 6;

        protected  List<CharacterCard> FullCharacterCardList { get; private set; }

        protected Decks Deck { get; private set; }

        protected IUserAdapter UserAdapter;


        protected PlayerAction(IUserAdapter userAdapter, Decks deck, List<CharacterCard> fullCharacterCardList)
        {
            UserAdapter = userAdapter;
            Deck = deck;
            FullCharacterCardList = fullCharacterCardList;
        }

        public delegate void OneAction(Player currentPlayer);

        public void DoPlayerAction(Player currentPlayer, List<Player> players)
        {
            ChooseFirstAction(currentPlayer);
            DoCharacterAction(players, currentPlayer);
            //DoCharacterAction<CharacterActionArgs>(players);
        }

        protected void BuildDistricts(Player currentPlayer)
        {
            DistrictCard districtToBuild = UserAdapter.ChooseDistrictToBuild(currentPlayer);
            if (districtToBuild.Cost > currentPlayer.Gold)
            {
                UserAdapter.PlaygroundToHand(districtToBuild, currentPlayer);
                return;
            }
            if (currentPlayer.CanBuild > 0)
            {
                currentPlayer.BuildDistrict(districtToBuild);
            }
        }

        protected abstract void DoCharacterAction(List<Player> players, Player currentPlayer);

        private void ChooseFirstAction(Player currentPlayer)
        {
            var action = UserAdapter.ChooseOneFromTwoPlayerActions(TakeTwoGold, DrawDistrictCards);
            action(currentPlayer);
        }

        private void TakeTwoGold(Player currentPlayer)
        {
            List<Player> onePlayerList = new List<Player>();
            onePlayerList.Add(currentPlayer);
            currentPlayer.Gold += 2;
            UserAdapter.UpdatePanels(onePlayerList);
        }

        private void DrawDistrictCards(Player currentPlayer)
        {
            List<DistrictCard> oneToPick = new List<DistrictCard>();
            Deck.Shuffle(Deck.DistrictDeck, new Random());     //potasowanie talii
            oneToPick.Add(Deck.DistrictDeck[0]);       
            oneToPick.Add(Deck.DistrictDeck[1]);       //wybranie dwóch wierzchnich kart
            Deck.DistrictDeck.RemoveAt(0);
            Deck.DistrictDeck.RemoveAt(0);             //usunięcie ich z talii
            DistrictCard pickedCard = UserAdapter.PickOneOfTwo(oneToPick);   //lista przez referencję czy nie trzeba?
            currentPlayer.Hand.Add(pickedCard);        //wybrana karta trafia do ręki gracza
            oneToPick.Remove(pickedCard);
            Deck.DistrictDeck.Add(oneToPick[0]);       //niewybrana karta wraca do stosu
            // draw picked card
            UserAdapter.DrawCardFromDeck(pickedCard, currentPlayer);
        }

        //protected abstract void DoCharacterAction<T>(List<Player> players, T args = null) where T: CharacterActionArgs;
    }
}
