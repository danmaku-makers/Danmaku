using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Danmaku.GameEngine.Input
{
	struct InputComponent
	{
		private Action<GameObject> move;
		private int index;

		public int Index
		{
			get { return index; }
		}
		public InputComponent(Action<GameObject> move, int index)
		{
			this.move = move;
			this.index = index;
		}
		public void Move(GameObject puppet)
		{
			move(puppet);
		}
	}
}
