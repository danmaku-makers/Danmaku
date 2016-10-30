using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Danmaku.GameEngine
{
	class ObjectStorage : IEnumerable<GameObject>
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

		public IEnumerator<GameObject> GetEnumerator()
		{
			int i = 0;
			while (i < objectArray.Count)
			{
				if (objectArray[i])
					yield return objectArray[i++];
				else
				{
					objectArray[i] = objectArray[objectArray.Count - 1];
					objectArray.RemoveAt(objectArray.Count - 1);
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Instance.GetEnumerator();
		}
	}
}
