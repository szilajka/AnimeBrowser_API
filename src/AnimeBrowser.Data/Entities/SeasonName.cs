using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

#nullable disable

namespace AnimeBrowser.Data.Entities
{
    public partial class SeasonName
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long SeasonId { get; set; }

        public virtual Season Season { get; set; }

        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        });
    }
}
