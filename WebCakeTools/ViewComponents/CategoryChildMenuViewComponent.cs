using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCakeTools.Controllers;
using WebCakeTools.Models;
namespace WebCakeTools.ViewComponents
{
    public class CategoryChildMenuViewComponent : ViewComponent
    {
        private readonly CaketoolsContext _caketoolsContext;


        public CategoryChildMenuViewComponent(CaketoolsContext caketoolsContext)
        {

            _caketoolsContext = caketoolsContext;

        }
        public IViewComponentResult Invoke(int index = 0) // mặc định là lấy phần tử đầu tiên
        {
            var parentCategories = _caketoolsContext.Categories
                .Where(c => c.ParentId == null)
                .OrderBy(c => c.CategoryId) // nên có thứ tự rõ ràng
                .ToList();

            if (index >= parentCategories.Count)
            {
                return View(new List<Category>());
            }

            var parentCategory = parentCategories[index];

            var childCategories = _caketoolsContext.Categories
                .Where(c => c.ParentId == parentCategory.CategoryId)
                .ToList();

            return View(childCategories);
        }
        
    }
}
