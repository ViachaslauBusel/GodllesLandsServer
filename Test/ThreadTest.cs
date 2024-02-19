using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{

    internal class ThreadTest
    {
        [Test]
        public void TestTH()
        {
            TestTH2();
        }


        public async void TestTH2()
        {
            Console.WriteLine($"ThreadID:{Thread.CurrentThread.ManagedThreadId}");
            await RunTask();
            Console.WriteLine($"ThreadID:{Thread.CurrentThread.ManagedThreadId}");
        }   

        public async Task RunTask()
        {
            Console.WriteLine($"ThreadID:{Thread.CurrentThread.ManagedThreadId}");
            await RunTask2();
        }

        private async Task RunTask2()
        { 
            Console.WriteLine($"ThreadID:{Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(11);
        }
    }
}
