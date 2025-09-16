using DogusCay.WebUI.DTOs.CategoryDtos;
using DogusCay.WebUI.DTOs.ProductDtos;
using DogusCay.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DogusCay.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/[controller]/[action]/{id?}")]
    public class ProductController : Controller
    {
        private readonly HttpClient _client;

        public ProductController(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("EduClient");
        }

        // 09.05.2025 - Ana ve alt kategori bilgilerini içeren ürün listesi eklendi
        public async Task<IActionResult> Index()
        {
            var values = await _client.GetFromJsonAsync<List<ResultProductDto>>("products/getproductswithcategorydetails");
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            var categoryList = await _client.GetFromJsonAsync<List<ResultCategoryDto>>("categories");

            var mainCategories = categoryList
                .Where(x => x.ParentCategoryId == null)
                .Select(x => new SelectListItem
                {
                    Text = x.CategoryName,
                    Value = x.CategoryId.ToString()
                }).ToList();

            var model = new ProductCreateViewModel
            {
                MainCategories = mainCategories,
                SubCategories = new List<SelectListItem>(),
                CreateProductDto = new CreateProductDto()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var categoryList = await _client.GetFromJsonAsync<List<ResultCategoryDto>>("categories");

                model.MainCategories = categoryList
                    .Where(x => x.ParentCategoryId == null)
                    .Select(x => new SelectListItem
                    {
                        Text = x.CategoryName,
                        Value = x.CategoryId.ToString()
                    }).ToList();

                model.SubCategories = categoryList
                    .Where(x => x.ParentCategoryId == model.SelectedMainCategoryId)
                    .Select(x => new SelectListItem
                    {
                        Text = x.CategoryName,
                        Value = x.CategoryId.ToString()
                    }).ToList();

                return View(model);
            }

            await _client.PostAsJsonAsync("products", model.CreateProductDto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ShowOnHome(int id)
        {
            await _client.GetAsync("products/showonhome/" + id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DontShowOnHome(int id)
        {
            await _client.GetAsync("products/dontshowonhome/" + id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetSubCategories(int id)
        {
            var categoryList = await _client.GetFromJsonAsync<List<ResultCategoryDto>>("categories");

            var subCategories = categoryList
                .Where(x => x.ParentCategoryId == id)
                .Select(x => new SelectListItem
                {
                    Text = x.CategoryName,
                    Value = x.CategoryId.ToString()
                }).ToList();

            return Json(subCategories);
        }
    }
}
