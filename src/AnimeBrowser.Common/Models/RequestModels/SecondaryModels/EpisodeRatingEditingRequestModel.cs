using AnimeBrowser.Common.Models.BaseModels.SecondaryModels;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace AnimeBrowser.Common.Models.RequestModels.SecondaryModels
{
    public class EpisodeRatingEditingRequestModel : EpisodeRatingRequestModel
    {
        public EpisodeRatingEditingRequestModel(long id, int rating, long episodeId, string userId, string message = "")
            : base(rating: rating, episodeId: episodeId, userId: userId, message: message)
        {
            this.Id = id;
        }

        public long Id { get; set; }


        [ExcludeFromCodeCoverage]
        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        });
    }
}
