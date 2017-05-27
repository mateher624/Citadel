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
    class UIGlyphButton : UIButton
    {
        Texture face;
        Sprite body;

        public UIGlyphButton(int start_x, int start_y, int width, int height, Texture face)
        {
            state = 0;
            this.start_x = start_x;
            this.start_y = start_y;
            this.width = width;
            this.height = height;
            this.face = face;

            this.body = new Sprite();
            this.body.Texture = this.face;
            this.body.TextureRect = new IntRect(0, 0, this.width, this.height);
            this.body.Position = new Vector2f(this.start_x, this.start_y);
        }

        protected override void Update()
        {
            if (state == -1) this.body.TextureRect = new IntRect(this.width, this.height, this.width, this.height);
            else if (state == 0) this.body.TextureRect = new IntRect(0, 0, this.width, this.height);
            else if (state == 1) this.body.TextureRect = new IntRect(this.width, 0, this.width, this.height);
            else if (state == 2) this.body.TextureRect = new IntRect(0, this.height, this.width, this.height);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            Update();
            if (visible) target.Draw(body, states);
        }
    }
}
