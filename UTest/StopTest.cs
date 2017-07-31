using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading;
using LogTest;
using System.Linq;
/// <summary>
/// This unit test check The stop behavior is working as described 
/// </summary>
namespace UTest
{
    [TestClass]
    public class StopTest
    {
        [TestMethod]
        public void TestMethod2()
        {
            Array.ForEach(Directory.GetFiles(@"C:\LogTest"), File.Delete);//clean the folder 
            StopWithoutFlushTest();
            Thread.Sleep(2000);
            StopWithFlushTest();
            bool value = CheckLine();
            Thread.Sleep(2000);
            Assert.IsTrue(value);
        }
        /// <summary>
        /// Run and  Stop With Flush
        /// </summary>
        static void StopWithoutFlushTest()
        {
            ILog logger = new AsyncLog();
            for (int i = 0; i < 1000; i++)
            {
                 logger.Write("Number without Flush: " + i.ToString());
                if (i == 500)
                    logger.StopWithoutFlush();
            }
            logger.CloseStreamWriter();
            Thread.Sleep(2000);
        }
        /// <summary>
        ///  Run and  Stop Without Flush
        /// </summary>
        static void StopWithFlushTest()
        {
            ILog logger2 = new AsyncLog();
            for (int i = 0; i < 1000; i++)
            {
                logger2.Write("Number with Flush: " + i.ToString());
                if (i == 500)
                    logger2.StopWithFlush();
            }
            logger2.CloseStreamWriter();
            Thread.Sleep(2000);
        }
        /// <summary>
        /// Check the line number of both created file and compare
        /// </summary>
        /// <returns></returns>
        static bool CheckLine()
        {
            string[] s = Directory.GetFiles(@"C:\LogTest", "*.log", SearchOption.AllDirectories);
            bool ISCheck=false;
            int lineCountWithFlush = 0;
            int lineCountWithoutFlush = 0;
            var Lastrow1 = File.ReadLines(s[0]).Last();
            var Lastrow2 = File.ReadLines(s[1]).Last();
            if (Lastrow1.Contains("Number with Flush"))
            {
                lineCountWithFlush = File.ReadLines(s[0]).Count();
                lineCountWithoutFlush = File.ReadLines(s[1]).Count();
            }
            else if (Lastrow1.Contains("Number without Flush"))
            {
                lineCountWithoutFlush = File.ReadLines(s[0]).Count();
                lineCountWithFlush = File.ReadLines(s[1]).Count();
            }
            if(lineCountWithFlush>= lineCountWithoutFlush)
            {
                ISCheck = true;
            }
         
            return ISCheck;
          
        }
    }
}
