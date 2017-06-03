using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using citadelGame.UI;
using Citadel_v1;
using SFML.Graphics;
using SFML.System;

// Event denture is a class that is able to modify parameters of _RPG class

namespace citadelGame
{
    class EventDenture
    {
        _test_RPG mainGame;
        public BoardState state;

        public UIMessage message;
        public UserAdapter userAdapter;
        public SynchronizationController synchronizationController;

        public Texture deckTexture = new Texture("../../Resources/pdeckhd.png");
        private int textureWidth = 144;
        private int textureHeight = 200;

        private int messageWidth = 1200;
        private int messafeHeight = 500;

        private List<Player> playersMemo;
        private int playerIndexMemo;
        public int generalPhase = 0;

        public int pickUpState = 0;

        //private List<CharacterCard> availableCards;

        public EventDenture(BoardState state, UIMessage message, SynchronizationController synchronizationController, _test_RPG game)
        {
            mainGame = game;
            this.message = message;
            this.state = state;
            //this.userAdapter = userAdapter;
            this.synchronizationController = synchronizationController;
    } 

        public void ChooseCharacterCard(List<CharacterCard> availableCards, int type, Player currentPlayer)
        {
            // set up a message with Choose Carater Card Action
            mainGame.hands[currentPlayer.PlayerId - 1].UnCoverCards();
            //this.availableCards = availableCards;
            TestAether aether = new TestAether();
            
            state.boardStableState = false;
            List<TestCard> dilemaCardList = new List<TestCard>();
            foreach (var card in availableCards)
            {
                dilemaCardList.Add(new TestCard(0, 20 + 15 * 1, 20, textureWidth, textureHeight, deckTexture, card.Id-1, 0, aether, true));
            }
            string msg;
            if (type == 0) msg = "Wybiera gracz numer "+currentPlayer.PlayerId.ToString()+". Kliknij kartę postaci w którą chcesz się wcielić w tej rundzie.";
            else if (type == 1) msg = "Kliknij kartę postaci którą chcesz wyeliminować w tej rundzie.";
            else msg = "Kliknij kartę postaci którą chcesz okraść w tej rundzie.";
            mainGame.message = new UIDilema(1600 / 2 - messageWidth/2, 900 / 2 - messafeHeight/2, messageWidth, messafeHeight, "Wybierz kartę postaci", msg, 1600, 900, dilemaCardList);
            state.boardActive = false;
        }

        public void ChooseActionCard(Player currentPlayer, params PlayerAction.OneAction[] availableActions)
        {
            TestAether aether = new TestAether();

            state.boardStableState = false;

            mainGame.hands[currentPlayer.PlayerId - 1].UnCoverCards();
            mainGame.hands[currentPlayer.PlayerId - 1].Active = true;
            mainGame.playgrounds[currentPlayer.PlayerId - 1].Active = true;

            List<TestCard> dilemaCardList = new List<TestCard>();
            int i = 0;
            foreach (var action in availableActions)
            {
                dilemaCardList.Add(new TestCard(0, 20 + 15 * 1, 20, textureWidth, textureHeight, deckTexture, i, 2, aether, true));
                i++;
            }
            mainGame.message = new UIDilema(1600 / 2 - messageWidth / 2, 900 / 2 - messafeHeight/2, messageWidth, messafeHeight, "Wybierz akcję", "Kliknij kartę akcji którą chcesz wykonać w tej turze.", 1600, 900, dilemaCardList);
            state.boardActive = false;
        }

        public void PickOneOfTwo(List<DistrictCard> oneToPick)
        {
            TestAether aether = new TestAether();

            state.boardStableState = false;
            List<TestCard> dilemaCardList = new List<TestCard>();
            int i = 0;
            Texture deckTexture = new Texture("../../Resources/cdeck.gif");
            foreach (var card in oneToPick)
            {
                dilemaCardList.Add(new TestCard(0, 20 + 15 * 1, 20, 100, 100, deckTexture, i, 2, aether, true));
                i++;
            }
            mainGame.message = new UIDilema(1600 / 2 - messageWidth / 2, 900 / 2 - messafeHeight/2, messageWidth, messafeHeight, "Wybierz dzielnicę", "Kliknij kartę dzielnicy którą chcesz wybrać.", 1600, 900, dilemaCardList);
            state.boardActive = false;
        }

        public void MagicianActionChoice(Player currentPlayer, params MagicianPlayerAction.MagicianActionChoice[] availableActions)
        {
            TestAether aether = new TestAether();

            state.boardStableState = false;
            List<TestCard> dilemaCardList = new List<TestCard>();
            int i = 0;
            foreach (var action in availableActions)
            {
                if (i != 1) dilemaCardList.Add(new TestCard(0, 20 + 15 * 1, 20, textureWidth, textureHeight, deckTexture, i+2, 2, aether, true));
                else
                {
                    if (mainGame.hands[currentPlayer.PlayerId - 1].CardList.Count != 0) dilemaCardList.Add(new TestCard(0, 20 + 15 * 1, 20, textureWidth, textureHeight, deckTexture, i+2, 2, aether, true));
                }
                i++;
            }
            
            mainGame.message = new UIDilema(1600 / 2 - messageWidth / 2, 900 / 2 - messafeHeight/2, messageWidth, messafeHeight, "Wybierz akcję Magika", "Kliknij kartę akcji którą chcesz wykonać w tej turze.", 1600, 900, dilemaCardList);
            state.boardActive = false;
        }

        public void ChoosePlayerToExchangeCardsWith(List<Player> players)
        {
            TestAether aether = new TestAether();

            
            state.boardStableState = false;
            List<TestCard> dilemaCardList = new List<TestCard>();
            int i = 0;
            foreach (var player in players)
            {
                dilemaCardList.Add(new TestCard(0, 20 + 15 * 1, 20, textureWidth, textureHeight, deckTexture, i, 1, aether, true));
                i++;
            }
            mainGame.message = new UIDilema(1600 / 2 - messageWidth / 2, 900 / 2 - messafeHeight/2, messageWidth, messafeHeight, "Wybierz gracza", "Kliknij kartę gracza z którym chcesz wymienić się kartami.", 1600, 900, dilemaCardList);
            state.boardActive = false;
        }

        // nie działa dobrze: da się discardować tylko jedną kartę 

        public void ChooseCardsToDiscard(List<DistrictCard> availableCards)
        {
            TestAether aether = new TestAether();

            state.boardStableState = false;
            List<TestCard> dilemaCardList = new List<TestCard>();
            int i = 0;
            Texture deckTexture = new Texture("../../Resources/cdeck.gif");
            foreach (var card in availableCards)
            {
                dilemaCardList.Add(new TestCard(0, 20 + 15 * 1, 20, 100, 100, deckTexture, card.CoordinateX, card.CoordinateY, aether, true));
 
            }
            mainGame.message = new UIDilema(1600 / 2 - messageWidth / 2, 900 / 2 - messafeHeight/2, messageWidth, messafeHeight, "Wybierz kartę dzielnicy", "Kliknij kartę którą chcesz odrzucić.", 1600, 900, dilemaCardList);
            state.boardActive = false;
        }

        public void ChooseDistrictToBuild(Player currentPlayer)
        {
            //TestAether aether = new TestAether();

            //state.boardStableState = false;
            //List<TestCard> dilemaCardList = new List<TestCard>();
            //int i = 0;
            //Texture deckTexture = new Texture("../../Resources/cdeck.gif");
            //foreach (var card in currentPlayerHand)
            //{
            //    dilemaCardList.Add(new TestCard(20 + 15 * 1, 20, 100, 100, deckTexture, i, 4, aether, true));
            //    i++;
            //}
            //mainGame.message = new UIDilema(1600 / 2 - messageWidth / 2, 900 / 2 - messafeHeight/2, messageWidth, messafeHeight, "Wybierz kartę dzielnicy", "Kliknij kartę dzielnicy którą chcesz zbudować.", 1600, 900, dilemaCardList);
            //state.boardActive = false;

            // set up board for player
            state.boardStableState = true;
            
            mainGame.hands[currentPlayer.PlayerId - 1].Active = true;
            mainGame.playgrounds[currentPlayer.PlayerId - 1].Active = true;
            pickUpState = 1;
            state.boardActive = true;
        }

        public void DecideToBuildDistrict()
        {
            state.boardStableState = false;
            TestAether aether = new TestAether();
            List<TestCard> cardMemo = new List<TestCard>();
            cardMemo.Add(new TestCard(0, 20 + 15 * 1, 20, textureWidth, textureHeight, deckTexture, 4, 2, aether, true));
            mainGame.message = new UIChoice(1600 / 2 - 300, 900 / 2 - 200, 600, 400, "Wybór", "Czy chcesz zbudować dzielnicę?.", 1600, 900, cardMemo);
            state.boardActive = false;
        }

        public void DecideToDestroyDistrict()
        {
            state.boardStableState = false;
            TestAether aether = new TestAether();
            List<TestCard> cardMemo = new List<TestCard>();
            cardMemo.Add(new TestCard(0, 20 + 15 * 1, 20, textureWidth, textureHeight, deckTexture, 5, 2, aether, true));
            mainGame.message = new UIChoice(1600 / 2 - 300, 900 / 2 - 200, 600, 400, "Wybór", "Czy chcesz zniszczyć dzielnicę?.", 1600, 900, cardMemo);
            state.boardActive = false;
        }

        public void ReturnChosenCardIndex(int index)
        {
            pickUpState = 0;
            userAdapter.chosenCardOrDilema = true;
            userAdapter.chosenCardOrDilemaIndex = index;
            synchronizationController.ResetEventModel.Set();
            synchronizationController.ResetEventController.Reset();
            //throw new NotImplementedException();
        }

        public void ReturnChoice(bool choice)
        {
            pickUpState = 0;
            userAdapter.chosenCardOrDilema = true;
            userAdapter.choice = choice;
            synchronizationController.ResetEventModel.Set();
            synchronizationController.ResetEventController.Reset();
        }

        public void NextPlayerChosesCard(int playerIndex)
        {
            for (int i = 0; i < 6; i++)
            {
                mainGame.hands[i].CoverCards();
                mainGame.hands[i].Active = false;
                mainGame.playgrounds[i].Active = false;
            }
            state.boardStableState = false;
            TestAether aether = new TestAether();
            List<TestCard> cardMemo = new List<TestCard>();
            cardMemo.Add(new TestCard(0, 20 + 15 * 1, 20, textureWidth, textureHeight, deckTexture, playerIndex-1, 1, aether, true));
            mainGame.message = new UIInfo(1600 / 2 - 300, 900 / 2 - 200, 600, 400, "Informacja", "Gracz numer " + playerIndex + " wybiera kartę postaci.", 1600, 900, cardMemo);
            state.boardActive = false;
        }

        public void NextPlayerMakeTurn(Player currentPlayer)
        {
            state.boardStableState = false;

            for (int i = 0; i < 6; i++)
            {
                mainGame.hands[i].CoverCards();
                mainGame.hands[i].Active = false;
                mainGame.playgrounds[i].Active = false;
            }
            
            TestAether aether = new TestAether();
            List<TestCard> cardMemo = new List<TestCard>();
            cardMemo.Add(new TestCard(0, 20 + 15 * 1, 20, textureWidth, textureHeight, deckTexture, currentPlayer.CharacterCard.Id-1, 0, aether, true));
            mainGame.message = new UIInfo(1600 / 2 - 300, 900 / 2 - 200, 600, 400, "Informacja", "Nadchodzi tura gracza numer " + currentPlayer.PlayerId.ToString() + ". Gracz gra postacią: "+currentPlayer.CharacterCard.Name, 1600, 900, cardMemo);
            mainGame.panels[currentPlayer.PlayerId - 1].SetImage(new Vector2f(currentPlayer.CharacterCard.Id-1, 0));
            state.boardActive = false;
        }

        public void NextPlayerIsDead(Player currentPlayer)
        {
            state.boardStableState = false;
            for (int i = 0; i < 6; i++)
            {
                mainGame.hands[i].CoverCards();
                mainGame.hands[i].Active = false;
                mainGame.playgrounds[i].Active = false;
            }
            TestAether aether = new TestAether();
            List<TestCard> cardMemo = new List<TestCard>();
            cardMemo.Add(new TestCard(0, 20 + 15 * 1, 20, textureWidth, textureHeight, deckTexture, 6, 2, aether, true));
            mainGame.message = new UIInfo(1600 / 2 - 300, 900 / 2 - 200, 600, 400, "Informacja", "Gracz (" + currentPlayer.PlayerId.ToString() + ") grający postacią: " + currentPlayer.CharacterCard.Name + " został zabity.", 1600, 900, cardMemo);
            mainGame.panels[currentPlayer.PlayerId - 1].SetImage(new Vector2f(currentPlayer.CharacterCard.Id - 1, 0));
            state.boardActive = false;
        }

        public void ResetPanels()
        {
            for (int i = 0; i < 6; i++)
            {
                mainGame.panels[i].SetImage(new Vector2f(i, 1));
            }
            synchronizationController.ResetEventModel.Set();
            synchronizationController.ResetEventController.Reset();
        }

        public void GeneralChoosePlayerToDestroy(List<Player> players)
        {
            //TestAether aether = new TestAether();
            //playersMemo = players;
            //generalPhase = 1;
            //state.boardStableState = false;
            //List<TestCard> dilemaCardList = new List<TestCard>();
            //int i = 0;
            //foreach (var player in players)
            //{
            //    dilemaCardList.Add(new TestCard(0, 20 + 15 * 1, 20, textureWidth, textureHeight, deckTexture, i, 1, aether, true));
            //    i++;
            //}
            //mainGame.message = new UIDilema(1600 / 2 - messageWidth / 2, 900 / 2 - messafeHeight / 2, messageWidth, messafeHeight, "Wybierz gracza", "Kliknij kartę gracza któremu chcesz zniszczyć kartę dzielnicy.", 1600, 900, dilemaCardList);
            //state.boardActive = false;

            state.boardStableState = true;

            foreach (var hand in mainGame.hands)
            {
                hand.Active = false;
            }
            foreach (var playground in mainGame.playgrounds)
            {
                playground.Active = true;
            }

            generalPhase = 2;
            state.boardActive = true;
        }

        public void GeneralChooseCardToDestroy(int index)
        {
            /*TestAether aether = new TestAether();
            generalPhase = 2;
            state.boardStableState = false;
            List<TestCard> dilemaCardList = new List<TestCard>();
            int i = 0;
            Texture deckTexture = new Texture("../../Resources/cdeck.gif");
            foreach (var card in playersMemo[index].Hand)
            {
                dilemaCardList.Add(new TestCard(0, 20 + 15 * 1, 20, 100, 100, deckTexture, i, 3, aether, true));
                i++;
            }
            if (dilemaCardList.Count == 0) NoCardsReturned();
            else
            {
                mainGame.message = new UIDilema(1600 / 2 - messageWidth / 2, 900 / 2 - messafeHeight / 2, messageWidth,
                    messafeHeight, "Wybierz kartę dzielnicy", "Kliknij kartę którą chcesz odrzucić.", 1600, 900,
                    dilemaCardList);
                state.boardActive = false;
            }*/
            throw new NotImplementedException();
        }

        public void GeneralReturnChoice(WarlordPlayerAction.DistrictCardToDestroy districtCard)
        {
            userAdapter.chosenCardToDestroy = districtCard;
            generalPhase = 0;
            userAdapter.chosenCardOrDilema = true;
            synchronizationController.ResetEventModel.Set();
            synchronizationController.ResetEventController.Reset();
        }

        public void NoCardsReturned()
        {
            generalPhase = 0;
            userAdapter.chosenCardOrDilema = false;
            synchronizationController.ResetEventModel.Set();
            synchronizationController.ResetEventController.Reset();
        }

        public void UpdatePanels(List<Player> players)
        {
            foreach (var player in players)
            {
                mainGame.panels[player.PlayerId-1].SetInfo(player.Hand.Count(), player.Table.Count(), player.Gold, player.IsKing);
            }
            synchronizationController.ResetEventModel.Set();
            synchronizationController.ResetEventController.Reset();
        }

        public void UpdateCurrentPanel(Player player)
        {
            mainGame.panels[player.PlayerId - 1].SetInfo(player.Hand.Count(), player.Table.Count(), player.Gold, player.IsKing);
            synchronizationController.ResetEventModel.Set();
            synchronizationController.ResetEventController.Reset();
        }

        public void DrawCardFromDeck(DistrictCard card, Player currentPlayer)
        {
            // flow
            if (mainGame.deck.CardList.Count > 0)
            {
                TestCard cardDummy = mainGame.deck.CardList.Find(x => x.id == card.Id);
                if (cardDummy == null) throw new NotImplementedException();
                mainGame.deck.RemoveCard(cardDummy);
                mainGame.hands[currentPlayer.PlayerId-1].AddCard(cardDummy);
            }
            synchronizationController.ResetEventModel.Set();
            synchronizationController.ResetEventController.Reset();
        }

        public void LetThingsHappen()
        {
            synchronizationController.ResetEventModel.Set();
            synchronizationController.ResetEventController.Reset();
        }

        public void AddCardToDeck(DistrictCard card)
        {
            mainGame.deck.AddCard(card.Id, card.CoordinateX, card.CoordinateY);
            synchronizationController.ResetEventModel.Set();
            synchronizationController.ResetEventController.Reset();
        }

        public void HandToPlayground(DistrictCard card, Player currentPlayer)
        {
            if (mainGame.hands[currentPlayer.PlayerId-1].CardList.Count > 0)
            {
                TestCard cardDummy = mainGame.hands[currentPlayer.PlayerId - 1].CardList.Find(x => x.id == card.Id);
                if (cardDummy == null) throw new NotImplementedException();
                mainGame.hands[currentPlayer.PlayerId - 1].RemoveCard(cardDummy);
                mainGame.playgrounds[currentPlayer.PlayerId - 1].AddCard(cardDummy);
            }
            synchronizationController.ResetEventModel.Set();
            synchronizationController.ResetEventController.Reset();
        }

        public void PlaygroundToHand(DistrictCard card, Player currentPlayer)
        {
            if (mainGame.playgrounds[currentPlayer.PlayerId - 1].CardList.Count > 0)
            {
                TestCard cardDummy = mainGame.playgrounds[currentPlayer.PlayerId - 1].CardList.Find(x => x.id == card.Id);
                if (cardDummy == null) throw new NotImplementedException();
                mainGame.playgrounds[currentPlayer.PlayerId - 1].RemoveCard(cardDummy);
                mainGame.hands[currentPlayer.PlayerId - 1].AddCard(cardDummy);
            }
            synchronizationController.ResetEventModel.Set();
            synchronizationController.ResetEventController.Reset();
        }

        public void HandDiscard(DistrictCard card, Player currentPlayer)
        {
            if (mainGame.hands[currentPlayer.PlayerId - 1].CardList.Count > 0)
            {
                TestCard cardDummy = mainGame.hands[currentPlayer.PlayerId - 1].CardList.Find(x => x.id == card.Id);
                if (cardDummy == null) throw new NotImplementedException();
                mainGame.hands[currentPlayer.PlayerId - 1].RemoveCard(cardDummy);
                mainGame.graveyard.AddCard(cardDummy);
            }
            synchronizationController.ResetEventModel.Set();
            synchronizationController.ResetEventController.Reset();
        }

        public void PlaygroundDiscard(DistrictCard card, Player currentPlayer)
        {
            if (mainGame.playgrounds[currentPlayer.PlayerId - 1].CardList.Count > 0)
            {
                TestCard cardDummy = mainGame.playgrounds[currentPlayer.PlayerId - 1].CardList.Find(x => x.id == card.Id);
                if (cardDummy == null) throw new NotImplementedException();
                mainGame.playgrounds[currentPlayer.PlayerId - 1].RemoveCard(cardDummy);
                mainGame.graveyard.AddCard(cardDummy);
            }
            synchronizationController.ResetEventModel.Set();
            synchronizationController.ResetEventController.Reset();
        }

        public void HandsExchange(Player player1, Player player2)
        {
            List<int> memoCardsId1 = new List<int>();
            List<int> memoCardsId2 = new List<int>();
            foreach (var card in mainGame.hands[player1.PlayerId - 1].CardList)
            {
                memoCardsId1.Add(card.id);
            }
            foreach (var card in mainGame.hands[player2.PlayerId - 1].CardList)
            {
                memoCardsId2.Add(card.id);
            }
            foreach (var i in memoCardsId1)
            {
                TestCard cardDummy = mainGame.hands[player1.PlayerId - 1].CardList.Find(x => x.id == i);
                if (cardDummy == null) throw new NotImplementedException();
                mainGame.hands[player1.PlayerId - 1].RemoveCard(cardDummy);
                mainGame.hands[player2.PlayerId - 1].AddCard(cardDummy);
            }
            foreach (var i in memoCardsId2)
            {
                TestCard cardDummy = mainGame.hands[player2.PlayerId - 1].CardList.Find(x => x.id == i);
                if (cardDummy == null) throw new NotImplementedException();
                mainGame.hands[player2.PlayerId - 1].RemoveCard(cardDummy);
                mainGame.hands[player1.PlayerId - 1].AddCard(cardDummy);
            }
            synchronizationController.ResetEventModel.Set();
            synchronizationController.ResetEventController.Reset();
        }
    }
}
