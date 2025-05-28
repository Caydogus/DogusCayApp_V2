using AutoMapper;
using DogusCay.Business.Abstract;
using DogusCay.DTO.DTOs.ProductDtos;
using DogusCay.DTOs.ProductDtos;
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
        // ✅ TÜM ÜRÜNLER
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get()
        {
            var values = _productService.TGetAllProductsWithCategories();
            var products = _mapper.Map<List<ResultProductDto>>(values);
            return Ok(products);
        }

        // ✅ ID'ye göre tek ürün
        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var value = _productService.TGetById(id);
            return Ok(value);
        }

        // ✅ SİLME
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _productService.TDelete(id);
            return Ok("Ürün silindi");
        }

        // ✅ EKLEME
        [HttpPost]
        public IActionResult Create(CreateProductDto createProductDto)
        {
            var newValue = _mapper.Map<Product>(createProductDto);
            _productService.TCreate(newValue);
            return Ok("Ürün oluşturuldu");
        }

        // ✅ GÜNCELLEME
        [HttpPut]
        public IActionResult Update(UpdateProductDto updateProductDto)
        {
            var value = _mapper.Map<Product>(updateProductDto);
            _productService.TUpdate(value);
            return Ok("Ürün güncellendi");
        }

        // ✅ ANASAYFADA GÖSTER
        [HttpGet("ShowOnHome/{id}")]
        public IActionResult ShowOnHome(int id)
        {
            _productService.TShowOnHome(id);
            return Ok("Ürün ana sayfada gösteriliyor");
        }

        // ✅ ANASAYFADAN KALDIR
        [HttpGet("DontShowOnHome/{id}")]
        public IActionResult DontShowOnHome(int id)
        {
            _productService.TDontShowOnHome(id);
            return Ok("Ürün ana sayfadan kaldırıldı");
        }

        // ✅ SADECE AKTİF ÜRÜNLER
        [AllowAnonymous]
        [HttpGet("GetActiveProducts")]
        public IActionResult GetActiveProducts()
        {
            var values = _productService.TGetFilteredList(x => x.IsShown == true);
            return Ok(values);
        }

        // ✅ ÜRÜN SAYISI
        [AllowAnonymous]
        [HttpGet("GetProductCount")]
        public IActionResult GetProductCount()
        {
            var productCount = _productService.TCount();
            return Ok(productCount);
        }

        // ✅ BELİRLİ KATEGORİYE GÖRE ÜRÜNLER
        [AllowAnonymous]
        [HttpGet("GetProductsByCategoryId/{id}")]
        public IActionResult GetProductsByCategoryId(int id)
        {
            var values = _productService.TGetAllProductsWithCategories(x => x.CategoryId == id);
            return Ok(values);
        }

        // ✅ TÜM ÜRÜNLER + KATEGORİ DETAYLARI
        [AllowAnonymous]
        [HttpGet("GetProductsWithCategoryDetails")]
        public IActionResult GetProductsWithCategoryDetails()
        {
            var values = _productService.TGetAllProductsWithCategoryDetails();
            return Ok(values);
        }

        
        // ✨ YENİ EKLENENLER (KATEGORİ ZİNCİRİNE UYUMLU)

        // ✅ ALT KATEGORİYE GÖRE ÜRÜNLERİ GETİR
        [AllowAnonymous]
        [HttpGet("by-subcategory/{subCategoryId}")]
        public IActionResult GetProductsBySubCategory(int subCategoryId)
        {
            var products = _productService.TGetProductsBySubCategoryId(subCategoryId);
            var result = _mapper.Map<List<ResultProductDto>>(products);
            return Ok(result);
        }

        // ✅ ÜRÜN DETAYI (fiyat, kategori vs.)
        [AllowAnonymous]
        [HttpGet("details/{productId}")]
        public IActionResult GetProductDetails(int productId)
        {
            var product = _productService.TGetProductWithCategory(productId);
            if (product == null)
                return NotFound("Ürün bulunamadı");

            var result = _mapper.Map<ResultProductDto>(product);
            return Ok(result);
        }
    }


}
