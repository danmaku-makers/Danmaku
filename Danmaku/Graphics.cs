using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using danmakuForm.OpenGL; 

namespace danmakuForm
{
    static class Graphics
    {
        private static DrawableObject backGround;
        private static DrawableObject HUD;
        public static void SetBackground(string imagePath)
        {
            backGround = new DrawableObject(imagePath);
            PlatformSpecificGraphics.ScreenToImage(backGround.Image);
        }
        public static void SetHud(string imagePath)
        {
            HUD = new DrawableObject(imagePath);
        }
        public static void Initialize()
        {
            PlatformSpecificGraphics.Initialize();
        }
        public static void DrawFrame()
        {
            PlatformSpecificGraphics.StartFrame();
            PlatformSpecificGraphics.Draw(backGround);
            PlatformSpecificGraphics.EndFrame();
        }
    }
}
