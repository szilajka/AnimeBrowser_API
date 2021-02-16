using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace AnimeBrowser.Common.Models.BaseModels
{
    public class AnimeInfoResponseModel
    {
        public AnimeInfoResponseModel(long id, string title = "", string description = "", bool isNsfw = false)
        {
            this.Id = id;
            this.Title = title;
            this.Description = description;
            this.IsNsfw = isNsfw;
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsNsfw { get; set; }


        [ExcludeFromCodeCoverage]
        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
    }
}
