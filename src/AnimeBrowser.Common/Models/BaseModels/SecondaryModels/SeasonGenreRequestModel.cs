using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace AnimeBrowser.Common.Models.BaseModels.SecondaryModels
{
    public class SeasonGenreRequestModel
    {
        public SeasonGenreRequestModel(long genreId, long seasonId)
        {
            this.GenreId = genreId;
            this.SeasonId = seasonId;
        }

        public long GenreId { get; set; }
        public long SeasonId { get; set; }

        [ExcludeFromCodeCoverage]
        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        });
    }
}
