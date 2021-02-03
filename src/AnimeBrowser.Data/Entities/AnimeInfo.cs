using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AnimeBrowser.Data.Entities
{
    public partial class AnimeInfo : Identifiable<long>
    {
        public AnimeInfo()
        {
            AnimeInfoNames = new HashSet<AnimeInfoName>();
            Episodes = new HashSet<Episode>();
            Seasons = new HashSet<Season>();
        }

        //public long Id { get; set; }
        
        [Attr]
        [Required]
        [MinLength(1)]
        [MaxLength(255)]
        public string Title { get; set; }

        [Attr]
        [MaxLength(30000)]
        public string Description { get; set; }

        [Attr]
        [Required]
        public bool IsNsfw { get; set; }

        [HasMany]
        public virtual ICollection<AnimeInfoName> AnimeInfoNames { get; set; }
        [HasMany]
        public virtual ICollection<Episode> Episodes { get; set; }
        [HasMany]
        public virtual ICollection<Season> Seasons { get; set; }
    }
}
