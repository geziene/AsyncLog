using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
/// <summary>
/// This Class is use to Change Datetime of the system
/// It will Add one day to Current Day
/// Added By Navideh To use in UnitTest
/// When a method declaration includes an extern modifier,
/// that method is said to be an external method. External methods are implemented externally,
/// typically using a language other than C#.
/// Because an external method declaration provides no actual implementation,
/// the method-body of an external method simply consists of a semicolon.
/// An external method may not be generic. The extern modifier is typically used in conjunction with
/// a DllImport attribute, allowing external methods to be implemented by DLLs (Dynamic Link Libraries).
/// The execution environment may support other mechanisms whereby implementations of external methods
/// can be provided. When an external method includes a DllImport attribute, 
/// the method declaration must also include a static modifier.
/// </summary>
namespace LogTest
{
    public static class ChangeCurrentDate
    {
        public struct SystemTime
        {
            public ushort Year;
            public ushort Month;
            public ushort DayOfWeek;
            public ushort Day;
            public ushort Hour;
            public ushort Minute;
            public ushort Second;
            public ushort Millisecond;
        };
        [DllImport("kernel32.dll", EntryPoint = "GetSystemTime", SetLastError = true)]
        public extern static void Win32GetSystemTime(ref SystemTime sysTime);

        [DllImport("kernel32.dll", EntryPoint = "SetSystemTime", SetLastError = true)]
        public extern static bool Win32SetSystemTime(ref SystemTime sysTime);
        public static void ChangeDatetoTommorrow()
        {
            SystemTime updatedTime = new SystemTime();
            DateTime Nowd = DateTime.Now;
            Nowd = Nowd.AddDays(1);
            updatedTime.Year = (ushort)Nowd.Year;
            updatedTime.Month = (ushort)Nowd.Month;
            updatedTime.Day = (ushort)Nowd.Day;
            updatedTime.Hour = (ushort)Nowd.Hour;
            updatedTime.Minute = (ushort)Nowd.Minute;
            updatedTime.Second = (ushort)Nowd.Second;
            Win32SetSystemTime(ref updatedTime);
        }

    }
}

