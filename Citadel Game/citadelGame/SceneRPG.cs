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
    class SceneRPG : Scene
    {
        Vector2f worldCoords;

        private Thread threadModel;

        public BoardState state;

        //_test_Tilemap map;
        Texture tileset;
        Texture buttonFace;
        Texture deckTexture;
        List<UIButton> buttonList;
        //List<_test_Card> cardSpriteList;
        List<UIContainer> elementsContainers;

        public List<UIHand> hands;
        public List<UIPlayground> playgrounds;
        public List<UIPlayerPanel> panels;

        private UIAether aether;

        public UIDeck deck;

        private UIImage backgroundImage;
        public UIMessage message;
        private string messageTitle;
        private string messageCaption;

        private EventDenture eventDenture;
        public UserAdapter userAdapter;
        private IDecksFactory deckFactory;

        public UIDeck graveyard;
        private UICard cursorDockedCard;
        private bool mousePressed = false;

        private bool deckToPlaygroundLock;
        private bool foreignDeckOrPlaygroundLock;

        private Game gameLogic;

        public SynchronizationController synchronizationController;

        private int playerCount;

        public SceneRPG() : base(1600, 900, "Citadel Game Alpha", Color.Cyan)
        {
            buttonList = new List<UIButton>();
            elementsContainers = new List<UIContainer>();

            hands = new List<UIHand>();
            playgrounds = new List<UIPlayground>();
            panels = new List<UIPlayerPanel>();
        }

        // Message generation and some shit functions
        //
        //
        // End

        protected override void CheckCollide(MouseMoveEventArgs e)
        {
            Vector2i mouseCoords = new Vector2i(e.X, e.Y);
            worldCoords = Window.MapPixelToCoords(mouseCoords);

            foreach (UIButton button in buttonList) button.Collide((int)worldCoords.X, (int)worldCoords.Y);

            if (state.boardActive == true) // normal game
            {
                

                foreach (UIContainer container in elementsContainers)
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
                if (message.GetType() == typeof(UIDilema) || message.GetType() == typeof(UIChoice))
                {
                    foreach (var card in message.CardList)
                    {
                        card.Collide((int)worldCoords.X, (int)worldCoords.Y);
                    }
                    message.ButtonToggle.Collide((int)worldCoords.X, (int)worldCoords.Y);
                }
            }
        }
        
        protected override void CheckClick(MouseButtonEventArgs e)
        {
            Vector2i mouseCoords = new Vector2i(e.X, e.Y);
            worldCoords = Window.MapPixelToCoords(mouseCoords);
            mousePressed = true;

            foreach (UIButton button in buttonList)
                button.Clicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);

            if (state.boardActive == true) // normal game
            {
                

                // TROCHĘ MNIEJ UPOŚLEDZONA FUNKCJA ELEMENTU AKTYWNEGO
                foreach (UIContainer container in elementsContainers) container.Clicked(e, worldCoords, ref cursorDockedCard);
                if (cursorDockedCard != null)
                {
                    if (cursorDockedCard.Origin.GetType() == typeof(UIDeck))
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
                if (message.GetType() == typeof(UIDilema) || message.GetType() == typeof(UIChoice))
                    message.ButtonToggle.Clicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
            }
        }

        protected override void CheckUnClick(MouseButtonEventArgs e)
        {
            Vector2i mouseCoords = new Vector2i(e.X, e.Y);
            worldCoords = Window.MapPixelToCoords(mouseCoords);

            bool button0Clicked = buttonList[0].UnClicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
            if (button0Clicked == true)
            {
                OReturn = true;
                Window.Close();
            }

            if (state.boardActive == true) // normal game
            {
                //BUTTON FUNCTIONS

                //TUTAJ JEST FOREACH
                elementsContainers.ForEach((element) => element.UnClicked(e, worldCoords) );
                //foreach (UIContainer container in elementsContainers) container.UnClicked(e, worldCoords);

                if (eventDenture.generalPhase == 2) foreach (var playground in playgrounds)
                {
                    if (playground.Active == true)
                    {
                        UICard cardDummy = null;
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
                            if (index == -1) throw new NotImplementedException();
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
                if (message.GetType() == typeof(UIDilema) || message.GetType() == typeof(UIChoice))
                {
                    int i = 0;
                    if (message.Visible)
                    {
                        foreach (var card in message.CardList)
                        {
                            bool cardUnclicked = card.Clicked((int) worldCoords.X, (int) worldCoords.Y, e.Button);
                            if (cardUnclicked == true)
                            {
                                if (eventDenture.generalPhase == 1) eventDenture.GeneralChooseCardToDestroy(i);
                                else if (eventDenture.generalPhase == 2) throw new NotImplementedException();
                                else eventDenture.ReturnChosenCardIndex(i);
                                state.boardStableState = true;
                            }
                            i++;
                        }
                    }
                    bool buttonToggleClicked = message.ButtonToggle.UnClicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
                    if (buttonToggleClicked == true)
                    {
                        if (message.Visible) message.Visible = false;
                        else message.Visible = true;
                    }
                }  
            }

            mousePressed = false;
            if (deckToPlaygroundLock)
            {
                deckToPlaygroundLock = false;
                playgrounds.ForEach((element) => element.Active = element.OldState);
                /*foreach (var playground in playgrounds)
                {
                    playground.Active = playground.OldState;
                }*/
            }
        }

        protected override void LoadContent()
        {
            // load some graphics

            tileset = new Texture("../../Resources/DungeonTileset.png");
            buttonFace = new Texture("../../Resources/sbutton.png");
            deckTexture = new Texture("../../Resources/cdeck.gif");

            backgroundImage = new UIImage(0,0, 1600, 900, new Texture("../../Resources/background.png"));
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
            
            threadModel = new Thread(new ThreadStart(gameLogic.StartNewRound));
            
            //while (!threadModel.IsAlive) ;

            
            state.boardActive = true;
            state.boardStableState = true;

            // init UI objects

            int districtCardWidth = 100;
            int districtCardHeight = 100;

            aether = new UIAether();
            deck = new UIDeck(644, 19, districtCardWidth, 100, deckTexture, districtCardWidth, districtCardHeight);
            deck.Active = false;
            graveyard = new UIDeck(858, 19, districtCardWidth, 100, deckTexture, districtCardWidth, districtCardHeight);
            graveyard.Active = false;
            graveyard.FlipDeck();

            Texture panelTexture = new Texture("../../Resources/pdeckhd.png");
            int panelTextureWidth = 144;
            int panelTextureHeight = 200;
            panels.Add(new UIPlayerPanel(643, 150, 100, 223, panelTexture, panelTextureWidth, panelTextureHeight, new Vector2f(0,1)));
            panels.Add(new UIPlayerPanel(643, 400, 100, 223, panelTexture, panelTextureWidth, panelTextureHeight, new Vector2f(1, 1)));
            panels.Add(new UIPlayerPanel(643, 650, 100, 223, panelTexture, panelTextureWidth, panelTextureHeight, new Vector2f(2, 1)));
            panels.Add(new UIPlayerPanel(857, 150, 100, 223, panelTexture, panelTextureWidth, panelTextureHeight, new Vector2f(3, 1)));
            panels.Add(new UIPlayerPanel(857, 400, 100, 223, panelTexture, panelTextureWidth, panelTextureHeight, new Vector2f(4, 1)));
            panels.Add(new UIPlayerPanel(857, 650, 100, 223, panelTexture, panelTextureWidth, panelTextureHeight, new Vector2f(5, 1)));

            hands.Add(new UIHand(60, 150, 550, 100, deckTexture, districtCardWidth, districtCardHeight, 0));
            hands.Add(new UIHand(60, 400, 550, 100, deckTexture, districtCardWidth, districtCardHeight, 0));
            hands.Add(new UIHand(60, 650, 550, 100, deckTexture, districtCardWidth, districtCardHeight, 0));
            hands.Add(new UIHand(990, 150, 550, 100, deckTexture, districtCardWidth, districtCardHeight, 0));
            hands.Add(new UIHand(990, 400, 550, 100, deckTexture, districtCardWidth, districtCardHeight, 0));
            hands.Add(new UIHand(990, 650, 550, 100, deckTexture, districtCardWidth, districtCardHeight, 0));

            playgrounds.Add(new UIPlayground(60, 273, 550, 100, deckTexture, districtCardWidth, districtCardHeight));
            playgrounds.Add(new UIPlayground(60, 523, 550, 100, deckTexture, districtCardWidth, districtCardHeight));
            playgrounds.Add(new UIPlayground(60, 773, 550, 100, deckTexture, districtCardWidth, districtCardHeight));
            playgrounds.Add(new UIPlayground(990, 273, 550, 100, deckTexture, districtCardWidth, districtCardHeight));
            playgrounds.Add(new UIPlayground(990, 523, 550, 100, deckTexture, districtCardWidth, districtCardHeight));
            playgrounds.Add(new UIPlayground(990, 773, 550, 100, deckTexture, districtCardWidth, districtCardHeight));

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

            buttonList.Add(new UIGlyphButton(10, 10, 195, 45, buttonFace, "Menu główne", 55));

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
            if (state.boardActive == false && state.boardStableState)
            {
                // destroy message
                message = null;
                state.boardActive = true;
            }

            // Update panels
        }

        public void Abort()
        {
            //Emergency Thread Aborter
            threadModel.Abort();
        }

        protected override void Render()
        {
            Window.Draw(backgroundImage);

            foreach (UIContainer container in elementsContainers) Window.Draw(container);    

            

            foreach (var panel in panels)
            {
                Window.Draw(panel);
            }

            foreach (UIContainer container in elementsContainers)
            {
                /*if (container.GetType() == typeof(TestDeck))
                {
                    if (container.CardList.Count != 0) Window.Draw(container.CardList[0]);
                }
                else */foreach (UICard card in container.CardList)
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
                if ((message.GetType() == typeof(UIInfo) || message.GetType() == typeof(UIChoice)) && message.Visible)
                    Window.Draw(message.ButtonOK);
                if ((message.GetType() == typeof(UIChoice)) && message.Visible)
                    Window.Draw(message.ButtonCancel);
                if ((message.GetType() == typeof(UIDilema)  || message.GetType() == typeof(UIChoice) || message.GetType() == typeof(UIInfo)) && message.Visible)
                    foreach (var card in message.CardList)
                    {
                        Window.Draw(card);
                    }
            }

            foreach (UIButton button in buttonList)
            {
                Window.Draw(button);
            }
        }
    }
}
