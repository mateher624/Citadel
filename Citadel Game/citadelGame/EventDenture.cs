﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using citadelGame.UI;
using Citadel_v1;
using SFML.Graphics;

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

        //private List<CharacterCard> availableCards;

        public EventDenture(BoardState state, UIMessage message, SynchronizationController synchronizationController, _test_RPG game)
        {
            mainGame = game;
            this.message = message;
            this.state = state;
            //this.userAdapter = userAdapter;
            this.synchronizationController = synchronizationController;
    } 

        public void ChooseCharacterCard(List<CharacterCard> availableCards)
        {
            // set up a message with Choose Carater Card Action

            //this.availableCards = availableCards;
            TestAether aether = new TestAether();
            
            state.boardStableState = false;
            List<TestCard> dilemaCardList = new List<TestCard>();
            foreach (var card in availableCards)
            {
                dilemaCardList.Add(new TestCard(20 + 15 * 1, 20, textureWidth, textureHeight, deckTexture, card.Id-1, 0, aether, true));
            }
            mainGame.message = new UIDilema(1600 / 2 - messageWidth/2, 900 / 2 - messafeHeight/2, messageWidth, messafeHeight, "Wybierz kartę postaci", "Kliknij kartę postaci którą w którą chcesz się wcielić w tej rundzie.", 1600, 900, dilemaCardList);
            state.boardActive = false;
        }

        public void ChooseActionCard(params PlayerAction.OneAction[] availableActions)
        {
            TestAether aether = new TestAether();

            state.boardStableState = false;
            List<TestCard> dilemaCardList = new List<TestCard>();
            int i = 0;
            foreach (var action in availableActions)
            {
                dilemaCardList.Add(new TestCard(20 + 15 * 1, 20, textureWidth, textureHeight, deckTexture, i, 2, aether, true));
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
                dilemaCardList.Add(new TestCard(20 + 15 * 1, 20, 100, 100, deckTexture, i, 2, aether, true));
                i++;
            }
            mainGame.message = new UIDilema(1600 / 2 - messageWidth / 2, 900 / 2 - messafeHeight/2, messageWidth, messafeHeight, "Wybierz dzielnicę", "Kliknij kartę dzielnicy którą chcesz wybrać.", 1600, 900, dilemaCardList);
            state.boardActive = false;
        }

        public void MagicianActionChoice(params MagicianPlayerAction.MagicianActionChoice[] availableActions)
        {
            TestAether aether = new TestAether();

            state.boardStableState = false;
            List<TestCard> dilemaCardList = new List<TestCard>();
            int i = 0;
            foreach (var action in availableActions)
            {
                dilemaCardList.Add(new TestCard(20 + 15 * 1, 20, textureWidth, textureHeight, deckTexture, i+2, 2, aether, true));
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
                dilemaCardList.Add(new TestCard(20 + 15 * 1, 20, textureWidth, textureHeight, deckTexture, i, 1, aether, true));
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
                dilemaCardList.Add(new TestCard(20 + 15 * 1, 20, 100, 100, deckTexture, i, 3, aether, true));
                i++;
            }
            mainGame.message = new UIDilema(1600 / 2 - messageWidth / 2, 900 / 2 - messafeHeight/2, messageWidth, messafeHeight, "Wybierz kartę dzielnicy", "Kliknij kartę którą chcesz odrzucić.", 1600, 900, dilemaCardList);
            state.boardActive = false;
        }

        public void ChooseDistrictToBuild(List<DistrictCard> currentPlayerHand)
        {
            TestAether aether = new TestAether();

            state.boardStableState = false;
            List<TestCard> dilemaCardList = new List<TestCard>();
            int i = 0;
            Texture deckTexture = new Texture("../../Resources/cdeck.gif");
            foreach (var card in currentPlayerHand)
            {
                dilemaCardList.Add(new TestCard(20 + 15 * 1, 20, 100, 100, deckTexture, i, 4, aether, true));
                i++;
            }
            mainGame.message = new UIDilema(1600 / 2 - messageWidth / 2, 900 / 2 - messafeHeight/2, messageWidth, messafeHeight, "Wybierz kartę dzielnicy", "Kliknij kartę dzielnicy którą chcesz zbudować.", 1600, 900, dilemaCardList);
            state.boardActive = false;
        }

        public void DecideToBuildDistrict()
        {
            state.boardStableState = false;
            mainGame.message = new UIChoice(1600 / 2 - 300, 900 / 2 - 200, 600, 400, "Wybór", "Czy chcesz zbudować dzielnicę?.", 1600, 900);
            state.boardActive = false;
        }

        public void DecideToDestroyDistrict()
        {
            state.boardStableState = false;
            mainGame.message = new UIChoice(1600 / 2 - 300, 900 / 2 - 200, 600, 400, "Wybór", "Czy chcesz zniszczyć dzielnicę?.", 1600, 900);
            state.boardActive = false;
        }

        public void ReturnChosenCardIndex(int index)
        {
            userAdapter.chosenCardOrDilema = true;
            userAdapter.chosenCardOrDilemaIndex = index;
            synchronizationController.ResetEventModel.Set();
            synchronizationController.ResetEventController.Reset();
            //throw new NotImplementedException();
        }

        public void ReturnChoice(bool choice)
        {
            userAdapter.chosenCardOrDilema = true;
            userAdapter.choice = choice;
            synchronizationController.ResetEventModel.Set();
            synchronizationController.ResetEventController.Reset();
        }

        public void NextPlayerChosesCard(int playerIndex)
        {
            state.boardStableState = false;
            mainGame.message = new UIInfo(1600 / 2 - 300, 900 / 2 - 200, 600, 400, "Informacja", "Gracz numer " + playerIndex + " wybiera kartę postaci.", 1600, 900);
            state.boardActive = false;
        }

        public void NextPlayerMakeTurn(int playerIndex, string charName)
        {
            state.boardStableState = false;
            mainGame.message = new UIInfo(1600 / 2 - 300, 900 / 2 - 200, 600, 400, "Informacja", "Nadchodzi tura gracza numer " + playerIndex.ToString() + ". Gracz gra postacią: "+charName, 1600, 900);
            state.boardActive = false;
        }


    }
}
