using System;
using System.Collections.Generic;
using System.Text;

namespace EarlyCare.Infrastructure
{
    public static class Utilities
    {
        public static DateTime GetCurrentTime()
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
        }
    }
}
