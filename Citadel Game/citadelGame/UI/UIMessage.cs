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

        protected bool Visible = true;

        protected RectangleShape Body;
        protected RectangleShape Shroud;
        protected Text TextTitle;
        protected Text TextCaption;

        public List<TestCard> CardList;
        public UIPrimitiveButton ButtonOK;
        public UIPrimitiveButton ButtonCancel;

        public UIMessage(int startX, int startY, int width, int height, string title, string caption, int screenW, int screenH)
        {

        }

        public abstract void Draw(RenderTarget target, RenderStates states);
    }
}
