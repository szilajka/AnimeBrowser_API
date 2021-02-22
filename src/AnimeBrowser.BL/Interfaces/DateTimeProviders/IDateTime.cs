using System;

namespace AnimeBrowser.BL.Interfaces.DateTimeProviders
{
    public interface IDateTime
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }

        DateTime FromYearUtc(int year);
        DateTime FromDateUtc(DateTime dateTime);
        DateTime FromDateUtc(int year, int month, int day);
    }
}
