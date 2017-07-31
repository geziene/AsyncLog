using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
/// <summary>
/// Main class do Async Log Writing
/// </summary>
namespace LogTest
{
    public class AsyncLog :  ILog
    {
        #region  Fields
        Queue<LogLine> queue = new Queue<LogLine>();//use Queue insted of list
        ManualResetEvent hasNewItems = new ManualResetEvent(false);
        ManualResetEvent terminate = new ManualResetEvent(false);
        ManualResetEvent waiting = new ManualResetEvent(false);
        private Thread _runThread;
        private StreamWriter _writer;
        private bool _exit;
        private bool _QuitWithFlush = false;
        DateTime _curDate = DateTime.Now;
        #endregion

        #region Constructors
        public AsyncLog()
        {
            CreateLogfile();
            _runThread = new Thread(new ThreadStart(MainLoop));
            _runThread.IsBackground = true;
            _runThread.Start();//invoke some method
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Create the file on C:\LogTest and write th first line 
        /// </summary>
        void CreateLogfile()
        {
            if (!Directory.Exists(@"C:\LogTest"))
                Directory.CreateDirectory(@"C:\LogTest");
            this._writer = File.AppendText(@"C:\LogTest\Log" + DateTime.Now.ToString("yyyyMMdd HHmmss fff") + ".log");
            this._writer.Write("Timestamp".PadRight(25, ' ') + "\t" + "Data".PadRight(15, ' ') + "\t" + Environment.NewLine);
            this._writer.AutoFlush = true;
        }
        /// <summary>
        /// Doing the Main task which will run on the thread
        /// </summary>
        void MainLoop()
        {
            try
            {
                while (!this._exit)
                {
                    //terminate was signaled and prevent from doing when queue is empty 
                    waiting.Set();//run all thread
                  //  //This method is used for sending the signal to all waiting threads. Set() Method set the ManualResetEvent object boolean variable to true. All the waiting threads are unblocked and proceed further.
              //    int i = ManualResetEvent.WaitAny(new WaitHandle[] { hasNewItems, terminate });
                    //A WaitHandle array containing the objects for which the current instance will wait.
                  //  if (i == 1) return;
                      hasNewItems.Reset();//block
                    waiting.Reset();//block  Reset method change the boolean value to false.
                    //We must immediately call Reset method after calling Set method
                    //if we want to send signal to threads multiple times. 
                    Queue<LogLine> queueCopy = new Queue<LogLine>();
                    //A lock statement begins with the keyword lock, 
                    //which is given an object as an argument, and followed by a code block
                    //that is to be executed by only one thread at a time.
                    lock (queue)
                    {
                        queueCopy = new Queue<LogLine>(queue);
                        queue.Clear();
                    }
                    foreach (var log in queueCopy)
                    {
                        if (!this._exit || this._QuitWithFlush)
                        {
                            WriteOnFile(log);
                        }
                    }
                    if (this._QuitWithFlush && queue.Count == 0)
                    {
                        this._exit = true;
                        break;
                    }

                }
            }
            catch
            {

            }

        }
        /// <summary>
        /// Write a Log Line on the File and Check if it is Midnighht then create new Log file
        /// </summary>
        /// <param name="log"></param>
        void WriteOnFile(LogLine log)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                if ((DateTime.Now - _curDate).Days != 0)  //midnight check
                {
                    _curDate = DateTime.Now;
                    CreateLogfile();
                }
                stringBuilder.Append(log.Timestamp.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                stringBuilder.Append("\t");
                stringBuilder.Append(log.LineText());
                stringBuilder.Append("\t");
                stringBuilder.Append(Environment.NewLine);
                this._writer.Write(stringBuilder.ToString());
            }
            catch
            {

            }
        }
        #endregion

        #region Public Methods

        public void Write(string text)
        {
            lock (queue)
            {
                queue.Enqueue(new LogLine() { Text = text, Timestamp = DateTime.Now });
            }
            hasNewItems.Set();
            //notify thread when new log add to queue
        }
        public void Dispose()
        {
            terminate.Set();
            _runThread.Join();
        }
        public void StopWithoutFlush()
        {
            this._exit = true;
            this._writer.AutoFlush = false;//doesnt write the buffer
            //Well, the call to Flush does exactly what it says it will.
            //Streams will buffer information for the sake of performance; 
            //more often than not, it's easier to get larger packets of bytes and then write
            //that to the underlying resource (disk, network) than it is to perform a write to the underlying resource every time.
           // Calling Flush will ensure that whatever is buffered is written to the underlying resource.

        }
        public void StopWithFlush()
        {
            this._QuitWithFlush = true;
        }
        public void CloseStreamWriter()
        {
            this._writer.Close();

        }
        #endregion

    }
}