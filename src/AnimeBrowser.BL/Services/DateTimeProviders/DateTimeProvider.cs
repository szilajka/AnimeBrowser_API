using AnimeBrowser.BL.Interfaces.DateTimeProviders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace AnimeBrowser.BL.Services.DateTimeProviders
{
    [ExcludeFromCodeCoverage]
    public class DateTimeProvider : IDateTime
    {
        public DateTime Now { get => DateTime.Now; }
        public DateTime UtcNow { get => DateTime.UtcNow; }

        public DateTime FromYearUtc(int year)
        {
            return new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        }

        public DateTime FromDateUtc(DateTime dateTime)
        {
            var dt = dateTime.ToUniversalTime();
            return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, DateTimeKind.Utc);
        }

        public DateTime FromDateUtc(int year, int month, int day)
        {
            return new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
        }
    }
}
