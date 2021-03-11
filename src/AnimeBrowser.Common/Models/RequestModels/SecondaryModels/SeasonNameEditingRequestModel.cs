using AnimeBrowser.Common.Models.BaseModels.SecondaryModels;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace AnimeBrowser.Common.Models.RequestModels.SecondaryModels
{
    public class SeasonNameEditingRequestModel : SeasonNameRequestModel
    {
        public SeasonNameEditingRequestModel(long id, string title, long seasonId)
            : base(title: title, seasonId: seasonId)
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
