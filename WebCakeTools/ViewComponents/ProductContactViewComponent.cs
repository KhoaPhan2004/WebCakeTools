using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCakeTools.Models;

namespace WebCakeTools.ViewComponents
{
	public class ProductContactViewComponent : ViewComponent
	{
		private readonly CaketoolsContext _caketoolsContext;


		public ProductContactViewComponent(CaketoolsContext caketoolsContext)
		{

			_caketoolsContext = caketoolsContext;

		}
		public IViewComponentResult Invoke()
		{
			var topProducts = _caketoolsContext.Products
				.OrderByDescending(p => p.Contact)
				.Take(10)
				.ToList();

			return View(topProducts);
		}

	}
}
