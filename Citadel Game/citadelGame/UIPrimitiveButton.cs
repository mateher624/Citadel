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
    class UIPrimitiveButton : UIButton
    {
        RectangleShape body;

        public UIPrimitiveButton(int start_x, int start_y, int width, int height)
        {
            state = 0;
            this.start_x = start_x;
            this.start_y = start_y;
            this.width = width;
            this.height = height;
            this.body = new RectangleShape();

            this.body.FillColor = Color.Green;
            this.body.OutlineColor = Color.Magenta;
            this.body.OutlineThickness = 1.0f;
            this.body.Size = new Vector2f(width, height);
            this.body.Position = new Vector2f(this.start_x, this.start_y);
        }

        protected override void Update()
        {
            if (state == -1)
            {
                this.body.FillColor = Color.White;
                this.body.OutlineColor = Color.Black;
            }
            else if (state == 0)
            {
                this.body.FillColor = Color.Green;
                this.body.OutlineColor = Color.Magenta;
            }
            else if (state == 1)
            {
                this.body.FillColor = Color.Magenta;
                this.body.OutlineColor = Color.Green;
            }
            else if (state == 2)
            {
                this.body.FillColor = Color.Blue;
                this.body.OutlineColor = Color.Red;
            }
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            Update();
            target.Draw(body, states);
        }

    }
}
