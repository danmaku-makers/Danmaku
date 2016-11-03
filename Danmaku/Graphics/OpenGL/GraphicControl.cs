using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tao.DevIl;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform.Windows;

namespace Danmaku.Drawing.OpenGL
{
    static class GraphicControl
    {
        static SimpleOpenGlControl screen;
        static int width = 0;
        static int height = 0;
        static DrawableObject background, overlay;

        static public void SetBackground(DrawableObject newBackground)
        {
            background = newBackground;
            SizeToImage(background);
        }
        static public void SetOverlay(DrawableObject newOverlay)
        {
            overlay = newOverlay;
        }

        static public void DrawOverlay()
        {
            if (overlay != null)
                PlatformSpecificGraphics.DrawUIElement(overlay);
        }
        static public void DrawBackground()
        {
            if (background != null)
                PlatformSpecificGraphics.DrawUIElement(background);
        }

        static public void Invalidate()
        {
            screen.Invalidate();
        }

        static public void Load(SimpleOpenGlControl newScreen)
        {
            screen = newScreen;

            if (width != 0)
                screen.Width = width;
            else
                width = screen.Width;

            if (height != 0)
                screen.Height = height;
            else
                height = screen.Height;

            Gl.glViewport(0, 0, width, height);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();

            Glu.gluOrtho2D(0, width, 0, height);
            Gl.glTranslated(width / 2, height / 2, 0);
        }
        /// <summary>
        /// Устанавливает размер экрана равным размеру изобаржения
        /// </summary>
        /// <param name="imageSource"></param>
        static void SizeToImage(DrawableObject imageSource)
        {
            if (!imageSource.Image.IsLoaded)
            {
                throw new ArgumentException("Изображение не загружено!");
            }
            width = (int)imageSource.Image.Width;
            height = (int)imageSource.Image.Height;
            if (screen != null)
            {
                screen.Width = width;
                screen.Height = height;
            }

            Gl.glViewport(0, 0, width, height);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();

            Glu.gluOrtho2D(0, width, 0, height);
            Gl.glTranslated(width / 2, height / 2, 0);
        }
    }
}
