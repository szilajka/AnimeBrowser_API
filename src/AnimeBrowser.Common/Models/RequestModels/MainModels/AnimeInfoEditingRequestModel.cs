using AnimeBrowser.Common.Models.BaseModels.MainModels;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace AnimeBrowser.Common.Models.RequestModels.MainModels
{
    public class AnimeInfoEditingRequestModel : AnimeInfoRequestModel
    {
        public AnimeInfoEditingRequestModel(long id, string title = "", string description = "", bool isNsfw = false)
            : base(title: title, description: description, isNsfw: isNsfw)
        {
            Id = id;
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
