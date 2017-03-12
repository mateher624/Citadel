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
    class _test_Playground : Drawable
    {
        private int startX;
        private int startY;
        private int width;
        private int height;
        int offset = 50;

        private bool mouseOver = false;

        RectangleShape body;

        private int cardCount;
        Texture deck;
        public List<_test_Card> cardList;

        public _test_Playground(int startX, int startY, int width, int height, Texture deck)
        {
            this.startX = startX;
            this.startY = startY;
            this.width = width;
            this.height = height;
            this.deck = deck;
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
            if (mousePressed == false) mouseOver = false;
            else if (x >= (this.startX - 2 * offset)  && x <= (this.startX + width + 2 * offset) && y >= (this.startY - offset) && y <= (this.startY + height + 1 * offset)) mouseOver = true;
            else mouseOver = false;
            return mouseOver;
        }

        public void AddCard(int texture_x, int texture_y, _test_Card addedCard)
        {
            int i = 0;
            cardCount++;
            //width = Math.Min((int)(72 * 1.2 * (cardCount + 1)), maxTableWidth);
            height = 100;
            cardList.Add(new _test_Card(0, startY, 72, 100, deck, texture_x, texture_y));
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
