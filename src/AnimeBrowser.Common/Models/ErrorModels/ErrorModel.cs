using AnimeBrowser.Common.Attributes;

namespace AnimeBrowser.Common.Models.ErrorModels
{
    [ToJsonString]
    public partial class ErrorModel
    {
        public ErrorModel(string code, string title, string description, string source)
        {
            this.Code = code;
            this.Title = title;
            this.Description = description;
            this.Source = source;
        }

        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
    }
}
