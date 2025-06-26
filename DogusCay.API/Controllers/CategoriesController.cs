using AutoMapper;
using DogusCay.Business.Abstract;
using DogusCay.DTO.DTOs.CategoryDtos;
using DogusCay.DTO.DTOs.ProductDtos;
using DogusCay.DTOs.ProductDtos;
using DogusCay.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DogusCay.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryService categoryService, IProductService productService, IMapper mapper)
        {
            _categoryService = categoryService;
            _productService = productService;
            _mapper = mapper;
        }

        // Tüm kategorileri getirir
        
        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = _categoryService.TGetList();
            var dto = _mapper.Map<List<ResultCategoryDto>>(categories);
            return Ok(dto);
        }

        // ID ile kategori getirir
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = _categoryService.TGetById(id);
            var dto = _mapper.Map<GetByIdCategoryDto>(category);
            return Ok(dto);
        }

        // Kategori siler
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _categoryService.TDelete(id);
            return Ok("Kategori silindi.");
        }

        // Yeni kategori oluşturur
        [HttpPost]
        public IActionResult Create(CreateCategoryDto createDto)
        {
            var category = _mapper.Map<Category>(createDto);
            _categoryService.TCreate(category);
            return Ok("Kategori oluşturuldu.");
        }

        // Kategori günceller
        [HttpPut]
        public IActionResult Update(UpdateCategoryDto updateDto)
        {
            var category = _mapper.Map<Category>(updateDto);
            _categoryService.TUpdate(category);
            return Ok("Kategori güncellendi.");
        }

        [HttpGet("ShowOnHome/{id}")]
        public IActionResult ShowOnHome(int id)
        {
            _categoryService.TShowOnHome(id);
            return Ok("Ana sayfada gösteriliyor.");
        }

        [HttpGet("DontShowOnHome/{id}")]
        public IActionResult DontShowOnHome(int id)
        {
            _categoryService.TDontShowOnHome(id);
            return Ok("Ana sayfada gizlendi.");
        }

        // IsShown == true olan kategorileri getirir
      
        [HttpGet("GetActiveCategories")]
        public IActionResult GetActiveCategories()
        {
            var activeCategories = _categoryService.TGetFilteredList(x => x.IsShown);
            var dto = _mapper.Map<List<ResultCategoryDto>>(activeCategories);
            return Ok(dto);
        }

        // Tüm kategorileri ağaç yapısıyla döner
        [HttpGet("GetTree")]
        public IActionResult GetCategoryTree()
        {
            var categories = _categoryService.TGetList();
            var dtoList = _mapper.Map<List<ResultCategoryDto>>(categories);
            var tree = BuildTree(dtoList);
            return Ok(tree);
        }

        private List<ResultCategoryDto> BuildTree(List<ResultCategoryDto> flatList)
        {
            var lookup = flatList.ToLookup(c => c.ParentCategoryId);

            foreach (var category in flatList)
            {
                category.SubCategories = lookup[category.CategoryId].ToList();
            }

            return flatList.Where(c => c.ParentCategoryId == null).ToList();
        }

        // Alt kategorileri getirir (ParentId = id)
        [HttpGet("{id}/children")]
        public IActionResult GetSubCategories(int id)
        {
            //Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:5055");
            var subCategories = _categoryService.TGetFilteredList(c => c.ParentCategoryId == id);
            var dto = _mapper.Map<List<ResultCategoryDto>>(subCategories);
            return Ok(dto);
        }

        // ANA Kategorileri getirir (ParentCategoryId == null)
        [HttpGet("MainCategories")]
        public IActionResult GetMainCategories()
        {
            //Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:5055");
            var mainCategories = _categoryService.TGetFilteredList(c => c.ParentCategoryId == null);
            var dto = _mapper.Map<List<ResultCategoryDto>>(mainCategories);
            return Ok(dto);
        }

        // Alt kategoriye ait ürünleri getirir
        [HttpGet("{subCategoryId}/products")]
        public IActionResult GetProductsBySubCategory(int subCategoryId)
        {
            //Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:5055");
            var products = _productService.TGetFilteredList(p => p.CategoryId == subCategoryId);
            var dto = _mapper.Map<List<ResultProductDto>>(products);
            return Ok(dto);
        }

        [HttpGet("{categoryId}/products-recursive")]
        public IActionResult GetProductsWithSubCategories(int categoryId)
        {
            var allCategories = _categoryService.TGetList(); // Tüm kategorileri al
            var allIds = new List<int> { categoryId };

            // Alt kategorileri recursive olarak bul
            CollectChildCategories(categoryId, allCategories, allIds);

            var products = _productService.TGetFilteredList(p => allIds.Contains(p.CategoryId));
            var dto = _mapper.Map<List<ResultProductDto>>(products);
            return Ok(dto);
        }

        private void CollectChildCategories(int parentId, List<Category> all, List<int> result)
        {
            var children = all.Where(x => x.ParentCategoryId == parentId).ToList();
            foreach (var child in children)
            {
                result.Add(child.CategoryId);
                CollectChildCategories(child.CategoryId, all, result); // recursive çağrı
            }
        }

    }
}
