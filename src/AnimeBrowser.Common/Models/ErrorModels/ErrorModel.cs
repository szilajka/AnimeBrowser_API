using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace AnimeBrowser.Common.Models.ErrorModels
{
    public class ErrorModel
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

        [ExcludeFromCodeCoverage]
        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
    }
}
