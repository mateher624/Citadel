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
    abstract class UiButton : Drawable
    {
        public int StartX;
        public int StartY;
        public int Width;
        public int Height;

        protected Text _text;

        public bool Visible = true;
         
        public int State;

        protected abstract void Update();

        public void Collide(int x, int y)
        {
            if (State != 2 && State != -1)
            {
                if (x >= this.StartX && x <= (this.StartX + Width) && y >= this.StartY && y <= (this.StartY + Height)) State = 1;
                else State = 0;
            }
        }

        public void Clicked(int x, int y, Mouse.Button button)
        {
            if (State != 2 && State != -1)
            {
                if (x >= this.StartX && x <= (this.StartX + Width) && y >= this.StartY && y <= (this.StartY + Height) && button.ToString() == "Left") State = 2;
                else Collide(x, y);
            }
        }

        public bool UnClicked(int x, int y, Mouse.Button button)
        {
            if (State == 2)
            {
                if (x >= this.StartX && x <= (this.StartX + Width) && y >= this.StartY && y <= (this.StartY + Height))
                {
                    State = 1;
                    return true;
                }
                else
                {
                    State = 0;
                    return false;
                }
            }
            else return false;
        }

        public abstract void Draw(RenderTarget target, RenderStates states);
    }
}
