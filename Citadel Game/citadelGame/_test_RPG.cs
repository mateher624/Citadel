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

        int oldCardIndex = 0;
        

        _test_Tilemap map;
        Texture tileset;
        Texture buttonFace;
        Texture deck;
        List<UIButton> buttonList;
        List<_test_Card> cardSpriteList;

        _test_Hand hand;
        _test_Card cusrsorDockedCard = null;

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
            bool cardFound = false;
            for (int i = cardSpriteList.Count - 1; i >= 0; i--)
            {
                bool active;
                if (cardFound == false)
                {
                    active = cardSpriteList[i].Collide((int)worldCoords.X, (int)worldCoords.Y);
                    if (active == true)
                    {
                        cardFound = true;
                        cardIndex = i;
                        cardSpriteList[i].Drag((int)worldCoords.X, (int)worldCoords.Y);
                    }
                }
                else active = false;
                cardSpriteList[i].MouseCollide(active);
            }


            // TYLKO DLA hand
            cardFound = false;
            cardIndex = -1;
            int onOrOff = 0;
            if (cusrsorDockedCard == null)
            {
                for (int i = hand.cardList.Count - 1; i >= 0; i--)
                {
                    bool active;
                    if (cardFound == false)
                    {
                        active = hand.cardList[i].Collide((int)worldCoords.X, (int)worldCoords.Y);
                        if (active == true)
                        {
                            cardFound = true;
                            cardIndex = i;
                            hand.cardList[i].Drag((int)worldCoords.X, (int)worldCoords.Y);

                        }
                    }
                    else active = false;
                    if (onOrOff == 0) onOrOff = hand.cardList[i].OnOrOff(active);
                    else
                    {
                        int onOrOff2 = hand.cardList[i].OnOrOff(active);
                        if (onOrOff2 != 0)
                        {
                            //handFree();
                            onOrOff = 1;
                        }
                    }
                    hand.cardList[i].MouseCollide(active);

                    //if (hand.cardList[i].mouseOver == true) flowEnabler = true;
                }
                if (onOrOff == 1)
                {
                    hand.Update(cardIndex);
                }
                if (onOrOff == -1)
                {
                    foreach (_test_Card card in hand.cardList)
                    {
                        card.dockX = card.handStartX;
                        card.Free();
                    }
                }
            }
            else  cusrsorDockedCard.Drag((int)worldCoords.X, (int)worldCoords.Y);

            //eventHappened = false;
            ////bool cardChanged = false;
            //foreach (_test_Card card in hand.cardList)
            //{
            //    card.mouseOver = false;

            //    card.start_x = card.dockX;
            //    //card.DisExpose();  
            //    bool active = card.Collide((int)worldCoords.X, (int)worldCoords.Y);
            //    if (active == true)
            //    {
            //        cardIndex = hand.cardList.IndexOf(card);
            //        chosenCard = card;
            //        eventHappened = true;
            //    }

            //}
            //if (eventHappened == true)
            //{
            //    //card_sprite_list.RemoveAt(dummy_position);
            //    //card_sprite_list.Add(dummy_card);
            //    chosenCard.Drag((int)worldCoords.X, (int)worldCoords.Y);
            //    if (hand.activeCardActive == false)
            //    {
            //        chosenCard.MouseCollide(true);
            //        //chosenCard.mouseOver = true;
            //        //chosenCard.Expose();
            //    }
            //    if (cardIndex != oldCardIndex)
            //    {
            //        hand.Update(cardIndex, false);
            //        Console.WriteLine("Card Changed form {0} to {1}", cardIndex, oldCardIndex);
            //        oldCardIndex = cardIndex;
            //    }
            //    else
            //    {
            //        hand.Update(cardIndex, true);
            //    }
            //}
            //else
            //{
            //    oldCardIndex = -1;
            //    foreach (_test_Card card in hand.cardList)
            //    {
            //        card.Free();
            //    }
            //}
        }
        
        protected override void CheckClick(MouseButtonEventArgs e)
        {
            Vector2i mouseCoords = new Vector2i(e.X, e.Y);
            Vector2f worldCoords = window.MapPixelToCoords(mouseCoords);
            
            foreach (UIButton button in buttonList) button.Clicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);

            // TROCHĘ MNIEJ UPOŚLEDZONA FUNKCJA ELEMENTU AKTYWNEGO
            int cardIndex = 0;
            _test_Card chosenCard = new _test_Card(0, 0, 0, 0, deck, 0, 0); 
            bool eventHappened = false;
            foreach (_test_Card card in cardSpriteList)
            {
                if (card.mouseOver == true)
                {
                    cardIndex = cardSpriteList.IndexOf(card);
                    chosenCard = card;
                    eventHappened = true;
                }
                //bool active = card.Clicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
                //if (active == true)
                //{
                //    cardIndex = cardSpriteList.IndexOf(card);
                //    chosenCard = card;
                //    eventHappened = true;
                //}
            }
            if (eventHappened == true)
            {
                cardSpriteList.RemoveAt(cardIndex);
                cardSpriteList.Add(chosenCard);
                //cusrsorDockedCard = chosenCard;
                Console.WriteLine("Card Taken");
                chosenCard.ClickExecute((int)worldCoords.X, (int)worldCoords.Y, e.Button);
            }

            // TYLKO DLA HAND
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
                cusrsorDockedCard = chosenCard;
                Console.WriteLine("Card Taken");
                hand.activeCard.ClickExecute((int)worldCoords.X, (int)worldCoords.Y, e.Button);
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
            foreach (_test_Card card in cardSpriteList) card.UnClicked((int)worldCoords.X, (int)worldCoords.Y);
            //foreach (_test_Card card in hand.cardList) card.UnClicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
            if (hand.activeCardActive == true)
            {
                hand.activeCard.UnClicked((int)worldCoords.X, (int)worldCoords.Y);
                hand.activeCardActive = false;
                foreach (_test_Card card in hand.cardList)
                {
                    card.dockX = card.handStartX;
                    card.Free();
                }
            }
            cusrsorDockedCard = null;
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
                cardSpriteList.Add(new _test_Card(20 + 15 * i, 20, 72, 100, deck, i, 3));
                cardSpriteList.Add(new _test_Card(20 + 15 * i, 130, 72, 100, deck, i, 2));
                cardSpriteList.Add(new _test_Card(20 + 15 * i, 240, 72, 100, deck, i, 1));
                cardSpriteList.Add(new _test_Card(20 + 15 * i, 350, 72, 100, deck, i, 0));
            }
            //cardSpriteList.Add(new _test_Card(20 + 15 * 2, 350, 72, 100, deck, 2, 0));
            //hand.AddCard(1,3);
            hand.AddCard(4,2);
            hand.AddCard(6,1);
            hand.AddCard(10,2);
            hand.AddCard(0,0);
            hand.AddCard(12,3);
            hand.AddCard(5,1);
            hand.AddCard(6,0);
            hand.AddCard(8,2);
            //hand.AddCard(3,3);
            //hand.AddCard(3,2);
            //hand.AddCard(4,1);
            //hand.AddCard(11,0);
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
