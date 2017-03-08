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
    class _test_RPG : _test_Game
    {
        _test_Tilemap map;
        _test_Tilemap map2;
        Texture tileset;
        Texture tileset2;

        public _test_RPG() : base(800, 600, "Game Name", Color.Cyan)
        {

        }

        protected override void LoadContent()
        {
            tileset = new Texture("Resources/DungeonTileset.png");
            //tileset2 = new Texture("Resources/deck.gif");
        }

        protected override void Initialize()
        {
            map = new _test_Tilemap(tileset, 4, 4, 32.0f, 32.0f);
        }

        protected override void Tick()
        {
            //throw new NotImplementedException();
        }

        protected override void Render()
        {
            window.Draw(map);
        }
    }
}
