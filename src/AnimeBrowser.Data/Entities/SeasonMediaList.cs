﻿using AnimeBrowser.Common.Attributes;

#nullable disable

namespace AnimeBrowser.Data.Entities
{
    [ToJsonString]
    public partial class SeasonMediaList
    {
        public long Id { get; set; }
        public long ListId { get; set; }
        public long SeasonId { get; set; }

        public virtual MediaList List { get; set; }
        public virtual Season Season { get; set; }
    }
}
