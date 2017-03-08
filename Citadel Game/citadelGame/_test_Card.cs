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
    class _test_Card
    {
        public int posx;
        public int posy;

        private int width;
        private int height;

        private int texturex;
        private int texturey;

        public Color color;

        public _test_Card(int x, int y, int width, int height, Color color)
        {
            this.posx = x;
            this.posy = y;
            this.color = color;
        }
    }
}
