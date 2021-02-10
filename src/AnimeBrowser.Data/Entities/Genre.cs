using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
    }
}
