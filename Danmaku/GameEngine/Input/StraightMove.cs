using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Danmaku.GameEngine.Input
{
	static class StraightMove
	{
		private static List<Vector> velocity = new List<Vector>();

		public static void Move(GameObject puppet)
		{
			puppet.Position = puppet.Position + velocity[puppet.Input.Index];
		}

		public static void Lead(GameObject puppet, Vector speed)
		{
			puppet.Input = new InputComponent(Move, velocity.Count);
			velocity.Add(speed);
		}
	}
}
