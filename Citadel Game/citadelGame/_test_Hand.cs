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
    class _test_Hand : Drawable
    {
        int width;
        int maxHandWidth;
        int cardCount;
        int height;
        int startX;
        int startY;
        int offset = 50;

        private bool mouseOver = false;

        RectangleShape body;

        Texture deck;
        public List<_test_Card> cardList;
        public _test_Card activeCard;
        public bool activeCardActive = false;

        public _test_Hand(int startX, int startY, int maxHandWidth, Texture deck)
        {
            this.startX = startX;
            this.startY = startY;
            this.maxHandWidth = maxHandWidth;
            this.width = 1;
            this.deck = deck;
            cardList = new List<_test_Card>();

            this.body = new RectangleShape();

            this.body.FillColor = Color.Green;
            this.body.OutlineColor = Color.Green;
            this.body.OutlineThickness = 1.0f;
            this.body.Size = new Vector2f(width + 4 * offset, height + 2 * offset);
            this.body.Position = new Vector2f(this.startX - 2*offset, this.startY - offset);
        }

        public void SlipCards(int cardIndex)
        {
            //int i = 0;

            //if (activeCardActive == false)
            {
                //foreach (_test_Card card in cardList)
                for (int i = cardList.Count - 1; i >= 0; i--)
                {
                    //cardList[i].ResPos();
                    int distance = Math.Abs(cardIndex - i);
                    //double deltaX = Math.Abs(1 / (float)distance * card.width * 0.6f);
                    int deltaX = 0;
                    if (cardCount * (cardList[i].width * cardList[i].exposeSize + 1) > width)
                    {
                        deltaX = (int)Math.Pow(Math.Abs(Math.Cos(Math.PI / 2 * Math.Min(cardCount / 3.0f, distance) / (cardCount / 3.0f)) * cardList[i].width * 0.6f), 1.04f);
                    }
                   
                    if (i < cardIndex && deltaX != 0)
                    {
                        //cardList[i].magnetAnimationLock = false;
                        cardList[i].destinationX = (int)(cardList[i].handStartX - deltaX);
                        cardList[i].dockX = cardList[i].destinationX;
                        cardList[i].ForceFree();
                        //if (cardList[i].currentX == cardList[i].destinationX)
                        //{
                        //    cardList[i].dockX = cardList[i].handStartX;
                        //    cardList[i].freezePosition = true;
                        //}
                        //if (freeze == true) card.currentX = (int)(card.currentX - deltaX);
                        //else card.Flow((int)(card.currentX - deltaX));

                    }
                    else if (i > cardIndex && deltaX != 0)
                    {
                        cardList[i].destinationX = (int)(cardList[i].handStartX + deltaX);
                        cardList[i].dockX = cardList[i].destinationX;
                        cardList[i].ForceFree();
                        //if (cardList[i].currentX == cardList[i].destinationX)
                        //{
                        //    cardList[i].dockX = cardList[i].handStartX;
                        //    cardList[i].freezePosition = true;
                        //}
                        //if (freeze == true) card.currentX = (int)(card.currentX + deltaX);
                        //else card.Flow((int)(card.currentX + deltaX));
                    }
                    else
                    {
                        cardList[i].destinationX = (int)(cardList[i].handStartX);
                        cardList[i].dockX = cardList[i].destinationX;
                        cardList[i].ForceFree();
                    }
                    //if (freeze == false) card.Free();
                    //i++;
                }
            }
        }

        public void AddCard(int texture_x, int texture_y)
        {
            int i = 0;
            cardCount++;
            width = Math.Min((int)(72 * 1.2 * (cardCount+1)), maxHandWidth);
            height = 100;
            cardList.Add(new _test_Card(0, startY, 72, 100, deck, texture_x, texture_y));
            foreach (_test_Card card in cardList)
            {
                card.currentX = startX + (i * (width+1) / (cardCount));
                card.handStartX = card.currentX;
                card.currentY = startY;
                card.dockX = card.currentX;
                card.dockY = card.currentY;
                i++;
            }
            this.body.Size = new Vector2f(width + 4 * offset, height + 2 * offset);
        }

        public void RemoveCard(_test_Card removedCard)
        {
            int i = 0;
            cardCount--;
            cardList.Remove(removedCard);
            width = Math.Min((int)(72 * 1.2 * (cardCount + 1)), maxHandWidth);
            height = 100;
            foreach (_test_Card card in cardList)
            {
                card.currentX = startX + (i * (width + 1) / (cardCount));
                card.handStartX = card.currentX;
                card.currentY = startY;
                card.dockX = card.currentX;
                card.dockY = card.currentY;
                i++;
            }
            this.body.Size = new Vector2f(width + 4 * offset, height + 2 * offset);
        }

        public bool Collide(int x, int y, bool mousePressed)
        {
            if (mousePressed == false) mouseOver = false;
            else if (x >= (this.startX - 2 * offset) && x <= (this.startX + width + 2 * offset) && y >= (this.startY - offset) && y <= (this.startY + height + 1 * offset)) mouseOver = true;
            else mouseOver = false;
            return mouseOver;
        }

        public void Update()
        {
            if (mouseOver == true)
            {
                this.body.FillColor = Color.Cyan;
                this.body.OutlineColor = Color.Green;
            }
            else
            {
                this.body.FillColor = Color.Green;
                this.body.OutlineColor = Color.Green;
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            Update();
            target.Draw(body, states);
        }
    }
}
