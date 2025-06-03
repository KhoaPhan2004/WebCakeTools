using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCakeTools.Controllers;
using WebCakeTools.Models;
namespace WebCakeTools.Components
{
	public class CategoryMenuViewComponent : ViewComponent
	{
		
		private readonly CaketoolsContext _caketoolsContext;
		

		public CategoryMenuViewComponent(CaketoolsContext caketoolsContext)
		{
			
			_caketoolsContext = caketoolsContext;
			
		}
		public IViewComponentResult Invoke()
		{
            var categories = _caketoolsContext.Categories.Where(c => c.ParentId == null).ToList();
            return View(categories);
		}

       
        
    }
}
