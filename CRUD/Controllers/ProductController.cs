using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CRUD.Models;
using Microsoft.EntityFrameworkCore;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        int pageSize = 10;  // Adjust the page size as needed
        var productsQuery = _context.Products
            .Select(p => new ProductViewModel
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                CategoryName = p.Category.CategoryName // Assuming Product has a Category navigation property
            });

        var paginatedProducts = await PaginatedList<ProductViewModel>.CreateAsync(productsQuery, page, pageSize);

        return View(paginatedProducts);
    }

    public IActionResult Create()
    {
        var categories = _context.Categories
            .Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.CategoryName
            })
            .ToList();

        var viewModel = new ProductViewModel
        {
            Categories = categories
        };

        return View(viewModel);
    }

    public IActionResult Edit(int id)
    {
        var product = _context.Products
            .Include(p => p.Category)
            .FirstOrDefault(p => p.ProductId == id);

        if (product == null)
        {
            return NotFound();
        }

        var viewModel = new ProductViewModel
        {
            ProductId = product.ProductId,
            ProductName = product.ProductName,
            CategoryId = product.CategoryId,
            Categories = _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                })
                .ToList()
        };

        return View(viewModel);
    }


    [HttpPost]
    public IActionResult Create(ProductViewModel model)
    {
        if (ModelState.IsValid)
        {
            var product = new Product
            {
                ProductName = model.ProductName,
                CategoryId = model.CategoryId 
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Product saved successfully!";
            return RedirectToAction("Index", "Product");
        }
        model.Categories = _context.Categories
        .Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.CategoryName })
        .ToList();
        return View(model);
    }


    [HttpPost]
    public IActionResult Edit(ProductViewModel model)
    {
        if (ModelState.IsValid)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == model.ProductId);
            if (product != null)
            {
                product.ProductName = model.ProductName;
                product.CategoryId = model.CategoryId;

                _context.SaveChanges();
                TempData["SuccessMessage"] = "Product updated successfully!";
                return RedirectToAction("Index");
            }
        }

        model.Categories = _context.Categories
            .Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.CategoryName })
            .ToList();

        return View(model);
    }


    public IActionResult Delete(int id)
    {
        var product = _context.Products.Find(id);
        if (product == null)
        {
            return NotFound();
        }

        _context.Products.Remove(product);
        _context.SaveChanges();
        TempData["SuccessMessage"] = "Product deleted successfully!";
        return RedirectToAction("Index", "Product");
    }

}
