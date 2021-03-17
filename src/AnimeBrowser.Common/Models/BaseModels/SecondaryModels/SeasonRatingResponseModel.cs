using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace AnimeBrowser.Common.Models.BaseModels.SecondaryModels
{
    public class SeasonRatingResponseModel
    {
        public SeasonRatingResponseModel(long id, int rating, long seasonId, string userId, string message = "")
        {
            this.Id = id;
            this.Rating = rating;
            this.Message = message;
            this.SeasonId = seasonId;
            this.UserId = userId;
        }

        public long Id { get; set; }
        public int Rating { get; set; }
        public string Message { get; set; }
        public long SeasonId { get; set; }
        public string UserId { get; set; }


        [ExcludeFromCodeCoverage]
        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
    }
}
