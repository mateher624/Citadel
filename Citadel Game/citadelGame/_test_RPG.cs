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
        Vector2f worldCoords;

        _test_Tilemap map;
        Texture tileset;
        Texture buttonFace;
        Texture deckTexture;
        List<UIButton> buttonList;
        //List<_test_Card> cardSpriteList;
        List<_test_Container> elementsContainers;

        _test_Aether aether;
        _test_Hand hand;
        _test_Deck deck;
        _test_Deck deck2;
        _test_Playground playground;
        _test_Card cursorDockedCard = null;
        bool mousePressed = false;

        public _test_RPG() : base(1600, 900, "Citadel Game Alpha", Color.Cyan)
        {
            buttonList = new List<UIButton>();
            //cardSpriteList = new List<_test_Card>();
            elementsContainers = new List<_test_Container>();
        }

        protected override void CheckCollide(MouseMoveEventArgs e)
        {
            //int cardIndex = 0;
            //_test_Card chosenCard = null;
            Vector2i mouseCoords = new Vector2i(e.X, e.Y);
            worldCoords = window.MapPixelToCoords(mouseCoords);
            foreach (UIButton button in buttonList) button.Collide((int)worldCoords.X, (int)worldCoords.Y);

            // TYLKO DLA cardSpriteList
            foreach (_test_Container container in elementsContainers) container.MouseMove(worldCoords, ref cursorDockedCard);
            //aether.MouseMove(worldCoords, ref cursorDockedCard);
            //deck.MouseMove(worldCoords, ref cursorDockedCard);
            //deck2.MouseMove(worldCoords, ref cursorDockedCard);
            //hand.MouseMove(worldCoords, ref cursorDockedCard);

            playground.Collide((int)worldCoords.X, (int)worldCoords.Y, mousePressed);
            hand.Collide((int)worldCoords.X, (int)worldCoords.Y, mousePressed);
        }
        
        protected override void CheckClick(MouseButtonEventArgs e)
        {
            Vector2i mouseCoords = new Vector2i(e.X, e.Y);
            worldCoords = window.MapPixelToCoords(mouseCoords);
            mousePressed = true;
            foreach (UIButton button in buttonList) button.Clicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);

            // TROCHĘ MNIEJ UPOŚLEDZONA FUNKCJA ELEMENTU AKTYWNEGO
            foreach (_test_Container container in elementsContainers) container.Clicked(e, worldCoords, ref cursorDockedCard);
            //aether.Clicked(e, worldCoords, ref cursorDockedCard);
            //deck.Clicked(e, worldCoords, ref cursorDockedCard);
            //deck2.Clicked(e, worldCoords, ref cursorDockedCard);
            //hand.Clicked(e, worldCoords, ref cursorDockedCard);
        }

        protected override void CheckUnClick(MouseButtonEventArgs e)
        {
            Vector2i mouseCoords = new Vector2i(e.X, e.Y);
            worldCoords = window.MapPixelToCoords(mouseCoords);

            //foreach (UIButton button in buttonList) button.UnClicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
            Random rndX = new Random();
            Random rndY = new Random();
            for (int i = buttonList.Count - 1; i >= 0; i--)
            {
                int texX = rndX.Next(0, 13);
                int texY = rndY.Next(0, 4);
                bool done = buttonList[i].UnClicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
                if (done == true) deck.AddCard(texX, texY);
            }

            foreach (_test_Container container in elementsContainers) container.UnClicked(e, worldCoords);
            //aether.UnClicked(e, worldCoords);
            //deck.UnClicked(e, worldCoords);
            //deck2.UnClicked(e, worldCoords);
            //hand.UnClicked(e, worldCoords);

            // UPUSZCZANIE KARD DO PÓL
            if (cursorDockedCard != null)
            { 
                bool playgroundCollide = playground.Collide((int)worldCoords.X, (int)worldCoords.Y, mousePressed);
                bool handCollide = hand.Collide((int)worldCoords.X, (int)worldCoords.Y, mousePressed);

                if (cursorDockedCard == null) Console.WriteLine("NULL w kursorze");
                if (playgroundCollide == true && cursorDockedCard.origin.GetType() != typeof(_test_Playground))
                {
                    cursorDockedCard.origin.RemoveCard(cursorDockedCard);
                    if (cursorDockedCard.origin.GetType() != typeof(_test_Hand)) cursorDockedCard.Flip();
                    playground.AddCard(cursorDockedCard);
                    //cursorDockedCard.Free();
                }
                if (handCollide == true && cursorDockedCard.origin.GetType() != typeof(_test_Hand))
                {
                    cursorDockedCard.origin.RemoveCard(cursorDockedCard);
                    if (cursorDockedCard.origin.GetType() != typeof(_test_Hand)) cursorDockedCard.Flip();
                    hand.AddCard(cursorDockedCard);
                    //cursorDockedCard.Free();
                }      
                cursorDockedCard = null;
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
            hand = new _test_Hand(400, 600, 600, 100, deckTexture, 72, 100, 0);
            deck = new _test_Deck(900, 100, 72, 100, deckTexture, 72, 100);
            deck2 = new _test_Deck(1200, 100, 72, 100, deckTexture, 72, 100);
            playground = new _test_Playground(400, 350, 600, 100, deckTexture, 72, 100);

            elementsContainers.Add(aether);
            elementsContainers.Add(playground);
            elementsContainers.Add(hand);
            elementsContainers.Add(deck);
            elementsContainers.Add(deck2);

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
                aether.AddCard(new _test_Card(20 + 15 * i, 20, 72, 100, deckTexture, i, 3, aether));
                aether.AddCard(new _test_Card(20 + 15 * i, 130, 72, 100, deckTexture, i, 2, aether));
                aether.AddCard(new _test_Card(20 + 15 * i, 240, 72, 100, deckTexture, i, 1, aether));
                aether.AddCard(new _test_Card(20 + 15 * i, 350, 72, 100, deckTexture, i, 0, aether));
            }
            //cardSpriteList.Add(new _test_Card(20 + 15 * 2, 350, 72, 100, deck, 2, 0));
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
        }

        protected override void Tick()
        {

        }

        protected override void Render()
        {
            window.Draw(map);
            foreach (_test_Container container in elementsContainers) window.Draw(container);
                
            //window.Draw(playground);
            //window.Draw(deck);
            //window.Draw(deck2);
            //window.Draw(hand);

            foreach (UIButton button in buttonList)
            {
                window.Draw(button);
            }

            foreach (_test_Container container in elementsContainers)
            {
                foreach (_test_Card card in container.cardList)
                {
                    window.Draw(card);
                }
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
