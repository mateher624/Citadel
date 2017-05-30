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
    class TestTilemap : Drawable
    {
        private Texture _tileset;
        private VertexArray _vertexArray;

        private int _mapWidth;
        private int _mapHeight;

        private float _tileTextureDimension;
        private float _tileWorldDimension;

        public TestTilemap(Texture tileset, int mapWidth, int mapHeight, float tileTextureDimension, float tileWorldDimension)
        {
            this._tileset = tileset;
            this._mapWidth = mapWidth;
            this._mapHeight = mapHeight;
            this._tileTextureDimension = tileTextureDimension;
            this._tileWorldDimension = tileWorldDimension;

            _vertexArray = new VertexArray(PrimitiveType.Quads, (uint)(mapWidth * mapHeight * 4));

            TestTile tile = new TestTile(0, 3, Color.White); // 10 16
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
            states.Texture = _tileset;
            target.Draw(_vertexArray, states);
        }

        private void AddTileVerticies(TestTile tile, Vector2f position)
        {
            _vertexArray.Append(new Vertex((new Vector2f(0.0f, 0.0f) + position) * _tileWorldDimension,
                new Vector2f(_tileTextureDimension * tile.X, _tileTextureDimension * tile.Y)));
            _vertexArray.Append(new Vertex((new Vector2f(1.0f, 0.0f) + position) * _tileWorldDimension,
                new Vector2f(_tileTextureDimension * (tile.X + 1), _tileTextureDimension * tile.Y)));
            _vertexArray.Append(new Vertex((new Vector2f(1.0f, 1.0f) + position) * _tileWorldDimension,
                new Vector2f(_tileTextureDimension * (tile.X + 1), _tileTextureDimension * (tile.Y + 1))));
            _vertexArray.Append(new Vertex((new Vector2f(0.0f, 1.0f) + position) * _tileWorldDimension,
                new Vector2f(_tileTextureDimension * tile.X, _tileTextureDimension * (tile.Y + 1))));
        }
    }
}
