using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace AnimeBrowser.Common.Models.BaseModels.SecondaryModels
{
    public class AnimeInfoNameResponseModel
    {
        public AnimeInfoNameResponseModel(long id, long animeInfoId, string title = "")
        {
            this.Id = id;
            this.Title = title;
            this.AnimeInfoId = animeInfoId;
        }

        public long Id { get; set; }
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
