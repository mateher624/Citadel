using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace citadelGame
{
    class _test_Aether : _test_Container
    {
        public _test_Aether()
        {
            cardList = new List<_test_Card>();
        }

        public override void MouseMove(Vector2f worldCoords, ref _test_Card cursorDockedCard)
        {
            //int cardIndex;
            bool cardFound = false;
            for (int i = cardList.Count - 1; i >= 0; i--)
            {
                bool active;
                if (cardFound == false)
                {
                    active = cardList[i].Collide((int)worldCoords.X, (int)worldCoords.Y);
                    if (active == true)
                    {
                        cardFound = true;
                        //cardIndex = i;
                        cardList[i].Drag((int)worldCoords.X, (int)worldCoords.Y);
                    }
                }
                else active = false;
                cardList[i].MouseCollide(active);
            }
        }

        public override void Clicked(MouseButtonEventArgs e, Vector2f worldCoords, ref _test_Card cursorDockedCard)
        {
            int cardIndex = 0;
            _test_Card chosenCard = null;
            bool eventHappened = false;
            foreach (_test_Card card in cardList)
            {
                if (card.mouseOver == true)
                {
                    cardIndex = cardList.IndexOf(card);
                    chosenCard = card;
                    eventHappened = true;
                }
            }
            if (eventHappened == true)
            {
                cardList.RemoveAt(cardIndex);
                cardList.Add(chosenCard);
                //cusrsorDockedCard = chosenCard;
                Console.WriteLine("Card Taken");
                chosenCard.ClickExecute((int)worldCoords.X, (int)worldCoords.Y, e.Button);
            }
        }

        public override void UnClicked(MouseButtonEventArgs e, Vector2f worldCoords)
        {
            foreach (_test_Card card in cardList) card.UnClicked((int)worldCoords.X, (int)worldCoords.Y);
        }

        public override void RemoveCard(_test_Card removedCard)
        {

        }

        public override void AddCard(_test_Card addedCard)
        {
            cardList.Add(addedCard);
        }

        public override void AddCard(int texture_x, int texture_y)
        {
            _test_Card newCard = new _test_Card(0, 0, cardWidth, cardHeight, face, texture_x, texture_y, this, true);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {

        }

        protected override void SetObjectTransform()
        {
            throw new NotImplementedException();
        }
    }
}
