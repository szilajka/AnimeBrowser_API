using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

#nullable disable

namespace AnimeBrowser.Data.Entities
{
    public partial class AnimeInfo
    {
        public AnimeInfo()
        {
            AnimeInfoNames = new HashSet<AnimeInfoName>();
            Episodes = new HashSet<Episode>();
            Seasons = new HashSet<Season>();
        }

        public long Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(255)]
        public string Title { get; set; }

        [MaxLength(30000)]
        public string Description { get; set; }

        [Required]
        public bool IsNsfw { get; set; }

        public virtual ICollection<AnimeInfoName> AnimeInfoNames { get; set; }
        public virtual ICollection<Episode> Episodes { get; set; }
        public virtual ICollection<Season> Seasons { get; set; }

        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        });
    }
}
