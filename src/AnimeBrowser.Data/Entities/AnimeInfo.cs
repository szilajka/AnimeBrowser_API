using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;

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
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsNsfw { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<AnimeInfoName> AnimeInfoNames { get; set; }
        public virtual ICollection<Episode> Episodes { get; set; }
        public virtual ICollection<Season> Seasons { get; set; }


        [ExcludeFromCodeCoverage]
        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
    }
}
