using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace Danmaku.GameEngine
{
    static class GameLoop
    {
        public static void StartLoop()
        {
            DateTime startTimePoint = DateTime.Now;
            int frameTime = 1000 / 60;

            Action<Barrier> endDelegate = (x) =>
            {
                frame++;
                frameEven = !frameEven;
                SpinWait.SpinUntil( () => ((DateTime.Now - startTimePoint).TotalMilliseconds >= frameTime)   );
                startTimePoint = DateTime.Now;
            };
            Barrier barrier = new Barrier(3, endDelegate);
            Thread actThread = new Thread(
                () =>
                {
                    for (;;)
                    {
                        ObjectsAct();
                        barrier.SignalAndWait();
                    }
                }
                );
            Thread moveThread = new Thread(
                () =>
                {
                    for (;;)
                    {
                        //ObjectsMove();
                        barrier.SignalAndWait();
                    }
                }
                );

            actThread.Priority = ThreadPriority.Highest;
            moveThread.Priority = ThreadPriority.Highest;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            startTimePoint = DateTime.Now;

            actThread.Start();
            moveThread.Start();
            for (;;)
            {
                Graphics.Graphics.DrawFrame();
                barrier.SignalAndWait();
            }
        }


        private static int frame = 0;
        private static bool frameEven = true;

        public static int CurrentFrame
        {
            get { return frame; }
        }
        public static bool FrameEven
        {
            get { return frameEven; }
        }

        private static void ObjectsAct()
        {
            foreach (var obj in ObjectStorage.Instance)
            {
                obj.Act();
            }
        }

        public static void Act()
        {
            ++frame;
            frameEven = !frameEven;
        }
    }
}
