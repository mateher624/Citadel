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
        private RectangleShape body;
        private Text text;
        private Color primaryColor;
        private Color secondaryColor;

        public UIPrimitiveButton(int start_x, int start_y, int width, int height, Color fillColor, Color outlineColor, string caption)
        {
            Font font = new Font("../../Resources/arial.ttf");
            state = 0;
            this.start_x = start_x;
            this.start_y = start_y;
            this.width = width;
            this.height = height;
            primaryColor = fillColor;
            secondaryColor = outlineColor;
            this.body = new RectangleShape();

            this.body.FillColor = primaryColor;
            this.body.OutlineColor = outlineColor;
            this.body.OutlineThickness = 1.0f;
            this.body.Size = new Vector2f(width, height);
            this.body.Position = new Vector2f(this.start_x, this.start_y);

            this.text = new Text();
            text.Font = font;
            text.Position = new Vector2f(this.start_x + 10, this.start_y + 10);
            text.DisplayedString = caption;
            text.CharacterSize = 20;
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
                this.body.FillColor = primaryColor;
                this.body.OutlineColor = secondaryColor;
            }
            else if (state == 1)
            {
                this.body.FillColor = secondaryColor;
                this.body.OutlineColor = primaryColor;
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
            if (visible) target.Draw(body, states);
            if (visible) target.Draw(text, states);
        }

    } 
}
