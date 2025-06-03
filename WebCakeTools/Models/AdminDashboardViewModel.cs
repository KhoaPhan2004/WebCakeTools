namespace WebCakeTools.Models
{
    public class AdminDashboardViewModel
    {
        public int TotalProducts { get; set; }
        public List<Product> TopContactProducts { get; set; }
        public List<Product> LatestProducts { get; set; }
    }
}
