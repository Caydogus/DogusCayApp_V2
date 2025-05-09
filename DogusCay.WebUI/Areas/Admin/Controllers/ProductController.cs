using DogusCay.WebUI.DTOs.CategoryDtos;
using DogusCay.WebUI.DTOs.ProductDtos;
using DogusCay.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DogusCay.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]/[action]/{id?}")]
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
            var values = await _client.GetFromJsonAsync<List<ResultProductDto>>("Products/GetProductsWithCategoryDetails");
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            var categoryList = await _client.GetFromJsonAsync<List<ResultCategoryDto>>("Categories");

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
                SubCategories = new List<SelectListItem>(), // Başlangıçta boş
                CreateProductDto = new CreateProductDto()
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Kategorileri yeniden yükle
                var categoryList = await _client.GetFromJsonAsync<List<ResultCategoryDto>>("Categories");

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

            await _client.PostAsJsonAsync("Products", model.CreateProductDto);
            return RedirectToAction("Index");
        }


        #region
        //[HttpGet]
        //public async Task<IActionResult> UpdateProduct(int id)
        //{
        //    var product = await _client.GetFromJsonAsync<UpdateProductDto>("Products/" + id);
        //    var categoryList = await _client.GetFromJsonAsync<List<ResultCategoryDto>>("Categories");

        //    var selectedCategory = categoryList.FirstOrDefault(c => c.CategoryId == product.CategoryId);
        //    var selectedMainCategoryId = selectedCategory?.ParentCategoryId;

        //    var mainCategories = categoryList
        //        .Where(x => x.ParentCategoryId == null)
        //        .Select(x => new SelectListItem
        //        {
        //            Text = x.CategoryName,
        //            Value = x.CategoryId.ToString()
        //        }).ToList();

        //    var subCategories = categoryList
        //        .Where(x => x.ParentCategoryId == selectedMainCategoryId)
        //        .Select(x => new SelectListItem
        //        {
        //            Text = x.CategoryName,
        //            Value = x.CategoryId.ToString()
        //        }).ToList();

        //    ViewBag.MainCategories = new SelectList(mainCategories, "Value", "Text", selectedMainCategoryId);
        //    ViewBag.SubCategories = new SelectList(subCategories, "Value", "Text", product.CategoryId);

        //    ViewBag.SelectedMainCategoryId = selectedMainCategoryId;

        //    return View(product);
        //}
        //[HttpPost]
        //public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProductDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        // Hatalıysa dropdown'ları yeniden doldur
        //        var categoryList = await _client.GetFromJsonAsync<List<ResultCategoryDto>>("Categories");

        //        var selectedCategory = categoryList.FirstOrDefault(c => c.CategoryId == updateProductDto.CategoryId);
        //        var selectedMainCategoryId = selectedCategory?.ParentCategoryId;

        //        var mainCategories = categoryList
        //            .Where(x => x.ParentCategoryId == null)
        //            .Select(x => new SelectListItem
        //            {
        //                Text = x.CategoryName,
        //                Value = x.CategoryId.ToString()
        //            }).ToList();

        //        var subCategories = categoryList
        //            .Where(x => x.ParentCategoryId == selectedMainCategoryId)
        //            .Select(x => new SelectListItem
        //            {
        //                Text = x.CategoryName,
        //                Value = x.CategoryId.ToString()
        //            }).ToList();

        //        ViewBag.MainCategories = mainCategories;
        //        ViewBag.SubCategories = subCategories;
        //        ViewBag.SelectedMainCategoryId = selectedMainCategoryId;

        //        return View(updateProductDto);
        //    }

        //    await _client.PutAsJsonAsync("Products", updateProductDto);
        //    return RedirectToAction("Index");
        //}
        //public async Task<IActionResult> DeleteProduct(int id)
        //{
        //    await _client.DeleteAsync("Products/" + id);
        //    return RedirectToAction("Index");
        //}
        #endregion

        public async Task<IActionResult> ShowOnHome(int id)
        {
            await _client.GetAsync("Products/ShowOnHome/" + id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DontShowOnHome(int id)
        {
            await _client.GetAsync("Products/DontShowOnHome/" + id);
            return RedirectToAction("Index");
        }

        // AJAX: Alt kategorileri getir
        public async Task<IActionResult> GetSubCategories(int id)
        {
            var categoryList = await _client.GetFromJsonAsync<List<ResultCategoryDto>>("Categories");

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
