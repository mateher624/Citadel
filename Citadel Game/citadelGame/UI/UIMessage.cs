﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace citadelGame
{
    abstract class UIMessage : Drawable
    {
        protected int StartX;
        protected int StartY;
        protected int Width;
        protected int Height;
        protected int Offset = 10;

        public bool Visible = true;

        protected RectangleShape Body;
        protected Sprite Background;
        protected RectangleShape Shroud;
        protected Text TextTitle;
        protected Text TextCaption;

        public List<UICard> CardList;
        protected RectangleShape cardArea;
        protected int cardAreaStartX;
        protected int cardAreaStartY;
        protected int cardAreaWidth;


        public UIGlyphButton ButtonOK;
        public UIGlyphButton ButtonCancel;
        public UIGlyphButton ButtonToggle;

        public UIMessage(int startX, int startY, int width, int height, string title, string caption, int screenW, int screenH)
        {

        }

        protected virtual void SetUpCards()
        {
            int i = 0;
            cardAreaWidth = Math.Min((int)((CardList[0].Width * CardList[0].ExposeSize + 1) * (CardList.Count + 1)), Width + 2 * 20);
            cardAreaStartX = (int)(((Width - cardAreaWidth) / 2.0) + StartX);
            //this.cardArea.Size = new Vector2f(cardAreaWidth, CardList[0].Height);
            //this.cardArea.Position = new Vector2f(cardAreaStartX, cardAreaStartY);
            foreach (UICard card in CardList)
            {
                card.DockX = cardAreaStartX + ((i + 1) * (cardAreaWidth) / (CardList.Count + 1)) - CardList[0].Width / 2;
                card.DockY = cardAreaStartY;
                card.CurrentX = card.DockX;
                card.CurrentY = card.DockY;
                card.OldStartX = card.DockX;
                card.OldStartY = card.DockY;
                i++;
            }
        }

        public abstract void Draw(RenderTarget target, RenderStates states);
    }
}
