using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace citadelGame
{
    class UIPlayerPanel : Drawable
    {
        private int startX;
        private int startY;
        private int width;
        private int height;
        private int offset = 10;

        private bool visible = true;

        private int rotation = 0;

        public int cardWidth;
        public int cardHeight;

        private RectangleShape body;
        private Texture face;

        private Text textCaptionGold;
        //private Text textAmountGold;

        private Text textCaptionCards;
        //private Text textAmountCards;

        private Text textCaptionBuildings;
        //private Text textAmountBuildings;

        private bool mouseOver = false;

        private Sprite portrait;
        private Vector2f portraitCoords = new Vector2f(3, 4);

        private int handCount = 0;
        private int playgroundCount = 0;
        private int goldCount = 0;

        public UIPlayerPanel(int startX, int startY, int width, int height, Texture face, int cardWidth, int cardHeight)
        {
            Font font = new Font("../../Resources/arial.ttf");

            this.startX = startX;
            this.startY = startY;
            this.width = width;
            this.height = height;
            this.face = face;
            this.cardWidth = cardWidth;
            this.cardHeight = cardHeight;
            float ratio = (float)cardHeight / cardWidth;
            //int cardScaledHeight = (int)(cardHeight * ratio);

            this.body = new RectangleShape();

            this.body.FillColor = Color.Green;
            this.body.OutlineColor = Color.Green;
            this.body.OutlineThickness = 1.0f;
            this.body.Size = new Vector2f(width + 2 * offset, height + 2 * offset);
            this.body.Position = new Vector2f(this.startX - offset, this.startY - offset);

            this.portrait = new Sprite();
            this.portrait.Texture = this.face;
            this.portrait.TextureRect = new IntRect((int)portraitCoords.X * this.cardWidth, (int)portraitCoords.Y * this.cardHeight, this.cardWidth, this.cardHeight);
            this.portrait.Scale = new Vector2f(ratio, ratio);
            this.portrait.Position = new Vector2f(this.startX, this.startY);

            textCaptionGold = new Text();
            //textAmountGold = new Text();
            textCaptionCards = new Text();
            //textAmountCards = new Text();
            textCaptionBuildings = new Text();
            //textAmountBuildings = new Text();

            textCaptionGold.Font = font;
            //textAmountGold.Font = font;
            textCaptionCards.Font = font;
            //textAmountCards.Font = font;
            textCaptionBuildings.Font = font;
            //textAmountBuildings.Font = font;

            textCaptionGold.Position = new Vector2f(this.startX, this.startY+(int)(cardHeight *ratio)+10);
            //textAmountGold.Position = new Vector2f(this.startX + 50, this.startY + (int) (cardHeight * ratio) + 10);
            textCaptionCards.Position = new Vector2f(this.startX, this.startY + (int)(cardHeight * ratio) + 35);
            //textAmountCards.Position = new Vector2f(this.startX + 50, this.startY + (int)(cardHeight * ratio) + 35);
            textCaptionBuildings.Position = new Vector2f(this.startX, this.startY + (int)(cardHeight * ratio) + 60);
            //textAmountBuildings.Position = new Vector2f(this.startX + 50, this.startY + (int)(cardHeight * ratio) + 60);

            textCaptionGold.DisplayedString = "Gold: 0";
            //textAmountGold.DisplayedString = "0";
            textCaptionCards.DisplayedString = "Cards: 0";
            //textAmountCards.DisplayedString = "0";
            textCaptionBuildings.DisplayedString = "Builds: 0";
            //textAmountBuildings.DisplayedString = "0";

            textCaptionGold.CharacterSize = 20;
            //textAmountGold.CharacterSize = 20;
            textCaptionCards.CharacterSize = 20;
            //textAmountCards.CharacterSize = 20;
            textCaptionBuildings.CharacterSize = 20;
            //textAmountBuildings.CharacterSize = 20;
        }

        public void SetInfo(int handCount, int playgroundCount, int goldCount)
        {
            this.handCount = handCount;
            this.playgroundCount = playgroundCount;
            this.goldCount = goldCount;
        }

        private void Update()
        {
            textCaptionGold.DisplayedString = "Gold: "  + goldCount.ToString();
            textCaptionCards.DisplayedString = "Cards: " + handCount.ToString();
            textCaptionBuildings.DisplayedString = "Builds: " + playgroundCount.ToString();
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            Update();
            if (visible) target.Draw(body, states);
            if (visible) target.Draw(portrait, states);

            if (visible) target.Draw(textCaptionGold, states);
            //if (visible) target.Draw(textAmountGold, states);
            if (visible) target.Draw(textCaptionCards, states);
            //if (visible) target.Draw(textAmountCards, states);
            if (visible) target.Draw(textCaptionBuildings, states);
            //if (visible) target.Draw(textAmountBuildings, states);
        }
    }
}
