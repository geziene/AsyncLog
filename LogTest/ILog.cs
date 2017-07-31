using System.Linq;
using System.Threading.Tasks;

namespace LogTest
 {
    /// <summary>
    /// Interface
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Stop the logging. If any outstadning logs theses will not be written to Log
        /// </summary>
        void StopWithoutFlush();
        /// <summary>
        /// Stop the logging. The call will not return until all all logs have been written to Log.
        /// </summary>
        void StopWithFlush();
        /// <summary>
        /// Add input string into a Queue of Loglines
        /// </summary>
        /// <param name="text"></param>
        void Write(string text);
        /// <summary>
         /// Close the streamWriter after writing
         /// Added By Navideh
         /// </summary>
        void CloseStreamWriter();

    }
}
