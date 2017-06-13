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
    class UIHand : UIContainer
    {
        public UICard ActiveCard;
        public bool ActiveCardActive = false;

        private bool _uncovered;

        public UIHand(int startX, int startY, int width, int height, Texture face, int cardWidth, int cardHeight, int rotation)
        {
            this.StartX = startX;
            this.StartY = startY;
            this.Width = width;
            this.Height = height;

            this.Rotation = rotation;

            this.Face = face;
            this.CardWidth = cardWidth;
            this.CardHeight = cardHeight;
            

            CardList = new List<UICard>();

            this.Body = new RectangleShape();

            this.Body.FillColor = Color.Green;
            this.Body.OutlineColor = Color.Green;
            this.Body.OutlineThickness = 1.0f;

            SetObjectTransform();

        }

        //public void FlipHand()
        //{
        //    _uncovered = _uncovered == false;
        //    foreach (var card in CardList)
        //    {
        //        card.Flip();
        //        //card.flipped = card.flipped == false;
        //    }
        //}

        public void UnCoverCards()
        {
            if (_uncovered == false)
            {
                _uncovered = true;
                foreach (var card in CardList)
                {
                    card.Flip();
                }
            }
        }

        public void CoverCards()
        {
            if (_uncovered == true)
            {
                _uncovered = false;
                foreach (var card in CardList)
                {
                    card.Flip();
                }
            }
        }

        public void SlipCards(int cardIndex)
        {
            for (int i = CardList.Count - 1; i >= 0; i--)
            {
                int distance = Math.Abs(cardIndex - i);
                //double deltaX = Math.Abs(1 / (float)distance * card.width * 0.6f);
                int deltaX = 0;
                if (CardList.Count * (CardList[i].Width * CardList[i].ExposeSize + 1) > Width)
                {
                    deltaX = (int)Math.Pow(Math.Abs(Math.Cos(Math.PI / 2 * Math.Min(CardList.Count / 3.0f, distance) / (CardList.Count / 3.0f)) * CardList[i].Width * 0.6f), 1.04f);
                }
                   
                if (i < cardIndex && deltaX != 0)
                {
                    CardList[i].DestinationX = CardList[i].HandStartX - deltaX;
                    CardList[i].DockX = CardList[i].DestinationX;
                    CardList[i].ForceFree();
                }
                else if (i > cardIndex && deltaX != 0)
                {
                    CardList[i].DestinationX = (int)(CardList[i].HandStartX + deltaX);
                    CardList[i].DockX = CardList[i].DestinationX;
                    CardList[i].ForceFree();
                }
                else
                {
                    CardList[i].DestinationX = (int)(CardList[i].HandStartX);
                    CardList[i].DockX = CardList[i].DestinationX;
                    CardList[i].ForceFree();
                }
            }
        }

        public override void AddCard(int id, int textureX, int textureY)
        {
            int i = 0;
            CardList.Add(new UICard(id, 0, StartY+Height/2, CardWidth, CardHeight, Face, textureX, textureY, this, _uncovered));
            CardList[CardList.Count - 1].Origin = this;
            CardList[CardList.Count - 1].Flipped = _uncovered;

            CardAreaWidth = Math.Min((int)((CardList[0].Width * CardList[0].ExposeSize) * (CardList.Count + 1)), Width);
            CardAreaStartX = (int)(((Width - CardAreaWidth) / 2.0) + StartX);
            foreach (UICard card in CardList)
            {
                card.DockX = CardAreaStartX + ((i + 1) * (CardAreaWidth) / (CardList.Count + 1)) - CardList[0].Width / 2;
                card.HandStartX = card.DockX;
                card.DockY = StartY;
                card.Visible = visible;
                //card.dockX = card.currentX;
                //card.dockY = card.currentY;
                card.Free();
                i++;
            }
            //this.body.Size = new Vector2f(width + 4 * offset, height + 2 * offset);
        }

        public override void AddCard(UICard addedCard)
        {
            int i = 0;
            CardList.Add(addedCard);
            CardList[CardList.Count - 1].Origin = this;
            //width = Math.Min((int)(cardList[0].width * cardList[0].exposeSize * (cardCount+1)), maxHandWidth);
            //width = maxHandWidth;
            CardAreaWidth = Math.Min((int)((CardList[0].Width * CardList[0].ExposeSize) * (CardList.Count + 1)), Width);
            CardAreaStartX = (int)(((Width - CardAreaWidth) / 2.0) + StartX);
            if (addedCard.Flipped != _uncovered) addedCard.Flip();
            //height = cardHeight;
            foreach (UICard card in CardList)
            {
                card.DockX = CardAreaStartX + ((i + 1) * (CardAreaWidth) / (CardList.Count + 1)) - CardList[0].Width / 2;
                card.HandStartX = card.DockX;
                card.Visible = visible;
                //card.dockY = (int)(startY - this.height / 2.0f - offset);
                card.DockY = StartY;
                //card.dockX = card.currentX;
                //card.dockY = card.currentY;
                card.Free();
                i++;
            }
            //this.body.Size = new Vector2f(width + 4 * offset, height + 2 * offset);
        }

        public override void RemoveCard(UICard removedCard)
        {
            int i = 0;
            //width = Math.Min((int)(cardList[0].width * cardList[0].exposeSize * (cardCount + 1)), maxHandWidth);
            //width = maxHandWidth;
            CardAreaWidth = Math.Min((int)((CardList[0].Width * CardList[0].ExposeSize) * (CardList.Count)), Width);
            CardAreaStartX = (int)(((Width - CardAreaWidth) / 2.0) + StartX);
            //height = cardHeight;
            CardList.Remove(removedCard);
            
            foreach (UICard card in CardList)
            {
                card.DockX = CardAreaStartX + ((i + 1) * (CardAreaWidth) / (CardList.Count + 1)) - CardList[0].Width / 2;
                card.HandStartX = card.DockX;
                card.DockY = StartY;
                //card.dockX = card.currentX;
                //card.dockY = card.currentY;
                card.Free();
                i++;
            }
            //this.body.Size = new Vector2f(width + 4 * offset, height + 2 * offset);
        }

        public bool Collide(int x, int y, bool mousePressed)
        {
            if (Active == false) return false;
            if (mousePressed == false) MouseOver = false;
            else if (x >= (this.StartX - 2 * Offset) && x <= (this.StartX + Width + 2 * Offset) && y >= (this.StartY - Offset) && y <= (this.StartY + Height + 1 * Offset)) MouseOver = true;
            else MouseOver = false;
            return MouseOver;
        }

        protected override void SetObjectTransform()
        {
            Body.Size = new Vector2f(Width + 4 * Offset, Height + 2 * Offset);
            //body.Position = new Vector2f(this.startX - 2 * offset, this.startY - offset);
            Body.Position = new Vector2f(this.StartX - 2 * Offset, this.StartY - Offset);
            //body.Origin = new Vector2f(this.body.Size.X / 2, this.body.Size.Y / 2);
            this.Body.Rotation = this.Rotation;
        }

        public override void MouseMove(Vector2f worldCoords, ref UICard cursorDockedCard)
        {
            bool cardFound = false;
            int cardIndex = -1;
            int onOrOff = 0;
            if (Active == true)
            {
                if (cursorDockedCard == null)
                {
                    for (int i = CardList.Count - 1; i >= 0; i--)
                    {
                        bool active;
                        if (cardFound == false)
                        {
                            active = CardList[i].Collide((int) worldCoords.X, (int) worldCoords.Y);
                            if (active == true)
                            {
                                cardFound = true;
                                cardIndex = i;
                                CardList[i].Drag((int) worldCoords.X, (int) worldCoords.Y);
                            }
                        }
                        else active = false;
                        if (onOrOff == 0) onOrOff = CardList[i].OnOrOff(active);
                        else
                        {
                            int onOrOff2 = CardList[i].OnOrOff(active);
                            if (onOrOff2 != 0) onOrOff = 1;
                        }
                        CardList[i].MouseCollide(active);
                    }
                    if (onOrOff == 1)
                    {
                        SlipCards(cardIndex);
                    }
                    if (onOrOff == -1)
                    {
                        foreach (UICard card in CardList)
                        {
                            card.DockX = card.HandStartX;
                            card.Free();
                        }
                    }
                }
                else cursorDockedCard.Drag((int) worldCoords.X, (int) worldCoords.Y);
            }
        }

        public override void Clicked(MouseButtonEventArgs e, Vector2f worldCoords, ref UICard cursorDockedCard)
        {
            if (Active == true)
            {
                int cardIndex = -1;
                bool eventHappened = false;
                UICard chosenCard = null;
                foreach (UICard card in CardList)
                {
                    bool active = card.Clicked((int) worldCoords.X, (int) worldCoords.Y, e.Button);
                    if (active == true)
                    {
                        cardIndex = CardList.IndexOf(card);
                        chosenCard = card;
                        eventHappened = true;
                    }
                }
                if (eventHappened == true)
                {
                    ActiveCard = chosenCard;
                    ActiveCardActive = true;
                    cursorDockedCard = chosenCard;
                    Console.WriteLine("Card Taken");
                    ActiveCard.ClickExecute((int) worldCoords.X, (int) worldCoords.Y, e.Button);
                }
            }
        }

        public override void UnClicked(MouseButtonEventArgs e, Vector2f worldCoords)
        {
            foreach (UICard card in CardList) card.UnClicked((int)worldCoords.X, (int)worldCoords.Y);
        }

        public bool CardDroppedEvent(UICard cursorDockedCard, Vector2f worldCoords, bool mousePressed)
        {
            bool containerCollide = Collide((int)worldCoords.X, (int)worldCoords.Y, mousePressed);

            if (containerCollide == true && cursorDockedCard.Origin != this)
            {
                cursorDockedCard.Origin.RemoveCard(cursorDockedCard);
                AddCard(cursorDockedCard);
                return true;
                //cursorDockedCard.Free();
            }
            return false;
        }

        public void Update()
        {
            if (MouseOver == true)
            {
                this.Body.FillColor = Color.Cyan;
                this.Body.OutlineColor = Color.Green;
            }
            else
            {
                this.Body.FillColor = Color.Green;
                this.Body.OutlineColor = Color.Green;
            }
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            Update();
            //if (visible) target.Draw(Body, states);
        }
    }
}
