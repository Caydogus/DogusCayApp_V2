using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;

namespace DogusCay.DTO.DTOs.CategoryDtos
{
    public class ResultCategoryDto
    {
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; }
        public int? ParentCategoryId { get; set; }
        public bool IsShown { get; set; }


        //public ICollection<ResultCategoryDto> SubCategories { get; set; }

        //public ICollection<ProductDto> Products { get; set; }
    }
}
