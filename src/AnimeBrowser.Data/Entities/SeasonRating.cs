using AnimeBrowser.Data.Entities.Identity;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

#nullable disable

namespace AnimeBrowser.Data.Entities
{
    public partial class SeasonRating : Identifiable<long>
    {
        //public long Id { get; set; }
        [Attr]
        public int Rating { get; set; }
        [Attr]
        public string Message { get; set; }
        public long SeasonId { get; set; }
        public string UserId { get; set; }

        [HasOne]
        public virtual Season Season { get; set; }
        [HasOne]
        public virtual User User { get; set; }
    }
}
