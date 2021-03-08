using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace AnimeBrowser.Common.Models.BaseModels.SecondaryModels
{
    public class AnimeInfoNameRequestModel
    {
        public AnimeInfoNameRequestModel(long animeInfoId, string title = "")
        {
            this.Title = title;
            this.AnimeInfoId = animeInfoId;
        }

        public string Title { get; set; }
        public long AnimeInfoId { get; set; }


        [ExcludeFromCodeCoverage]
        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        });
    }
}
