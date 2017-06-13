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
    class UIDeck : UIContainer
    {
        private int _cardStartX;
        private int _maxStackSize;

        Sprite _bodyGround;
        private Vector2f _backTextureCoords = new Vector2f(1, 10);

        public UIDeck(int startX, int startY, int width, int height, Texture face, int cardWidth, int cardHeight)
        {
            this.StartX = startX;
            this.StartY = startY;
            this.Width = width;
            this.Height = height;
            this.Face = face;
            this.CardWidth = cardWidth;
            this.CardHeight = cardHeight;
            CardList = new List<UICard>();

            this.Body = new RectangleShape();

            this.Body.FillColor = Color.Green;
            this.Body.OutlineColor = Color.Green;
            this.Body.OutlineThickness = 1.0f;
            this.Body.Size = new Vector2f(width + 2 * Offset, height + 2 * Offset);
            this.Body.Position = new Vector2f(this.StartX - Offset, this.StartY - Offset);

            this._bodyGround = new Sprite();
            this._bodyGround.Texture = this.Face;
            this._bodyGround.TextureRect = new IntRect((int)_backTextureCoords.X * this.Width, (int)_backTextureCoords.Y * this.Height, this.Width, this.Height);
            this._bodyGround.Position = new Vector2f(this.StartX, this.StartY);
        }

        public void FlipDeck()
        {
            //visible = visible == false;
            foreach (var card in CardList)
            {
                card.Flipped = card.Flipped == false;
            }
        }

        public override void RemoveCard(UICard removedCard)
        {
            int i = 0;
            CardList.Remove(removedCard);
            foreach (UICard card in CardList)
            {
                card.DockX = StartX;
                card.DockY = StartY;
                //card.handStartX = card.dockX;

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
            //cardList[cardList.Count - 1].flipped = false;

            if (addedCard.Flipped != visible) addedCard.Flip();

            foreach (UICard card in CardList)
            {
                card.DockX = StartX;
                card.DockY = StartY;
                card.Visible = visible;
                //card.handStartX = card.dockX;

                //card.dockX = card.currentX;
                //card.dockY = card.currentY;
                card.Free();
                i++;
            }
        }

        public override void AddCard(int id, int textureX, int textureY)
        {
            int i = 0;
            CardList.Add(new UICard(id, StartX, 0, CardWidth, CardHeight, Face, textureX, textureY, this, false));
            CardList[CardList.Count - 1].Origin = this;
            CardList[CardList.Count - 1].Flipped = false;
            foreach (UICard card in CardList)
            {
                card.DockX = StartX;
                card.DockY = StartY;
                card.Visible = visible;
                //card.handStartX = card.dockX;

                //card.dockX = card.currentX;
                //card.dockY = card.currentY;
                card.Free();
                i++;
            }
            //this.body.Size = new Vector2f(width + 4 * offset, height + 2 * offset);
        }

        public override void MouseMove(Vector2f worldCoords, ref UICard cursorDockedCard)
        {
            if (Active == true)
            {
                int cardIndex = -1;
                int maxCardIndex = CardList.Count - 1;
                if (cursorDockedCard == null)
                {
                    if (maxCardIndex >= 0)
                    {
                        bool active;
                        active = CardList[maxCardIndex].Collide((int) worldCoords.X, (int) worldCoords.Y);
                        if (active == true)
                        {
                            cardIndex = maxCardIndex;
                            CardList[maxCardIndex].Drag((int) worldCoords.X, (int) worldCoords.Y);
                        }
                        CardList[maxCardIndex].MouseCollide(active);
                    }

                }
                else cursorDockedCard.Drag((int) worldCoords.X, (int) worldCoords.Y);
            }
        }

        public override void Clicked(MouseButtonEventArgs e, Vector2f worldCoords, ref UICard cursorDockedCard)
        {
            if (Active == true)
            {
                int maxCardIndex = CardList.Count - 1;
            if (maxCardIndex >= 0)
            {
                bool active = CardList[maxCardIndex].Clicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
                if (active == true)
                {
                    Console.WriteLine("Card Taken");
                    CardList[maxCardIndex].ClickExecute((int)worldCoords.X, (int)worldCoords.Y, e.Button);
                    cursorDockedCard = CardList[maxCardIndex];
                }
            }
            }
        }

        public override void UnClicked(MouseButtonEventArgs e, Vector2f worldCoords)
        {
            //foreach (_test_Card card in cardList)
            if (CardList.Count - 1 >= 0) CardList[CardList.Count - 1].UnClicked((int)worldCoords.X, (int)worldCoords.Y);
            //cardList[cardList.Count - 1].Flip();
        }

        protected void Update()
        {

        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            Update();
            //if (visible) target.Draw(Body, states);
            if (visible) target.Draw(_bodyGround, states);
        }

        protected override void SetObjectTransform()
        {
            throw new NotImplementedException();
        }
    }
}
