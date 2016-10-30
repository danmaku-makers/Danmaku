using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.DevIl;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform.Windows;


namespace Danmaku.Graphics.OpenGL
{
    struct Image
    {
        //
        #region Конструкторы
        /// <summary>
        /// Создает изображение на основе указанного имени объекта.
        /// </summary>
        /// <param name="objectName"></param>
        public Image(string objectName)
        {
            //
            //TODO - проверка на инициализированность графики
            //
            string fileName = objectName; //TODO - функция генерации имени файла из имени объекта
            if (fileName != null)
            {
                this.isLoaded = BoundImage(fileName, out this.defaultWidth, out this.defaultHeight, out this.id);
                this.width = defaultWidth;
                this.height = defaultHeight;
            }
            else
            {
                this.width = this.defaultHeight = this.defaultWidth = default(int);
                this.height = default(int);
                this.id = default(uint);
                this.isLoaded = false;
            }
        }
        #endregion
        //
        #region Приватные поля
        private readonly uint id;
        readonly private int defaultWidth, defaultHeight;
        private double width, height;
        private bool isLoaded;
        #endregion
        //
        #region Публичные свойства
        public bool IsLoaded
        {
            get { return this.isLoaded; }
        }
        public uint ID
        {
            get { return this.id; }
        }
        public double Height
        {
            get { return this.height; }
        }
        public double Width
        {
            get { return this.width; }
        }
        #endregion
        //
        #region Приватные методы
        static private bool BoundImage(string fileName, out int width, out int height, out uint id)
        {
            //
            //TODO - проверка на инициализированность графики
            //
            int imageID;
            Il.ilGenImages(1, out imageID);
            Il.ilBindImage(imageID);

            bool imageFound = Il.ilLoadImage(fileName);
            if (!imageFound)
            {
                width = default(int);
                height = default(int);
                id = default(int);
                return false;
            }

            width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
            height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);

            Gl.glGenTextures(1, out id);
            Gl.glPixelStorei(Gl.GL_UNPACK_ALIGNMENT, 1);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, id);

            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);

            Gl.glTexEnvf(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_REPLACE);

            switch (Il.ilGetInteger(Il.IL_IMAGE_BITS_PER_PIXEL))
            {
                case 24:
                    Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB, width, height, 0, Gl.GL_RGB, Gl.GL_UNSIGNED_BYTE, Il.ilGetData());
                    break;

                case 32:
                    Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, width, height, 0, Gl.GL_RGBA, Gl.GL_UNSIGNED_BYTE, Il.ilGetData());
                    break;
            }
            return true;
        }
        #endregion
        //
        #region Публичные методы

        /// <summary>
        /// Умножает ширину и длинну на scale
        /// </summary>
        /// <param name="scale"></param>
        public void ScaleSize(double scale)
        {
            this.width *= scale;

        }

        /// <summary>
        /// Устанавилвает изображению исхлодные ширину и высоту
        /// </summary>
        public void ResetSize()
        {
            this.width = defaultWidth;
            this.height = defaultHeight;
        }

        #endregion
    }
}
