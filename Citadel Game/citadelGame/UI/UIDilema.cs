using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace citadelGame
{
    class UiDilema : UiMessage
    {
        public List<TestCard> CardList;

        private RectangleShape _cardArea;

        private int _cardAreaStartX;
        private int _cardAreaStartY;

        private int _cardAreaWidth;

        public UiDilema(int startX, int startY, int width, int height, string title, string caption, int screenW, int screenH, List<TestCard> cardList) : base(startX, startY, width, height, title, caption, screenW, screenH)
        {
            Font font = new Font("../../Resources/arial.ttf");

            this.StartX = startX;
            this.StartY = startY;
            this.Width = width;
            this.Height = height;

            _cardAreaStartX = startX + 20+30;
            _cardAreaStartY = startY + 100;
            _cardAreaWidth = width - 2 * 20;

            this.CardList = cardList;

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

            this._cardArea = new RectangleShape();

            this._cardArea.FillColor = Color.Red;
            this._cardArea.OutlineColor = Color.Red;
            this._cardArea.OutlineThickness = 1.0f;
            this._cardArea.Size = new Vector2f(_cardAreaWidth, cardList[0].Height);
            this._cardArea.Position = new Vector2f(_cardAreaStartX, _cardAreaStartY);

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

            SetUpCards();

            ButtonOk = new UiPrimitiveButton(this.StartX + this.Width / 2 - 120, this.StartY + this.Height - 50, 100, 30, Color.Cyan, Color.Magenta, "OK");
            ButtonCancel = new UiPrimitiveButton(this.StartX + this.Width / 2 + 20, this.StartY + this.Height - 50, 100, 30, Color.Cyan, Color.Magenta, "Cancel");
        }

        private void SetUpCards()
        {
            int i = 0;
            _cardAreaWidth = Math.Min((int)((CardList[0].Width * CardList[0].ExposeSize + 1) * (CardList.Count)), Width - 2 * 20);
            _cardAreaStartX = (int)((Width - _cardAreaWidth) / 2.0 + StartX);
            foreach (TestCard card in CardList)
            {
                card.DockX = _cardAreaStartX + (i * (_cardAreaWidth + 1) / (CardList.Count));
                card.DockY = _cardAreaStartY;
                card.CurrentX = card.DockX;
                card.CurrentY = card.DockY;
                card.OldStartX = card.DockX;
                card.OldStartY = card.DockY;
        //card.Free();
        i++;
            }
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (Visible) target.Draw(Shroud, states);
            if (Visible) target.Draw(Body, states);
            //if (visible) target.Draw(cardArea, states);
            if (Visible) target.Draw(TextTitle, states);
            if (Visible) target.Draw(TextCaption, states);
        }
    }
}
