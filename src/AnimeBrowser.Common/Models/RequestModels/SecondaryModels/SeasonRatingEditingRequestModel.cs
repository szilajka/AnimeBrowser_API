using AnimeBrowser.Common.Models.BaseModels.SecondaryModels;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace AnimeBrowser.Common.Models.RequestModels.SecondaryModels
{
    public class SeasonRatingEditingRequestModel : SeasonRatingRequestModel
    {
        public SeasonRatingEditingRequestModel(long id, int rating, long seasonId, string userId, string message = "")
             : base(rating: rating, seasonId: seasonId, userId: userId, message: message)
        {
            this.Id = id;
        }

        public long Id { get; set; }


        [ExcludeFromCodeCoverage]
        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
    }
}
