using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace AnimeBrowser.Common.Models.ErrorModels
{
    public class ErrorModel
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }

        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
    }
}
