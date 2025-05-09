using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DogusCay.DTOs.ProductDtos;

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

        public ICollection<ResultProductDto> Products { get; set; } = new List<ResultProductDto>();

        public ICollection<ResultCategoryDto> SubCategories { get; set; } = new List<ResultCategoryDto>();
    }
}
