#nullable disable

using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace AnimeBrowser.Data.Entities
{
    public partial class SeasonName : Identifiable<long>
    {
        //public long Id { get; set; }
        [Attr]
        public string Title { get; set; }
        public long SeasonId { get; set; }

        [HasOne]
        public virtual Season Season { get; set; }
    }
}
