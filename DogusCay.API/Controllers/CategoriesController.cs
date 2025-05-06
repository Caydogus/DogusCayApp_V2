using AutoMapper;
using DogusCay.Business.Abstract;
using DogusCay.DTO.DTOs.CategoryDtos;
using DogusCay.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DogusCay.API.Controllers
{// API/Controllers/CategoryController.cs
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryService _categoryService, IMapper _mapper) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get()
        {
            var values = _categoryService.TGetList();
            var courseCategories = _mapper.Map<List<ResultCategoryDto>>(values);
            return Ok(courseCategories);
        }

        [HttpGet("{id}")]

        public IActionResult GetById(int id)
        {
            var value = _categoryService.TGetById(id);
            return Ok(value);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _categoryService.TDelete(id);
            return Ok("Kategori Alanı Silindi");
        }

        [HttpPost]
        public IActionResult Create(CreateCategoryDto createCategoryDto)
        {
            var newValue = _mapper.Map<Category>(createCategoryDto);
            _categoryService.TCreate(newValue);
            return Ok(" Kategori Oluşturuldu");
        }

        [HttpPut]
        public IActionResult Update(UpdateCategoryDto updateCategoryDto)
        {
            var value = _mapper.Map<Category>(updateCategoryDto);
            _categoryService.TUpdate(value);
            return Ok("Kategori Güncellendi");
        }

        [HttpGet("ShowOnHome/{id}")]
        public IActionResult ShowOnHome(int id)
        {
            _categoryService.TShowOnHome(id);
            return Ok("Ana Sayfada Gösteriliyor");
        }

        [HttpGet("DontShowOnHome/{id}")]
        public IActionResult DontShowOnHome(int id)
        {
            _categoryService.TDontShowOnHome(id);
            return Ok("Ana Sayfada Gösterilmiyor");
        }
        [AllowAnonymous]
        [HttpGet("GetActiveCategories")]
        public IActionResult GetActiveCategories()
        {
            var values = _categoryService.TGetFilteredList(x => x.IsShown == true);
            return Ok(values);
        }
        [AllowAnonymous]
        [HttpGet("GetCategoryCount")]
        public IActionResult GetCategoryCount()
        {
            var courseCount = _categoryService.TCount();
            return Ok(courseCount);
        }


    }

}
