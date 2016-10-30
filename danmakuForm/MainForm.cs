using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Tao.DevIl;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform.Windows;

namespace danmakuForm
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            InitializeComponent();
            screen.InitializeContexts();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Window.Load(screen);
            //screen.Width = 1000;
            //screen.Height = 1000;
            //screen.Size = new Size(1000, 1000);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // pdGraphics.ContTest(screen);
            TestClass.SomeTests();
        }
    }
}

