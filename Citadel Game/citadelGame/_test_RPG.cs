using System;
using System.CodeDom;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.Window;
using SFML.System;


namespace citadelGame
{
    class _test_RPG : _test_Game
    {
        Vector2f worldCoords;
        private bool boardActive;
        private bool boardStableState;

        //_test_Tilemap map;
        Texture tileset;
        Texture buttonFace;
        Texture deckTexture;
        List<UIButton> buttonList;
        //List<_test_Card> cardSpriteList;
        List<_test_Container> elementsContainers;

        private List<_test_Hand> hands;
        private List<_test_Playground> playgrounds;
        private List<UIPlayerPanel> panels;

        private _test_Aether aether;

        private _test_Deck deck;

        private UIMessage message;
        private string messageTitle;
        private string messageCaption;

        private _test_Deck graveyard;
        private _test_Card cursorDockedCard;
        bool mousePressed = false;

        private bool deckToPlaygroundLock;
        private bool foreignDeckOrPlaygroundLock;


        public _test_RPG() : base(1600, 900, "Citadel Game Alpha", Color.Cyan)
        {
            buttonList = new List<UIButton>();
            elementsContainers = new List<_test_Container>();

            hands = new List<_test_Hand>();
            playgrounds = new List<_test_Playground>();
            panels = new List<UIPlayerPanel>();
        }

        protected override void CheckCollide(MouseMoveEventArgs e)
        {
            if (boardActive == true)
            {
                Vector2i mouseCoords = new Vector2i(e.X, e.Y);
                worldCoords = window.MapPixelToCoords(mouseCoords);
                foreach (UIButton button in buttonList) button.Collide((int) worldCoords.X, (int) worldCoords.Y);

                foreach (_test_Container container in elementsContainers)
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
            else
            {
                message.buttonCANCEL.Collide((int)worldCoords.X, (int)worldCoords.Y);
                message.buttonOK.Collide((int)worldCoords.X, (int)worldCoords.Y);
            }
        }
        
        protected override void CheckClick(MouseButtonEventArgs e)
        {
            Vector2i mouseCoords = new Vector2i(e.X, e.Y);
            worldCoords = window.MapPixelToCoords(mouseCoords);
            mousePressed = true;
            if (boardActive == true)
            {
                foreach (UIButton button in buttonList)
                    button.Clicked((int) worldCoords.X, (int) worldCoords.Y, e.Button);

                // TROCHĘ MNIEJ UPOŚLEDZONA FUNKCJA ELEMENTU AKTYWNEGO
                foreach (_test_Container container in elementsContainers) container.Clicked(e, worldCoords, ref cursorDockedCard);
                if (cursorDockedCard != null)
                {
                    if (cursorDockedCard.origin.GetType() == typeof(_test_Deck))
                        foreach (var playground in playgrounds)
                        {
                            playground.oldState = playground.active;
                            playground.active = false;
                            deckToPlaygroundLock = true;
                        }
                }
            }
            else
            {
                message.buttonCANCEL.Clicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
                message.buttonOK.Clicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
            }
        }

        protected override void CheckUnClick(MouseButtonEventArgs e)
        {
            Vector2i mouseCoords = new Vector2i(e.X, e.Y);
            worldCoords = window.MapPixelToCoords(mouseCoords);

            if (boardActive == true)
            {
                //BUTTON FUNCTIONS


                bool button0Clicked = buttonList[0].UnClicked((int) worldCoords.X, (int) worldCoords.Y, e.Button);
                if (button0Clicked == true)
                {
                    if (playgrounds[1].cardList.Count > 0)
                    {
                        _test_Card cardDummy = playgrounds[1].cardList[0];
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

                foreach (_test_Container container in elementsContainers) container.UnClicked(e, worldCoords);

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
            else
            {
                bool buttonOKClicked = message.buttonCANCEL.UnClicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
                if (buttonOKClicked == true)
                {
                    boardStableState = true;
                }

                bool buttonCancelClicked = message.buttonOK.UnClicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
                if (buttonCancelClicked == true)
                {
                    boardStableState = true;
                }
            }
            mousePressed = false;
            if (deckToPlaygroundLock)
            {
                deckToPlaygroundLock = false;
                foreach (var playground in playgrounds)
                {
                    playground.active = playground.oldState;
                }
            }
        }

        protected override void LoadContent()
        {
            tileset = new Texture("../../Resources/DungeonTileset.png");
            buttonFace = new Texture("../../Resources/btn_play.bmp");
            deckTexture = new Texture("../../Resources/deck.gif");
        }

        protected override void Initialize()
        {
            aether = new _test_Aether();
            deck = new _test_Deck(636, 10, 72, 100, deckTexture, 72, 100);
            deck.active = true;
            graveyard = new _test_Deck(892, 10, 72, 100, deckTexture, 72, 100);
            graveyard.active = false;
            graveyard.FlipDeck();

            panels.Add(new UIPlayerPanel(643, 150, 100, 223, deckTexture, 72, 100));
            panels.Add(new UIPlayerPanel(643, 400, 100, 223, deckTexture, 72, 100));
            panels.Add(new UIPlayerPanel(643, 650, 100, 223, deckTexture, 72, 100));
            panels.Add(new UIPlayerPanel(857, 150, 100, 223, deckTexture, 72, 100));
            panels.Add(new UIPlayerPanel(857, 400, 100, 223, deckTexture, 72, 100));
            panels.Add(new UIPlayerPanel(857, 650, 100, 223, deckTexture, 72, 100));

            hands.Add(new _test_Hand(60, 150, 550, 100, deckTexture, 72, 100, 0));
            hands[0].active = true;
            hands[0].AddCard(1, 3);
            hands.Add(new _test_Hand(60, 400, 550, 100, deckTexture, 72, 100, 0));
            hands[1].active = true;
            hands[1].FlipHand();
            hands.Add(new _test_Hand(60, 650, 550, 100, deckTexture, 72, 100, 0));
            hands[2].active = true;
            hands[2].AddCard(1, 3);

            hands.Add(new _test_Hand(990, 150, 550, 100, deckTexture, 72, 100, 0));
            hands[3].active = true;
            hands[3].FlipHand();
            hands.Add(new _test_Hand(990, 400, 550, 100, deckTexture, 72, 100, 0));
            hands[4].active = true;
            hands[4].AddCard(1, 3);
            hands.Add(new _test_Hand(990, 650, 550, 100, deckTexture, 72, 100, 0));
            hands[5].active = true;
            hands[5].FlipHand();

            playgrounds.Add(new _test_Playground(60, 273, 550, 100, deckTexture, 72, 100));
            playgrounds[0].active = true;
            playgrounds.Add(new _test_Playground(60, 523, 550, 100, deckTexture, 72, 100));
            playgrounds[1].active = true;
            playgrounds.Add(new _test_Playground(60, 773, 550, 100, deckTexture, 72, 100));
            playgrounds[2].active = true;

            playgrounds.Add(new _test_Playground(990, 273, 550, 100, deckTexture, 72, 100));
            playgrounds[3].active = true;
            playgrounds.Add(new _test_Playground(990, 523, 550, 100, deckTexture, 72, 100));
            playgrounds[4].active = true;
            playgrounds.Add(new _test_Playground(990, 773, 550, 100, deckTexture, 72, 100));
            playgrounds[5].active = true;

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
            //    aether.AddCard(new _test_Card(20 + 15 * i, 20, 72, 100, deckTexture, i, 3, aether, true));
            //    aether.AddCard(new _test_Card(20 + 15 * i, 130, 72, 100, deckTexture, i, 2, aether, true));
            //    aether.AddCard(new _test_Card(20 + 15 * i, 240, 72, 100, deckTexture, i, 1, aether, false));
            //    aether.AddCard(new _test_Card(20 + 15 * i, 350, 72, 100, deckTexture, i, 0, aether, true));
            //}
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
                    panels[i].SetInfo(hands[i].cardList.Count, playgrounds[i].cardList.Count, 1);
                }
            }
            else if (boardActive && boardStableState == false)
            {
                // init message
                message = new UIMessage(1600/2-300, 900/2-200, 600, 400, "Test Nazwy", "test pola tekstowego", 1600, 900);
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
            foreach (_test_Container container in elementsContainers) window.Draw(container);    

            foreach (UIButton button in buttonList)
            {
                window.Draw(button);
            }

            foreach (var panel in panels)
            {
                window.Draw(panel);
            }

            foreach (_test_Container container in elementsContainers)
            {
                foreach (_test_Card card in container.cardList)
                {
                    if (card != cursorDockedCard) window.Draw(card);
                }
            }
            if (cursorDockedCard != null)
            {
                window.Draw(cursorDockedCard);
            }

            if (boardActive == false && boardStableState == false)
            {
                window.Draw(message); 
                window.Draw(message.buttonCANCEL);
                window.Draw(message.buttonOK);
            }

            //foreach (_test_Card card in aether.cardList)
            //{
            //    //card.SetMouseCoords(worldCoords);
            //    window.Draw(card);
            //}
            //foreach (_test_Card card in playground.cardList)
            //{
            //    //card.SetMouseCoords(worldCoords);
            //    window.Draw(card);
            //}
            //foreach (_test_Card card in hand.cardList)
            //{
            //    //card.SetMouseCoords(worldCoords);
            //    window.Draw(card);
            //}
            //foreach (_test_Card card in deck.cardList)
            //{
            //    //card.SetMouseCoords(worldCoords);
            //    window.Draw(card);
            //}
            //foreach (_test_Card card in deck2.cardList)
            //{
            //    //card.SetMouseCoords(worldCoords);
            //    window.Draw(card);
            //}
        }
    }
}
