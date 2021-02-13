#nullable disable

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace AnimeBrowser.Data.Entities
{
    public partial class SeasonMediaList
    {
        public long Id { get; set; }
        public long ListId { get; set; }
        public long SeasonId { get; set; }

        public virtual MediaList List { get; set; }
        public virtual Season Season { get; set; }

        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
    }
}
