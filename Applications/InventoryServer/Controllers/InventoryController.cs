using InventoryServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {      

        private readonly ILogger<InventoryController> _logger;

        public InventoryController(ILogger<InventoryController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "Get")]
        public IEnumerable<InventoryInfo> Get()
        {
            return new List<InventoryInfo> { new InventoryInfo() };           
        }
    }
}