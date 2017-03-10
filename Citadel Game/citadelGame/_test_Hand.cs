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
    class _test_Hand
    {
        int handWidth;
        int handSize;
        int startX;
        int startY;

        Texture deck;
        public List<_test_Card> cardList;
        public _test_Card activeCard;
        public bool activeCardActive = false;

        public _test_Hand(int startX, int startY, int handWidth, Texture deck)
        {
            this.startX = startX;
            this.startY = startY;
            this.handWidth = handWidth;
            this.deck = deck;
            cardList = new List<_test_Card>();
        }

        public void Update(int cardIndex)
        {
            int i = 0;
            foreach (_test_Card card in cardList)
            {
                int distance = Math.Abs(cardIndex - i);
                //double deltaX = Math.Abs(1 / (float)distance * card.width * 0.6f);
                double deltaX = Math.Pow(Math.Abs(Math.Cos(Math.PI / 2 * Math.Min(handSize / 2.0f, distance) / (handSize / 2.0f)) * card.width * 0.6f), 1.04f);
                if (i < cardIndex)
                {
                    card.start_x = (int)(card.start_x - deltaX);
                }
                else if (i > cardIndex)
                {
                    card.start_x = (int)(card.start_x + deltaX);
                }
                i++;
            }
        }

        public void AddCard()
        {
            int i = 1;
            handSize++;
            cardList.Add(new _test_Card(0, startY, 72, 100, deck, 6, 4));
            foreach (_test_Card card in cardList)
            {
                card.start_x = startX + (i * handWidth / (handSize + 1));
                card.handStartX = card.start_x;
                card.dockX = card.start_x;
                card.dockY = card.start_y;
                i++;
            }
        }
    }
}
