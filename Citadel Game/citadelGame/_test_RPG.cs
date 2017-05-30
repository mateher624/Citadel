using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Threading;
//using Citadel_v1;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using Color = SFML.Graphics.Color;


namespace citadelGame
{
    class TestRpg : TestGame
    {
        Vector2f worldCoords;
        private bool boardActive;
        private bool boardStableState;

        //_test_Tilemap map;
        Texture tileset;
        Texture buttonFace;
        Texture deckTexture;
        List<UiButton> buttonList;
        //List<_test_Card> cardSpriteList;
        List<TestContainer> elementsContainers;

        private List<TestHand> hands;
        private List<TestPlayground> playgrounds;
        private List<UiPlayerPanel> panels;

        private TestAether aether;

        private TestDeck deck;

        private UiDilema message;
        private string messageTitle;
        private string messageCaption;

        private TestDeck graveyard;
        private TestCard cursorDockedCard;
        bool mousePressed = false;

        private bool deckToPlaygroundLock;
        private bool foreignDeckOrPlaygroundLock;

        //private Game gameLogic;
        private AutoResetEvent resetEventController;
        private AutoResetEvent resetEventModel;



        public TestRpg() : base(1600, 900, "Citadel Game Alpha", Color.Cyan)
        {
            buttonList = new List<UiButton>();
            elementsContainers = new List<TestContainer>();

            hands = new List<TestHand>();
            playgrounds = new List<TestPlayground>();
            panels = new List<UiPlayerPanel>();
        }

        protected override void CheckCollide(MouseMoveEventArgs e)
        {
            Vector2i mouseCoords = new Vector2i(e.X, e.Y);
            worldCoords = Window.MapPixelToCoords(mouseCoords);

            if (boardActive == true) // normal game
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
                message.ButtonCancel.Collide((int)worldCoords.X, (int)worldCoords.Y);
                message.ButtonOk.Collide((int)worldCoords.X, (int)worldCoords.Y);
            }
        }
        
        protected override void CheckClick(MouseButtonEventArgs e)
        {
            Vector2i mouseCoords = new Vector2i(e.X, e.Y);
            worldCoords = Window.MapPixelToCoords(mouseCoords);
            mousePressed = true;
            if (boardActive == true) // normal game
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
                message.ButtonCancel.Clicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
                message.ButtonOk.Clicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
                foreach (var card in message.CardList)
                {
                    //card.Clicked((int) worldCoords.X, (int) worldCoords.Y, e.Button);
                }
            }
        }

        protected override void CheckUnClick(MouseButtonEventArgs e)
        {
            Vector2i mouseCoords = new Vector2i(e.X, e.Y);
            worldCoords = Window.MapPixelToCoords(mouseCoords);

            if (boardActive == true) // normal game
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
                    int texX = rndX.Next(0, 13);
                    int texY = rndY.Next(0, 4);
                    deck.AddCard(texX, texY);
                }

                bool button2Clicked = buttonList[2].UnClicked((int) worldCoords.X, (int) worldCoords.Y, e.Button);
                if (button2Clicked == true)
                {
                    // message start
                    boardStableState = false;
                }

                foreach (TestContainer container in elementsContainers) container.UnClicked(e, worldCoords);

                // UPUSZCZANIE KARD DO PÓL
                if (cursorDockedCard != null)
                {
                    foreach (var playground in playgrounds)
                    {
                        playground.CardDroppedEvent(cursorDockedCard, worldCoords, mousePressed);
                    }
                    foreach (var hand in hands)
                    {
                        hand.CardDroppedEvent(cursorDockedCard, worldCoords, mousePressed);
                    }
                    cursorDockedCard = null;
                }
            }
            else // message up
            {
                bool buttonOkClicked = message.ButtonCancel.UnClicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
                if (buttonOkClicked == true)
                {
                    boardStableState = true;
                }

                bool buttonCancelClicked = message.ButtonOk.UnClicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
                if (buttonCancelClicked == true)
                {
                    boardStableState = true;
                }
                foreach (var card in message.CardList)
                {
                    bool cardUnclicked = card.Clicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
                    if (cardUnclicked == true)
                    {
                        boardStableState = true;
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
            deckTexture = new Texture("../../Resources/deck.gif");
        }

        protected override void Initialize()
        {
            // init game model thread

            resetEventController = new AutoResetEvent(false);
            resetEventModel = new AutoResetEvent(false);
            //Thread threadModel = new Thread(new ThreadStart(gameLogic = new Game()));
            //threadModel.Start();
            //while (!threadModel.IsAlive) ;


            // init UI objects

            aether = new TestAether();
            deck = new TestDeck(636, 10, 72, 100, deckTexture, 72, 100);
            deck.Active = true;
            graveyard = new TestDeck(892, 10, 72, 100, deckTexture, 72, 100);
            graveyard.Active = false;
            graveyard.FlipDeck();

            panels.Add(new UiPlayerPanel(643, 150, 100, 223, deckTexture, 72, 100));
            panels.Add(new UiPlayerPanel(643, 400, 100, 223, deckTexture, 72, 100));
            panels.Add(new UiPlayerPanel(643, 650, 100, 223, deckTexture, 72, 100));
            panels.Add(new UiPlayerPanel(857, 150, 100, 223, deckTexture, 72, 100));
            panels.Add(new UiPlayerPanel(857, 400, 100, 223, deckTexture, 72, 100));
            panels.Add(new UiPlayerPanel(857, 650, 100, 223, deckTexture, 72, 100));

            hands.Add(new TestHand(60, 150, 550, 100, deckTexture, 72, 100, 0));
            hands.Add(new TestHand(60, 400, 550, 100, deckTexture, 72, 100, 0));
            hands.Add(new TestHand(60, 650, 550, 100, deckTexture, 72, 100, 0));
            hands.Add(new TestHand(990, 150, 550, 100, deckTexture, 72, 100, 0));
            hands.Add(new TestHand(990, 400, 550, 100, deckTexture, 72, 100, 0));
            hands.Add(new TestHand(990, 650, 550, 100, deckTexture, 72, 100, 0));

            playgrounds.Add(new TestPlayground(60, 273, 550, 100, deckTexture, 72, 100));
            playgrounds.Add(new TestPlayground(60, 523, 550, 100, deckTexture, 72, 100));
            playgrounds.Add(new TestPlayground(60, 773, 550, 100, deckTexture, 72, 100));
            playgrounds.Add(new TestPlayground(990, 273, 550, 100, deckTexture, 72, 100));
            playgrounds.Add(new TestPlayground(990, 523, 550, 100, deckTexture, 72, 100));
            playgrounds.Add(new TestPlayground(990, 773, 550, 100, deckTexture, 72, 100));

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

            buttonList.Add(new UiPrimitiveButton(10, 10, 180, 40, Color.Magenta, Color.Green, "Zniszcz kartę"));
            buttonList.Add(new UiPrimitiveButton(200, 10, 180, 40, Color.Blue, Color.Cyan, "Dodaj kartę"));
            buttonList.Add(new UiPrimitiveButton(10, 60, 180, 40, Color.Blue, Color.Cyan, "Message"));
            //buttonList.Add(new UIGlyphButton(320, 140, 95, 53, buttonFace));
            //buttonList.Add(new UIGlyphButton(500, 140, 95, 53, buttonFace));

            //UIGlyphButton buttonOff = new UIGlyphButton(320, 240, 95, 53, buttonFace);
            //buttonOff.state = -1;
            //buttonList.Add(buttonOff);

            //for (int i = 0; i < 13; i++)
            //{
            //    aether.AddCard(new _test_Card(20 + 15 * i, 20, 72, 100, deckTexture, i, 3, aether, true));
            //    aether.AddCard(new _test_Card(20 + 15 * i, 130, 72, 100, deckTexture, i, 2, aether, true));
            //    aether.AddCard(new _test_Card(20 + 15 * i, 240, 72, 100, deckTexture, i, 1, aether, false));
            //    aether.AddCard(new _test_Card(20 + 15 * i, 350, 72, 100, deckTexture, i, 0, aether, true));
            //}

            // some random cards

            deck.AddCard(1,3);
            deck.AddCard(4,2);
            deck.AddCard(6,1);
            deck.AddCard(10,2);
            deck.AddCard(0,0);
            deck.AddCard(12,3);
            deck.AddCard(5,1);
            deck.AddCard(6,0);
            deck.AddCard(8,2);
            deck.AddCard(3,3);
            deck.AddCard(3,2);
            deck.AddCard(4,1);
            deck.AddCard(11,0);
            deck.AddCard(0,1);
            deck.AddCard(12,1);
            deck.AddCard(11,1);

            hands[0].AddCard(1, 3);
            hands[2].AddCard(1, 3);
            hands[4].AddCard(1, 3);

            // some init attributes

            hands[1].FlipHand();
            hands[3].FlipHand();
            hands[5].FlipHand();
                      
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

            boardActive = true;
            boardStableState = true;
        }

        protected override void Tick()
        {
            if (boardActive && boardStableState)
            {
                // normal game

                for (int i = 0; i < panels.Count; i++)
                {
                    panels[i].SetInfo(hands[i].CardList.Count, playgrounds[i].CardList.Count, 1);
                }

            }
            else if (boardActive && boardStableState == false)
            {
                // init message
                List<TestCard> dilemaCardList = new List<TestCard>();
                dilemaCardList.Add(new TestCard(20 + 15 * 1, 20, 72, 100, deckTexture, 1, 3, aether, true));
                dilemaCardList.Add(new TestCard(20 + 15 * 1, 130, 72, 100, deckTexture, 1, 2, aether, true));
                dilemaCardList.Add(new TestCard(20 + 15 * 1, 240, 72, 100, deckTexture, 1, 1, aether, false));
                dilemaCardList.Add(new TestCard(20 + 15 * 1, 350, 72, 100, deckTexture, 1, 0, aether, true));
                dilemaCardList.Add(new TestCard(20 + 15 * 1, 350, 72, 100, deckTexture, 2, 0, aether, true));
                dilemaCardList.Add(new TestCard(20 + 15 * 1, 350, 72, 100, deckTexture, 3, 0, aether, true));
                dilemaCardList.Add(new TestCard(20 + 15 * 1, 350, 72, 100, deckTexture, 3, 0, aether, true));
                message = new UiDilema(1600/2-300, 900/2-200, 600, 400, "Test Nazwy", "test pola tekstowego", 1600, 900, dilemaCardList);
                boardActive = false;
            }
            else if (boardActive == false && boardStableState == false)
            {

                // message duration

            }
            else if (boardActive == false && boardStableState)
            {
                // destroy message
                message = null;
                boardActive = true;
            }


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

            if (boardActive == false && boardStableState == false)
            {
                Window.Draw(message); 
                Window.Draw(message.ButtonCancel);
                Window.Draw(message.ButtonOk);
                foreach (var card in message.CardList)
                {
                    Window.Draw(card);
                }
            }
        }
    }
}
