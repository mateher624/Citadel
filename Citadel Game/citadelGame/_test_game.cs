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
    abstract class _test_Game
    {
        protected RenderWindow window;
        protected Color clearColor;

        public _test_Game(uint width, uint height, string name, Color clearColor)
        {
            this.window = new RenderWindow(new VideoMode(width, height), name, Styles.Default);
            this.clearColor = clearColor;

            // Set up events
            window.Closed += OnClosed;
            window.KeyPressed += Window_KeyPressed;
            window.MouseButtonPressed += Window_MouseButtonPressed;
            window.MouseButtonReleased += Window_MouseButtonReleased;
            window.MouseMoved += Window_MouseMoved;
            window.MouseWheelMoved += Window_MouseWheelMoved;
        }

        private void Window_MouseWheelMoved(object sender, MouseWheelEventArgs e)
        {
            Console.WriteLine(e.Delta);
        }

        private void Window_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            Console.WriteLine("X: {0} Y: {1}", e.X, e.Y);
            CheckCollide(e);
        }

        private void Window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            CheckUnClick(e);
        }

        private void Window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine(e.Button);
            CheckClick(e);
        }

        private void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            Console.WriteLine(e.Code);
        }

        void OnClosed(object sender, EventArgs e)
        {
            window.Close();
        }

        public void Run()
        {
            LoadContent();
            Initialize();

            // MAIN GAME LOOP
            while (window.IsOpen)
            {
                window.DispatchEvents();
                Tick();

                window.Clear(clearColor);
                Render();
                window.Display();
            }
        }
        protected abstract void CheckCollide(MouseMoveEventArgs e);
        protected abstract void CheckClick(MouseButtonEventArgs e);
        protected abstract void CheckUnClick(MouseButtonEventArgs e);

        protected abstract void LoadContent();
        protected abstract void Initialize();

        protected abstract void Tick();
        protected abstract void Render();



    }
}
