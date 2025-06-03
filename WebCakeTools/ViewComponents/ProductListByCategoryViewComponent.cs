using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCakeTools.Controllers;
using WebCakeTools.Models;
namespace WebCakeTools.ViewComponents
{
    public class ProductListByCategoryViewComponent : ViewComponent
    {
        private readonly CaketoolsContext _caketoolsContext;


        public ProductListByCategoryViewComponent(CaketoolsContext caketoolsContext)
        {

            _caketoolsContext = caketoolsContext;

        }
        public IViewComponentResult Invoke(int index = 0)
        {
            // Lấy danh sách danh mục cha (ParentID == null) sắp xếp theo CategoryID
            var parentCategories = _caketoolsContext.Categories
                .Where(c => c.ParentId == null)
                .OrderBy(c => c.CategoryId)
                .ToList();

            if (index < 0 || index >= parentCategories.Count)
            {
                // Nếu index không hợp lệ thì trả về rỗng
                return View(new List<Product>());
            }

            // Lấy danh mục cha theo vị trí index
            var selectedParentCategory = parentCategories[index];

            // Lấy 8 sản phẩm thuộc danh mục cha đã chọn
            var products = _caketoolsContext.ProductCategories
                .Where(pc => pc.CategoryId == selectedParentCategory.CategoryId)
                .Select(pc => pc.Product)
                .OrderByDescending(p => p.CreatedAt)
                .Take(8)
                .ToList();

            return View(products);
        }


    }
}
