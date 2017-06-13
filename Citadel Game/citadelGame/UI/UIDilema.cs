using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace citadelGame
{
    class UIDilema : UIMessage
    {
        private RectangleShape cardArea;
        private int cardAreaStartX;
        private int cardAreaStartY;
        private int cardAreaWidth;

        public UIDilema(int startX, int startY, int width, int height, string title, string caption, int screenW, int screenH, List<UICard> cardList) : base(startX, startY, width, height, title, caption, screenW, screenH)
        {
            Font font = new Font("../../Resources/AGaramondPro-Bold.otf");
            Texture btnTexture = new Texture("../../Resources/sbutton.png");

            this.StartX = startX;
            this.StartY = startY;
            this.Width = width;
            this.Height = height;

            cardAreaStartX = startX;
            cardAreaStartY = startY + 180;
            cardAreaWidth = width - 2 * 20;

            this.CardList = cardList;

            this.Body = new RectangleShape();
            this.Background = new Sprite();

            this.Body.FillColor = Color.Green;
            this.Body.OutlineColor = Color.Green;
            this.Body.OutlineThickness = 1.0f;
            this.Body.Size = new Vector2f(width + 2 * Offset, height + 2 * Offset);
            this.Body.Position = new Vector2f(this.StartX - Offset, this.StartY - Offset);

            this.Background.Texture = new Texture("../../Resources/dilemabg.png");
            this.Background.TextureRect = new IntRect(0, 0, this.Width + 2 * Offset, this.Height + 2 * Offset);
            this.Background.Position = new Vector2f(this.StartX - Offset, this.StartY - Offset);

            this.Shroud = new RectangleShape();

            this.Shroud.FillColor = new Color(0, 0, 0, 128);
            this.Shroud.Size = new Vector2f(screenW, height + screenH);
            this.Shroud.Position = new Vector2f(0, 0);

            this.cardArea = new RectangleShape();

            this.cardArea.FillColor = Color.Red;
            this.cardArea.OutlineColor = Color.Red;
            this.cardArea.OutlineThickness = 1.0f;
            this.cardArea.Size = new Vector2f(cardAreaWidth, cardList[0].Height);
            this.cardArea.Position = new Vector2f(cardAreaStartX, cardAreaStartY);

            TextTitle = new Text();
            TextCaption = new Text();

            TextTitle.Font = font;
            TextCaption.Font = font;

            TextTitle.Position = new Vector2f(this.StartX + 40, this.StartY + 30);
            TextCaption.Position = new Vector2f(this.StartX + 40, this.StartY + 80);

            TextTitle.DisplayedString = title;
            TextCaption.DisplayedString = caption;

            TextTitle.CharacterSize = 40;
            TextCaption.CharacterSize = 20;

            //ButtonToggle = new UIPrimitiveButton(screenW-190, 10, 180, 40, Color.Red, Color.Magenta, "Toggle Message");

            ButtonToggle = new UIGlyphButton(screenW - 205, 10, 195, 45, btnTexture, "Toggle Message", 65);

            SetUpCards();
        }

        protected override void SetUpCards()
        {
            int i = 0;
            cardAreaWidth = Math.Min((int)((CardList[0].Width * CardList[0].ExposeSize) * (CardList.Count+1)), Width + 2* 20);
            cardAreaStartX = (int)(((Width - cardAreaWidth) / 2.0) + StartX);
            this.cardArea.Size = new Vector2f(cardAreaWidth, CardList[0].Height);
            this.cardArea.Position = new Vector2f(cardAreaStartX, cardAreaStartY);
            foreach (UICard card in CardList)
            {
                card.DockX = cardAreaStartX + ((i+1) * (cardAreaWidth) / (CardList.Count+1))-CardList[0].Width/2;
                card.DockY = cardAreaStartY;
                card.CurrentX = card.DockX;
                card.CurrentY = card.DockY;
                card.OldStartX = card.DockX;
                card.OldStartY = card.DockY;
                i++;
            }
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (Visible) target.Draw(Shroud, states);
            //if (Visible) target.Draw(Body, states);
            if (Visible) target.Draw(Background, states);
            //if (Visible) target.Draw(cardArea, states);
            if (Visible) target.Draw(TextTitle, states);
            if (Visible) target.Draw(TextCaption, states);
            target.Draw(ButtonToggle, states);
        }
    }
}
