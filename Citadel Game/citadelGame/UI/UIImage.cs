using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace citadelGame.UI 
{
    class UIImage : Drawable
    {
        private Texture image;
        private Sprite body;
        private int StartX;
        private int StartY;
        private int Width;
        private int Height;

        public UIImage(int x, int y, int width, int height, Texture image)
        {
            this.image = image;

            this.StartX = x;
            this.StartY = y;
            this.Width = width;
            this.Height = height;
            this.image = image;

            this.body = new Sprite();
            this.body.Texture = this.image;
            this.body.TextureRect = new IntRect(0, 0, this.Width, this.Height);
            this.body.Position = new Vector2f(this.StartX, this.StartY);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(body, states);
        }
    }
}
