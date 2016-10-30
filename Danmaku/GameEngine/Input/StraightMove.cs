using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Danmaku.GameEngine.Input
{
	class StraightMove : IPuppeteer
	{
		private List<Vector> velocity = new List<Vector>();
		private static StraightMove instance = new StraightMove();

		public static StraightMove Instance
		{
			get { return instance; }
		}

		public void Lead(GameObject puppet, Vector speed)
		{
			if (puppet.Input.Puppeteer == this)
			{
				velocity[puppet.Input.Index] = speed;
			}
			else
			{
				if (puppet.Input.Puppeteer != null)
					puppet.Input.Puppeteer.Unbind(puppet);
				puppet.Input = new MoveComponent(this, velocity.Count);
				velocity.Add(speed);
			}
		}

		public void Move(GameObject puppet)
		{
			puppet.Position = puppet.Position + velocity[puppet.Input.Index];
		}

		public void Unbind(GameObject puppet)
		{
			velocity.RemoveAt(puppet.Input.Index);
		}
	}
}
