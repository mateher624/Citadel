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
    class _test_button : Drawable
    {
        public int start_x;
        public int start_y;
        public int width;
        public int height;

        private int texture_type;
        Texture face;

        RectangleShape body;
        Sprite body_textured;

        public int state;
        

        public _test_button(int start_x, int start_y, int width, int height)
        {
            state = 0;
            this.start_x = start_x;
            this.start_y = start_y;
            this.width = width;
            this.height = height;
            this.texture_type = 0;
            this.body = new RectangleShape();

            this.body.FillColor = Color.Green;
            this.body.OutlineColor = Color.Magenta;
            this.body.OutlineThickness = 1.0f;
            this.body.Size = new Vector2f(width, height);
            this.body.Position = new Vector2f(this.start_x, this.start_y);
        }

        public _test_button(int start_x, int start_y, int width, int height, Texture face)
        {
            state = 0;
            this.start_x = start_x;
            this.start_y = start_y;
            this.width = width;
            this.height = height;

            this.texture_type = 1;
            this.face = face;

            this.body_textured = new Sprite();

            this.body_textured.Texture = this.face;
            this.body_textured.TextureRect = new IntRect(0, 0, this.width, this.height);
            //this.body.Size = new Vector2f(width, height);
            this.body_textured.Position = new Vector2f(this.start_x, this.start_y);
        }

        public void Update()
        {
            if (texture_type == 0)
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
            else
            {
                if (state == -1)
                {
                    this.body_textured.TextureRect = new IntRect(this.width, this.height, this.width, this.height);
                }
                if (state == 0)
                {
                    this.body_textured.TextureRect = new IntRect(0, 0, this.width, this.height);
                }
                else if (state == 1)
                {
                    this.body_textured.TextureRect = new IntRect(this.width, 0, this.width, this.height);
                }
                else if (state == 2)
                {
                    this.body_textured.TextureRect = new IntRect(0, this.height, this.width, this.height);
                }
            }
        }

        public void Collide(int x, int y)
        {
            if (state != 2 && state != -1)
            {
                if (x >= this.start_x && x <= (this.start_x+width) && y >= this.start_y && y <= (this.start_y+height)) state = 1;
                else state = 0;
            }
        }

        public void Clicked(int x, int y, Mouse.Button button)
        {
            if (state != 2 && state != -1)
            {
                if (x >= this.start_x && x <= (this.start_x + width) && y >= this.start_y && y <= (this.start_y + height) && button.ToString() == "Left") state = 2;
                else Collide(x, y);
            }
        }

        public void UnClicked(int x, int y, Mouse.Button button)
        {
            if (state == 2)
            {
                if (x >= this.start_x && x <= (this.start_x + width) && y >= this.start_y && y <= (this.start_y + height)) state = 1;
                else state = 0;
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            Update();
            if (texture_type == 0) target.Draw(body, states);
            else if (texture_type == 1) target.Draw(body_textured, states);

        }
    }
}
