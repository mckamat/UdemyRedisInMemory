using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.IO;

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
            DistributedCacheEntryOptions opt = new DistributedCacheEntryOptions();

            opt.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            Product product = new Product() { Id=1,Name="kitap",Price =100};

            string jsonProduct = JsonConvert.SerializeObject(product);

            _cache.SetString("product:1", jsonProduct,opt);
           
            //_cache.SetString("name", "mucahit", opt);

            return View();
        }

        public  IActionResult Show()
        {
            string name = _cache.GetString("name");
            string jsonProduct = _cache.GetString("product:1");
            Product p = JsonConvert.DeserializeObject<Product>(jsonProduct);
            ViewBag.Name = name;
            ViewBag.Product = p;
            return View();
        }
        public IActionResult Remove()
        {
            _cache.Remove("name");
            return View();
        }

        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/test3.png");

            Byte[] imageByte = System.IO.File.ReadAllBytes(path);

            _cache.Set("resim", imageByte);

            return View();
        }

        public IActionResult ImageUrl()
        {
            byte[] imageByte = _cache.Get("resim");

            return File(imageByte, "image/png");
        }
    }
}
