using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Danmaku.Drawing.OpenGL;
using Danmaku.GameEngine;
namespace Danmaku.Drawing
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


        public static void SetBackground(string imagePath)
        {
            GraphicControl.SetBackground(new DrawableObject(imagePath));
        }
        public static void SetOverlay(string imagePath)
        {
            GraphicControl.SetOverlay(new DrawableObject(imagePath));
        }

        public static void Initialize()
        {
            PlatformSpecificGraphics.Initialize();
        }
        public static void DrawFrame()
        {
            PlatformSpecificGraphics.StartFrame();
            GraphicControl.DrawBackground();
            PlatformSpecificGraphics.Draw(objectsToDraw);
            GraphicControl.DrawOverlay();
            PlatformSpecificGraphics.EndFrame();
        }
    }
}
