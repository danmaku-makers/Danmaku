using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Danmaku.GameEngine
{
	class ObjectStorage : IEnumerable
	{
		private List<GameObject> objectArray = new List<GameObject>();
		private static ObjectStorage instance = new ObjectStorage();
		private ObjectStorage()
		{

		}

		public static ObjectStorage Instance
		{
			get { return instance; }
		}
		public void Add(GameObject obj)
		{
			objectArray.Add(obj);
		}
		public IEnumerator GetEnumerator()
		{
			for (int i = 0; i < objectArray.Count; ++i)
			{
				yield return objectArray[i];
			}
		}
	}
}
