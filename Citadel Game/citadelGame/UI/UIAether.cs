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
    class TestAether : TestContainer
    {
        public TestAether()
        {
            CardList = new List<TestCard>();
        }

        public override void MouseMove(Vector2f worldCoords, ref TestCard cursorDockedCard)
        {
            //int cardIndex;
            bool cardFound = false;
            for (int i = CardList.Count - 1; i >= 0; i--)
            {
                bool active;
                if (cardFound == false)
                {
                    active = CardList[i].Collide((int)worldCoords.X, (int)worldCoords.Y);
                    if (active == true)
                    {
                        cardFound = true;
                        //cardIndex = i;
                        CardList[i].Drag((int)worldCoords.X, (int)worldCoords.Y);
                    }
                }
                else active = false;
                CardList[i].MouseCollide(active);
            }
        }

        public override void Clicked(MouseButtonEventArgs e, Vector2f worldCoords, ref TestCard cursorDockedCard)
        {
            int cardIndex = 0;
            TestCard chosenCard = null;
            bool eventHappened = false;
            foreach (TestCard card in CardList)
            {
                if (card.MouseOver == true)
                {
                    cardIndex = CardList.IndexOf(card);
                    chosenCard = card;
                    eventHappened = true;
                }
            }
            if (eventHappened == true)
            {
                CardList.RemoveAt(cardIndex);
                CardList.Add(chosenCard);
                //cusrsorDockedCard = chosenCard;
                Console.WriteLine("Card Taken");
                chosenCard.ClickExecute((int)worldCoords.X, (int)worldCoords.Y, e.Button);
            }
        }

        public override void UnClicked(MouseButtonEventArgs e, Vector2f worldCoords)
        {
            foreach (TestCard card in CardList) card.UnClicked((int)worldCoords.X, (int)worldCoords.Y);
        }

        public override void RemoveCard(TestCard removedCard)
        {

        }

        public override void AddCard(TestCard addedCard)
        {
            CardList.Add(addedCard);
        }

        public override void AddCard(int id, int textureX, int textureY)
        {
            TestCard newCard = new TestCard(id, 0, 0, CardWidth, CardHeight, Face, textureX, textureY, this, true);
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
