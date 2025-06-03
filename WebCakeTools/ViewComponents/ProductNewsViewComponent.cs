using Microsoft.AspNetCore.Mvc;
using WebCakeTools.Models;

namespace WebCakeTools.ViewComponents
{
	public class ProductNewsViewComponent : ViewComponent
	{
		private readonly CaketoolsContext _caketoolsContext;


		public ProductNewsViewComponent(CaketoolsContext caketoolsContext)
		{

			_caketoolsContext = caketoolsContext;

		}
		public IViewComponentResult Invoke()
		{
			var newestProducts = _caketoolsContext.Products
				.OrderByDescending(p => p.CreatedAt)
				.Take(10)
				.ToList();

			return View(newestProducts);
		}
		
	}
}
