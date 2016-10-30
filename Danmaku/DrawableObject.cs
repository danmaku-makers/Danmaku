using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using danmakuForm.OpenGL;

namespace Danmaku.Graphics
{
    class DrawableObject
    {
        private Image image;
        public virtual Image Image
        {
            get
            {
                return image;
            }
        }
        public DrawableObject(string imagePath)
        {
            image = new Image(imagePath);
        }
        public DrawableObject(Image image)
        {
            this.image = image;
        }
    }
}
