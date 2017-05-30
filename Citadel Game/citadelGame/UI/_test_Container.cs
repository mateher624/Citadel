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
    abstract class TestContainer : Drawable
    {
        protected int StartX;
        protected int StartY;
        protected int Width;
        protected int Height;
        protected int Offset = 10;

        public bool Active;
        public bool OldState;

        protected bool visible = true;

        protected int Rotation = 0;

        public int CardWidth;
        public int CardHeight;

        protected int CardAreaWidth;
        protected int CardAreaStartX;
        protected int CardAreaStartY;

        protected RectangleShape Body;
        protected Texture Face;

        public List<TestCard> CardList;

        protected bool MouseOver = false;

        protected abstract void SetObjectTransform();
        public abstract void MouseMove(Vector2f worldCoords, ref TestCard cursorDockedCard);
        public abstract void Clicked(MouseButtonEventArgs e, Vector2f worldCoords, ref TestCard cursorDockedCard);
        public abstract void UnClicked(MouseButtonEventArgs e, Vector2f worldCoords);
        public abstract void RemoveCard(TestCard removedCard);
        public abstract void AddCard(TestCard addedCard);
        public abstract void AddCard(int textureX, int textureY);
        public abstract void Draw(RenderTarget target, RenderStates states);

        public virtual void Visible()
        {
            if (visible)
            {
                visible = false;
                foreach (var card in CardList)
                {
                    card.Visible = false;
                }
            }
            else
            {
                visible = true;
                foreach (var card in CardList)
                {
                    card.Visible = true;
                }
            }
        }
    }
}
