using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GraphicControl = Tao.Platform.Windows.SimpleOpenGlControl;

namespace danmakuForm
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
            Graphics.Initialize();
            Graphics.SetBackground(@"D:\projects\danmaku\danmaku\danmakugraphics\danmakugraphics\resources\Menu.png");
            Graphics.DrawFrame();
        }

    
    }
}
