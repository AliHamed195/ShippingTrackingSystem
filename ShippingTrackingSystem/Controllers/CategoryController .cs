using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShippingTrackingSystem.BackEnd.Interfaces;
using ShippingTrackingSystem.BackEnd.Repository;
using ShippingTrackingSystem.Models;

namespace ShippingTrackingSystem.Controllers
{
    [Route("Category")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: Categories/All
        [HttpGet("All")]
        public async Task<IActionResult> AllCategories()
        {
            var (success, errorMessage, categories) = await _categoryRepository.GetAllCategoriesAsync();
            if (success)
            {
                return View(categories);
            }

            ModelState.AddModelError("", errorMessage: errorMessage);
            return View(Enumerable.Empty<Category>());
        }

        // GET: Category/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                var (success, errorMessage, createdCategory) = await _categoryRepository.CreateCategoryAsync(category: category);
                if (success)
                {
                    return RedirectToAction(nameof(AllCategories));
                }
                ModelState.AddModelError("", errorMessage: errorMessage);
            }
            return View(category);
        }

        // GET: Category/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var (success, errorMessage, category) = await _categoryRepository.GetCategoryByIdAsync(categoryId: id.Value);
            if (success)
            {
                return View(category);
            }

            ModelState.AddModelError("", errorMessage: errorMessage);
            return NotFound();
        }

        // POST: Category/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var (success, errorMessage) = await _categoryRepository.UpdateCategoryAsync(category: category);
                if (success)
                {
                    return RedirectToAction(nameof(AllCategories));
                }

                ModelState.AddModelError("", errorMessage: errorMessage);
            }
            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var (success, errorMessage) = await _categoryRepository.DeleteCategoryAsync(id);
                if (success)
                {
                    return Json(new { success = true, message = "Category successfully deleted." });
                }
                else
                {
                    return Json(new { success = false, message = errorMessage });
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occurred while deleting the Category." });
            }
        }

        // GET: Category/Products/5
        [HttpGet("Products/{id}")]
        public async Task<IActionResult> CategoryProducts(int id)
        {
            var (success, errorMessage, products) = await _categoryRepository.GetProductsByCategoryIdAsync(categoryId: id);
            if (success)
            {
                var(categorySuccess, categoryErrorMessage, category) = await _categoryRepository.GetCategoryByIdAsync(categoryId: id);
                ViewBag.CategoryName = category.Name;
                return View(products);
            }

            ModelState.AddModelError("", errorMessage);
            return View(Enumerable.Empty<Product>());
        }

    }
}
