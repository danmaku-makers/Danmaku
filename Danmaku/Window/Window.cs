using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Danmaku.Graphics;
using GraphicControl = Tao.Platform.Windows.SimpleOpenGlControl;

namespace Danmaku
{
    static class Window
    {
        private static GraphicControl graphicControl;
        public static GraphicControl GraphicControl
        {
            get
            {
                return graphicControl;
            }
        }
        public static void Load(GraphicControl newGraphicControl)
        {
            graphicControl = newGraphicControl;
            Graphics.Graphics.Initialize();
            Graphics.Graphics.SetBackground(@"../../Sprites/Menu.png");
            Graphics.Graphics.DrawFrame();
        }

    
    }
}
