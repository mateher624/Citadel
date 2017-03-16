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
    class _test_Hand : _test_Container
    {
        public _test_Card activeCard;
        public bool activeCardActive = false;

        public _test_Hand(int startX, int startY, int width, int height, Texture face, int cardWidth, int cardHeight)
        {
            this.startX = startX;
            this.startY = startY;
            this.width = width;
            this.height = height;

            this.face = face;
            this.cardWidth = cardWidth;
            this.cardHeight = cardHeight;

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
                if (cardList.Count * (cardList[i].width * cardList[i].exposeSize + 1) > width)
                {
                    deltaX = (int)Math.Pow(Math.Abs(Math.Cos(Math.PI / 2 * Math.Min(cardList.Count / 3.0f, distance) / (cardList.Count / 3.0f)) * cardList[i].width * 0.6f), 1.04f);
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

        public override void AddCard(int texture_x, int texture_y)
        {
            int i = 0;
            cardList.Add(new _test_Card(0, startY, cardWidth, cardHeight, face, texture_x, texture_y, this));
            cardList[cardList.Count - 1].origin = this;
            //width = Math.Min((int)(cardList[0].width * cardList[0].exposeSize * (cardCount+1)), maxHandWidth);
            //width = maxHandWidth;
            cardAreaWidth = Math.Min((int)((cardWidth * cardList[0].exposeSize + 1) * (cardList.Count)), width);
            cardAreaStartX = (int)((width - cardAreaWidth) / 2.0 + startX);
            //height = cardHeight;
            foreach (_test_Card card in cardList)
            {
                card.dockX = cardAreaStartX + (i * (cardAreaWidth + 1) / (cardList.Count));
                card.handStartX = card.dockX;
                card.dockY = startY;
                //card.dockX = card.currentX;
                //card.dockY = card.currentY;
                card.Free();
                i++;
            }
            //this.body.Size = new Vector2f(width + 4 * offset, height + 2 * offset);
        }

        public override void AddCard(_test_Card addedCard)
        {
            int i = 0;
            cardList.Add(addedCard);
            cardList[cardList.Count - 1].origin = this;
            //width = Math.Min((int)(cardList[0].width * cardList[0].exposeSize * (cardCount+1)), maxHandWidth);
            //width = maxHandWidth;
            cardAreaWidth = Math.Min((int)((cardWidth * cardList[0].exposeSize + 1) * (cardList.Count)), width);
            cardAreaStartX = (int)((width - cardAreaWidth) / 2.0 + startX);
            //height = cardHeight;
            foreach (_test_Card card in cardList)
            {
                card.dockX = cardAreaStartX + (i * (cardAreaWidth + 1) / (cardList.Count));
                card.handStartX = card.dockX;
                card.dockY = startY;
                //card.dockX = card.currentX;
                //card.dockY = card.currentY;
                card.Free();
                i++;
            }
            //this.body.Size = new Vector2f(width + 4 * offset, height + 2 * offset);
        }

        public override void RemoveCard(_test_Card removedCard)
        {
            int i = 0;
            //width = Math.Min((int)(cardList[0].width * cardList[0].exposeSize * (cardCount + 1)), maxHandWidth);
            //width = maxHandWidth;
            cardAreaWidth = Math.Min((int)((cardWidth * cardList[0].exposeSize + 1) * (cardList.Count-1)), width);
            cardAreaStartX = (int)((width - cardAreaWidth) / 2.0 + startX);
            //height = cardHeight;
            cardList.Remove(removedCard);
            
            foreach (_test_Card card in cardList)
            {
                card.dockX = cardAreaStartX + (i * (cardAreaWidth + 1) / (cardList.Count));
                card.handStartX = card.dockX;
                card.dockY = startY;
                //card.dockX = card.currentX;
                //card.dockY = card.currentY;
                card.Free();
                i++;
            }
            //this.body.Size = new Vector2f(width + 4 * offset, height + 2 * offset);
        }

        public bool Collide(int x, int y, bool mousePressed)
        {
            if (mousePressed == false) mouseOver = false;
            else if (x >= (this.startX - 2 * offset) && x <= (this.startX + width + 2 * offset) && y >= (this.startY - offset) && y <= (this.startY + height + 1 * offset)) mouseOver = true;
            else mouseOver = false;
            return mouseOver;
        }

        public override void MouseMove(Vector2f worldCoords, ref _test_Card cursorDockedCard)
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

        public override void Clicked(MouseButtonEventArgs e, Vector2f worldCoords, ref _test_Card cursorDockedCard)
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

        public override void UnClicked(MouseButtonEventArgs e, Vector2f worldCoords)
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

        public override void Draw(RenderTarget target, RenderStates states)
        {
            Update();
            target.Draw(body, states);
        }
    }
}
