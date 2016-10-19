using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Danmaku.GameEngine.Input
{
	interface IPuppeteer
	{
		void Move(GameObject puppet);
		void Unbind(GameObject puppet);
	}
}
