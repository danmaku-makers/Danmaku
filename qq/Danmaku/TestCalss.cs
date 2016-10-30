using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace danmakuForm
{
    class TestClass
    {
        static int counter;
        static DateTime measure = DateTime.Now;
        const string testsResultFile = @"D:\Dropbox\Dropbox\тесты\timer_tests1.txt";
            static System.IO.StreamWriter file = new System.IO.StreamWriter(testsResultFile, true);
        static public void SomeTests()
        {
            if (counter++ < 20)
            {
                DateTime now = DateTime.Now;
                file.WriteLine((now - measure).TotalMilliseconds);
                measure = now;
            }
            else {
                file.Close();
                System.Windows.Forms.Application.Exit();
            }
        }
    }
}
