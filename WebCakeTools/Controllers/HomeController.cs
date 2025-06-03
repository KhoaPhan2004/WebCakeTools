using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebCakeTools.Models;
using X.PagedList;
using X.PagedList.Extensions;

namespace WebCakeTools.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CaketoolsContext _caketoolsContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, CaketoolsContext caketoolsContext, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _caketoolsContext = caketoolsContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(int? categoryId)
        {
			var FirstId = _caketoolsContext.Categories.FirstOrDefault()?.CategoryId;
			var SecondId = _caketoolsContext.Categories.Skip(1).FirstOrDefault()?.CategoryId;
			var thirdId = _caketoolsContext.Categories.Skip(2).FirstOrDefault()?.CategoryId;

			ViewBag.FirstId = FirstId;
			ViewBag.SecondId = SecondId;
			ViewBag.thirdId = thirdId;



			var FirstName = _caketoolsContext.Categories.FirstOrDefault()?.CategoryName;
			var SecondName = _caketoolsContext.Categories.Skip(1).FirstOrDefault()?.CategoryName;
			var thirdName = _caketoolsContext.Categories.Skip(2).FirstOrDefault()?.CategoryName;

			ViewBag.FirstName = FirstName;
			ViewBag.SecondName = SecondName;
			ViewBag.thirdName = thirdName;
			ViewBag.SelectedCategoryId = categoryId ?? 0;
            var top3 = _caketoolsContext.Products
		.OrderByDescending(p => p.Contact)
		.Take(3)
		.ToList();

			return View(top3);
		}

		public IActionResult ProductList(string searchTerm, int? page)
		{
			var query = _caketoolsContext.Products.AsQueryable();

			if (!string.IsNullOrEmpty(searchTerm))
			{
				query = query.Where(p => p.ProductName.Contains(searchTerm));
			}

			var pagedProducts = query.ToPagedList(page ?? 1, 12);

			ViewBag.SearchTerm = searchTerm; // để giữ lại giá trị tìm kiếm

			return View(pagedProducts);
		}

		public IActionResult About()
        {
            return View();
        }
		public IActionResult News()
		{
			return View();
		}
		public IActionResult Contact()
		{
			return View();
		}
        public IActionResult Service()
        {
            return View();
        }

		public IActionResult AddProductContact(int id)
		{
			var product = _caketoolsContext.Products.FirstOrDefault(p => p.ProductId == id);
			if (product != null)
			{
				product.Contact += 1;
		
				_caketoolsContext.SaveChanges();
			}

			return RedirectToAction("Contact"); 
		}
		public IActionResult ProductByCategory(int id, int? childId = null, int page = 1)
		{
			int pageSize = 12;
			List<int> categoryIds;

			var selectedCategory = _caketoolsContext.Categories.FirstOrDefault(c => c.CategoryId == id);
			var childCategories = _caketoolsContext.Categories.Where(c => c.ParentId == id).ToList();

			ViewBag.CategoryName = selectedCategory?.CategoryName ?? "Danh mục";
			ViewBag.ChildCategories = childCategories;
			ViewBag.CategoryId = id;
			ViewBag.SelectedChildId = childId;

			if (childId.HasValue)
			{
				// Chỉ lấy sản phẩm theo danh mục con được chọn
				categoryIds = new List<int> { childId.Value };
			}
			else
			{
				// Lấy sản phẩm của danh mục cha + tất cả danh mục con
				categoryIds = new List<int> { id };
				categoryIds.AddRange(childCategories.Select(c => c.CategoryId));
			}

			// Lấy tất cả productIds liên quan
			var productIds = _caketoolsContext.ProductCategories
				.Where(pc => categoryIds.Contains(pc.CategoryId))
				.Select(pc => pc.ProductId)
				.Distinct()
				.ToList();

			var totalProducts = _caketoolsContext.Products
				.Where(p => productIds.Contains(p.ProductId))
				.Count();

			int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

			var products = _caketoolsContext.Products
				.Where(p => productIds.Contains(p.ProductId))
				.OrderByDescending(p => p.CreatedAt)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToList();

			ViewBag.CurrentPage = page;
			ViewBag.TotalPages = totalPages;

			return View(products);
		}






		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
