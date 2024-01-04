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
    }
}
