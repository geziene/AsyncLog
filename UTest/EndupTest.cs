using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Text;
using LogTest;
using System.Linq;
/// <summary>
/// This unit test check A call to Ilog will end up in writing "Finish"
/// </summary>
namespace UTest
{
    [TestClass]
    public class EndupTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Array.ForEach(Directory.GetFiles(@"C:\LogTest"), File.Delete);//clean the folder 
            DoWork();
            Boolean value = ReadFiles();
            Thread.Sleep(2000);
            Assert.IsTrue(value);
        }
        /// <summary>
        /// If the works are done completely add "finish" to the last line
        /// </summary>
        static void DoWork()
        {
            ILog logger = new AsyncLog();
            for (int i = 0; i < 15; i++)
            {
                logger.Write("Number with Flush: " + i.ToString());
                Thread.Sleep(50);
            }
            logger.Write("Finish");
            
            ILog logger2 = new AsyncLog();

            for (int i = 100; i > 0; i--)
            {
                logger2.Write("Number with No flush: " + i.ToString());
                Thread.Sleep(20);
            }
            logger2.Write("Finish");
            logger.CloseStreamWriter();
            logger2.CloseStreamWriter();

            Thread.Sleep(5000);
        }
        /// <summary>
        /// Check if the last line contain "Finish"
        /// </summary>
        /// <returns></returns>
        bool ReadFiles()
        {
            DirectoryInfo d = new DirectoryInfo(@"C:\LogTest");
            Boolean IsEnd = false;
            foreach (var file in d.GetFiles("*.log"))
            {
                var Lastrow = File.ReadLines(file.FullName).Last();
                if (!Lastrow.Contains("Finish"))
                {
                    IsEnd = false;
                    break;
                }
                else if (Lastrow.Contains("Finish"))
                    IsEnd = true;
            }
            return IsEnd;
        }

    }
}
