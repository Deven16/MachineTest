using Microsoft.AspNetCore.Mvc;
using CRUD.Models;

public class CategoryController : Controller
{
    private readonly ApplicationDbContext _context;

    public CategoryController(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index(int? page)
    {
        int pageSize = 10;
        var categories = _context.Categories.AsQueryable();

        var paginatedCategories = await PaginatedList<Category>.CreateAsync(categories, page ?? 1, pageSize);

        return View(paginatedCategories);
    }
    public IActionResult Create()
    {
        return View();
    }
    public IActionResult Edit(int id)
    {
        var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);

        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }


    [HttpPost]
    public IActionResult Create(Category category)
    {
        if (ModelState.IsValid)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Category saved successfully!"; 
            return RedirectToAction("Index", "Category"); 
        }
        return View(category);
    }

    [HttpPost]
    public IActionResult Edit(Category model)
    {
        if (ModelState.IsValid)
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == model.CategoryId);
            if (category != null)
            {
                category.CategoryName = model.CategoryName;

                _context.SaveChanges();
                TempData["SuccessMessage"] = "Category updated successfully!";
                return RedirectToAction("Index");
            }
        }

        return View(model);
    }


    public IActionResult Delete(int id)
    {
        var category = _context.Categories.Find(id);
        if (category == null)
        {
            return NotFound();
        }
        _context.Categories.Remove(category);
        _context.SaveChanges();
        TempData["SuccessMessage"] = "Category deleted successfully!";
        return RedirectToAction("Index", "Category");
    }
}
