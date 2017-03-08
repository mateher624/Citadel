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
        Texture tileset;
        Texture btn_face;
        List<UIButton> button_list;

        public _test_RPG() : base(800, 600, "Game Name", Color.Cyan)
        {
            button_list = new List<UIButton>();
        }

        protected override void CheckCollide(MouseMoveEventArgs e)
        {
            foreach (UIButton button in button_list) button.Collide(e.X, e.Y);
        }

        protected override void CheckClick(MouseButtonEventArgs e)
        {
            foreach (UIButton button in button_list) button.Clicked(e.X, e.Y, e.Button);
        }

        protected override void CheckUnClick(MouseButtonEventArgs e)
        {
            foreach (UIButton button in button_list) button.UnClicked(e.X, e.Y, e.Button);
        }

        protected override void LoadContent()
        {
            tileset = new Texture("Resources/DungeonTileset.png");
            btn_face = new Texture("Resources/btn_play.bmp");
        }

        protected override void Initialize()
        {
            map = new _test_Tilemap(tileset, 4, 4, 32.0f, 64.0f);
            button_list.Add(new UIPrimitiveButton(320, 20, 180, 40));
            button_list.Add(new UIPrimitiveButton(320, 80, 180, 40));
            button_list.Add(new UIGlyphButton(320, 140, 95, 53, btn_face));
            button_list.Add(new UIGlyphButton(500, 140, 95, 53, btn_face));

            UIGlyphButton button_off = new UIGlyphButton(320, 240, 95, 53, btn_face);
            button_off.state = -1;
            button_list.Add(button_off);
        }

        protected override void Tick()
        {

        }

        protected override void Render()
        {
            window.Draw(map);
            foreach (UIButton button in button_list)
            {
                window.Draw(button);
            }
        }
    }
}
