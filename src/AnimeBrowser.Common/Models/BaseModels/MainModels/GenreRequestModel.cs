using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace AnimeBrowser.Common.Models.BaseModels.MainModels
{
    public class GenreRequestModel
    {
        public GenreRequestModel(string genreName = "", string description = "")
        {
            this.GenreName = genreName;
            this.Description = description;
        }

        public string GenreName { get; set; }
        public string Description { get; set; }

        [ExcludeFromCodeCoverage]
        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        });
    }
}
