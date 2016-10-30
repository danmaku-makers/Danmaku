using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Danmaku.Accessary
{
	class FreeList<T> : IEnumerable<T>
	{
		private List<T> array = new List<T>();

		public void Add(T obj)
		{
			array.Add(obj);
		}

		public IEnumerator GetEnumerator()
		{
			throw new NotImplementedException();
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}
}
