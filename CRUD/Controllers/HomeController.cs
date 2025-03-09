using Microsoft.AspNetCore.Mvc;
using CRUD.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
    {
        var query = _context.Products.Include(p => p.Category).OrderBy(p => p.ProductId);

        var paginatedList = await PaginatedList<Product>.CreateAsync(query, page, pageSize);

        return View(paginatedList);
    }

}
