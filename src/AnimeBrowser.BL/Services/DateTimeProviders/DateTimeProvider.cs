using AnimeBrowser.BL.Interfaces.DateTimeProviders;
using System;

namespace AnimeBrowser.BL.Services.DateTimeProviders
{
    public class DateTimeProvider : IDateTime
    {
        public DateTime Now { get => DateTime.Now; }
        public DateTime UtcNow { get => DateTime.UtcNow; }

        public DateTime FromYear(int year)
        {
            return new DateTime(1900, 1, 1);
        }
    }
}
