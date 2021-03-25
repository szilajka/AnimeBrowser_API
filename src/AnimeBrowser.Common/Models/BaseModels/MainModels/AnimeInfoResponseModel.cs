using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace AnimeBrowser.Common.Models.BaseModels.MainModels
{
    public class AnimeInfoResponseModel
    {
        public AnimeInfoResponseModel(long id, string title = "", string description = "", bool isNsfw = false, bool isActive = true)
        {
            this.Id = id;
            this.Title = title;
            this.Description = description;
            this.IsNsfw = isNsfw;
            this.IsActive = isActive;
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsNsfw { get; set; }
        public bool IsActive { get; set; }


        [ExcludeFromCodeCoverage]
        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
    }
}
