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
    abstract class UIButton : Drawable
    {
        public int start_x;
        public int start_y;
        public int width;
        public int height;

        public bool visible = true;
         
        public int state;

        protected abstract void Update();

        public void Collide(int x, int y)
        {
            if (state != 2 && state != -1)
            {
                if (x >= this.start_x && x <= (this.start_x + width) && y >= this.start_y && y <= (this.start_y + height)) state = 1;
                else state = 0;
            }
        }

        public void Clicked(int x, int y, Mouse.Button button)
        {
            if (state != 2 && state != -1)
            {
                if (x >= this.start_x && x <= (this.start_x + width) && y >= this.start_y && y <= (this.start_y + height) && button.ToString() == "Left") state = 2;
                else Collide(x, y);
            }
        }

        public bool UnClicked(int x, int y, Mouse.Button button)
        {
            if (state == 2)
            {
                if (x >= this.start_x && x <= (this.start_x + width) && y >= this.start_y && y <= (this.start_y + height))
                {
                    state = 1;
                    return true;
                }
                else
                {
                    state = 0;
                    return false;
                }
            }
            else return false;
        }

        public abstract void Draw(RenderTarget target, RenderStates states);
    }
}
