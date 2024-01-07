using Microsoft.AspNetCore.Mvc;
using ShippingTrackingSystem.BackEnd.Interfaces;

namespace ShippingTrackingSystem.Controllers.Api
{
    [ApiController]
    [Route("api/statistics")]
    public class StatisticsController : Controller
    {
        private readonly ISystemStatisticsRepository _statsRepo;

        public StatisticsController(ISystemStatisticsRepository statsRepo)
        {
            _statsRepo = statsRepo;
        }

        [HttpGet("Totalusers")]
        public async Task<ActionResult> GetTotalUsers()
        {
            int count = await _statsRepo.GetUserCountAsync();
            return Ok(new { count });
        }

        [HttpGet("CustomerRoleCount")]
        public async Task<ActionResult> GetCustomerRoleUserCount()
        {
            int count = await _statsRepo.GetCustomerRoleUserCountAsync();
            return Ok(new { count });
        }

        [HttpGet("DeliveryRoleCount")]
        public async Task<ActionResult> GetDeliveryRoleUserCount()
        {
            int count = await _statsRepo.GetDeliveryRoleUserCountAsync();
            return Ok(new { count });
        }

        [HttpGet("WarehouseRoleCount")]
        public async Task<ActionResult> GetWarehouseRoleUserCount()
        {
            int count = await _statsRepo.GetWarehouseRoleUserCountAsync();
            return Ok(new { count });
        }

        [HttpGet("OrderStatusCounts")]
        public async Task<ActionResult> GetOrderStatusCounts()
        {
            try
            {
                var orderStatusCounts = await _statsRepo.GetOrderStatusCountsAsync();
                return Ok(orderStatusCounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("ProductsAvailable")]
        public async Task<ActionResult> GetProductsAvailableCount()
        {
            try
            {
                var count = await _statsRepo.GetAvailableProductsCountAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("ProductsNotAvailable")]
        public async Task<ActionResult> GetProductsNotAvailableCount()
        {
            try
            {
                var count = await _statsRepo.GetOutOfStockProductsCountAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

    }
}
