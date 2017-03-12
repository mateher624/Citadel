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
            //int i = 0;

            //if (activeCardActive == false)
            {
                //foreach (_test_Card card in cardList)
                for (int i = cardList.Count - 1; i >= 0; i--)
                {
                    //cardList[i].ResPos();
                    int distance = Math.Abs(cardIndex - i);
                    //double deltaX = Math.Abs(1 / (float)distance * card.width * 0.6f);
                    int deltaX = (int)Math.Pow(Math.Abs(Math.Cos(Math.PI / 2 * Math.Min(handSize / 3.0f, distance) / (handSize / 3.0f)) * cardList[i].width * 0.6f), 1.04f);
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
            int i = 1;
            handSize++;
            cardList.Add(new _test_Card(0, startY, 72, 100, deck, texture_x, texture_y));
            foreach (_test_Card card in cardList)
            {
                card.currentX = startX + (i * handWidth / (handSize + 1));
                card.handStartX = card.currentX;
                card.currentY = startY;
                card.dockX = card.currentX;
                card.dockY = card.currentY;
                i++;
            }
        }
    }
}
