#nullable disable
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.ComponentModel.DataAnnotations;

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


        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
    }
}
