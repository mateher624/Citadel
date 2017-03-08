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
        Texture deck;
        List<UIButton> button_list;
        List<_test_Card> card_sprite_list;

        public _test_RPG() : base(800, 600, "Game Name", Color.Cyan)
        {
            button_list = new List<UIButton>();
            card_sprite_list = new List<_test_Card>();
        }

        protected override void CheckCollide(MouseMoveEventArgs e)
        {
            Vector2i mouse_coords = new Vector2i(e.X, e.Y);
            Vector2f world_coords = window.MapPixelToCoords(mouse_coords);
            foreach (UIButton button in button_list) button.Collide((int)world_coords.X, (int)world_coords.Y);
            foreach (_test_Card card in card_sprite_list) card.Collide((int)world_coords.X, (int)world_coords.Y);
        }

        protected override void CheckClick(MouseButtonEventArgs e)
        {
            Vector2i mouse_coords = new Vector2i(e.X, e.Y);
            Vector2f world_coords = window.MapPixelToCoords(mouse_coords);
            foreach (UIButton button in button_list) button.Clicked((int)world_coords.X, (int)world_coords.Y, e.Button);
            foreach (_test_Card card in card_sprite_list) card.Clicked((int)world_coords.X, (int)world_coords.Y, e.Button);
        }

        protected override void CheckUnClick(MouseButtonEventArgs e)
        {
            Vector2i mouse_coords = new Vector2i(e.X, e.Y);
            Vector2f world_coords = window.MapPixelToCoords(mouse_coords);
            foreach (UIButton button in button_list) button.UnClicked((int)world_coords.X, (int)world_coords.Y, e.Button);
            foreach (_test_Card card in card_sprite_list) card.UnClicked((int)world_coords.X, (int)world_coords.Y, e.Button);
        }

        protected override void LoadContent()
        {
            tileset = new Texture("Resources/DungeonTileset.png");
            btn_face = new Texture("Resources/btn_play.bmp");
            deck = new Texture("Resources/deck.gif");
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

            card_sprite_list.Add(new _test_Card(1, 1, 72, 100, deck, 0, 2));
            card_sprite_list.Add(new _test_Card(37, 51, 72, 100, deck, 1, 2));
            card_sprite_list.Add(new _test_Card(73, 101, 72, 100, deck, 2, 2));
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
            foreach (_test_Card card in card_sprite_list)
            {
                window.Draw(card);
            }
        }
    }
}
