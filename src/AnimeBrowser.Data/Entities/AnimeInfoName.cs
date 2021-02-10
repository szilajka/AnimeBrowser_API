#nullable disable

using System.ComponentModel.DataAnnotations;

namespace AnimeBrowser.Data.Entities
{
    public partial class AnimeInfoName
    {
        public long Id { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(255)]
        public string Title { get; set; }
        public long AnimeInfoId { get; set; }

        public virtual AnimeInfo AnimeInfo { get; set; }
    }
}
