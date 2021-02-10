﻿#nullable disable


namespace AnimeBrowser.Data.Entities
{
    public partial class SeasonGenre
    {
        public long Id { get; set; }
        public long GenreId { get; set; }
        public long SeasonId { get; set; }

        public virtual Genre Genre { get; set; }
        public virtual Season Season { get; set; }
    }
}
