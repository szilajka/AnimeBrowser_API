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
        TooLongProperty = 11
    }
}
