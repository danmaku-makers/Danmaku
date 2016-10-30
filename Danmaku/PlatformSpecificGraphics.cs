using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.DevIl;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform.Windows;
using System.Windows.Forms;

namespace danmakuForm
{
    namespace OpenGL
    {

        static class PlatformSpecificGraphics
        {
            static Image testImage;
            static public void TestScreen(SimpleOpenGlControl screen)
            {
                testImage = new Image(@"d:\projects\danmaku\danmaku\danmakugraphics\danmakugraphics\resources\Menu.png");

                Gl.glViewport(0, 0, (int)testImage.Width, (int)testImage.Height);
                Gl.glMatrixMode(Gl.GL_MODELVIEW);
                Gl.glLoadIdentity();

                Glu.gluOrtho2D(0, (int)testImage.Width, 0, (int)testImage.Height);
                Gl.glTranslated((int)testImage.Width / 2, (int)testImage.Height / 2, 0);

                StartFrame();
                Draw(testImage, 0, 0);
                screen.Invalidate();

            }


            #region Поля
            static bool isInitialized = false;
            static bool frameStarted = false;
            #endregion

            #region Публичные методы

            /// <summary>
            /// Устанавливает размер окна под размер изображения
            /// </summary>
            /// <param name="image"></param>
            public static void ScreenToImage(Image image)
            {
                if (!image.IsLoaded)
                {
                    throw new ArgumentException("Изображение не загржуено!");
                }
                int newWidth = (int)image.Width, newHeight = (int)image.Height;

                //Window.GraphicControl.Width = newWidth;
                // Window.GraphicControl.Height = newHeight;
                Window.GraphicControl.Size = new System.Drawing.Size(newWidth, newHeight);
                Gl.glViewport(0, 0, newWidth, newHeight);
                Gl.glMatrixMode(Gl.GL_MODELVIEW);
                Gl.glLoadIdentity();

                Glu.gluOrtho2D(0, newWidth, 0, newHeight);
                Gl.glTranslated(newWidth / 2, newHeight / 2, 0);
            }

            /// <summary>
            /// Рисует коллекцию объектов
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="objectsToDraw"></param>
            public static void Draw(IEnumerable<DrawableObject> objectsToDraw)
            {
                if (!frameStarted)
                {
                    StartFrame();
                }
                foreach (DrawableObject @object in objectsToDraw)
                {
                    Draw(@object.Image, 0, 0);
                }
            }
            public static void Draw(DrawableObject objectToDraw)
            {
                if (!frameStarted)
                {
                    StartFrame();
                }
                Draw(objectToDraw.Image, 0, 0);
            }

            /// <summary>
            /// Инициализирует работу графики
            /// </summary>
            public static void Initialize()
            {
                if (!isInitialized)
                {
                    Glut.glutInit();
                    Glut.glutInitDisplayMode(Glut.GLUT_RGBA | Glut.GLUT_DOUBLE);
                    Il.ilInit();
                    Il.ilEnable(Il.IL_ORIGIN_SET);
                    Gl.glClearColor(255, 0, 0, 1);
                    isInitialized = true;
                }
            }

            /// <summary>
            /// Инициализирует отрисовку кадра
            /// </summary>
            public static void StartFrame()
            {
                if (!PlatformSpecificGraphics.isInitialized)
                {
                    PlatformSpecificGraphics.Initialize();
                }

                Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
                Gl.glClearColor(0, 0, 0, 1);

                Gl.glEnable(Gl.GL_BLEND);
                Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
                Gl.glEnable(Gl.GL_TEXTURE_2D);
                frameStarted = true;
            }

            /// <summary>
            /// Заершает отрисовку кадра
            /// </summary>
            public static void EndFrame()
            {
                Gl.glDisable(Gl.GL_TEXTURE_2D);
                Gl.glDisable(Gl.GL_BLEND);
                Window.GraphicControl.Invalidate();
                frameStarted = false;
            }

            #endregion

            #region Приватные методы

            // Рисует изображение с центром в (x,y) подглом angleInRadians в радианах.
            private static void Draw(Image image, double x, double y, double angleInRadians)
            {
                if (!image.IsLoaded)
                    throw new ArgumentException("Изображение не загружено!");

                double angle = 180 * angleInRadians / Math.PI;
                double xOffset = image.Width / 2;
                double yOffset = image.Height / 2;

                Gl.glBindTexture(Gl.GL_TEXTURE_2D, image.ID);

                Gl.glPushMatrix();
                Gl.glTranslated(x, y, 0);
                Gl.glRotated(angle, 0, 0, 1);

                Gl.glBegin(Gl.GL_QUADS);

                Gl.glVertex2d(xOffset, -yOffset);
                Gl.glTexCoord2f(1, 0);
                Gl.glVertex2d(xOffset, -yOffset);
                Gl.glTexCoord2f(0, 0);
                Gl.glVertex2d(-xOffset, -yOffset);
                Gl.glTexCoord2f(0, 1);
                Gl.glVertex2d(-xOffset, yOffset);
                Gl.glTexCoord2f(1, 1);

                Gl.glEnd();

                Gl.glPopMatrix();

            }

            // Рисует изображение вертикально с центром в (x,y).
            public static void Draw(Image image, double x, double y)
            {
                if (!image.IsLoaded)
                    throw new ArgumentException("Изображение не загружено!");

                double xOffset = image.Width / 2;
                double yOffset = image.Height / 2;

                Gl.glBindTexture(Gl.GL_TEXTURE_2D, image.ID);

                Gl.glBegin(Gl.GL_QUADS);
                //
                Gl.glTexCoord2f(0, 0);
                Gl.glVertex2d(x - xOffset, y - yOffset);
                Gl.glTexCoord2f(1, 0);
                Gl.glVertex2d(x + xOffset, y - yOffset);
                Gl.glTexCoord2f(1, 1);
                Gl.glVertex2d(x + xOffset, y + yOffset);
                Gl.glTexCoord2f(0, 1);
                Gl.glVertex2d(x - xOffset, y + yOffset);
                //
                Gl.glEnd();
            }

            #endregion
        }
    }
}