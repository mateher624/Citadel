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

        public int currentX;
        public int currentY;
        public int width;
        public int height;
        public int texture_x;
        public int texture_y;
        private int dock_pos_x;
        private int dock_pos_y;
        public int oldStartX;
        public int oldStartY;

        public int handStartX;

        private Texture face;
        private Sprite body;

        private Vector2f mouseCoords;
        public bool mouseOver = false;

        public float exposeSize = 1.2f;

        private bool exposeAnimationLock = false;
        private int exposeAnimationDuration = 61;
        private int exposeAnimationStep = 0;

        private bool magnetAnimationLock = false;
        private int magnetAnimationDuration = 100;
        private int magnetAnimationStep = 0;
        public int dockX;
        public int dockY;

        private bool flipAnimationLock = false;
        private int flipAnimationDuration = 100;
        private int flipAnimationStep = 0;

        public int destinationX;
        public int destinationY;

        public int state;

        public bool freezePosition = false;

        private bool exposed;

        //public Color color;

        public bool dock;
        //public bool handHeld;

        public _test_Container origin;

        public _test_Card(int start_x, int start_y, int width, int height, Texture face, int texture_x, int texture_y, _test_Container origin)
        {
            this.origin = origin;
            state = 1;
            this.dockX = start_x;
            this.dockY = start_y;
            this.currentX = 0;
            this.currentY = 0;
            this.width = width;
            this.height = height;
            this.texture_x = texture_x;
            this.texture_y = texture_y;
            this.face = face;
            this.handStartX = start_x;

            //this.handHeld = true;

            this.body = new Sprite();
            this.body.Texture = this.face;
            this.body.TextureRect = new IntRect(0, 0, this.width, this.height);
            this.body.Position = new Vector2f(this.currentX, this.currentY);
            Free();
        }

        protected void Update()
        {
            //MouseCollide(Collide((int)mouseCoords.X, (int)mouseCoords.Y));
            if (magnetAnimationLock == true)
            {
                //
                if (magnetAnimationStep == -1) magnetAnimationLock = false;
                else
                {
                    //if (orgin != Orgin.hand) MouseCollide(Collide((int)mouseCoords.X, (int)mouseCoords.Y));
                    currentX = oldStartX - (int)((oldStartX - dockX) * (magnetAnimationDuration - magnetAnimationStep) / (float)magnetAnimationDuration);
                    currentY = oldStartY - (int)((oldStartY - dockY) * (magnetAnimationDuration - magnetAnimationStep) / (float)magnetAnimationDuration);
                    magnetAnimationStep--;
                }
            }
            int currentXMod = currentX;
            int currentYMod = currentY;
            //if (mouseOver == false && flowAnimationLock == false) destinationX = 0;
            if (state == 0) this.body.TextureRect = new IntRect(4 * this.width, 4 * this.height, this.width, this.height);
            else if (state == 1) this.body.TextureRect = new IntRect(texture_x * this.width, texture_y * this.height, this.width, this.height);

            if (flipAnimationLock == true && exposeAnimationLock == false)
            {
                float newScaleX = 1;
                if (flipAnimationStep == -1)
                {
                    flipAnimationLock = false;
                }
                else if (flipAnimationStep < flipAnimationDuration / 2)
                {
                    newScaleX = Math.Abs((((flipAnimationDuration / 2.0f) - flipAnimationStep) / (flipAnimationDuration / 2.0f)));
                    body.Scale = new Vector2f(newScaleX, 1);
                    flipAnimationStep--;
                }
                else if (flipAnimationStep == flipAnimationDuration / 2)
                {
                    newScaleX = Math.Abs((((flipAnimationDuration / 2.0f) - flipAnimationStep) / (flipAnimationDuration / 2.0f)));
                    body.Scale = new Vector2f(newScaleX, 1);
                    if (state == 1) state = 0;
                    else state = 1;
                    flipAnimationStep--;
                }
                else if (flipAnimationStep <= flipAnimationDuration)
                {
                    newScaleX = Math.Abs((((flipAnimationDuration / 2.0f) - flipAnimationStep) / (flipAnimationDuration / 2.0f)));
                    body.Scale = new Vector2f(newScaleX, 1);
                    flipAnimationStep--;
                }
                if (newScaleX != 1)
                {
                    currentXMod = (int)(currentXMod + ((width * (1.0f - newScaleX)) / 2.0f));
                }
            }

            // EXPOSE PROCESS
            if (mouseOver == true && exposed == false)
            {
                if (exposeAnimationLock == true)
                {
                    if (exposeAnimationStep == -1)
                    {
                        //float newExposeSize = 1 + (exposeSize - 1) * (animationDuration - animationStep) / animationDuration;
                        //this.body.Scale = new Vector2f(exposeSize, exposeSize);
                        //this.body.Position = new Vector2f(this.start_x - this.width * (exposeSize - 1) / 2, this.start_y - this.height * (exposeSize - 1) / 2);
                        exposeAnimationLock = false;
                        exposed = true;
                    }
                    else if (exposeAnimationStep >= 0)
                    {
                        float newExposeSize = 1 + (exposeSize - 1) * (exposeAnimationDuration - exposeAnimationStep) / exposeAnimationDuration;
                        this.body.Scale = new Vector2f(newExposeSize, newExposeSize);
                        currentXMod = (int)(currentXMod - this.width * (newExposeSize - 1) / 2);
                        currentYMod = (int)(currentYMod - this.height * (newExposeSize - 1) / 2);
                        //this.body.Position = new Vector2f(this.currentX - this.width * (newExposeSize - 1) / 2, this.currentY - this.height * (newExposeSize - 1) / 2);
                        exposeAnimationStep--;
                    }
                }
            }
            else if (mouseOver == true && exposed == true)
            {
                this.body.Scale = new Vector2f(exposeSize, exposeSize);
                currentXMod = (int)(currentXMod - this.width * (exposeSize - 1) / 2);
                currentYMod = (int)(currentYMod - this.height * (exposeSize - 1) / 2);
                //this.body.Position = new Vector2f(this.currentX - this.width * (exposeSize - 1) / 2, this.currentY - this.height * (exposeSize - 1) / 2);
            }
            else if (mouseOver == false && exposed == false)
            {
                if (exposeAnimationLock == true)
                {
                    if (exposeAnimationStep == exposeAnimationDuration+1) exposeAnimationLock = false;
                    else//if (exposeAnimationStep > 0)
                    {
                        float newExposeSize = 1 + (exposeSize - 1) * (exposeAnimationDuration - exposeAnimationStep) / exposeAnimationDuration;
                        this.body.Scale = new Vector2f(newExposeSize, newExposeSize);
                        currentXMod = (int)(currentXMod - this.width * (newExposeSize - 1) / 2);
                        currentYMod = (int)(currentYMod - this.height * (newExposeSize - 1) / 2);
                        //this.body.Position = new Vector2f(this.currentX - this.width * (newExposeSize - 1) / 2, this.currentY - this.height * (newExposeSize - 1) / 2);
                        exposeAnimationStep++;
                    }
                }
            }
            this.body.Position = new Vector2f(currentXMod, currentYMod);
        }

        public bool IsOnTop(int x, int y)
        {
            return false;
        }

        public void Expose()
        {
            if (exposeAnimationLock == false && flipAnimationLock == false && exposed == false)
            {
                exposeAnimationLock = true;
                exposeAnimationStep = exposeAnimationDuration;
            }
            //this.body.Scale = new Vector2f(exposeSize, exposeSize);
            //this.body.Position = new Vector2f(this.start_x - this.width * (exposeSize - 1) / 2, this.start_y - this.height * (exposeSize - 1) / 2);
        }

        public void DisExpose()
        {
            if (exposeAnimationLock == false && flipAnimationLock == false && exposed == true)
            {
                exposeAnimationLock = true;
                exposeAnimationStep = 0;
                exposed = false;
            }
            //this.body.Scale = new Vector2f(1.0f, 1.0f);
            //this.body.Position = new Vector2f(this.start_x, this.start_y);
        }

        public void Drag(int x, int y)
        {
            if (dock == true)
            {
                this.currentX = x - dock_pos_x;
                this.currentY = y - dock_pos_y;

                // WYŁĄCZENIE PRZYCIĄGANIA PRZY PRZESUNIĘCIU
                if (origin.GetType() == typeof(_test_Aether))
                {
                    this.dockX = this.currentX;
                    this.dockY = this.currentY;
                }
                this.body.Position = new Vector2f(this.currentX, this.currentY);
            }
        }

        public bool Collide(int x, int y)
        {
            if (dock == true) return true;
            if (x >= this.currentX && x <= (this.currentX + width) && y >= this.currentY && y <= (this.currentY + height)) return true;
            else return false;
        }

        public bool Clicked(int x, int y, Mouse.Button button)
        {
            if (x >= this.currentX && x <= (this.currentX + width) && y >= this.currentY && y <= (this.currentY + height))
            {
                return true;
            }
            return false;
        }

        public void ClickExecute(int x, int y, Mouse.Button button)
        {
            if (button.ToString() == "Left")
            {
                dock = true;
                dock_pos_x = x - this.currentX;
                dock_pos_y = y - this.currentY;
            }
            else if (button.ToString() == "Right")
            {
                if (state == 0) state = 1;
                else state = 0;
            }
        }

        public int MouseCollide(bool ifCollide)
        {
            if (mouseOver == true && ifCollide == false)       // ZEJŚCIE MYSZKI
            {
                Console.WriteLine("Mouse left card");
                mouseOver = false;
                DisExpose();
                return -1;
                //if (handHeld == true) freezePosition = false;
            }
            else if (mouseOver == false && ifCollide == true)  // WEJŚCIE MYSZKI
            {
                Console.WriteLine("Mouse entered card");
                mouseOver = true;
                Expose();
                return 1;
            }
            else return 0;
        }

        public int OnOrOff(bool ifCollide)
        {
            if (mouseOver == true && ifCollide == false) return -1;      // ZEJŚCIE MYSZKI
            else if (mouseOver == false && ifCollide == true) return 1;  // WEJŚCIE MYSZKI
            else return 0;
        }

        public void ForceFree()
        {
            freezePosition = false;
            if ((dockX != currentX || dockY != currentY))
            {
                if (origin.GetType() != typeof(_test_Hand)) MouseCollide(false);
                magnetAnimationLock = true;
                oldStartX = currentX;
                oldStartY = currentY;
                magnetAnimationStep = magnetAnimationDuration;
            }
        }

        public void Free()
        {
            freezePosition = false;
            if ((dockX != currentX || dockY != currentY) && magnetAnimationLock == false && freezePosition == false)
            {
                if (origin.GetType() != typeof(_test_Hand)) MouseCollide(false);
                magnetAnimationLock = true;
                oldStartX = currentX;
                oldStartY = currentY;
                magnetAnimationStep = magnetAnimationDuration;
            }
        }

        public void Flip()
        {
            if (flipAnimationLock == false && origin.GetType() != typeof(_test_Hand))
            {
                flipAnimationLock = true;
                flipAnimationStep = flipAnimationDuration;
            }
        }

        public void UnClicked(int x, int y)
        {
            dock = false;
            if (origin.GetType() == typeof(_test_Hand))
            {
                MouseCollide(false);
                exposed = false;
            }
            Free();
        }

        public void SetMouseCoords(Vector2f e)
        {
            this.mouseCoords = e;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            Update();
            target.Draw(body, states);
        }
    }
}
