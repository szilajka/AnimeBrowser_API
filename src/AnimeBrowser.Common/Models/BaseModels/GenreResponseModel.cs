using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace AnimeBrowser.Common.Models.BaseModels
{
    public class GenreResponseModel
    {
        public GenreResponseModel(long id, string genreName = "", string description = "")
        {
            this.Id = id;
            this.GenreName = genreName;
            this.Description = description;
            this.Description = description;
        }

        public long Id { get; set; }
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
