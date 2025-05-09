using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;

namespace DogusCay.WebUI.DTOs.CategoryDtos
{
    public class UpdateCategoryDto
    {
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; }
        public int? ParentCategoryId { get; set; }
        public bool IsShown { get; set; }
    }
}
