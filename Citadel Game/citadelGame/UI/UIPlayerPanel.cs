using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace citadelGame
{
    class UiPlayerPanel : Drawable
    {
        private int _startX;
        private int _startY;
        private int _width;
        private int _height;
        private int _offset = 10;

        private bool _visible = true;

        private int _rotation = 0;

        public int CardWidth;
        public int CardHeight;

        private RectangleShape _body;
        private Texture _face;

        private Text _textCaptionGold;
        //private Text textAmountGold;

        private Text _textCaptionCards;
        //private Text textAmountCards;

        private Text _textCaptionBuildings;
        //private Text textAmountBuildings;

        private bool _mouseOver = false;

        private Sprite _portrait;
        private Sprite crown;
        public Vector2f portraitCoords = new Vector2f(0, 1);

        private int _handCount = 0;
        private int _playgroundCount = 0;
        private int _goldCount = 0;

        public bool isKing;

        public UiPlayerPanel(int startX, int startY, int width, int height, Texture face, int cardWidth, int cardHeight, Vector2f portraitCoords)
        {
            Font font = new Font("../../Resources/AGaramondPro-Bold.otf");

            this._startX = startX;
            this._startY = startY;
            this._width = width;
            this._height = height;
            this._face = face;
            this.CardWidth = cardWidth;
            this.CardHeight = cardHeight;
            float ratio = 0.70f; //(float)cardHeight / cardWidth;
            //int cardScaledHeight = (int)(cardHeight * ratio);
            this.portraitCoords = portraitCoords;
            this._body = new RectangleShape();

            this._body.FillColor = Color.Green;
            this._body.OutlineColor = Color.Green;
            this._body.OutlineThickness = 1.0f;
            this._body.Size = new Vector2f(width + 2 * _offset, height + 2 * _offset);
            this._body.Position = new Vector2f(this._startX - _offset, this._startY - _offset);

            this._portrait = new Sprite();
            this._portrait.Texture = this._face;
            this._portrait.TextureRect = new IntRect((int)portraitCoords.X * this.CardWidth, (int)portraitCoords.Y * this.CardHeight, this.CardWidth, this.CardHeight);
            this._portrait.Scale = new Vector2f(ratio, ratio);
            this._portrait.Position = new Vector2f(this._startX, this._startY);

            this.crown = new Sprite();
            this.crown.Texture = new Texture("../../Resources/crown.png");
            this.crown.Scale = new Vector2f(ratio, ratio);
            this.crown.Position = new Vector2f(this._startX+10, this._startY+24);

            _textCaptionGold = new Text();
            //textAmountGold = new Text();
            _textCaptionCards = new Text();
            //textAmountCards = new Text();
            _textCaptionBuildings = new Text();
            //textAmountBuildings = new Text();

            _textCaptionGold.Font = font;
            //textAmountGold.Font = font;
            _textCaptionCards.Font = font;
            //textAmountCards.Font = font;
            _textCaptionBuildings.Font = font;
            //textAmountBuildings.Font = font;

            _textCaptionGold.Position = new Vector2f(this._startX, this._startY+(int)(cardHeight *ratio)+10);
            //textAmountGold.Position = new Vector2f(this.startX + 50, this.startY + (int) (cardHeight * ratio) + 10);
            _textCaptionCards.Position = new Vector2f(this._startX, this._startY + (int)(cardHeight * ratio) + 35);
            //textAmountCards.Position = new Vector2f(this.startX + 50, this.startY + (int)(cardHeight * ratio) + 35);
            _textCaptionBuildings.Position = new Vector2f(this._startX, this._startY + (int)(cardHeight * ratio) + 60);
            //textAmountBuildings.Position = new Vector2f(this.startX + 50, this.startY + (int)(cardHeight * ratio) + 60);

            _textCaptionGold.DisplayedString = "Gold: 0";
            //textAmountGold.DisplayedString = "0";
            _textCaptionCards.DisplayedString = "Cards: 0";
            //textAmountCards.DisplayedString = "0";
            _textCaptionBuildings.DisplayedString = "Builds: 0";
            //textAmountBuildings.DisplayedString = "0";

            _textCaptionGold.CharacterSize = 20;
            //textAmountGold.CharacterSize = 20;
            _textCaptionCards.CharacterSize = 20;
            //textAmountCards.CharacterSize = 20;
            _textCaptionBuildings.CharacterSize = 20;
            //textAmountBuildings.CharacterSize = 20;
        }

        public void SetImage(Vector2f coords)
        {
            this.portraitCoords = coords;
            this._portrait.TextureRect = new IntRect((int)portraitCoords.X * this.CardWidth, (int)portraitCoords.Y * this.CardHeight, this.CardWidth, this.CardHeight);
        }

        public void SetInfo(int handCount, int playgroundCount, int goldCount, bool isKing)
        {
            this._handCount = handCount;
            this._playgroundCount = playgroundCount;
            this._goldCount = goldCount;
            this.isKing = isKing;
        }

        private void Update()
        {
            _textCaptionGold.DisplayedString = "Gold: "  + _goldCount.ToString();
            _textCaptionCards.DisplayedString = "Cards: " + _handCount.ToString();
            _textCaptionBuildings.DisplayedString = "Builds: " + _playgroundCount.ToString();
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            Update();
            //if (_visible) target.Draw(_body, states);
            if (_visible) target.Draw(_portrait, states);

            if (_visible) target.Draw(_textCaptionGold, states);
            //if (visible) target.Draw(textAmountGold, states);
            if (_visible) target.Draw(_textCaptionCards, states);
            //if (visible) target.Draw(textAmountCards, states);
            if (_visible) target.Draw(_textCaptionBuildings, states);
            //if (visible) target.Draw(textAmountBuildings, states);
            if (_visible && isKing) target.Draw(crown, states);
        }
    }
}
