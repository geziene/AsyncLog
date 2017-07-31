using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using LogTest;
using System.IO;
using System.Linq;
using System.Collections.Generic;
/// <summary>
/// This test check if New files are created if midnight is crossed
/// </summary>
namespace UTest
{
    [TestClass]
    public class MidnightTest
    {
        [TestMethod]
        public void TestMethod3()
        {
            Array.ForEach(Directory.GetFiles(@"C:\LogTest"), File.Delete);//clean the folder 
            DoWork();
            Boolean value = CheckFiles();
            Assert.IsTrue(value);
        }
        /// <summary>
        /// Run the task and change the date dynamicly 
        /// </summary>
        void DoWork()
        {
            ILog logger2 = new AsyncLog();
            for (int i = 1000; i > 0; i--)
            {
                logger2.Write("Number with No flush: " + i.ToString());
                if (i == 500)
                    ChangeCurrentDate.ChangeDatetoTommorrow();
                Thread.Sleep(20);
            }
            logger2.CloseStreamWriter();
          
            Thread.Sleep(5000);
        }
        /// <summary>
        /// Check if new file is created
        /// </summary>
        /// <returns></returns>

        bool CheckFiles()
        {
            string[] s;
            s=Directory.GetFiles(@"C:\LogTest", "*.log", SearchOption.AllDirectories);
            bool IsCheck = false;
            if (s.Count()==2 && s[0]!=s[1])
            {
                IsCheck = true;
            }
            else IsCheck = false;
            return IsCheck;

        }

    }

 }

