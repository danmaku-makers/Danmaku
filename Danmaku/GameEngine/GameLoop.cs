using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Danmaku.GameEngine
{
	static class GameLoop
	{
		private static int frame = 0;
		private static bool frameEven = true;

		public static int Frame
		{
			get { return frame; }
		}
		public static bool FrameEven
		{
			get { return frameEven; }
		}

		private static void ObjectsAct()
		{
			foreach (var obj in ObjectStorage.Instance)
			{
				obj.Act();
			}
		}

		public static void Act()
		{
			++frame;
			frameEven = !frameEven;
		}
	}
}
