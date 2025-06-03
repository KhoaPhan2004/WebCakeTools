using Microsoft.AspNetCore.Mvc;

namespace WebCakeTools.Controllers
{
	public class ProductController : Controller
	{
		public IActionResult GetProductsByCategory(int categoryId)
		{
			if (categoryId == 2004)
			{
				return ViewComponent("ProductListByCategory", new { index = 0 });
			}
			if (categoryId == 2003)
			{
				return ViewComponent("ProductListByCategory", new { index = 1 });
			}
			if (categoryId == 2002)
			{
				return ViewComponent("ProductListByCategory", new { index = 2 });
			}
			else
			{
				return ViewComponent("ProductListByChill", new { categoryId = categoryId });
			}
		}
		
		
	}
}
