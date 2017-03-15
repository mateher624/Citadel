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
        protected int offset = 50;

        protected int cardAreaWidth;
        protected int cardAreaStartX;
        protected int cardAreaStartY;

        protected RectangleShape body;
        protected Texture face;
        protected Texture deck;

        protected int cardCount;
        public List<_test_Card> cardList;

        protected bool mouseOver = false;

        public abstract void RemoveCard(_test_Card removedCard);
        public abstract void AddCard(_test_Card removedCard);
        public abstract void AddCard(int texture_x, int texture_y);
        public abstract void Draw(RenderTarget target, RenderStates states);
    }
}
