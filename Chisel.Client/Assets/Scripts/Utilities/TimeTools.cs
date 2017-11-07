using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class TimeTools
{
    public static long AddSeconds(long input, long seconds)
    {
        return input + (seconds * 10000000);
    }
}
