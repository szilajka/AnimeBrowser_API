#nullable disable

using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;
using System.ComponentModel.DataAnnotations;

namespace AnimeBrowser.Data.Entities
{
    public partial class AnimeInfoName : Identifiable<long>
    {
        //public long Id { get; set; }
        [Attr]
        [Required]
        [MinLength(1)]
        [MaxLength(255)]
        public string Title { get; set; }
        public long AnimeInfoId { get; set; }

        [HasOne]
        public virtual AnimeInfo AnimeInfo { get; set; }
    }
}
