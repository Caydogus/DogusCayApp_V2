using DogusCay.WebUI.DTOs.ProductDtos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DogusCay.WebUI.Models
{
    public class ProductCreateViewModel
    {
        public CreateProductDto CreateProductDto { get; set; }

        public List<SelectListItem> MainCategories { get; set; } = new();
        public List<SelectListItem> SubCategories { get; set; } = new();

        public int? SelectedMainCategoryId { get; set; }
    }

}
