#nullable disable
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace AnimeBrowser.Data.Entities
{
    public partial class EpisodeMediaList
    {
        public long Id { get; set; }
        public long ListId { get; set; }
        public long EpisodeId { get; set; }

        public virtual Episode Episode { get; set; }
        public virtual MediaList List { get; set; }


        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        });
    }
}
