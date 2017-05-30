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
    class UIPrimitiveButton : UiButton
    {
        private RectangleShape _body;
        private Text _text;
        private Color _primaryColor;
        private Color _secondaryColor;

        public UIPrimitiveButton(int startX, int startY, int width, int height, Color fillColor, Color outlineColor, string caption)
        {
            Font font = new Font("../../Resources/arial.ttf");
            State = 0;
            this.StartX = startX;
            this.StartY = startY;
            this.Width = width;
            this.Height = height;
            _primaryColor = fillColor;
            _secondaryColor = outlineColor;
            this._body = new RectangleShape();

            this._body.FillColor = _primaryColor;
            this._body.OutlineColor = outlineColor;
            this._body.OutlineThickness = 1.0f;
            this._body.Size = new Vector2f(width, height);
            this._body.Position = new Vector2f(this.StartX, this.StartY);

            this._text = new Text();
            _text.Font = font;
            _text.Position = new Vector2f(this.StartX + 10, this.StartY + 10);
            _text.DisplayedString = caption;
            _text.CharacterSize = 20;
        }

        protected override void Update()
        {
            if (State == -1)
            {
                this._body.FillColor = Color.White;
                this._body.OutlineColor = Color.Black;
            }
            else if (State == 0)
            {
                this._body.FillColor = _primaryColor;
                this._body.OutlineColor = _secondaryColor;
            }
            else if (State == 1)
            {
                this._body.FillColor = _secondaryColor;
                this._body.OutlineColor = _primaryColor;
            }
            else if (State == 2)
            {
                this._body.FillColor = Color.Blue;
                this._body.OutlineColor = Color.Red;
            }
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            Update();
            if (Visible) target.Draw(_body, states);
            if (Visible) target.Draw(_text, states);
        }

    } 
}
