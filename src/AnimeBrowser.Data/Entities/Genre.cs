using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AnimeBrowser.Data.Entities
{
    public partial class Genre : Identifiable<long>
    {
        public Genre()
        {
            SeasonGenres = new HashSet<SeasonGenre>();
        }

        //public long Id { get; set; }
        [Attr]
        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string GenreName { get; set; }

        [Attr]
        [Required]
        [MinLength(2)]
        [MaxLength(10000)]
        public string Description { get; set; }

        [HasMany]
        public virtual ICollection<SeasonGenre> SeasonGenres { get; set; }
    }
}
