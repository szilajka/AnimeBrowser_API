#nullable disable
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace AnimeBrowser.Data.Entities
{
    public partial class AnimeInfoName
    {
        public long Id { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(255)]
        public string Title { get; set; }
        public long AnimeInfoId { get; set; }

        public virtual AnimeInfo AnimeInfo { get; set; }


        [ExcludeFromCodeCoverage]
        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
    }
}
