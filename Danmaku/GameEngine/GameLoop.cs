using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Danmaku.GameEngine
{
	static class GameLoop
	{
		public static int frameNumber = 0;
		public static bool frameEven = true;

		public static int FrameNumber
		{
			get { }
		}

		public static void Frame()
		{
			++FrameNumber;
			FrameEven = !FrameEven;
		}
	}
}
