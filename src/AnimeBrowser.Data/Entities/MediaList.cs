using AnimeBrowser.Common.Attributes;
using AnimeBrowser.Data.Entities.Identity;
using System.Collections.Generic;

#nullable disable

namespace AnimeBrowser.Data.Entities
{
    [ToJsonString]
    public partial class MediaList
    {
        public MediaList()
        {
            EpisodeMediaLists = new HashSet<EpisodeMediaList>();
            SeasonMediaLists = new HashSet<SeasonMediaList>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public int ListType { get; set; }
        public bool IsPublic { get; set; }
        public string UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<EpisodeMediaList> EpisodeMediaLists { get; set; }
        public virtual ICollection<SeasonMediaList> SeasonMediaLists { get; set; }
    }
}
