#nullable disable

using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace AnimeBrowser.Data.Entities
{
    public partial class SeasonMediaList
    {
        public long Id { get; set; }
        public long ListId { get; set; }
        public long SeasonId { get; set; }

        public virtual MediaList List { get; set; }
        public virtual Season Season { get; set; }

        [ExcludeFromCodeCoverage]
        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
    }
}
