using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Danmaku.Drawing;
using GraphicControl = Tao.Platform.Windows.SimpleOpenGlControl;

namespace Danmaku
{
    static class Window
    {
        public static void Load(GraphicControl newGraphicControl)
        {
            Graphics.Initialize();
            Graphics.SetBackground(@"../../Sprites/Menu.png");
            Drawing.OpenGL.GraphicControl.Load(newGraphicControl);
            Graphics.DrawFrame();
        }

    
    }
}
