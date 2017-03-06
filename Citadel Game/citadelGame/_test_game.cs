using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace citadelGame
{
    abstract class _test_game
    {
        protected RenderWindow window;
        protected Color clearColor;

        public _test_game(uint width, uint height, string name, Color clearColor)
        {
            this.window = new RenderWindow(new VideoMode(width, height), name, Styles.Close);
            this.clearColor = clearColor;

            // Set up events

            window.Closed += OnClosed;
        }

        public void Run()
        {
            while (window.IsOpen)
            {
                window.DispatchEvents();
                Tick();

                window.Clear(clearColor);
                Render();
                window.Display();
            }
        }

        protected abstract void LoadContent();
        protected abstract void Initialize();

        protected abstract void Tick();
        protected abstract void Render();

        void OnClosed(object sender, EventArgs e)
        {
            window.Close();
        }

    }
}
