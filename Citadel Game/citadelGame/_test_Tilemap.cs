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
    class _test_Tilemap : Drawable
    {
        private Texture tileset;
        private VertexArray vertexArray;

        private int tileWidth;
        private int tileHeight;
        private float tileTextureDimension;
        private float tileWorldDimension;

        public _test_Tilemap(Texture tileset, int tileWidth, int tileHeight, float tileTextureDimension, float tileWorldDimension)
        {
            this.tileset = tileset;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.tileTextureDimension = tileTextureDimension;
            this.tileWorldDimension = tileWorldDimension;

            vertexArray = new VertexArray(PrimitiveType.Quads, (uint)(tileWidth * tileHeight * 4));

            _test_Tile tile = new _test_Tile(10, 16, Color.White);
            for (int i = 0; i < tileWidth; i++)
            {
                for (int j = 0; j < tileHeight; j++)
                {
                    AddTileVerticies(tile, new Vector2f((float)i, (float)j));
                }
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Texture = tileset;
            target.Draw(vertexArray, states);
        }

        private void AddTileVerticies(_test_Tile tile, Vector2f position)
        {
            vertexArray.Append(new Vertex((new Vector2f(0.0f, 0.0f) + position) * tileWorldDimension,
                new Vector2f(tileTextureDimension * tile.x, tileTextureDimension * tile.y)));
            vertexArray.Append(new Vertex((new Vector2f(1.0f, 0.0f) + position) * tileWorldDimension,
                new Vector2f(tileTextureDimension * (tile.x + 1), tileTextureDimension * tile.y)));
            vertexArray.Append(new Vertex((new Vector2f(1.0f, 1.0f) + position) * tileWorldDimension,
                new Vector2f(tileTextureDimension * (tile.x + 1), tileTextureDimension * (tile.y + 1))));
            vertexArray.Append(new Vertex((new Vector2f(0.0f, 1.0f) + position) * tileWorldDimension,
                new Vector2f(tileTextureDimension * tile.x, tileTextureDimension * (tile.y + 1))));
        }
    }
}
