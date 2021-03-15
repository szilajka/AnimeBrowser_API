using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace AnimeBrowser.Common.Models.BaseModels.SecondaryModels
{
    public class EpisodeRatingResponseModel
    {
        public EpisodeRatingResponseModel(long id, int rating, long episodeId, string userId, string message = "")
        {
            this.Id = id;
            this.Rating = rating;
            this.EpisodeId = episodeId;
            this.UserId = userId;
            this.Message = message;
        }

        public long Id { get; set; }
        public int Rating { get; set; }
        public string Message { get; set; }
        public long EpisodeId { get; set; }
        public string UserId { get; set; }

        [ExcludeFromCodeCoverage]
        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        });
    }
}
