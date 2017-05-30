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
        public List<_test_Card> cardList;

        private RectangleShape cardArea;

        private int cardAreaStartX;
        private int cardAreaStartY;

        private int cardAreaWidth;

        public UIDilema(int startX, int startY, int width, int height, string title, string caption, int screenW, int screenH, List<_test_Card> cardList) : base(startX, startY, width, height, title, caption, screenW, screenH)
        {
            Font font = new Font("../../Resources/arial.ttf");

            this.startX = startX;
            this.startY = startY;
            this.width = width;
            this.height = height;

            cardAreaStartX = startX + 20+30;
            cardAreaStartY = startY + 100;
            cardAreaWidth = width - 2 * 20;

            this.cardList = cardList;

            this.body = new RectangleShape();

            this.body.FillColor = Color.Green;
            this.body.OutlineColor = Color.Green;
            this.body.OutlineThickness = 1.0f;
            this.body.Size = new Vector2f(width + 2 * offset, height + 2 * offset);
            this.body.Position = new Vector2f(this.startX - offset, this.startY - offset);

            this.shroud = new RectangleShape();

            this.shroud.FillColor = new Color(0, 0, 0, 128);
            this.shroud.Size = new Vector2f(screenW, height + screenH);
            this.shroud.Position = new Vector2f(0, 0);

            this.cardArea = new RectangleShape();

            this.cardArea.FillColor = Color.Red;
            this.cardArea.OutlineColor = Color.Red;
            this.cardArea.OutlineThickness = 1.0f;
            this.cardArea.Size = new Vector2f(cardAreaWidth, cardList[0].height);
            this.cardArea.Position = new Vector2f(cardAreaStartX, cardAreaStartY);

            textTitle = new Text();
            textCaption = new Text();

            textTitle.Font = font;
            textCaption.Font = font;

            textTitle.Position = new Vector2f(this.startX, this.startY);
            textCaption.Position = new Vector2f(this.startX, this.startY + 30);

            textTitle.DisplayedString = title;
            textCaption.DisplayedString = caption;

            textTitle.CharacterSize = 40;
            textCaption.CharacterSize = 20;

            SetUpCards();

            buttonOK = new UIPrimitiveButton(this.startX + this.width / 2 - 120, this.startY + this.height - 50, 100, 30, Color.Cyan, Color.Magenta, "OK");
            buttonCANCEL = new UIPrimitiveButton(this.startX + this.width / 2 + 20, this.startY + this.height - 50, 100, 30, Color.Cyan, Color.Magenta, "Cancel");
        }

        private void SetUpCards()
        {
            int i = 0;
            cardAreaWidth = Math.Min((int)((cardList[0].width * cardList[0].exposeSize + 1) * (cardList.Count)), width - 2 * 20);
            cardAreaStartX = (int)((width - cardAreaWidth) / 2.0 + startX);
            foreach (_test_Card card in cardList)
            {
                card.dockX = cardAreaStartX + (i * (cardAreaWidth + 1) / (cardList.Count));
                card.dockY = cardAreaStartY;
                card.currentX = card.dockX;
                card.currentY = card.dockY;
                card.oldStartX = card.dockX;
                card.oldStartY = card.dockY;
        //card.Free();
        i++;
            }
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (visible) target.Draw(shroud, states);
            if (visible) target.Draw(body, states);
            //if (visible) target.Draw(cardArea, states);
            if (visible) target.Draw(textTitle, states);
            if (visible) target.Draw(textCaption, states);
        }
    }
}
