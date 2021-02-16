using AnimeBrowser.Data.Entities.Identity;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;

#nullable disable

namespace AnimeBrowser.Data.Entities
{
    public partial class SeasonRating
    {
        public long Id { get; set; }
        public int Rating { get; set; }
        public string Message { get; set; }
        public long SeasonId { get; set; }
        public string UserId { get; set; }

        public virtual Season Season { get; set; }
        public virtual User User { get; set; }

        [ExcludeFromCodeCoverage]
        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
    }
}
