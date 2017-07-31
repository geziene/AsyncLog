using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using LogTest;

namespace LogUsers
{
    class Program
    {
        static void Main(string[] args)
        {
            //a thread will be created to run 


            Do1();//in constructer of this class the thread  will run function mainloop
            Do2();
         
            Console.ReadLine();
        }
        static void Do1()
        {
            ILog logger = new AsyncLog();
            for (int i = 0; i < 15; i++)
            {
                logger.Write("Number with Flush: " + i.ToString());
                Thread.Sleep(50);
            }

            logger.StopWithFlush();
        }
        static void Do2()
        {
            ILog logger2 = new AsyncLog();

            for (int i = 50; i > 0; i--)
            {
                logger2.Write("Number with No flush: " + i.ToString());
                Thread.Sleep(20);
            }

            logger2.StopWithoutFlush();
        }



    }
}
