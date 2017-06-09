using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using citadelGame.UI;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace citadelGame
{
    class SceneMenu : TestGame
    {
        Vector2f worldCoords;
        List<UiButton> buttonList;

        private UIImage backgroundImage;
        private Texture buttonTexture;

        private bool mousePressed = false;

        public SceneMenu() : base(1600, 900, "Citadel Game Alpha", Color.Cyan)
        {
            buttonList = new List<UiButton>();
        }

        protected override void CheckCollide(MouseMoveEventArgs e)
        {
            Vector2i mouseCoords = new Vector2i(e.X, e.Y);
            worldCoords = Window.MapPixelToCoords(mouseCoords);

            foreach (UiButton button in buttonList) button.Collide((int)worldCoords.X, (int)worldCoords.Y);
        }

        protected override void CheckClick(MouseButtonEventArgs e)
        {
            Vector2i mouseCoords = new Vector2i(e.X, e.Y);
            worldCoords = Window.MapPixelToCoords(mouseCoords);
            mousePressed = true;
            foreach (UiButton button in buttonList) button.Clicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
        }

        protected override void CheckUnClick(MouseButtonEventArgs e)
        {
            Vector2i mouseCoords = new Vector2i(e.X, e.Y);
            worldCoords = Window.MapPixelToCoords(mouseCoords);

            //BUTTON FUNCTIONS

            bool button0Clicked = buttonList[0].UnClicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
            if (button0Clicked == true)
            {
                
                // STH
                OReturn = true;
                Window.Close();

            }

            bool button1Clicked = buttonList[1].UnClicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
            if (button1Clicked == true)
            {

            }

            bool button2Clicked = buttonList[2].UnClicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
            if (button2Clicked == true)
            {

            }

            bool button3Clicked = buttonList[3].UnClicked((int)worldCoords.X, (int)worldCoords.Y, e.Button);
            if (button3Clicked == true)
            {
                Window.Close();
            }

            mousePressed = false;
        }

        protected override void LoadContent()
        {
            buttonTexture = new Texture("../../Resources/mbutton.png"); 
            backgroundImage = new UIImage(0, 0, 1600, 900, new Texture("../../Resources/menubg.png"));
        }

        protected override void Initialize()
        {
            buttonList.Add(new UIGlyphButton(65, 270, 485, 45, buttonTexture, "Start", 20));
            buttonList.Add(new UIGlyphButton(65, 320, 485, 45, buttonTexture, "High Scores", 50));
            buttonList.Add(new UIGlyphButton(65, 370, 485, 45, buttonTexture, "About", 17));
            buttonList.Add(new UIGlyphButton(65, 780, 485, 45, buttonTexture, "Exit", 17));
        }

        protected override void Tick()
        {
            //throw new NotImplementedException();
        }

        protected override void Render()
        {
            Window.Draw(backgroundImage);
            foreach (UiButton button in buttonList)
            {
                Window.Draw(button);
            }
        }
    }
}
