using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace AnimeBrowser.Common.Models.BaseModels.SecondaryModels
{
    public class SeasonNameResponseModel
    {
        public SeasonNameResponseModel(long id, string title, long seasonId)
        {
            this.Id = id;
            this.Title = title;
            this.SeasonId = seasonId;
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public long SeasonId { get; set; }

        [ExcludeFromCodeCoverage]
        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        });
    }
}
