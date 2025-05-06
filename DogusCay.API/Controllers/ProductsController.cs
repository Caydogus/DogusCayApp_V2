using AutoMapper;
using DogusCay.Business.Abstract;
using DogusCay.DTO.DTOs.ProductDtos;
using DogusCay.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DogusCay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductService _productService, IMapper _mapper) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get()
        {
            var values = _productService.TGetAllProductsWithCategories();
            var Products = _mapper.Map<List<ResultProductDto>>(values);
            return Ok(Products);
        }


        [AllowAnonymous]
        [HttpGet("{id}")]

        public IActionResult GetById(int id)
        {
            var value = _productService.TGetById(id);
            return Ok(value);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _productService.TDelete(id);
            return Ok("ürün Silindi");
        }

        [HttpPost]
        public IActionResult Create(CreateProductDto createProductDto)
        {
            var newValue = _mapper.Map<Product>(createProductDto);
            _productService.TCreate(newValue);
            return Ok("ürün Oluşturuldu");
        }

        [HttpPut]
        public IActionResult Update(UpdateProductDto updateProductDto)
        {
            var value = _mapper.Map<Product>(updateProductDto);
            _productService.TUpdate(value);
            return Ok("ürün Güncellendi");
        }

        [HttpGet("ShowOnHome/{id}")]
        public IActionResult ShowOnHome(int id)
        {
            _productService.TShowOnHome(id);
            return Ok("ürün Gösteriliyor");
        }

        [HttpGet("DontShowOnHome/{id}")]
        public IActionResult DontShowOnHome(int id)
        {
            _productService.TDontShowOnHome(id);
            return Ok("ürün Gösterilmiyor");
        }
        [AllowAnonymous]
        [HttpGet("GetActiveProducts")]
        public IActionResult GetActiveProducts()
        {
            var values = _productService.TGetFilteredList(x => x.IsShown == true);
            return Ok(values);
        }

   
        [AllowAnonymous]
        [HttpGet("GetProductCount")]
        public IActionResult GetProductCount()
        {
            var ProductCount = _productService.TCount();
            return Ok(ProductCount);
        }
        [AllowAnonymous]
        [HttpGet("GetProductsByCategoryId/{id}")]
        public IActionResult GetProductsByCategoryId(int id)
        {
            var values = _productService.TGetAllProductsWithCategories(x => x.CategoryId == id);
            return Ok(values);
        }
    }
}
