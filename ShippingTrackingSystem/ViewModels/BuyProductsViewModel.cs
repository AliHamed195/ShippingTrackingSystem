namespace ShippingTrackingSystem.ViewModels
{
    public class BuyProductsViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<ProductOrderViewModel> Products { get; set; }
    }
}
