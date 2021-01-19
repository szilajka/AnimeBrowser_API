﻿using System.Collections.Generic;

#nullable disable

namespace AnimeBrowser.Data.Entities
{
    public partial class Genre
    {
        public Genre()
        {
            AnimeGenres = new HashSet<AnimeGenre>();
        }

        public long Id { get; set; }
        public string GenreName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<AnimeGenre> AnimeGenres { get; set; }
    }
}
