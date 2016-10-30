using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Danmaku.GameEngine.Input;

namespace Danmaku.GameEngine
{
	abstract class GameObject
	{
		private int id;
		private Vector positionEven;
		private Vector positionOdd;
		private double directionEven;
		private double directionOdd;

		private static int newID = 1;

		public int ID
		{
			get { return id; }
		}
		public readonly int CreationFrame;
		public int ExistingTime
		{
			get
			{
				return GameLoop.Frame - CreationFrame;
			}
		}
		public Vector Position
		{
			get
			{
				return GameLoop.FrameEven ? positionEven : positionOdd;
			}
			set
			{
				if (GameLoop.FrameEven)
					positionOdd = value;
				else
					positionEven = value;
			}
		}
		public Vector Speed
		{
			get
			{
				var difference = positionOdd - positionEven;
				return GameLoop.FrameEven ? difference : -difference;
			}
			set
			{
				if (GameLoop.FrameEven)
					positionOdd = positionEven + value;
				else
					positionEven = positionOdd + value;
			}
		}
		public double Direction
		{
			get
			{
				return GameLoop.FrameEven ? directionEven : directionOdd;
			}
			set
			{
				if (GameLoop.FrameEven)
					directionOdd = value;
				else
					directionEven = value;
			}
		}
		public MoveComponent Input { get; set; }

		public GameObject()
		{
			id = newID++;
			CreationFrame = GameLoop.Frame;
			ObjectStorage.Instance.Add(this);
		}
		public void Delete()
		{
			Input.Unbind(this);
			id = 0;
		}
		public abstract void Act();

		public static implicit operator bool(GameObject obj)
		{
			return obj.id != 0;
		}
	}
}
