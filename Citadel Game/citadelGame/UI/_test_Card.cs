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
    class TestCard : Drawable
    {
        public int id;

        public int CurrentX;
        public int CurrentY;
        public int Width;
        public int Height;
        public int TextureX;
        public int TextureY;
        private int _dockPosX;
        private int _dockPosY;
        public int OldStartX;
        public int OldStartY;

        public int HandStartX;

        private Texture _face;
        private Sprite _body;

        public int Rotation;
        public bool Visible = true;

        private Vector2f _mouseCoords;
        public bool MouseOver = false;

        public float ExposeSize = 1.2f;

        private bool _exposeAnimationLock = false;
        private int _exposeAnimationDuration = 61;
        private int _exposeAnimationStep = 0;

        private bool _magnetAnimationLock = false;
        private int _magnetAnimationDuration = 100;
        private int _magnetAnimationStep = 0;
        public int DockX;
        public int DockY;

        private bool _flipAnimationLock = false;
        private int _flipAnimationDuration = 100;
        private int _flipAnimationStep = 0;

        public int DestinationX;
        public int DestinationY;

        public bool Flipped;

        public bool FreezePosition = false;

        private bool _exposed;

        //public bool flipped;

        //public Color color;

        public bool Dock;
        //public bool handHeld;

        public TestContainer Origin;

        public TestCard(int id, int startX, int startY, int width, int height, Texture face, int textureX, int textureY, TestContainer origin, bool flipped)
        {
            this.id = id;
            this.Origin = origin;
            //this.flipped = 1;
            this.DockX = startX;
            this.DockY = startY;
            this.CurrentX = 0;
            this.CurrentY = 0;
            this.Width = width;
            this.Height = height;
            this.TextureX = textureX;
            this.TextureY = textureY;
            this._face = face;
            this.HandStartX = startX;
            this.Flipped = flipped;

            //this.handHeld = true;

            this._body = new Sprite();
            this._body.Texture = this._face;
            this._body.TextureRect = new IntRect(0, 0, this.Width, this.Height);
            this._body.Position = new Vector2f(this.CurrentX, this.CurrentY);
            Free();
        }

        protected void Update()
        {
            //MouseCollide(Collide((int)mouseCoords.X, (int)mouseCoords.Y));
            if (_magnetAnimationLock == true)
            {
                //
                if (_magnetAnimationStep == -1) _magnetAnimationLock = false;
                else
                {
                    //if (orgin != Orgin.hand) MouseCollide(Collide((int)mouseCoords.X, (int)mouseCoords.Y));
                    CurrentX = OldStartX - (int)((OldStartX - DockX) * (_magnetAnimationDuration - _magnetAnimationStep) / (float)_magnetAnimationDuration);
                    CurrentY = OldStartY - (int)((OldStartY - DockY) * (_magnetAnimationDuration - _magnetAnimationStep) / (float)_magnetAnimationDuration);
                    _magnetAnimationStep--;
                }
            }
            int currentXMod = CurrentX;
            int currentYMod = CurrentY;
            //if (mouseOver == false && flowAnimationLock == false) destinationX = 0;
            if (Flipped == false) this._body.TextureRect = new IntRect(0 * this.Width, 10 * this.Height, this.Width, this.Height);
            else if (Flipped == true) this._body.TextureRect = new IntRect(TextureX * this.Width, TextureY * this.Height, this.Width, this.Height);

            if (_flipAnimationLock == true && _exposeAnimationLock == false)
            {
                float newScaleX = 1;
                if (_flipAnimationStep == -1)
                {
                    _flipAnimationLock = false;
                    //flipped = flipped == false;
                }
                else if (_flipAnimationStep < _flipAnimationDuration / 2)
                {
                    newScaleX = Math.Abs((((_flipAnimationDuration / 2.0f) - _flipAnimationStep) / (_flipAnimationDuration / 2.0f)));
                    _body.Scale = new Vector2f(newScaleX, 1);
                    _flipAnimationStep--;
                }
                else if (_flipAnimationStep == _flipAnimationDuration / 2)
                {
                    newScaleX = Math.Abs((((_flipAnimationDuration / 2.0f) - _flipAnimationStep) / (_flipAnimationDuration / 2.0f)));
                    _body.Scale = new Vector2f(newScaleX, 1);
                    if (Flipped == true) Flipped = false;
                    else Flipped = true;
                    _flipAnimationStep--;
                }
                else if (_flipAnimationStep <= _flipAnimationDuration)
                {
                    newScaleX = Math.Abs((((_flipAnimationDuration / 2.0f) - _flipAnimationStep) / (_flipAnimationDuration / 2.0f)));
                    _body.Scale = new Vector2f(newScaleX, 1);
                    _flipAnimationStep--;
                }
                if (newScaleX != 1)
                {
                    currentXMod = (int)(currentXMod + ((Width * (1.0f - newScaleX)) / 2.0f));
                }
            }

            // EXPOSE PROCESS
            if (MouseOver == true && _exposed == false)
            {
                if (_exposeAnimationLock == true)
                {
                    if (_exposeAnimationStep == -1)
                    {
                        //float newExposeSize = 1 + (exposeSize - 1) * (animationDuration - animationStep) / animationDuration;
                        //this.body.Scale = new Vector2f(exposeSize, exposeSize);
                        //this.body.Position = new Vector2f(this.start_x - this.width * (exposeSize - 1) / 2, this.start_y - this.height * (exposeSize - 1) / 2);
                        _exposeAnimationLock = false;
                        _exposed = true;
                    }
                    else if (_exposeAnimationStep >= 0)
                    {
                        float newExposeSize = 1 + (ExposeSize - 1) * (_exposeAnimationDuration - _exposeAnimationStep) / _exposeAnimationDuration;
                        this._body.Scale = new Vector2f(newExposeSize, newExposeSize);
                        currentXMod = (int)(currentXMod - this.Width * (newExposeSize - 1) / 2);
                        currentYMod = (int)(currentYMod - this.Height * (newExposeSize - 1) / 2);
                        //this.body.Position = new Vector2f(this.currentX - this.width * (newExposeSize - 1) / 2, this.currentY - this.height * (newExposeSize - 1) / 2);
                        _exposeAnimationStep--;
                    }
                }
            }
            else if (MouseOver == true && _exposed == true)
            {
                this._body.Scale = new Vector2f(ExposeSize, ExposeSize);
                currentXMod = (int)(currentXMod - this.Width * (ExposeSize - 1) / 2);
                currentYMod = (int)(currentYMod - this.Height * (ExposeSize - 1) / 2);
                //this.body.Position = new Vector2f(this.currentX - this.width * (exposeSize - 1) / 2, this.currentY - this.height * (exposeSize - 1) / 2);
            }
            else if (MouseOver == false && _exposed == false)
            {
                if (_exposeAnimationLock == true)
                {
                    if (_exposeAnimationStep == _exposeAnimationDuration+1) _exposeAnimationLock = false;
                    else//if (exposeAnimationStep > 0)
                    {
                        float newExposeSize = 1 + (ExposeSize - 1) * (_exposeAnimationDuration - _exposeAnimationStep) / _exposeAnimationDuration;
                        this._body.Scale = new Vector2f(newExposeSize, newExposeSize);
                        currentXMod = (int)(currentXMod - this.Width * (newExposeSize - 1) / 2);
                        currentYMod = (int)(currentYMod - this.Height * (newExposeSize - 1) / 2);
                        //this.body.Position = new Vector2f(this.currentX - this.width * (newExposeSize - 1) / 2, this.currentY - this.height * (newExposeSize - 1) / 2);
                        _exposeAnimationStep++;
                    }
                }
            }
            this._body.Position = new Vector2f(currentXMod, currentYMod);
        }

        public bool IsOnTop(int x, int y)
        {
            return false;
        }

        public void Expose()
        {
            if (_exposeAnimationLock == false && _flipAnimationLock == false && _exposed == false)
            {
                _exposeAnimationLock = true;
                _exposeAnimationStep = _exposeAnimationDuration;
            }
            //this.body.Scale = new Vector2f(exposeSize, exposeSize);
            //this.body.Position = new Vector2f(this.start_x - this.width * (exposeSize - 1) / 2, this.start_y - this.height * (exposeSize - 1) / 2);
        }

        public void DisExpose()
        {
            if (_exposeAnimationLock == false && _flipAnimationLock == false && _exposed == true)
            {
                _exposeAnimationLock = true;
                _exposeAnimationStep = 0;
                _exposed = false;
            }
            //this.body.Scale = new Vector2f(1.0f, 1.0f);
            //this.body.Position = new Vector2f(this.start_x, this.start_y);
        }

        public void Drag(int x, int y)
        {
            if (Dock == true)
            {
                this.CurrentX = x - _dockPosX;
                this.CurrentY = y - _dockPosY;

                // WYŁĄCZENIE PRZYCIĄGANIA PRZY PRZESUNIĘCIU
                if (Origin.GetType() == typeof(TestAether))
                {
                    this.DockX = this.CurrentX;
                    this.DockY = this.CurrentY;
                }
                this._body.Position = new Vector2f(this.CurrentX, this.CurrentY);
            }
        }

        public bool Collide(int x, int y)
        {
            if (Dock == true) return true;
            if (x >= this.CurrentX && x <= (this.CurrentX + Width) && y >= this.CurrentY && y <= (this.CurrentY + Height)) return true;
            else return false;
        }

        public bool Clicked(int x, int y, Mouse.Button button)
        {
            if (x >= this.CurrentX && x <= (this.CurrentX + Width) && y >= this.CurrentY && y <= (this.CurrentY + Height))
            {
                return true;
            }
            return false;
        }

        public void ClickExecute(int x, int y, Mouse.Button button)
        {
            if (button.ToString() == "Left")
            {
                Dock = true;
                _dockPosX = x - this.CurrentX;
                _dockPosY = y - this.CurrentY;
            }
            /*else if (button.ToString() == "Right")
            {
                if (flipped == false) flipped = true;
                else flipped = true;
            }*/
        }

        public int MouseCollide(bool ifCollide)
        {
            if (MouseOver == true && ifCollide == false)       // ZEJŚCIE MYSZKI
            {
                Console.WriteLine("Mouse left card");
                MouseOver = false;
                DisExpose();
                return -1;
                //if (handHeld == true) freezePosition = false;
            }
            else if (MouseOver == false && ifCollide == true)  // WEJŚCIE MYSZKI
            {
                Console.WriteLine("Mouse entered card");
                MouseOver = true;
                Expose();
                return 1;
            }
            else return 0;
        }

        public int OnOrOff(bool ifCollide)
        {
            if (MouseOver == true && ifCollide == false) return -1;      // ZEJŚCIE MYSZKI
            else if (MouseOver == false && ifCollide == true) return 1;  // WEJŚCIE MYSZKI
            else return 0;
        }

        public void ForceFree()
        {
            FreezePosition = false;
            if ((DockX != CurrentX || DockY != CurrentY))
            {
                if (Origin.GetType() != typeof(TestHand)) MouseCollide(false);
                _magnetAnimationLock = true;
                OldStartX = CurrentX;
                OldStartY = CurrentY;
                _magnetAnimationStep = _magnetAnimationDuration;
            }
        }

        public void Free()
        {
            FreezePosition = false;
            if ((DockX != CurrentX || DockY != CurrentY) && _magnetAnimationLock == false && FreezePosition == false)
            {
                if (Origin.GetType() != typeof(TestHand)) MouseCollide(false);
                _magnetAnimationLock = true;
                OldStartX = CurrentX;
                OldStartY = CurrentY;
                _magnetAnimationStep = _magnetAnimationDuration;
            }
        }

        public void Flip()
        {
            if (_flipAnimationLock == false)
            {
                _flipAnimationLock = true;
                _flipAnimationStep = _flipAnimationDuration;
            }
        }

        public void UnClicked(int x, int y)
        {
            Dock = false;
            if (Origin.GetType() == typeof(TestHand))
            {
                MouseCollide(false);
                _exposed = false;
            }
            Free();
        }

        public void SetMouseCoords(Vector2f e)
        {
            this._mouseCoords = e;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            Update();
            if (Visible) target.Draw(_body, states);
        }
    }
}
