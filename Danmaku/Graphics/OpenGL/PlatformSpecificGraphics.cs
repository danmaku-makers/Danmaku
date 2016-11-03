﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Tao.DevIl;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform.Windows;
using Danmaku.GameEngine;

namespace Danmaku.Drawing.OpenGL
{


    static class PlatformSpecificGraphics
    {





        public static void DrawUIElement(DrawableObject objectToDraw)
        {
            if (!frameStarted)
            {
                StartFrame();
            }
            Draw(objectToDraw.Image, 0, 0);
        }
     
        
        #region Поля
        static bool isInitialized = false;
        static bool frameStarted = false;
        #endregion

        #region Публичные методы

        /// <summary>
        /// Рисует коллекцию объектов
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectsToDraw"></param>
        public static void Draw(IEnumerable<GameObject> objectsToDraw)
        {

            if (!frameStarted)
            {
                StartFrame();
            }
            foreach (GameObject @object in objectsToDraw)
            {
                Draw(@object.GraphicComponent.Image, @object.Position.X, @object.Position.Y);
            }
        }

        public static void Draw(GameObject objectToDraw)
        {
            Draw(objectToDraw.GraphicComponent.Image, objectToDraw.Position.X, objectToDraw.Position.Y);
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
            GraphicControl.Invalidate();
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