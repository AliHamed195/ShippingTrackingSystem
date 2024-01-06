namespace ShippingTrackingSystem.ViewModels
{
    public class ProductOrderViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }
        public int AvailableQuantity { get; set; }
        public int PurchaseQuantity { get; set; }
        public string? ImagePath { get; set; }
    }
}
