using AutoMapper;
using DogusCay.Business.Abstract;
using DogusCay.DTO.DTOs.CategoryDtos;
using DogusCay.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DogusCay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = _categoryService.TGetList();
            var dto = _mapper.Map<List<ResultCategoryDto>>(categories);
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = _categoryService.TGetById(id);
            var dto = _mapper.Map<GetByIdCategoryDto>(category);
            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _categoryService.TDelete(id);
            return Ok("Kategori silindi.");
        }

        [HttpPost]
        public IActionResult Create(CreateCategoryDto createDto)
        {
            var category = _mapper.Map<Category>(createDto);
            _categoryService.TCreate(category);
            return Ok("Kategori oluşturuldu.");
        }

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

        [AllowAnonymous]
        [HttpGet("GetActiveCategories")]
        public IActionResult GetActiveCategories()
        {
            var activeCategories = _categoryService.TGetFilteredList(x => x.IsShown);
            var dto = _mapper.Map<List<ResultCategoryDto>>(activeCategories);
            return Ok(dto);
        }

        [HttpGet("GetTree")]
        public IActionResult GetCategoryTree()
        {
            var categories = _categoryService.TGetList(); // SubCategories ve Products yüklü olmalı
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
        [HttpGet("{id}/children")]
        public IActionResult GetSubCategories(int id)
        {
            var subCategories = _categoryService.TGetFilteredList(c => c.ParentCategoryId == id);
            var dto = _mapper.Map<List<ResultCategoryDto>>(subCategories);
            return Ok(dto);
        }
    }
}
