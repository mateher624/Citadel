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
    class TestPlayground : TestContainer
    {
        public TestPlayground(int startX, int startY, int width, int height, Texture face, int cardWidth, int cardHeight)
        {
            this.StartX = startX;
            this.StartY = startY;
            this.Width = width;
            this.Height = height;
            this.Face = face;
            this.CardWidth = cardWidth;
            this.CardHeight = cardHeight;
            CardList = new List<TestCard>();

            this.Body = new RectangleShape();

            this.Body.FillColor = Color.Green;
            this.Body.OutlineColor = Color.Green;
            this.Body.OutlineThickness = 1.0f;
            this.Body.Size = new Vector2f(width + 4 * Offset, height + 2 * Offset);
            this.Body.Position = new Vector2f(this.StartX - 2 * Offset, this.StartY - Offset);
        }

        public bool Collide(int x, int y, bool mousePressed)
        {
            if (Active == false) return false;
            if (mousePressed == false) MouseOver = false;
            else if (x >= (this.StartX - 2 * Offset)  && x <= (this.StartX + Width + 2 * Offset) && y >= (this.StartY - Offset) && y <= (this.StartY + Height + 1 * Offset)) MouseOver = true;
            else MouseOver = false;
            return MouseOver;
        }

        public override void MouseMove(Vector2f worldCoords, ref TestCard cursorDockedCard)
        {

        }

        public override void Clicked(MouseButtonEventArgs e, Vector2f worldCoords, ref TestCard cursorDockedCard)
        {

        }

        public override void UnClicked(MouseButtonEventArgs e, Vector2f worldCoords)
        {

        }

        public override void RemoveCard(TestCard removedCard)
        {
            int i = 0;
            CardAreaWidth = Math.Min((int)((CardWidth * CardList[0].ExposeSize + 1) * (CardList.Count - 1)), Width);
            CardAreaStartX = (int)((Width - CardAreaWidth) / 2.0 + StartX);
            CardList.Remove(removedCard);

            foreach (TestCard card in CardList)
            {
                card.DockX = CardAreaStartX + (i * (CardAreaWidth + 1) / (CardList.Count));
                card.DockY = StartY;
                card.Free();
                i++;
            }
        }

        public override void AddCard(int id, int textureX, int textureY)
        {

        }

        public override void AddCard(TestCard addedCard)
        {
            int i = 0;
            CardList.Add(addedCard);
            CardList[CardList.Count - 1].Origin = this;
            //width = maxHandWidth;
            CardAreaWidth = Math.Min((int)((CardList[0].Width * CardList[0].ExposeSize + 1) * (CardList.Count)), Width);
            CardAreaStartX = (int)((Width - CardAreaWidth) / 2.0 + StartX);
            //height = cardList[0].height;
            if (addedCard.Flipped != true) addedCard.Flip();

            foreach (TestCard card in CardList)
            {
                //card.dockX = startX + (i * (cardAreaWidth) / (cardCount+1)) - card.width/2;
                card.DockX = CardAreaStartX + (i * (CardAreaWidth + 1) / (CardList.Count));
                card.DockY = StartY;
                card.Visible = visible;
                //card.handStartX = card.dockX;
                card.Free();
                //card.dockX = card.currentX;
                //card.dockY = card.currentY;
                i++;
            }
            //this.body.Size = new Vector2f(width + 4 * offset, height + 2 * offset);
        }

        public void CardDroppedEvent(TestCard cursorDockedCard, Vector2f worldCoords, bool mousePressed)
        {
            bool containerCollide = Collide((int)worldCoords.X, (int)worldCoords.Y, mousePressed);

            if (containerCollide == true && cursorDockedCard.Origin != this)
            {
                cursorDockedCard.Origin.RemoveCard(cursorDockedCard);
                //if (cursorDockedCard.origin.GetType() != typeof(_test_Hand)) cursorDockedCard.Flip();
                //if (cursorDockedCard.flipped == false) cursorDockedCard.Flip();
                AddCard(cursorDockedCard);
                //cursorDockedCard.Free();
            }
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
            if (visible) target.Draw(Body, states);
        }

        protected override void SetObjectTransform()
        {
            throw new NotImplementedException();
        }
    }
}
