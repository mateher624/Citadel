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
    class _test_Card : Drawable
    {
        public int dockX;
        public int dockY;
        public int start_x;
        public int start_y;
        public int width;
        public int height;
        public int texture_x;
        public int texture_y;
        private int dock_pos_x;
        private int dock_pos_y;
        public int oldStartX;
        public int oldStartY;

        public int handStartX;

        Texture face;
        Sprite body;

        public bool mouseOver = false;

        float exposeSize = 1.2f;

        private bool exposeAnimationLock = false;
        private int exposeAnimationDuration = 200;
        private int exposeAnimationStep = 0;

        private bool magnetAnimationLock = false;
        private int magnetAnimationDuration = 200;
        private int magnetAnimationStep = 0;

        public int state;

        //public Color color;

        public bool dock;

        public _test_Card(int start_x, int start_y, int width, int height, Texture face, int texture_x, int texture_y)
        {
            state = 1;
            this.dockX = start_x;
            this.dockY = start_y;
            this.start_x = start_x;
            this.start_y = start_y;
            this.width = width;
            this.height = height;
            this.texture_x = texture_x;
            this.texture_y = texture_y;
            this.face = face;
            this.handStartX = start_x;

            this.body = new Sprite();
            this.body.Texture = this.face;
            this.body.TextureRect = new IntRect(0, 0, this.width, this.height);
            this.body.Position = new Vector2f(this.start_x, this.start_y);
        }

        protected void Update()
        {
            if (state == 0) this.body.TextureRect = new IntRect(4 * this.width, 4 * this.height, this.width, this.height);
            else if (state == 1) this.body.TextureRect = new IntRect(texture_x * this.width, texture_y * this.height, this.width, this.height);

            if (magnetAnimationLock == true)
            {
                if (magnetAnimationStep == -1) magnetAnimationLock = false;
                else
                {
                    start_x = oldStartX - (int)((oldStartX - dockX) * (magnetAnimationDuration - magnetAnimationStep) / (float)magnetAnimationDuration);
                    start_y = oldStartY - (int)((oldStartY - dockY) * (magnetAnimationDuration - magnetAnimationStep) / (float)magnetAnimationDuration);
                    magnetAnimationStep--;
                }
            }

            if (mouseOver == true)
            {
                if (exposeAnimationStep > 0 && exposeAnimationLock == true)
                {
                    float newExposeSize = 1 + (exposeSize - 1) * (exposeAnimationDuration - exposeAnimationStep) / exposeAnimationDuration;
                    this.body.Scale = new Vector2f(newExposeSize, newExposeSize);
                    this.body.Position = new Vector2f(this.start_x - this.width * (newExposeSize - 1) / 2, this.start_y - this.height * (newExposeSize - 1) / 2);
                    exposeAnimationStep--;
                }
                else if (exposeAnimationStep == 0)
                {
                    //float newExposeSize = 1 + (exposeSize - 1) * (animationDuration - animationStep) / animationDuration;
                    this.body.Scale = new Vector2f(exposeSize, exposeSize);
                    this.body.Position = new Vector2f(this.start_x - this.width * (exposeSize - 1) / 2, this.start_y - this.height * (exposeSize - 1) / 2);       
                }
            }
            else
            {
                this.body.Scale = new Vector2f(1.0f, 1.0f);
                this.body.Position = new Vector2f(this.start_x, this.start_y);
                exposeAnimationLock = false;
            }
        }

        public bool IsOnTop(int x, int y)
        {
            return false;
        }

        public void Expose()
        {
            if (exposeAnimationLock == false)
            {
                exposeAnimationLock = true;
                exposeAnimationStep = exposeAnimationDuration;
            }
            //this.body.Scale = new Vector2f(exposeSize, exposeSize);
            //this.body.Position = new Vector2f(this.start_x - this.width * (exposeSize - 1) / 2, this.start_y - this.height * (exposeSize - 1) / 2);
        }

        public void DisExpose()
        {
            this.body.Scale = new Vector2f(1.0f, 1.0f);
            this.body.Position = new Vector2f(this.start_x, this.start_y);
        }

        public void Drag(int x, int y)
        {
            if (dock == true)
            {
                this.start_x = x - dock_pos_x;
                this.start_y = y - dock_pos_y;

                // WYŁĄCZENIE PRZYCIĄGANIA PRZY PRZESUNIĘCIU
                //this.dockX = this.start_x;
                //this.dockY = this.start_y;

                this.body.Position = new Vector2f(this.start_x, this.start_y);
            }
            else
            {
                
            }
        }

        public bool Collide(int x, int y)
        {

            if (dock == true)
            {
                //mouseOver = true;
                return true;
            }
            if (x >= this.start_x && x <= (this.start_x + width) && y >= this.start_y && y <= (this.start_y + height))
            {
                //mouseOver = true;
                return true;
            }
            else
            {
                //mouseOver = false;
                return false;
            }

        }

        public bool Clicked(int x, int y, Mouse.Button button)
        {
            if (x >= this.start_x && x <= (this.start_x + width) && y >= this.start_y && y <= (this.start_y + height))
            {
                if (button.ToString() == "Left")
                {
                    dock = true;
                    dock_pos_x = x - this.start_x;
                    dock_pos_y = y - this.start_y;
                }
                else if (button.ToString() == "Right")
                {
                    if (state == 0) state = 1;
                    else state = 0;
                }
                return true;
            }
            return false;
        }

        public void UnClicked(int x, int y, Mouse.Button button)
        {
            dock = false;

            if ((dockX != start_x || dockY != start_y) && magnetAnimationLock == false)
            {
                magnetAnimationLock = true;
                oldStartX = start_x;
                oldStartY = start_y;
                magnetAnimationStep = magnetAnimationDuration;
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            Update();
            target.Draw(body, states);
        }
    }
}
