using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;

#nullable disable

namespace AnimeBrowser.Data.Entities
{
    public partial class Genre
    {
        public Genre()
        {
            SeasonGenres = new HashSet<SeasonGenre>();
        }

        public long Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string GenreName { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(10000)]
        public string Description { get; set; }

        public virtual ICollection<SeasonGenre> SeasonGenres { get; set; }


        [ExcludeFromCodeCoverage]
        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
    }
}
