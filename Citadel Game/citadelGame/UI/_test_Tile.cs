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
    class TestTile
    {
        public int X;
        public int Y;

        public Color Color;

        public TestTile(int x, int y, Color color)
        {
            this.X = x;
            this.Y = y;
            this.Color = color;
        }
    }
}
