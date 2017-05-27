using System;
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

        //_test_Tilemap map;
        Texture tileset;
        Texture buttonFace;
        Texture deckTexture;
        List<UIButton> buttonList;
        //List<_test_Card> cardSpriteList;
        List<_test_Container> elementsContainers;

        private List<_test_Hand> hands;
        private List<_test_Playground> playgrounds;

        private _test_Aether aether;

        private _test_Deck deck;
        private _test_Deck deck2;

        private _test_Deck graveyard;
        private _test_Card cursorDockedCard;
        bool mousePressed = false;

        public _test_RPG() : base(1600, 900, "Citadel Game Alpha", Color.Cyan)
        {
            buttonList = new List<UIButton>();
            elementsContainers = new List<_test_Container>();

            hands = new List<_test_Hand>();
            playgrounds = new List<_test_Playground>();
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
            }
        }

        protected override void CheckUnClick(MouseButtonEventArgs e)
        {
            Vector2i mouseCoords = new Vector2i(e.X, e.Y);
            worldCoords = window.MapPixelToCoords(mouseCoords);

            if (boardActive == true)
            {
                //foreach (UIButton button in buttonList) button.UnClicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
                Random rndX = new Random();
                Random rndY = new Random();
                for (int i = buttonList.Count - 1; i >= 1; i--)
                {
                    int texX = rndX.Next(0, 13);
                    int texY = rndY.Next(0, 4);
                    bool done = buttonList[i].UnClicked((int) worldCoords.X, (int) worldCoords.Y, e.Button);
                    if (done == true) deck.AddCard(texX, texY);
                }
                bool grave = buttonList[0].UnClicked((int) worldCoords.X, (int) worldCoords.Y, e.Button);
                if (grave == true)
                {

                    if (playgrounds[1].cardList.Count > 0)
                    {
                        _test_Card cardDummy = playgrounds[1].cardList[0];
                        playgrounds[1].RemoveCard(cardDummy);
                        graveyard.AddCard(cardDummy);
                    }
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
            mousePressed = false;
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
            deck = new _test_Deck(900, 100, 72, 100, deckTexture, 72, 100);
            deck.active = true;
            deck2 = new _test_Deck(1100, 100, 72, 100, deckTexture, 72, 100);
            deck2.active = true;
            graveyard = new _test_Deck(1300, 100, 72, 100, deckTexture, 72, 100);
            graveyard.active = false;
            graveyard.FlipDeck();

            playgrounds.Add(new _test_Playground(100, 350, 600, 100, deckTexture, 72, 100));
            playgrounds[0].active = true;
            playgrounds.Add(new _test_Playground(1000, 350, 600, 100, deckTexture, 72, 100));
            playgrounds[1].active = true;

            hands.Add(new _test_Hand(100, 600, 600, 100, deckTexture, 72, 100, 0));
            hands[0].active = true;
            hands[0].AddCard(1, 3);
            hands.Add(new _test_Hand(1000, 600, 600, 100, deckTexture, 72, 100, 0));
            hands[1].active = true;
            hands[1].FlipHand();

            elementsContainers.Add(aether);
            elementsContainers.Add(deck);
            elementsContainers.Add(deck2);
            elementsContainers.Add(graveyard);

            foreach (var playground in playgrounds)
            {
                elementsContainers.Add(playground);
            }
            foreach (var hand in hands)
            {
                elementsContainers.Add(hand);
            }

            buttonList.Add(new UIPrimitiveButton(320, 20, 180, 40));
            buttonList.Add(new UIPrimitiveButton(320, 80, 180, 40));
            buttonList.Add(new UIGlyphButton(320, 140, 95, 53, buttonFace));
            buttonList.Add(new UIGlyphButton(500, 140, 95, 53, buttonFace));

            UIGlyphButton buttonOff = new UIGlyphButton(320, 240, 95, 53, buttonFace);
            buttonOff.state = -1;
            buttonList.Add(buttonOff);

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
            deck2.AddCard(8,2);
            deck2.AddCard(3,3);
            deck2.AddCard(3,2);
            deck2.AddCard(4,1);
            deck2.AddCard(11,0);
            deck2.AddCard(0,1);
            deck2.AddCard(12,1);
            deck2.AddCard(11,1);

            boardActive = true;
        }

        protected override void Tick()
        {

        }

        protected override void Render()
        {
            foreach (_test_Container container in elementsContainers) window.Draw(container);    

            foreach (UIButton button in buttonList)
            {
                window.Draw(button);
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
