using System.ComponentModel;

namespace AnimeBrowser.Common.Models.ErrorModels
{
    public enum ErrorCodes : int
    {
        [Description("Empty Object")]
        EmptyObject = 1,
        [Description("Empty Property")]
        EmptyProperty = 2,
        [Description("Too Long Property Value")]
        TooLongProperty = 11,
        [Description("Property Is Out Of Valid Range")]
        OutOfRangeProperty = 21,
        [Description("Mismatching Property Values")]
        MismatchingProperty = 31,
        [Description("Not Unique Property Value")]
        NotUniqueProperty = 41
    }
}
