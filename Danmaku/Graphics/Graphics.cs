using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Danmaku.Graphics.OpenGL;
using Danmaku.GameEngine;
namespace Danmaku.Graphics
{
    static class Graphics
    {
        const string spritesPath = @"../../Sprites/";
        public static void BindGraphicComponent(GameObject @object, string spriteType, string color)
        {
            string imagePath = spritesPath + spriteType + "_" + color + ".png";
            @object.GraphicComponent = new DrawableObject(new Image(imagePath));
            objectsToDraw.Add(@object);
        }
        static List<GameObject> objectsToDraw = new List<GameObject>();


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
            PlatformSpecificGraphics.Draw(objectsToDraw);
            PlatformSpecificGraphics.EndFrame();
        }
    }
}
