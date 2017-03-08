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

        private int mapWidth;
        private int mapHeight;

        private float tileTextureDimension;
        private float tileWorldDimension;

        public _test_Tilemap(Texture tileset, int mapWidth, int mapHeight, float tileTextureDimension, float tileWorldDimension)
        {
            this.tileset = tileset;
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
            this.tileTextureDimension = tileTextureDimension;
            this.tileWorldDimension = tileWorldDimension;

            vertexArray = new VertexArray(PrimitiveType.Quads, (uint)(mapWidth * mapHeight * 4));

            _test_Tile tile = new _test_Tile(0, 3, Color.White); // 10 16
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    AddTileVerticies(tile, new Vector2f((float)i, (float)j));
                }
            }
            //_test_Tile card = new _test_Tile(0, 3, Color.White);
            //AddTileVerticies(card, new Vector2f(100.0f, 100.0f));
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
