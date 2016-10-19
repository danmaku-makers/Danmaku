using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Danmaku.GameEngine.Input
{
	struct MoveComponent
	{
		private IPuppeteer puppeteer;
		private int index;

		public int Index
		{
			get { return index; }
		}
		public IPuppeteer Puppeteer
		{
			get { return puppeteer; }
		}

		public MoveComponent(IPuppeteer puppeteer, int index)
		{
			this.puppeteer = puppeteer;
			this.index = index;
		}
		public void Move(GameObject obj)
		{
			this.puppeteer.Move(obj);
		}
	}
}
