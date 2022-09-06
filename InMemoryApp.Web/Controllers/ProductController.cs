using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            //1.yol var olup olmadıgını kontrol etmek için
            if (String.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
            {
                _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            }
            //2.yol var olup olmadıgını kontrol etmek için
            if (!_memoryCache.TryGetValue("zaman", out string zamancache))
            {
                _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            }

            return View();
        }

        public IActionResult Show()
        {
            /*_memoryCache.GetOrCreate<string>("zaman", entry => { return DateTime.Now.ToString(); }); bu metodta zaman keyi ile ilgili bir key yoksa olusturur.ikinci parametre olarak func alır olusturacagım datayı basarım.*/
            _memoryCache.GetOrCreate<string>("zaman", entry => { return DateTime.Now.ToString(); });


            _memoryCache.Remove("zaman");//bu keye sahip olan datayı memoryden siliyor.
            ViewBag.zaman = _memoryCache.Get<string>("zaman");
            return View();
        }
    }
}
