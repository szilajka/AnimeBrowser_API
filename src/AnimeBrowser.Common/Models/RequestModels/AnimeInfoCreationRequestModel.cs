using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace AnimeBrowser.Common.Models.RequestModels
{
    public class AnimeInfoCreationRequestModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsNsfw { get; set; }

        [ExcludeFromCodeCoverage]
        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        });
    }
}
