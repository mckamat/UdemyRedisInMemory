using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private IDistributedCache _cache;
        public ProductsController(IDistributedCache cache)
        {
            _cache = cache; 
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
