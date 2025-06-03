using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCakeTools.Models;

namespace WebCakeTools.ViewComponents
{
    public class ProductListByChillViewComponent : ViewComponent
    {
        private readonly CaketoolsContext _caketoolsContext;


        public ProductListByChillViewComponent(CaketoolsContext caketoolsContext)
        {

            _caketoolsContext = caketoolsContext;

        }
        public IViewComponentResult Invoke(int categoryId)
        {
            // Lấy danh sách sản phẩm theo CategoryID
            var productIds = _caketoolsContext.ProductCategories
                .Where(pc => pc.CategoryId == categoryId)
                .Select(pc => pc.ProductId)
            .ToList();

            var products = _caketoolsContext.Products
                .Where(p => productIds.Contains(p.ProductId))
                .Take(8)
                .ToList();

            return View(products); // View nằm ở Views/Shared/Components/ProductByCategory/Default.cshtml
        }
    }
}
