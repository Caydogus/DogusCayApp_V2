using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;

namespace DogusCay.DTO.DTOs.CategoryDtos
{
    public class GetByIdCategoryDto
    {
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; }
        public int? ParentCategoryId { get; set; }
        public bool IsShown { get; set; }



    }
}
