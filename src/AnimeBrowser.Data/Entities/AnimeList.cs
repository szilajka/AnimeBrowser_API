using System.Collections.Generic;

#nullable disable

namespace AnimeBrowser.Data.Entities
{
    public partial class AnimeList
    {
        public AnimeList()
        {
            EpisodeUserLists = new HashSet<EpisodeUserList>();
            SeasonUserLists = new HashSet<SeasonUserList>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public int ListType { get; set; }
        public bool IsPublic { get; set; }
        public string UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<EpisodeUserList> EpisodeUserLists { get; set; }
        public virtual ICollection<SeasonUserList> SeasonUserLists { get; set; }
    }
}
