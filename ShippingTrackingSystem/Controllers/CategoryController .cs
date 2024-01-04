using Microsoft.AspNetCore.Mvc;
using ShippingTrackingSystem.BackEnd.Interfaces;
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

        // GET: Categories
        [HttpGet("All")]
        public async Task<IActionResult> AllCategories()
        {
            var (success, errorMessage, categories) = await _categoryRepository.GetAllCategoriesAsync();
            if (success)
            {
                return View(categories);
            }
            // need to do swal ... // Error
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
                var (success, errorMessage, createdCategory) = await _categoryRepository.CreateCategoryAsync(category);
                if (success)
                {
                    return RedirectToAction(nameof(AllCategories));
                }
                ModelState.AddModelError("", errorMessage);
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

            var (success, errorMessage, category) = await _categoryRepository.GetCategoryByIdAsync(id.Value);
            if (success)
            {
                return View(category);
            }

            // need to do swal ... // Error
            return NotFound();
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var (success, errorMessage) = await _categoryRepository.UpdateCategoryAsync(category);
                if (success)
                {
                    return RedirectToAction(nameof(AllCategories));
                }

                // need to do swal ... // Error
            }
            return View(category);
        }

        // GET: Category/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var (success, errorMessage, category) = await _categoryRepository.GetCategoryByIdAsync(id.Value);
            if (success)
            {
                return View(category);
            }

            // need to do swal ... // Error
            return NotFound();
        }

        // POST: Category/Delete/5
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, errorMessage) = await _categoryRepository.DeleteCategoryAsync(id);
            if (success)
            {
                return RedirectToAction(nameof(AllCategories));
            }

            // need to do swal ... // Error
            return View();
        }
    }
}
