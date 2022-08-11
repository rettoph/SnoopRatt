using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SnoopRatt.App.Database.ValueConverters
{
    internal class TimeZoneInfoValueConverter : ValueConverter<TimeZoneInfo, string>
    {
        public TimeZoneInfoValueConverter() : base(v => ToStringFromTimeZone(v), v => ToTimeZoneFromString(v))
        {
        }

        private static string ToStringFromTimeZone(TimeZoneInfo arg)
        {
            return arg.Id;
        }

        private static TimeZoneInfo ToTimeZoneFromString(string arg)
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(arg);
            }
            catch
            {
                return TimeZoneInfo.Local;
            }
        }
    }
}
