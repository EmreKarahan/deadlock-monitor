using System;
using System.Threading;

namespace Deadlock
{
    public class Program
    {
        public static void Main()
        {
            DeadLock.ExecuteDeadLockCode();
        }
    }

    public class DeadLock
    {
        private static readonly object LockA = new object();
        private static readonly object LockB = new object();

        #region Deadlock Case  

        private static void CompleteWork1()
        {
            lock (LockA)
            {
                Console.WriteLine("Trying to Acquire lock on lockB");
                lock (LockB)
                {
                    Console.WriteLine("Critical Section of CompleteWork1");
                    //Access some shared critical section.  
                }
            }
        }

        private static void CompleteWork2()
        {
            lock (LockB)
            {
                Console.WriteLine("Trying to Acquire lock on lockA");
                lock (LockA)
                {
                    Console.WriteLine("Critical Section of CompleteWork2");
                    //Access some shared critical section.  
                }
            }
        }

        public static void ExecuteDeadLockCode()
        {
            Thread thread1 = new Thread(CompleteWork1);
            Thread thread2 = new Thread(CompleteWork2);
            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();
            //Below code section will never execute due to deadlock.  
            Console.WriteLine("Processing Completed....");
        }


        #endregion
    }
}