using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace CRUD.Models
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null;
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    }
}
