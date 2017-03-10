using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using SFML.System;


namespace citadelGame
{
    class _test_RPG : _test_Game
    {
        _test_Tilemap map;
        Texture tileset;
        Texture buttonFace;
        Texture deck;
        List<UIButton> buttonList;
        List<_test_Card> cardSpriteList;

        _test_Hand hand;

        public _test_RPG() : base(1200, 800, "Game Name", Color.Cyan)
        {
            buttonList = new List<UIButton>();
            cardSpriteList = new List<_test_Card>();
        }

        protected override void CheckCollide(MouseMoveEventArgs e)
        {
            int cardIndex = 0;
            _test_Card chosenCard = new _test_Card(0, 0, 0, 0, deck, 0, 0);
            bool eventHappened = false;

            Vector2i mouseCoords = new Vector2i(e.X, e.Y);
            Vector2f worldCoords = window.MapPixelToCoords(mouseCoords);
            foreach (UIButton button in buttonList) button.Collide((int)worldCoords.X, (int)worldCoords.Y);

            // TYLKO DLA cardSpriteList
            foreach (_test_Card card in cardSpriteList)
            {
                card.mouseOver = false;
                card.DisExpose();
                bool active = card.Collide((int)worldCoords.X, (int)worldCoords.Y);
                if (active == true)
                {
                    cardIndex = cardSpriteList.IndexOf(card);
                    chosenCard = card;
                    eventHappened = true;
                }
            }
            if (eventHappened == true)
            {
                //card_sprite_list.RemoveAt(dummy_position);
                //card_sprite_list.Add(dummy_card);
                chosenCard.Drag((int)worldCoords.X, (int)worldCoords.Y);
                chosenCard.mouseOver = true;
                chosenCard.Expose();
            }

            // TYLKO DLA hand
            eventHappened = false;
            foreach (_test_Card card in hand.cardList)
            {
                card.mouseOver = false;
                card.start_x = card.handStartX;
                card.DisExpose();  
                bool active = card.Collide((int)worldCoords.X, (int)worldCoords.Y);
                if (active == true)
                {
                    cardIndex = hand.cardList.IndexOf(card);
                    chosenCard = card;
                    eventHappened = true;
                }
            }
            if (eventHappened == true)
            {
                //card_sprite_list.RemoveAt(dummy_position);
                //card_sprite_list.Add(dummy_card);
                chosenCard.Drag((int)worldCoords.X, (int)worldCoords.Y);
                chosenCard.mouseOver = true;
                chosenCard.Expose();
                hand.Update(cardIndex);
            }
        }
        
        protected override void CheckClick(MouseButtonEventArgs e)
        {
            Vector2i mouseCoords = new Vector2i(e.X, e.Y);
            Vector2f worldCoords = window.MapPixelToCoords(mouseCoords);
            
            foreach (UIButton button in buttonList) button.Clicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);

            // POTĘŻNIE UPOŚLEDZONA FUNKCJA ELEMENTU AKTYWNEGO
            int cardIndex = 0;
            _test_Card chosenCard = new _test_Card(0, 0, 0, 0, deck, 0, 0); 
            bool eventHappened = false;
            foreach (_test_Card card in cardSpriteList)
            {
                bool active = card.Clicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
                if (active == true)
                {
                    cardIndex = cardSpriteList.IndexOf(card);
                    chosenCard = card;
                    eventHappened = true;
                }
            }
            if (eventHappened == true)
            {
                cardSpriteList.RemoveAt(cardIndex);
                cardSpriteList.Add(chosenCard);
                //chosenCard.Drag((int)worldCoords.X, (int)worldCoords.Y);
            }

            eventHappened = false;
            foreach (_test_Card card in hand.cardList)
            {
                bool active = card.Clicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
                if (active == true)
                {
                    cardIndex = hand.cardList.IndexOf(card);
                    chosenCard = card;
                    eventHappened = true;
                }
            }
            if (eventHappened == true)
            {
                hand.activeCard = chosenCard;
                hand.activeCardActive = true;
                //hand.cardList.RemoveAt(cardIndex);
                //hand.cardList.Add(chosenCard);
                //chosenCard.Drag((int)worldCoords.X, (int)worldCoords.Y);
            }
        }

        protected override void CheckUnClick(MouseButtonEventArgs e)
        {
            Vector2i mouseCoords = new Vector2i(e.X, e.Y);
            Vector2f worldCoords = window.MapPixelToCoords(mouseCoords);
            foreach (UIButton button in buttonList) button.UnClicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
            foreach (_test_Card card in cardSpriteList) card.UnClicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
            //foreach (_test_Card card in hand.cardList) card.UnClicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
            if (hand.activeCardActive == true)
            {
                hand.activeCard.UnClicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
                hand.activeCardActive = false;
            }
        }

        protected override void LoadContent()
        {
            tileset = new Texture("Resources/DungeonTileset.png");
            buttonFace = new Texture("Resources/btn_play.bmp");
            deck = new Texture("Resources/deck.gif");
        }

        protected override void Initialize()
        {
            hand = new _test_Hand(100, 600, 600, deck);

            map = new _test_Tilemap(tileset, 4, 4, 32.0f, 64.0f);
            buttonList.Add(new UIPrimitiveButton(320, 20, 180, 40));
            buttonList.Add(new UIPrimitiveButton(320, 80, 180, 40));
            buttonList.Add(new UIGlyphButton(320, 140, 95, 53, buttonFace));
            buttonList.Add(new UIGlyphButton(500, 140, 95, 53, buttonFace));

            UIGlyphButton buttonOff = new UIGlyphButton(320, 240, 95, 53, buttonFace);
            buttonOff.state = -1;
            buttonList.Add(buttonOff);

            for (int i = 0; i < 13; i++)
            {
                cardSpriteList.Add(new _test_Card(20+15 * i, 20, 72, 100, deck, i, 3));
                cardSpriteList.Add(new _test_Card(20 + 15 * i, 130, 72, 100, deck, i, 2));
                cardSpriteList.Add(new _test_Card(20 + 15 * i, 240, 72, 100, deck, i, 1));
                cardSpriteList.Add(new _test_Card(20 + 15 * i, 350, 72, 100, deck, i, 0));
            }

            hand.AddCard();
            hand.AddCard();
            hand.AddCard();
            hand.AddCard();
            hand.AddCard();
            hand.AddCard();
            hand.AddCard();
            hand.AddCard();
            hand.AddCard();
            hand.AddCard();
            hand.AddCard();
            hand.AddCard();
            hand.AddCard();
            //hand.Add(new _test_Card(200, 600, 72, 100, deck, 4, 3));
            //hand.Add(new _test_Card(300, 600, 72, 100, deck, 2, 2));
            //hand.Add(new _test_Card(400, 600, 72, 100, deck, 11, 1));
            //hand.Add(new _test_Card(500, 600, 72, 100, deck, 7, 0));
        }

        protected override void Tick()
        {

        }

        protected override void Render()
        {
            window.Draw(map);
            foreach (UIButton button in buttonList)
            {
                window.Draw(button);
            }
            foreach (_test_Card card in cardSpriteList)
            {
                window.Draw(card);
            }
            foreach (_test_Card card in hand.cardList)
            {
                window.Draw(card);
            }

        }
    }
}
