using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;

#nullable disable

namespace AnimeBrowser.Data.Entities
{
    public partial class SeasonGenre
    {
        public long Id { get; set; }
        public long GenreId { get; set; }
        public long SeasonId { get; set; }

        public virtual Genre Genre { get; set; }
        public virtual Season Season { get; set; }


        [ExcludeFromCodeCoverage]
        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
    }
}
