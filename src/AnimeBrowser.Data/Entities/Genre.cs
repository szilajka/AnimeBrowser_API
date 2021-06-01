using AnimeBrowser.Common.Attributes;
using System.Collections.Generic;

#nullable disable

namespace AnimeBrowser.Data.Entities
{
    [ToJsonString]
    public partial class Genre
    {
        public Genre()
        {
            SeasonGenres = new HashSet<SeasonGenre>();
        }

        public long Id { get; set; }
        public string GenreName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<SeasonGenre> SeasonGenres { get; set; }
    }
}
