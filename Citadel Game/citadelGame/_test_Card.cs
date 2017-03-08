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
        public int start_x;
        public int start_y;
        public int width;
        public int height;
        public int texture_x;
        public int texture_y;
        private int dock_pos_x;
        private int dock_pos_y;

        Texture face;
        Sprite body;

        public int state;

        public Color color;

        public bool dock;

        public _test_Card(int start_x, int start_y, int width, int height, Texture face, int texture_x, int texture_y)
        {
            state = 1;
            this.start_x = start_x;
            this.start_y = start_y;
            this.width = width;
            this.height = height;
            this.texture_x = texture_x;
            this.texture_y = texture_y;
            this.face = face;


            this.body = new Sprite();
            this.body.Texture = this.face;
            this.body.TextureRect = new IntRect(0, 0, this.width, this.height);
            this.body.Position = new Vector2f(this.start_x, this.start_y);
        }

        protected void Update()
        {
            if (state == 0) this.body.TextureRect = new IntRect(4 * this.width, 4 * this.height, this.width, this.height);
            else if (state == 1) this.body.TextureRect = new IntRect(texture_x * this.width, texture_y * this.height, this.width, this.height);
        }

        public void Collide(int x, int y)
        {
            if (dock == true)
            {
                this.start_x = x - dock_pos_x;
                this.start_y = y - dock_pos_y;
                this.body.Position = new Vector2f(this.start_x, this.start_y);
            }
        }

        public void Clicked(int x, int y, Mouse.Button button)
        {
            if (x >= this.start_x && x <= (this.start_x + width) && y >= this.start_y && y <= (this.start_y + height) && button.ToString() == "Left")
            {
                dock = true;
                dock_pos_x = x - this.start_x;
                dock_pos_y = y - this.start_y;

                //if (state == 0) state = 1;
                //else state = 0;
            }
            
        }

        public void UnClicked(int x, int y, Mouse.Button button)
        {
            dock = false;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            Update();
            target.Draw(body, states);
        }
    }
}
