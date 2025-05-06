using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace DogusCay.Entity.Entities
{
    public class PointGroup
    {
        public int PointGroupId { get; set; }

        [Required, MaxLength(100)]
        public string GroupName { get; set; } // YEREL ZİNCİR, TOPTAN, ULUSAL ZİNCİR...

        public int KanalId { get; set; }           // Foreign key
        public Kanal Kanal { get; set; }

        public ICollection<Point> Points { get; set; }
    }

}
