using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Threading;
using citadelGame.UI;
using Citadel_v1;
using Citadel_v1_test;
//using Citadel_v1;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using Color = SFML.Graphics.Color;


namespace citadelGame
{
    class _test_RPG : TestGame
    {
        Vector2f worldCoords;

        public BoardState state;

        //_test_Tilemap map;
        Texture tileset;
        Texture buttonFace;
        Texture deckTexture;
        List<UiButton> buttonList;
        //List<_test_Card> cardSpriteList;
        List<TestContainer> elementsContainers;

        public List<TestHand> hands;
        public List<TestPlayground> playgrounds;
        public List<UiPlayerPanel> panels;

        private TestAether aether;

        public TestDeck deck;

        public UIMessage message;
        private string messageTitle;
        private string messageCaption;

        private EventDenture eventDenture;
        public UserAdapter userAdapter;
        private IDecksFactory deckFactory;

        public TestDeck graveyard;
        private TestCard cursorDockedCard;
        private bool mousePressed = false;

        private bool deckToPlaygroundLock;
        private bool foreignDeckOrPlaygroundLock;

        private Game gameLogic;

        public SynchronizationController synchronizationController;

        private int playerCount;

        public _test_RPG() : base(1600, 900, "Citadel Game Alpha", Color.Cyan)
        {
            buttonList = new List<UiButton>();
            elementsContainers = new List<TestContainer>();

            hands = new List<TestHand>();
            playgrounds = new List<TestPlayground>();
            panels = new List<UiPlayerPanel>();
        }

        // Message generation and some shit functions
        //
        //
        // End

        protected override void CheckCollide(MouseMoveEventArgs e)
        {
            Vector2i mouseCoords = new Vector2i(e.X, e.Y);
            worldCoords = Window.MapPixelToCoords(mouseCoords);

            if (state.boardActive == true) // normal game
            {
                foreach (UiButton button in buttonList) button.Collide((int) worldCoords.X, (int) worldCoords.Y);

                foreach (TestContainer container in elementsContainers)
                    container.MouseMove(worldCoords, ref cursorDockedCard);

                foreach (var playground in playgrounds)
                {
                    playground.Collide((int) worldCoords.X, (int) worldCoords.Y, mousePressed);
                }
                foreach (var hand in hands)
                {
                    hand.Collide((int) worldCoords.X, (int) worldCoords.Y, mousePressed);
                }
            }
            else // message up
            {
                if (message.GetType() == typeof(UIInfo) || message.GetType() == typeof(UIChoice))
                    message.ButtonOK.Collide((int)worldCoords.X, (int)worldCoords.Y);
                if (message.GetType() == typeof(UIChoice))
                    message.ButtonCancel.Collide((int)worldCoords.X, (int)worldCoords.Y);
                if (message.GetType() == typeof(UIDilema))
                    foreach (var card in message.CardList)
                    {
                        card.Collide((int) worldCoords.X, (int) worldCoords.Y);
                    }
            }
        }
        
        protected override void CheckClick(MouseButtonEventArgs e)
        {
            Vector2i mouseCoords = new Vector2i(e.X, e.Y);
            worldCoords = Window.MapPixelToCoords(mouseCoords);
            mousePressed = true;
            if (state.boardActive == true) // normal game
            {
                foreach (UiButton button in buttonList)
                    button.Clicked((int) worldCoords.X, (int) worldCoords.Y, e.Button);

                // TROCHĘ MNIEJ UPOŚLEDZONA FUNKCJA ELEMENTU AKTYWNEGO
                foreach (TestContainer container in elementsContainers) container.Clicked(e, worldCoords, ref cursorDockedCard);
                if (cursorDockedCard != null)
                {
                    if (cursorDockedCard.Origin.GetType() == typeof(TestDeck))
                        foreach (var playground in playgrounds)
                        {
                            playground.OldState = playground.Active;
                            playground.Active = false;
                            deckToPlaygroundLock = true;
                        }
                }
            }
            else // message up
            {
                if (message.GetType() == typeof(UIInfo) || message.GetType() == typeof(UIChoice))
                    message.ButtonOK.Clicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
                if (message.GetType() == typeof(UIChoice))
                    message.ButtonCancel.Clicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
            }
        }

        protected override void CheckUnClick(MouseButtonEventArgs e)
        {
            Vector2i mouseCoords = new Vector2i(e.X, e.Y);
            worldCoords = Window.MapPixelToCoords(mouseCoords);

            if (state.boardActive == true) // normal game
            {
                //BUTTON FUNCTIONS

                bool button0Clicked = buttonList[0].UnClicked((int) worldCoords.X, (int) worldCoords.Y, e.Button);
                if (button0Clicked == true)
                {
                    if (playgrounds[1].CardList.Count > 0)
                    {
                        TestCard cardDummy = playgrounds[1].CardList[0];
                        playgrounds[1].RemoveCard(cardDummy);
                        graveyard.AddCard(cardDummy);
                    }
                }

                bool button1Clicked = buttonList[1].UnClicked((int) worldCoords.X, (int) worldCoords.Y, e.Button);
                if (button1Clicked == true)
                {
                    Random rndX = new Random();
                    Random rndY = new Random();
                    int texX = rndX.Next(0, 7);
                    int texY = rndY.Next(0, 9);
                    deck.AddCard(0, texX, texY);
                }

                bool button2Clicked = buttonList[2].UnClicked((int) worldCoords.X, (int) worldCoords.Y, e.Button);
                if (button2Clicked == true)
                {
                    // message start
                    state.boardStableState = false;
                }

                foreach (TestContainer container in elementsContainers) container.UnClicked(e, worldCoords);

                if (eventDenture.generalPhase == 2) foreach (var playground in playgrounds)
                {
                    if (playground.Active == true)
                    {
                        TestCard cardDummy = null;
                        foreach (var card in playground.CardList)
                        {
                            if (card.Collide((int)worldCoords.X, (int)worldCoords.Y)) cardDummy = card;
                        }
                        if (cardDummy != null)
                        {
                            WarlordPlayerAction.DistrictCardToDestroy warlordCard = new WarlordPlayerAction.DistrictCardToDestroy();
                            warlordCard.Player = gameLogic.Players[playgrounds.IndexOf(playground)];
                            warlordCard.DistrictCard = gameLogic.Players[playgrounds.IndexOf(playground)].Table.Find(x => x.Id == cardDummy.id);
                            eventDenture.GeneralReturnChoice(warlordCard);
                        }
                    }
                }

                // UPUSZCZANIE KARD DO PÓL
                if (cursorDockedCard != null)
                {
                    foreach (var playground in playgrounds)
                    {
                        bool cardWasDropped = playground.CardDroppedEvent(cursorDockedCard, worldCoords, mousePressed);
                        if (cardWasDropped && eventDenture.pickUpState == 1)
                        {
                            int index = gameLogic.Players[playgrounds.IndexOf(playground)].Hand.FindIndex(x => x.Id == cursorDockedCard.id);
                            eventDenture.ReturnChosenCardIndex(index);
                        }
                    }
                    foreach (var hand in hands)
                    {
                        bool cardWasDropped = hand.CardDroppedEvent(cursorDockedCard, worldCoords, mousePressed);
                        
                    }
                    cursorDockedCard = null;
                }
            }
            else // message up
            {
                if (message.GetType() == typeof(UIInfo) || message.GetType() == typeof(UIChoice))
                {
                    bool buttonOkClicked = message.ButtonOK.UnClicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
                    if (buttonOkClicked == true)
                    {
                        eventDenture.ReturnChoice(true);
                        state.boardStableState = true;
                    }
                }
                if (message.GetType() == typeof(UIChoice))
                {
                    bool buttonCancelClicked = message.ButtonCancel.UnClicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
                    if (buttonCancelClicked == true)
                    {
                        eventDenture.ReturnChoice(false);
                        state.boardStableState = true;
                    }
                }
                if (message.GetType() == typeof(UIDilema))
                {
                    int i = 0;
                    foreach (var card in message.CardList)
                    {
                        bool cardUnclicked = card.Clicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
                        if (cardUnclicked == true)
                        {
                            if (eventDenture.generalPhase == 1 ) eventDenture.GeneralChooseCardToDestroy(i);
                            else if (eventDenture.generalPhase == 2) throw new NotImplementedException();
                            else eventDenture.ReturnChosenCardIndex(i);
                            state.boardStableState = true;
                        }
                        i++;
                    }
                }  
            }

            mousePressed = false;
            if (deckToPlaygroundLock)
            {
                deckToPlaygroundLock = false;
                foreach (var playground in playgrounds)
                {
                    playground.Active = playground.OldState;
                }
            }
        }

        protected override void LoadContent()
        {
            // load some graphics

            tileset = new Texture("../../Resources/DungeonTileset.png");
            buttonFace = new Texture("../../Resources/btn_play.bmp");
            deckTexture = new Texture("../../Resources/cdeck.gif");
        }

        protected void GameLogicInit()
        {
            
        }

        protected override void Initialize()
        {
            // init game model thread
            playerCount = 6;
            synchronizationController = new SynchronizationController(false);
            state = new BoardState();
            eventDenture = new EventDenture(state, message, synchronizationController, this);
            userAdapter = new UserAdapter(synchronizationController, eventDenture);
            deckFactory = new GameTestDecksFactory();
            gameLogic = new Game(playerCount, userAdapter, deckFactory.Create(), synchronizationController);
            
            Thread threadModel = new Thread(new ThreadStart(gameLogic.StartNewRound));
            
            //while (!threadModel.IsAlive) ;

            
            state.boardActive = true;
            state.boardStableState = true;

            // init UI objects

            int districtCardWidth = 100;
            int districtCardHeight = 100;

            aether = new TestAether();
            deck = new TestDeck(636, 10, districtCardWidth, 100, deckTexture, districtCardWidth, districtCardHeight);
            deck.Active = false;
            graveyard = new TestDeck(892, 10, districtCardWidth, 100, deckTexture, districtCardWidth, districtCardHeight);
            graveyard.Active = false;
            graveyard.FlipDeck();

            Texture panelTexture = new Texture("../../Resources/pdeckhd.png");
            int panelTextureWidth = 144;
            int panelTextureHeight = 200;
            panels.Add(new UiPlayerPanel(643, 150, 100, 223, panelTexture, panelTextureWidth, panelTextureHeight, new Vector2f(0,1)));
            panels.Add(new UiPlayerPanel(643, 400, 100, 223, panelTexture, panelTextureWidth, panelTextureHeight, new Vector2f(1, 1)));
            panels.Add(new UiPlayerPanel(643, 650, 100, 223, panelTexture, panelTextureWidth, panelTextureHeight, new Vector2f(2, 1)));
            panels.Add(new UiPlayerPanel(857, 150, 100, 223, panelTexture, panelTextureWidth, panelTextureHeight, new Vector2f(3, 1)));
            panels.Add(new UiPlayerPanel(857, 400, 100, 223, panelTexture, panelTextureWidth, panelTextureHeight, new Vector2f(4, 1)));
            panels.Add(new UiPlayerPanel(857, 650, 100, 223, panelTexture, panelTextureWidth, panelTextureHeight, new Vector2f(5, 1)));

            hands.Add(new TestHand(60, 150, 550, 100, deckTexture, districtCardWidth, districtCardHeight, 0));
            hands.Add(new TestHand(60, 400, 550, 100, deckTexture, districtCardWidth, districtCardHeight, 0));
            hands.Add(new TestHand(60, 650, 550, 100, deckTexture, districtCardWidth, districtCardHeight, 0));
            hands.Add(new TestHand(990, 150, 550, 100, deckTexture, districtCardWidth, districtCardHeight, 0));
            hands.Add(new TestHand(990, 400, 550, 100, deckTexture, districtCardWidth, districtCardHeight, 0));
            hands.Add(new TestHand(990, 650, 550, 100, deckTexture, districtCardWidth, districtCardHeight, 0));

            playgrounds.Add(new TestPlayground(60, 273, 550, 100, deckTexture, districtCardWidth, districtCardHeight));
            playgrounds.Add(new TestPlayground(60, 523, 550, 100, deckTexture, districtCardWidth, districtCardHeight));
            playgrounds.Add(new TestPlayground(60, 773, 550, 100, deckTexture, districtCardWidth, districtCardHeight));
            playgrounds.Add(new TestPlayground(990, 273, 550, 100, deckTexture, districtCardWidth, districtCardHeight));
            playgrounds.Add(new TestPlayground(990, 523, 550, 100, deckTexture, districtCardWidth, districtCardHeight));
            playgrounds.Add(new TestPlayground(990, 773, 550, 100, deckTexture, districtCardWidth, districtCardHeight));

            // add references to reference container

            elementsContainers.Add(aether);
            elementsContainers.Add(deck);
            elementsContainers.Add(graveyard);

            foreach (var playground in playgrounds)
            {
                elementsContainers.Add(playground);
            }
            foreach (var hand in hands)
            {
                elementsContainers.Add(hand);
            }

            // init buttons

            buttonList.Add(new UIPrimitiveButton(10, 10, 180, 40, Color.Magenta, Color.Green, "Zniszcz kartę"));
            buttonList.Add(new UIPrimitiveButton(200, 10, 180, 40, Color.Blue, Color.Cyan, "Dodaj kartę"));
            buttonList.Add(new UIPrimitiveButton(10, 60, 180, 40, Color.Blue, Color.Cyan, "Message"));
            //buttonList.Add(new UIGlyphButton(320, 140, 95, 53, buttonFace));
            //buttonList.Add(new UIGlyphButton(500, 140, 95, 53, buttonFace));

            //UIGlyphButton buttonOff = new UIGlyphButton(320, 240, 95, 53, buttonFace);
            //buttonOff.state = -1;
            //buttonList.Add(buttonOff);

            //for (int i = 0; i < 13; i++)
            //{
            //    aether.AddCard(new _test_Card(20 + 15 * i, 20, districtCardWidth, 100, deckTexture, i, 3, aether, true));
            //    aether.AddCard(new _test_Card(20 + 15 * i, 130, districtCardWidth, 100, deckTexture, i, 2, aether, true));
            //    aether.AddCard(new _test_Card(20 + 15 * i, 240, districtCardWidth, 100, deckTexture, i, 1, aether, false));
            //    aether.AddCard(new _test_Card(20 + 15 * i, 350, districtCardWidth, 100, deckTexture, i, 0, aether, true));
            //}

            // some random cards

            //deck.AddCard(0, 2,4);
            //deck.AddCard(0, 2, 5);
            //deck.AddCard(0, 2, 6);
            //deck.AddCard(0, 2, 7);
            //deck.AddCard(0, 2, 8);
            //deck.AddCard(0, 2, 9);
            //deck.AddCard(0, 3, 0);
            //deck.AddCard(0, 3, 1);
            //deck.AddCard(0, 3, 2);
            //deck.AddCard(0, 3, 3);
            //deck.AddCard(0, 3, 4);
            //deck.AddCard(0, 3, 5);
            //deck.AddCard(0, 3, 6);
            //deck.AddCard(0, 3, 7);
            //deck.AddCard(0, 3, 8);
            //deck.AddCard(0, 3, 9);

            //hands[0].AddCard(0, 1, 1);
            //hands[1].AddCard(0, 1, 2);
            //hands[2].AddCard(0, 1, 3);
            //hands[3].AddCard(0, 1, 4);
            //hands[4].AddCard(0, 1, 5);
            //hands[5].AddCard(0, 1, 6);
            //hands[0].AddCard(0, 1, 7);
            //hands[1].AddCard(0, 1, 8);
            //hands[2].AddCard(0, 1, 9);
            //hands[3].AddCard(0, 2, 1);
            //hands[4].AddCard(0, 2, 2);
            //hands[5].AddCard(0, 2, 3);

            // some init attributes


            hands[0].Active = true;
            hands[1].Active = true;
            hands[2].Active = true;
            hands[3].Active = true;
            hands[4].Active = true;
            hands[5].Active = true;

            playgrounds[0].Active = true;
            playgrounds[1].Active = true;
            playgrounds[2].Active = true;
            playgrounds[3].Active = true;
            playgrounds[4].Active = true;
            playgrounds[5].Active = true;

            // starting normal game state
            threadModel.Start();

            synchronizationController.ResetEventController.WaitOne();

        }

        protected override void Tick()
        {
            

            if (state.boardActive && state.boardStableState)
            {
                // normal game

                

            }
            else if (state.boardActive && state.boardStableState == false)
            {
                // init message

            }
            else if (state.boardActive == false && state.boardStableState == false)
            {

                // message duration

            }
            else if (state.boardActive == false && state.boardStableState)
            {
                // destroy message
                message = null;
                state.boardActive = true;
            }

            // Update panels



        }

        protected override void Render()
        {
            foreach (TestContainer container in elementsContainers) Window.Draw(container);    

            foreach (UiButton button in buttonList)
            {
                Window.Draw(button);
            }

            foreach (var panel in panels)
            {
                Window.Draw(panel);
            }

            foreach (TestContainer container in elementsContainers)
            {
                foreach (TestCard card in container.CardList)
                {
                    if (card != cursorDockedCard) Window.Draw(card);
                }
            }
            if (cursorDockedCard != null)
            {
                Window.Draw(cursorDockedCard);
            }

            if (state.boardActive == false && state.boardStableState == false)
            {
                Window.Draw(message);
                if (message.GetType() == typeof(UIInfo) || message.GetType() == typeof(UIChoice))
                    Window.Draw(message.ButtonOK);
                if (message.GetType() == typeof(UIChoice))
                    Window.Draw(message.ButtonCancel);
                if (message.GetType() == typeof(UIDilema))
                    foreach (var card in message.CardList)
                    {
                        Window.Draw(card);
                    }
            }
        }
    }
}
