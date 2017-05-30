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
    class TestButton : Drawable
    {
        public int StartX;
        public int StartY;
        public int Width;
        public int Height;

        private int _textureType;
        Texture _face;

        RectangleShape _body;
        Sprite _bodyTextured;

        public int State;
        

        public TestButton(int startX, int startY, int width, int height)
        {
            State = 0;
            this.StartX = startX;
            this.StartY = startY;
            this.Width = width;
            this.Height = height;
            this._textureType = 0;
            this._body = new RectangleShape();

            this._body.FillColor = Color.Green;
            this._body.OutlineColor = Color.Magenta;
            this._body.OutlineThickness = 1.0f;
            this._body.Size = new Vector2f(width, height);
            this._body.Position = new Vector2f(this.StartX, this.StartY);
        }

        public TestButton(int startX, int startY, int width, int height, Texture face)
        {
            State = 0;
            this.StartX = startX;
            this.StartY = startY;
            this.Width = width;
            this.Height = height;

            this._textureType = 1;
            this._face = face;

            this._bodyTextured = new Sprite();

            this._bodyTextured.Texture = this._face;
            this._bodyTextured.TextureRect = new IntRect(0, 0, this.Width, this.Height);
            //this.body.Size = new Vector2f(width, height);
            this._bodyTextured.Position = new Vector2f(this.StartX, this.StartY);
        }

        public void Update()
        {
            if (_textureType == 0)
            {
                if (State == -1)
                {
                    this._body.FillColor = Color.White;
                    this._body.OutlineColor = Color.Black;
                }
                else if (State == 0)
                {
                    this._body.FillColor = Color.Green;
                    this._body.OutlineColor = Color.Magenta;
                }
                else if (State == 1)
                {
                    this._body.FillColor = Color.Magenta;
                    this._body.OutlineColor = Color.Green;
                }
                else if (State == 2)
                {
                    this._body.FillColor = Color.Blue;
                    this._body.OutlineColor = Color.Red;
                }
            }
            else
            {
                if (State == -1)
                {
                    this._bodyTextured.TextureRect = new IntRect(this.Width, this.Height, this.Width, this.Height);
                }
                if (State == 0)
                {
                    this._bodyTextured.TextureRect = new IntRect(0, 0, this.Width, this.Height);
                }
                else if (State == 1)
                {
                    this._bodyTextured.TextureRect = new IntRect(this.Width, 0, this.Width, this.Height);
                }
                else if (State == 2)
                {
                    this._bodyTextured.TextureRect = new IntRect(0, this.Height, this.Width, this.Height);
                }
            }
        }

        public void Collide(int x, int y)
        {
            if (State != 2 && State != -1)
            {
                if (x >= this.StartX && x <= (this.StartX+Width) && y >= this.StartY && y <= (this.StartY+Height)) State = 1;
                else State = 0;
            }
        }

        public void Clicked(int x, int y, Mouse.Button button)
        {
            if (State != 2 && State != -1)
            {
                if (x >= this.StartX && x <= (this.StartX + Width) && y >= this.StartY && y <= (this.StartY + Height) && button.ToString() == "Left") State = 2;
                else Collide(x, y);
            }
        }

        public void UnClicked(int x, int y, Mouse.Button button)
        {
            if (State == 2)
            {
                if (x >= this.StartX && x <= (this.StartX + Width) && y >= this.StartY && y <= (this.StartY + Height)) State = 1;
                else State = 0;
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            Update();
            if (_textureType == 0) target.Draw(_body, states);
            else if (_textureType == 1) target.Draw(_bodyTextured, states);

        }
    }
}
