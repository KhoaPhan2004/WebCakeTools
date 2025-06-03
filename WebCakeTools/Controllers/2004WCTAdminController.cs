using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebCakeTools.Attributes;
using WebCakeTools.Models;
using X.PagedList;
using X.PagedList.Extensions;


namespace WebCakeTools.Controllers
{
    public class _2004WCTAdminController : Controller
    {
        private readonly ILogger<_2004WCTAdminController> _logger;
        private readonly CaketoolsContext _caketoolsContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public _2004WCTAdminController(ILogger<_2004WCTAdminController> logger, CaketoolsContext caketoolsContext, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _caketoolsContext = caketoolsContext;
            _webHostEnvironment = webHostEnvironment;
        }
        [AdminAuthorize]
        public IActionResult Index()
        {
            var totalProducts = _caketoolsContext.Products.Count();

            var topContactProducts = _caketoolsContext.Products
                .OrderByDescending(p => p.Contact)
                .Take(5)
                .ToList();

            var latestProducts = _caketoolsContext.Products
                .OrderByDescending(p => p.CreatedAt)
                .Take(5)
                .ToList();

            var model = new AdminDashboardViewModel
            {
                TotalProducts = totalProducts,
                TopContactProducts = topContactProducts,
                LatestProducts = latestProducts
            };

            return View(model);
        }


        [AdminAuthorize]
        public IActionResult ProductList(int? page)
        {
            var products = _caketoolsContext.Products
                .Include(p => p.ProductCategories) // Include liên kết
                    .ThenInclude(pc => pc.Category) // Include danh mục từ liên kết
                .ToPagedList(page ?? 1, 10);

            return View(products);
        }



        [AdminAuthorize]
        public IActionResult CreateProduct()
        {
            var parentCategories = _caketoolsContext.Categories
                .Where(c => c.ParentId == null)
                .ToList();
            ViewBag.ParentCategories = new SelectList(parentCategories, "CategoryId", "CategoryName");
            return View();
        }
        [HttpGet]
        public JsonResult GetChildCategories(int parentId)
        {
            var childCategories = _caketoolsContext.Categories
                .Where(c => c.ParentId == parentId)
                .Select(c => new { c.CategoryId, c.CategoryName })
                .ToList();

            return Json(childCategories);
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductViewModel productVM)
        {
            if (ModelState.IsValid)
            {
                var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                var fileName = Guid.NewGuid().ToString() + "_" + productVM?.ProductImage?.FileName;
                var filePath = Path.Combine(uploadFolder, fileName);
                var fileStream = new FileStream(filePath, FileMode.Create);
                productVM?.ProductImage?.CopyTo(fileStream);

                Product product = new()
                {
                    ProductName = productVM.ProductName,
                    ProductDescribe = productVM.ProductDescribe,
                    Contact = 0,
                    ProductImage = fileName
                };

                _caketoolsContext.Products.Add(product);
                _caketoolsContext.SaveChanges();

                // Redirect đến trang CreateCategory, truyền dữ liệu qua TempData
                TempData["ProductId"] = product.ProductId;
                TempData["ParentCategoryId"] = productVM.SelectedParentCategoryId;
                TempData["ChildCategoryId"] = productVM.SelectedChildCategoryId;
                return RedirectToAction("AutoCreateCategory");

            }


            return View(productVM);

        }
        [AdminAuthorize]
        public IActionResult AutoCreateCategory()
        {
            ViewBag.ProductId = TempData["ProductId"];
            ViewBag.ParentCategoryId = TempData["ParentCategoryId"];
            ViewBag.ChildCategoryId = TempData["ChildCategoryId"];
            return View();
        }

        [HttpPost]
        public IActionResult AutoCreateCategory(int productId, int? parentCategoryId, int? childCategoryId)
        {
            if (!parentCategoryId.HasValue || parentCategoryId.Value == 0)
            {
                TempData["ErrorMessage"] = "Vui lòng chọn danh mục cha.";
                return RedirectToAction("ProductList");
            }

            _caketoolsContext.ProductCategories.Add(new ProductCategory
            {
                ProductId = productId,
                CategoryId = parentCategoryId.Value
            });

            if (childCategoryId.HasValue && childCategoryId.Value != 0)
            {
                _caketoolsContext.ProductCategories.Add(new ProductCategory
                {
                    ProductId = productId,
                    CategoryId = childCategoryId.Value
                });
            }

            _caketoolsContext.SaveChanges();
            TempData["SuccessMessage"] = "Thêm sản phẩm thành công.";

            return RedirectToAction("ProductList");
        }


        [AdminAuthorize]
        public IActionResult UpdateProduct(int id)
        {
            var product = _caketoolsContext.Products.Where(t => t.ProductId == id).FirstOrDefault();
            // Lấy 2 category (cha & con) của sản phẩm từ bảng ProductCategory
            var categories = _caketoolsContext.ProductCategories
                .Where(pc => pc.ProductId == id)
                .Select(pc => pc.Category)
                .ToList();
            var childCategory = categories.FirstOrDefault(c => c.ParentId != null);
            var parentCategory = childCategory != null
                ? _caketoolsContext.Categories.FirstOrDefault(p => p.CategoryId == childCategory.ParentId)
                : categories.FirstOrDefault(c => c.ParentId == null);

            //var parentCategories = _caketoolsContext.Categories
            //    .Where(c => c.ParentId == null)
            //    .ToList();
            //ViewBag.ParentCategories = new SelectList(parentCategories, "CategoryId", "CategoryName");


            var productVM = new ProductViewModel
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductDescribe = product.ProductDescribe,
                OldImagePath = product.ProductImage,
                SelectedParentCategoryId = parentCategory?.CategoryId,
                SelectedChildCategoryId = childCategory?.CategoryId
            };
            var parentCategories = _caketoolsContext.Categories
    .Where(c => c.ParentId == null)
    .ToList();
            ViewBag.ParentCategories = new SelectList(parentCategories, "CategoryId", "CategoryName", productVM.SelectedParentCategoryId);
            var childCategories = _caketoolsContext.Categories
        .Where(c => c.ParentId == productVM.SelectedParentCategoryId)
        .ToList();
            ViewBag.ChildCategories = new SelectList(childCategories, "CategoryId", "CategoryName", productVM.SelectedChildCategoryId);
            return View(productVM);

        }
        [HttpPost]
        public IActionResult UpdateProduct(ProductViewModel productVM)
        {
            if (ModelState.IsValid)
            {
                var product = _caketoolsContext.Products.FirstOrDefault(a => a.ProductId == productVM.ProductId);

                product.ProductName = productVM.ProductName;
                product.ProductDescribe = productVM.ProductDescribe;
                product.UpdatedAt = DateTime.Now;


                if (productVM.ProductImage != null)
                {
                    var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    var fileName = Guid.NewGuid().ToString() + "_" + productVM?.ProductImage?.FileName;
                    var filePath = Path.Combine(uploadFolder, fileName);
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    productVM?.ProductImage?.CopyTo(fileStream);

                    // Cập nhật ảnh mới
                    product.ProductImage = fileName;

                }
                else
                {
                    // Không chọn ảnh mới => giữ ảnh cũ
                    product.ProductImage = productVM.OldImagePath;
                }

                _caketoolsContext.Products.Update(product);
                _caketoolsContext.SaveChanges();

                TempData["ProductId"] = product.ProductId;
                TempData["ParentCategoryId"] = productVM.SelectedParentCategoryId;
                TempData["ChildCategoryId"] = productVM.SelectedChildCategoryId;
                return RedirectToAction("AutoUpdateCategory");
            }
            return View(productVM);
        }
        public IActionResult AutoUpdateCategory()
        {
            ViewBag.ProductId = TempData["ProductId"];
            ViewBag.ParentCategoryId = TempData["ParentCategoryId"];
            ViewBag.ChildCategoryId = TempData["ChildCategoryId"];
            return View();
        }

        [HttpPost]
        public IActionResult AutoUpdateCategory(int productId, int? parentCategoryId, int? childCategoryId)
        {
            if (!parentCategoryId.HasValue || parentCategoryId.Value == 0)
            {
                TempData["ErrorMessage"] = "Vui lòng chọn danh mục cha.";
                return RedirectToAction("ProductList");
            }
            // Xoá các mối liên kết cũ (nếu có)
            var existingRelations = _caketoolsContext.ProductCategories
                .Where(pc => pc.ProductId == productId);
            _caketoolsContext.ProductCategories.RemoveRange(existingRelations);

            // Thêm danh mục cha
            _caketoolsContext.ProductCategories.Add(new ProductCategory
            {
                ProductId = productId,
                CategoryId = parentCategoryId.Value
            });

            // Thêm danh mục con nếu có
            if (childCategoryId.HasValue && childCategoryId.Value != 0)
            {
                _caketoolsContext.ProductCategories.Add(new ProductCategory
                {
                    ProductId = productId,
                    CategoryId = childCategoryId.Value
                });
            }

            _caketoolsContext.SaveChanges();
            TempData["SuccessMessage"] = "sửa sản phẩm thành công.";
            return RedirectToAction("ProductList");
        }
        [AdminAuthorize]
        public IActionResult DeleteProduct(int id)
        {
            // Lấy tất cả bản ghi trong bảng liên kết ProductCategories
            var productCategories = _caketoolsContext.ProductCategories
                .Where(pc => pc.ProductId == id)
                .ToList();

            // Xóa tất cả bản ghi liên kết với product
            _caketoolsContext.ProductCategories.RemoveRange(productCategories);

            // Xóa bản ghi product
            var product = _caketoolsContext.Products.FirstOrDefault(p => p.ProductId == id);
            if (product != null)
            {
                _caketoolsContext.Products.Remove(product);
            }

            // Lưu thay đổi
            _caketoolsContext.SaveChanges();
            TempData["SuccessMessage"] = "xóa sản phẩm thành công.";
            return RedirectToAction("ProductList");
        }


        [AdminAuthorize]
        public IActionResult CategoryList(int? parentPage, int? childPage)
        {
            int parentPageNumber = parentPage ?? 1;
            int childPageNumber = childPage ?? 1;
            // Danh mục cha (ParentID == null)
            var parentCategories = _caketoolsContext.Categories
                .Where(c => c.ParentId == null)
                .ToPagedList(parentPageNumber, 10);

            // Danh mục con (ParentID != null)
            var childCategories = _caketoolsContext.Categories
                .Where(c => c.ParentId != null)
                .Include(c => c.Parent) // nếu muốn hiển thị tên danh mục cha
                .ToPagedList(childPageNumber, 10);

            ViewBag.ParentCategories = parentCategories;
            ViewBag.ChildCategories = childCategories;

            return View();
        }

        [AdminAuthorize]
        public IActionResult CreateCategory()
        {
            var parentCategories = _caketoolsContext.Categories
                .Where(c => c.ParentId == null)
                .ToList();
            ViewBag.ParentCategories = new SelectList(parentCategories, "CategoryId", "CategoryName");
            return View();
        }
        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {

            _caketoolsContext.Categories.Add(category);
            _caketoolsContext.SaveChanges();
            TempData["SuccessMessage"] = "Thêm danh mục thành công.";
            return RedirectToAction("CategoryList");
        }
        [AdminAuthorize]
        public IActionResult UpdateCategory(int id)
        {
            var category = _caketoolsContext.Categories.Where(t => t.CategoryId == id).FirstOrDefault();
            var parentCategories = _caketoolsContext.Categories
                .Where(c => c.ParentId == null && c.CategoryId != id)
                .ToList();
            ViewBag.ParentCategories = new SelectList(parentCategories, "CategoryId", "CategoryName");
            return View(category);
           

            
        }
        [HttpPost]
        public IActionResult UpdateCategory(Category category)
        {
            _caketoolsContext.Categories.Update(category);
            _caketoolsContext.SaveChanges();
            TempData["SuccessMessage"] = "cập nhật danh mục thành công.";
            return RedirectToAction("CategoryList");
        }
        [AdminAuthorize]
        public IActionResult DeleteCategory(int id)
        {
            // Kiểm tra xem có danh mục con không
            bool hasChildCategories = _caketoolsContext.Categories.Any(c => c.ParentId == id);
            if (hasChildCategories)
            {
                TempData["ErrorMessage"] = "Không thể xóa danh mục vì còn danh mục con. Vui lòng xóa danh mục con trước.";
                return RedirectToAction("CategoryList");
            }

            // Kiểm tra xem có sản phẩm nào đang sử dụng danh mục này không
            bool hasProducts = _caketoolsContext.ProductCategories.Any(pc => pc.CategoryId == id);
            if (hasProducts)
            {
                TempData["ErrorMessage"] = "Không thể xóa danh mục vì có sản phẩm đang sử dụng danh mục này. Vui lòng xóa hoặc di chuyển sản phẩm trước.";
                return RedirectToAction("CategoryList");
            }

            // Nếu không có liên kết nào, thì xóa danh mục
            var category = _caketoolsContext.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (category != null)
            {
                _caketoolsContext.Categories.Remove(category);
                _caketoolsContext.SaveChanges();
            }

            TempData["SuccessMessage"] = "Xóa danh mục thành công.";
            return RedirectToAction("CategoryList");
        }


       
        public IActionResult Logout()
        {
            // Xoá session khi đăng xuất
            HttpContext.Session.Clear();

            // Chuyển hướng về trang đăng nhập
            return RedirectToAction("Login", "Login");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
