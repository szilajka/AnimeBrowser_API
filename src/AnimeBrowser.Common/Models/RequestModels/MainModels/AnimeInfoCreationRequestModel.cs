using AnimeBrowser.Common.Models.BaseModels.MainModels;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace AnimeBrowser.Common.Models.RequestModels.MainModels
{
    public class AnimeInfoCreationRequestModel : AnimeInfoRequestModel
    {
        public AnimeInfoCreationRequestModel(string title = "", string description = "", bool isNsfw = false, bool isActive = true)
            : base(title: title, description: description, isNsfw: isNsfw)
        {
            this.IsActive = isActive;
        }

        public bool IsActive { get; set; }


        [ExcludeFromCodeCoverage]
        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        });
    }
}
