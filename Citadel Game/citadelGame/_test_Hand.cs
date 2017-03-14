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
        int startX;
        int startY;
        int width;
        int height;
        int offset = 50;

        int cardAreaWidth;
        int cardAreaStartX;
        int cardAreaStartY;

        int maxHandWidth;
        int cardCount;


        private bool mouseOver = false;

        RectangleShape body;

        Texture deck;
        public List<_test_Card> cardList;
        public _test_Card activeCard;
        public bool activeCardActive = false;

        public _test_Hand(int startX, int startY, int width, int height, Texture deck)
        {
            this.startX = startX;
            this.startY = startY;
            this.maxHandWidth = maxHandWidth;
            this.width = width;
            this.height = height;
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
            for (int i = cardList.Count - 1; i >= 0; i--)
            {
                int distance = Math.Abs(cardIndex - i);
                //double deltaX = Math.Abs(1 / (float)distance * card.width * 0.6f);
                int deltaX = 0;
                if (cardCount * (cardList[i].width * cardList[i].exposeSize + 1) > width)
                {
                    deltaX = (int)Math.Pow(Math.Abs(Math.Cos(Math.PI / 2 * Math.Min(cardCount / 3.0f, distance) / (cardCount / 3.0f)) * cardList[i].width * 0.6f), 1.04f);
                }
                   
                if (i < cardIndex && deltaX != 0)
                {
                    cardList[i].destinationX = (int)(cardList[i].handStartX - deltaX);
                    cardList[i].dockX = cardList[i].destinationX;
                    cardList[i].ForceFree();
                }
                else if (i > cardIndex && deltaX != 0)
                {
                    cardList[i].destinationX = (int)(cardList[i].handStartX + deltaX);
                    cardList[i].dockX = cardList[i].destinationX;
                    cardList[i].ForceFree();
                }
                else
                {
                    cardList[i].destinationX = (int)(cardList[i].handStartX);
                    cardList[i].dockX = cardList[i].destinationX;
                    cardList[i].ForceFree();
                }
            }
        }

        public void AddCard(int texture_x, int texture_y)
        {
            int i = 0;
            cardCount++;
            cardList.Add(new _test_Card(0, startY, 72, 100, deck, texture_x, texture_y));
            cardList[cardList.Count - 1].orgin = Orgin.hand;
            //width = Math.Min((int)(cardList[0].width * cardList[0].exposeSize * (cardCount+1)), maxHandWidth);
            //width = maxHandWidth;
            cardAreaWidth = Math.Min((int)((cardList[0].width * cardList[0].exposeSize + 1) * (cardCount)), width);
            cardAreaStartX = (int)((width - cardAreaWidth) / 2.0 + startX);
            height = cardList[0].height;
            foreach (_test_Card card in cardList)
            {
                card.dockX = cardAreaStartX + (i * (cardAreaWidth + 1) / (cardCount));
                card.handStartX = card.dockX;
                card.dockY = startY;
                //card.dockX = card.currentX;
                //card.dockY = card.currentY;
                card.Free();
                i++;
            }
            this.body.Size = new Vector2f(width + 4 * offset, height + 2 * offset);
        }

        public void AddCard(_test_Card addedCard)
        {
            int i = 0;
            cardCount++;
            cardList.Add(addedCard);
            cardList[cardList.Count - 1].orgin = Orgin.hand;
            //width = Math.Min((int)(cardList[0].width * cardList[0].exposeSize * (cardCount+1)), maxHandWidth);
            //width = maxHandWidth;
            cardAreaWidth = Math.Min((int)((cardList[0].width * cardList[0].exposeSize + 1) * (cardCount)), width);
            cardAreaStartX = (int)((width - cardAreaWidth) / 2.0 + startX);
            height = cardList[0].height;
            foreach (_test_Card card in cardList)
            {
                card.dockX = cardAreaStartX + (i * (cardAreaWidth + 1) / (cardCount));
                card.handStartX = card.dockX;
                card.dockY = startY;
                //card.dockX = card.currentX;
                //card.dockY = card.currentY;
                card.Free();
                i++;
            }
            this.body.Size = new Vector2f(width + 4 * offset, height + 2 * offset);
        }

        public void RemoveCard(_test_Card removedCard)
        {
            int i = 0;
            cardCount--;
            //width = Math.Min((int)(cardList[0].width * cardList[0].exposeSize * (cardCount + 1)), maxHandWidth);
            //width = maxHandWidth;
            cardAreaWidth = Math.Min((int)((cardList[0].width * cardList[0].exposeSize + 1) * (cardCount)), width);
            cardAreaStartX = (int)((width - cardAreaWidth) / 2.0 + startX);
            height = cardList[0].height;
            cardList.Remove(removedCard);
            
            foreach (_test_Card card in cardList)
            {
                card.dockX = cardAreaStartX + (i * (cardAreaWidth + 1) / (cardCount));
                card.handStartX = card.dockX;
                card.dockY = startY;
                //card.dockX = card.currentX;
                //card.dockY = card.currentY;
                card.Free();
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

        public void MouseMove(Vector2f worldCoords, ref _test_Card cursorDockedCard)
        {
            bool cardFound = false;
            int cardIndex = -1;
            int onOrOff = 0;
            if (cursorDockedCard == null)
            {
                for (int i = cardList.Count - 1; i >= 0; i--)
                {
                    bool active;
                    if (cardFound == false)
                    {
                        active = cardList[i].Collide((int)worldCoords.X, (int)worldCoords.Y);
                        if (active == true)
                        {
                            cardFound = true;
                            cardIndex = i;
                            cardList[i].Drag((int)worldCoords.X, (int)worldCoords.Y);
                        }
                    }
                    else active = false;
                    if (onOrOff == 0) onOrOff = cardList[i].OnOrOff(active);
                    else
                    {
                        int onOrOff2 = cardList[i].OnOrOff(active);
                        if (onOrOff2 != 0) onOrOff = 1;
                    }
                    cardList[i].MouseCollide(active);
                }
                if (onOrOff == 1)
                {
                    SlipCards(cardIndex);
                }
                if (onOrOff == -1)
                {
                    foreach (_test_Card card in cardList)
                    {
                        card.dockX = card.handStartX;
                        card.Free();
                    }
                }
            }
            else cursorDockedCard.Drag((int)worldCoords.X, (int)worldCoords.Y);
        }

        public void Clicked(MouseButtonEventArgs e, Vector2f worldCoords, ref _test_Card cursorDockedCard)
        {
            int cardIndex = -1;
            bool eventHappened = false;
            _test_Card chosenCard = null;
            foreach (_test_Card card in cardList)
            {
                bool active = card.Clicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
                if (active == true)
                {
                    cardIndex = cardList.IndexOf(card);
                    chosenCard = card;
                    eventHappened = true;
                }
            }
            if (eventHappened == true)
            {
                activeCard = chosenCard;
                activeCardActive = true;
                cursorDockedCard = chosenCard;
                Console.WriteLine("Card Taken");
                activeCard.ClickExecute((int)worldCoords.X, (int)worldCoords.Y, e.Button);
            }
        }

        public void UnClicked(MouseButtonEventArgs e, Vector2f worldCoords)
        {
            foreach (_test_Card card in cardList) card.UnClicked((int)worldCoords.X, (int)worldCoords.Y);
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
