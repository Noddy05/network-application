using System;
using System.Threading;

namespace NetworkApplication
{
    class Program
    {
        //Ticks per second
        public const int TPS = 10;

        static void Main(string[] args)
        {
            Console.Title = "Network Server";

            Server.Start(50, 26950);

            Thread mainThread = new Thread(new ThreadStart(UpdateThreadManager));
            Thread commandThread = new Thread(new ThreadStart(Command.InitializeThread));
            mainThread.Start();
            commandThread.Start();

            while (true) ;
        }

        private static void UpdateThreadManager()
        {
            while (true)
            {
                Thread.Sleep(1000 / TPS);
                ThreadManager.UpdateMain();
            }
        }
    }
}
