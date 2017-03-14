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
    class _test_Deck : Drawable
    {
        private int width;
        private int height;

        private int startX;
        private int cardStartX;
        private int startY;
        private int offset = 50;


        //int cardAreaWidth;
        //int cardAreaStartX;
        //int cardAreaStartY;
        private int maxStackSize;
        int cardCount;

        private bool mouseOver = false;

        RectangleShape body;
        Sprite body2;
        private Vector2f backTextureCoords = new Vector2f(2, 4);
        
        Texture face;
        public List<_test_Card> cardList;

        public _test_Deck(int startX, int startY, int width, int height, Texture face)
        {
            this.startX = startX;
            this.startY = startY;
            this.width = width;
            this.height = height;
            this.face = face;
            cardList = new List<_test_Card>();

            this.body = new RectangleShape();

            this.body.FillColor = Color.Green;
            this.body.OutlineColor = Color.Green;
            this.body.OutlineThickness = 1.0f;
            this.body.Size = new Vector2f(width + 2 * offset, height + 2 * offset);
            this.body.Position = new Vector2f(this.startX - offset, this.startY - offset);

            this.body2 = new Sprite();
            this.body2.Texture = this.face;
            this.body2.TextureRect = new IntRect((int)backTextureCoords.X * this.width, (int)backTextureCoords.Y * this.height, this.width, this.height);
            this.body2.Position = new Vector2f(this.startX, this.startY);
        }

        public void RemoveCard(_test_Card removedCard)
        {
            int i = 0;
            cardCount--;
            cardList.Remove(removedCard);
            foreach (_test_Card card in cardList)
            {
                card.dockX = startX;
                card.dockY = startY;
                //card.handStartX = card.dockX;

                //card.dockX = card.currentX;
                //card.dockY = card.currentY;
                card.Free();
                i++;
            }
            //this.body.Size = new Vector2f(width + 4 * offset, height + 2 * offset);
        }

        public void AddCard(int texture_x, int texture_y)
        {
            int i = 0;
            cardCount++;
            cardList.Add(new _test_Card(startX, 0, 72, 100, face, texture_x, texture_y));
            cardList[cardList.Count - 1].orgin = Orgin.deck;
            cardList[cardList.Count - 1].state = 0;
            foreach (_test_Card card in cardList)
            {
                card.dockX = startX;
                card.dockY = startY;
                //card.handStartX = card.dockX;

                //card.dockX = card.currentX;
                //card.dockY = card.currentY;
                card.Free();
                i++;
            }
            //this.body.Size = new Vector2f(width + 4 * offset, height + 2 * offset);
        }

        public void MouseMove(Vector2f worldCoords, ref _test_Card cursorDockedCard)
        {
            int cardIndex = -1;
            int maxCardIndex = cardList.Count - 1;
            if (cursorDockedCard == null)
            {
                if (maxCardIndex >= 0)
                {
                    bool active;

                        active = cardList[maxCardIndex].Collide((int)worldCoords.X, (int)worldCoords.Y);
                        if (active == true)
                        {
                            cardIndex = maxCardIndex;
                            cardList[maxCardIndex].Drag((int)worldCoords.X, (int)worldCoords.Y);
                        }

                    cardList[maxCardIndex].MouseCollide(active);
                }

            }
            else cursorDockedCard.Drag((int)worldCoords.X, (int)worldCoords.Y);
        }

        public void Clicked(MouseButtonEventArgs e, Vector2f worldCoords, ref _test_Card cursorDockedCard)
        {
            int maxCardIndex = cardList.Count - 1;
            if (maxCardIndex >= 0)
            {
                bool active = cardList[maxCardIndex].Clicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
                if (active == true)
                {
                    Console.WriteLine("Card Taken");
                    cardList[maxCardIndex].ClickExecute((int)worldCoords.X, (int)worldCoords.Y, e.Button);
                    cursorDockedCard = cardList[maxCardIndex];
                }
            }
        }

        public void UnClicked(MouseButtonEventArgs e, Vector2f worldCoords)
        {
            //foreach (_test_Card card in cardList)
            if (cardList.Count - 1 >= 0) cardList[cardList.Count - 1].UnClicked((int)worldCoords.X, (int)worldCoords.Y);
            //cardList[cardList.Count - 1].Flip();
        }

        protected void Update()
        {

        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            Update();
            target.Draw(body, states);
            target.Draw(body2, states);
        }
    }
}
