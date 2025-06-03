namespace WebCakeTools.Models
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public string? ProductDescribe { get; set; }

        public IFormFile? ProductImage { get; set; }
        public int? Contact { get; set; }
        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? SelectedParentCategoryId { get; set; }
        public int? SelectedChildCategoryId { get; set; }


        public string? OldImagePath { get; set; }
    }
}
