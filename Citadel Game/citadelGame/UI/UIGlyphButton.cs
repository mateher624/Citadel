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
    class UiGlyphButton : UiButton
    {
        Texture _face;
        Sprite _body;

        public UiGlyphButton(int startX, int startY, int width, int height, Texture face)
        {
            State = 0;
            this.StartX = startX;
            this.StartY = startY;
            this.Width = width;
            this.Height = height;
            this._face = face;

            this._body = new Sprite();
            this._body.Texture = this._face;
            this._body.TextureRect = new IntRect(0, 0, this.Width, this.Height);
            this._body.Position = new Vector2f(this.StartX, this.StartY);
        }

        protected override void Update()
        {
            if (State == -1) this._body.TextureRect = new IntRect(this.Width, this.Height, this.Width, this.Height);
            else if (State == 0) this._body.TextureRect = new IntRect(0, 0, this.Width, this.Height);
            else if (State == 1) this._body.TextureRect = new IntRect(this.Width, 0, this.Width, this.Height);
            else if (State == 2) this._body.TextureRect = new IntRect(0, this.Height, this.Width, this.Height);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            Update();
            if (Visible) target.Draw(_body, states);
        }
    }
}
