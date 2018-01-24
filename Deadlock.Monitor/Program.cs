using System;
using System.Threading;

namespace Deadlock.Monitor
{
    public class Program
    {
        public static void Main()
        {
            DeadLock.ExecuteDeadlockAvoidance();
        }
    }


    public class DeadLock
    {
        private static readonly object LockA = new object();
        private static readonly object LockB = new object();


        #region Deallock Avoidance  

        private static void MyWork1()
        {
            lock (LockA)
            {
                Console.WriteLine("Trying to acquire lock on lockB");
                // This will try to acquire lock for 5 seconds.  
                if (System.Threading.Monitor.TryEnter(LockB, 5000))
                {
                    try
                    {
                        // This block will never be executed.  
                        Console.WriteLine("In DoWork1 Critical Section.");
                        // Access some shared resource here.  
                    }
                    finally
                    {
                        System.Threading.Monitor.Exit(LockB);
                    }
                }
                else
                {
                    // Print lock not able to acquire message.  
                    Console.WriteLine("Unable to acquire lock, exiting MyWork1.");
                }
            }
        }

        private static void MyWork2()
        {
            lock (LockB)
            {
                Console.WriteLine("Trying to acquire lock on lockA");
                lock (LockA)
                {
                    Console.WriteLine("In MyWork2 Critical Section.");
                    // Access some shared resource here.  
                }
            }
        }

        public static void ExecuteDeadlockAvoidance()
        {
            // Initialize thread with address of DoWork1  
            Thread thread1 = new Thread(MyWork1);
            // Initilaize thread with address of DoWork2  
            Thread thread2 = new Thread(MyWork2);
            // Start the Threads.  
            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();
            Console.WriteLine("Done Processing...");
        }


        #endregion
    }
}