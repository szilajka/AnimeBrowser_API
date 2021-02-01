using AnimeBrowser.Data.Entities.Identity;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;
using System.Collections.Generic;

#nullable disable

namespace AnimeBrowser.Data.Entities
{
    public partial class MediaList : Identifiable<long>
    {
        public MediaList()
        {
            EpisodeMediaLists = new HashSet<EpisodeMediaList>();
            SeasonMediaLists = new HashSet<SeasonMediaList>();
        }

        //public long Id { get; set; }
        [Attr]
        public string Name { get; set; }
        [Attr]
        public int ListType { get; set; }
        [Attr]
        public bool IsPublic { get; set; }
        public string UserId { get; set; }

        [HasOne]
        public virtual User User { get; set; }
        [HasMany]
        public virtual ICollection<EpisodeMediaList> EpisodeMediaLists { get; set; }
        [HasMany]
        public virtual ICollection<SeasonMediaList> SeasonMediaLists { get; set; }
    }
}
