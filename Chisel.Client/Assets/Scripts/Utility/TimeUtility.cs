using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chisel
{
    public static class TimeUtility
    {
        public static long AddSeconds(long input, long seconds)
        {
            return input + (seconds * 10000000);
        }

        internal static long AddSeconds(long startTime, object taskTime)
        {
            throw new NotImplementedException();
        }
    }
}
