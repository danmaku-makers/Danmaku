using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Danmaku.GameEngine.Input;

namespace Danmaku.GameEngine
{
	class GameObject
	{
		private int creationTime;

		public Vector Position { get; set; }
		public Vector Speed { get; set; }
		public InputComponent Input { get; set; }
	}
}
