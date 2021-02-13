using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace AnimeBrowser.Common.Models.ResponseModels
{
    public class AnimeInfoCreationResponseModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsNsfw { get; set; }


        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
    }
}
