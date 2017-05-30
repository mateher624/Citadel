using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace citadelGame
{
    class UIMessage : Drawable
    {
        protected int startX;
        protected int startY;
        protected int width;
        protected int height;
        protected int offset = 10;

        protected bool visible = true;

        protected RectangleShape body;
        protected RectangleShape shroud;

        protected Text textTitle;
        protected Text textCaption;


        public UIPrimitiveButton buttonOK;
        public UIPrimitiveButton buttonCANCEL;

        public UIMessage(int startX, int startY, int width, int height, string title, string caption, int screenW, int screenH)
        {
            Font font = new Font("../../Resources/arial.ttf");

            this.startX = startX;
            this.startY = startY;
            this.width = width;
            this.height = height;

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

            buttonOK = new UIPrimitiveButton(this.startX + this.width/2 - 120, this.startY + this.height-50, 100, 30, Color.Cyan, Color.Magenta, "OK");
            buttonCANCEL = new UIPrimitiveButton(this.startX + this.width / 2 + 20, this.startY + this.height-50, 100, 30, Color.Cyan, Color.Magenta, "Cancel");
        }

        public virtual void Draw(RenderTarget target, RenderStates states)
        {
            if (visible) target.Draw(shroud, states);
            if (visible) target.Draw(body, states);
            if (visible) target.Draw(textTitle, states);
            if (visible) target.Draw(textCaption, states);
        }
    }
}
