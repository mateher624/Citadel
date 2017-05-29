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
    abstract class _test_Container : Drawable
    {
        protected int startX;
        protected int startY;
        protected int width;
        protected int height;
        protected int offset = 10;

        public bool active;
        public bool oldState;

        protected bool visible = true;

        protected int rotation = 0;

        public int cardWidth;
        public int cardHeight;

        protected int cardAreaWidth;
        protected int cardAreaStartX;
        protected int cardAreaStartY;

        protected RectangleShape body;
        protected Texture face;

        public List<_test_Card> cardList;

        protected bool mouseOver = false;

        protected abstract void SetObjectTransform();
        public abstract void MouseMove(Vector2f worldCoords, ref _test_Card cursorDockedCard);
        public abstract void Clicked(MouseButtonEventArgs e, Vector2f worldCoords, ref _test_Card cursorDockedCard);
        public abstract void UnClicked(MouseButtonEventArgs e, Vector2f worldCoords);
        public abstract void RemoveCard(_test_Card removedCard);
        public abstract void AddCard(_test_Card addedCard);
        public abstract void AddCard(int texture_x, int texture_y);
        public abstract void Draw(RenderTarget target, RenderStates states);

        public virtual void Visible()
        {
            if (visible)
            {
                visible = false;
                foreach (var card in cardList)
                {
                    card.visible = false;
                }
            }
            else
            {
                visible = true;
                foreach (var card in cardList)
                {
                    card.visible = true;
                }
            }
        }
    }
}
