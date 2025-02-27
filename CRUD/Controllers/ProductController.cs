using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUD.Data;
using CRUD.Models;
using System.Linq;
using System.Threading.Tasks;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;
    private const int PageSize = 10; // Number of records per page

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int? page)
    {
        int currentPage = page ?? 1; // Default to page 1 if null

        var totalRecords = await _context.Products.CountAsync();

        var products = await _context.Products
            .Include(p => p.Category)
            .OrderBy(p => p.ProductId)
            .Skip((currentPage - 1) * PageSize)  // Skip previous pages' records
            .Take(PageSize)  // Take only the required number of records
            .ToListAsync();

        var model = new PaginatedList<Product>(products, totalRecords, currentPage, PageSize);
        return View(model);
    }
}
