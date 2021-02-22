using System;

namespace AnimeBrowser.BL.Interfaces.DateTimeProviders
{
    public interface IDateTime
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }

        DateTime FromYear(int year);
    }
}
