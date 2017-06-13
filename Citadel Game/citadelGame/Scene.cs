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
    abstract class Scene
    {
        protected RenderWindow Window;
        protected Color ClearColor;

        protected bool OReturn = false;

        public Scene(uint width, uint height, string name, Color clearColor)
        {
            this.Window = new RenderWindow(new VideoMode(width, height), name, Styles.Default);
            this.ClearColor = clearColor;

            // Set up events
            Window.Closed += OnClosed;
            Window.KeyPressed += Window_KeyPressed;
            Window.MouseButtonPressed += Window_MouseButtonPressed;
            Window.MouseButtonReleased += Window_MouseButtonReleased;
            Window.MouseMoved += Window_MouseMoved;
            Window.MouseWheelMoved += Window_MouseWheelMoved;
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
            OReturn = false;
            Window.Close();
        }

        public bool Run()
        {
            LoadContent();
            Initialize();

            // MAIN GAME LOOP
            while (Window.IsOpen)
            {
                Window.DispatchEvents();
                Tick();

                Window.Clear(ClearColor);
                Render();
                Window.Display();
            }

            return OReturn;
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
