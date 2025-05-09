using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;
using DogusCay.WebUI.DTOs.ProductDtos;

namespace DogusCay.WebUI.DTOs.CategoryDtos
{
    public class ResultCategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? ParentCategoryId { get; set; }
        public bool IsShown { get; set; }
        public ICollection<ResultProductDto> Products { get; set; } = new List<ResultProductDto>();
        public ICollection<ResultCategoryDto> SubCategories { get; set; } = new List<ResultCategoryDto>();
    }
}
