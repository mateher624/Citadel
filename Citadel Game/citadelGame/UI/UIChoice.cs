using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace citadelGame.UI
{
    class UIChoice : UIMessage
    {
        

        public UIChoice(int startX, int startY, int width, int height, string title, string caption, int screenW, int screenH) : base(startX, startY, width, height, title, caption, screenW, screenH)
        {
            Font font = new Font("../../Resources/arial.ttf");

            this.StartX = startX;
            this.StartY = startY;
            this.Width = width;
            this.Height = height;

            this.Body = new RectangleShape();

            this.Body.FillColor = Color.Green;
            this.Body.OutlineColor = Color.Green;
            this.Body.OutlineThickness = 1.0f;
            this.Body.Size = new Vector2f(width + 2 * Offset, height + 2 * Offset);
            this.Body.Position = new Vector2f(this.StartX - Offset, this.StartY - Offset);

            this.Shroud = new RectangleShape();

            this.Shroud.FillColor = new Color(0, 0, 0, 128);
            this.Shroud.Size = new Vector2f(screenW, height + screenH);
            this.Shroud.Position = new Vector2f(0, 0);

            TextTitle = new Text();
            TextCaption = new Text();

            TextTitle.Font = font;
            TextCaption.Font = font;

            TextTitle.Position = new Vector2f(this.StartX, this.StartY);
            TextCaption.Position = new Vector2f(this.StartX, this.StartY + 30);

            TextTitle.DisplayedString = title;
            TextCaption.DisplayedString = caption;

            TextTitle.CharacterSize = 40;
            TextCaption.CharacterSize = 20;

            ButtonOK = new UIPrimitiveButton(this.StartX + this.Width / 2 - 120, this.StartY + this.Height - 50, 100, 30, Color.Cyan, Color.Magenta, "Tak");
            ButtonCancel = new UIPrimitiveButton(this.StartX + this.Width / 2 + 20, this.StartY + this.Height - 50, 100, 30, Color.Cyan, Color.Magenta, "Nie");
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (Visible) target.Draw(Shroud, states);
            if (Visible) target.Draw(Body, states);
            if (Visible) target.Draw(TextTitle, states);
            if (Visible) target.Draw(TextCaption, states);
        }
    }
}
