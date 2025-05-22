using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace DogusCay.Entity.Entities
{
    public class PointGroupType
    {
        public int PointGroupTypeId { get; set; }
        public string PointGroupTypeName { get; set; } // "YEREL ZİNCİR", "TOPTAN"

        public ICollection<Point> Points { get; set; }
    }


}
