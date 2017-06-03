using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace citadelGame
{
    abstract class UIMessage : Drawable
    {
        protected int StartX;
        protected int StartY;
        protected int Width;
        protected int Height;
        protected int Offset = 10;

        public bool Visible = true;

        protected RectangleShape Body;
        protected RectangleShape Shroud;
        protected Text TextTitle;
        protected Text TextCaption;

        public List<TestCard> CardList;
        protected RectangleShape cardArea;
        protected int cardAreaStartX;
        protected int cardAreaStartY;
        protected int cardAreaWidth;


        public UIPrimitiveButton ButtonOK;
        public UIPrimitiveButton ButtonCancel;
        public UIPrimitiveButton ButtonToggle;

        public UIMessage(int startX, int startY, int width, int height, string title, string caption, int screenW, int screenH)
        {

        }

        protected virtual void SetUpCards()
        {
            int i = 0;
            cardAreaWidth = Math.Min((int)((CardList[0].Width * CardList[0].ExposeSize + 1) * (CardList.Count)), Width - 2 * 20);
            cardAreaStartX = (int)((Width - cardAreaWidth) / 2.0 + StartX);
            foreach (TestCard card in CardList)
            {
                card.DockX = cardAreaStartX + (i * (cardAreaWidth + 1) / (CardList.Count));
                card.DockY = cardAreaStartY;
                card.CurrentX = card.DockX;
                card.CurrentY = card.DockY;
                card.OldStartX = card.DockX;
                card.OldStartY = card.DockY;
                i++;
            }
        }

        public abstract void Draw(RenderTarget target, RenderStates states);
    }
}
