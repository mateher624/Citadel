using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace citadelGame
{
    class UIInfo : UIMessage
    {
        public UIInfo(int startX, int startY, int width, int height, string title, string caption, int screenW, int screenH, List<TestCard> cardList) : base(startX, startY, width, height, title, caption, screenW, screenH)
        {
            Font font = new Font("../../Resources/arial.ttf");

            this.StartX = startX;
            this.StartY = startY;
            this.Width = width;
            this.Height = height;

            this.Body = new RectangleShape();
            this.Background = new Sprite();

            cardAreaStartX = startX + 20 + 40;
            cardAreaStartY = startY + 150;
            cardAreaWidth = width - 2 * 20;
            this.CardList = cardList;

            this.Body.FillColor = Color.Green;
            this.Body.OutlineColor = Color.Green;
            this.Body.OutlineThickness = 1.0f;
            this.Body.Size = new Vector2f(width + 2 * Offset, height + 2 * Offset);
            this.Body.Position = new Vector2f(this.StartX - Offset, this.StartY - Offset);

            this.Background.Texture = new Texture("../../Resources/infobg.png");
            this.Background.TextureRect = new IntRect(0, 0, this.Width + 2 * Offset, this.Height + 2 * Offset);
            this.Background.Position = new Vector2f(this.StartX - Offset, this.StartY - Offset);

            this.Shroud = new RectangleShape();

            this.Shroud.FillColor = new Color(0, 0, 0, 128);
            this.Shroud.Size = new Vector2f(screenW, height + screenH);
            this.Shroud.Position = new Vector2f(0, 0);

            TextTitle = new Text();
            TextCaption = new Text();

            TextTitle.Font = font;
            TextCaption.Font = font;

            TextTitle.Position = new Vector2f(this.StartX+40, this.StartY+30);
            TextCaption.Position = new Vector2f(this.StartX+40, this.StartY + 80);

            TextTitle.DisplayedString = title;
            TextCaption.DisplayedString = caption;

            TextTitle.CharacterSize = 40;
            TextCaption.CharacterSize = 20;

            ButtonOK = new UIPrimitiveButton(this.StartX + (this.Width-100)/2, this.StartY + this.Height - 90, 100, 30, Color.Cyan, Color.Magenta, "OK");
            ButtonToggle = new UIPrimitiveButton(screenW - 190, 10, 180, 40, Color.Red, Color.Magenta, "Toggle Message");

            SetUpCards();
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (Visible) target.Draw(Shroud, states);
            //if (Visible) target.Draw(Body, states);
            if (Visible) target.Draw(Background, states);
            if (Visible) target.Draw(TextTitle, states);
            if (Visible) target.Draw(TextCaption, states);
            target.Draw(ButtonToggle, states);
        }
    }
}
