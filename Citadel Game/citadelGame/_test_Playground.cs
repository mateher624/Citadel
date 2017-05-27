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
    class _test_Playground : _test_Container
    {
        public _test_Playground(int startX, int startY, int width, int height, Texture face, int cardWidth, int cardHeight)
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
            this.body.Position = new Vector2f(this.startX - 2 * offset, this.startY - offset);
        }

        public bool Collide(int x, int y, bool mousePressed)
        {
            if (active == false) return false;
            if (mousePressed == false) mouseOver = false;
            else if (x >= (this.startX - 2 * offset)  && x <= (this.startX + width + 2 * offset) && y >= (this.startY - offset) && y <= (this.startY + height + 1 * offset)) mouseOver = true;
            else mouseOver = false;
            return mouseOver;
        }

        public override void MouseMove(Vector2f worldCoords, ref _test_Card cursorDockedCard)
        {

        }

        public override void Clicked(MouseButtonEventArgs e, Vector2f worldCoords, ref _test_Card cursorDockedCard)
        {

        }

        public override void UnClicked(MouseButtonEventArgs e, Vector2f worldCoords)
        {

        }

        public override void RemoveCard(_test_Card removedCard)
        {
            int i = 0;
            cardAreaWidth = Math.Min((int)((cardWidth * cardList[0].exposeSize + 1) * (cardList.Count - 1)), width);
            cardAreaStartX = (int)((width - cardAreaWidth) / 2.0 + startX);
            cardList.Remove(removedCard);

            foreach (_test_Card card in cardList)
            {
                card.dockX = cardAreaStartX + (i * (cardAreaWidth + 1) / (cardList.Count));
                card.dockY = startY;
                card.Free();
                i++;
            }
        }

        public override void AddCard(int texture_x, int texture_y)
        {

        }

        public override void AddCard(_test_Card addedCard)
        {
            int i = 0;
            cardList.Add(addedCard);
            cardList[cardList.Count - 1].origin = this;
            //width = maxHandWidth;
            cardAreaWidth = Math.Min((int)((cardList[0].width * cardList[0].exposeSize + 1) * (cardList.Count)), width);
            cardAreaStartX = (int)((width - cardAreaWidth) / 2.0 + startX);
            //height = cardList[0].height;
            if (addedCard.flipped != true) addedCard.Flip();

            foreach (_test_Card card in cardList)
            {
                //card.dockX = startX + (i * (cardAreaWidth) / (cardCount+1)) - card.width/2;
                card.dockX = cardAreaStartX + (i * (cardAreaWidth + 1) / (cardList.Count));
                card.dockY = startY;
                //card.handStartX = card.dockX;
                card.Free();
                //card.dockX = card.currentX;
                //card.dockY = card.currentY;
                i++;
            }
            //this.body.Size = new Vector2f(width + 4 * offset, height + 2 * offset);
        }

        public void CardDroppedEvent(_test_Card cursorDockedCard, Vector2f worldCoords, bool mousePressed)
        {
            bool containerCollide = Collide((int)worldCoords.X, (int)worldCoords.Y, mousePressed);

            if (containerCollide == true && cursorDockedCard.origin != this)
            {
                cursorDockedCard.origin.RemoveCard(cursorDockedCard);
                //if (cursorDockedCard.origin.GetType() != typeof(_test_Hand)) cursorDockedCard.Flip();
                //if (cursorDockedCard.flipped == false) cursorDockedCard.Flip();
                AddCard(cursorDockedCard);
                //cursorDockedCard.Free();
            }
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

        protected override void SetObjectTransform()
        {
            throw new NotImplementedException();
        }
    }
}
