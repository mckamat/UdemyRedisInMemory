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
            //if (String.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
            //{
            //    _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            //}
            //2.yol var olup olmadıgını kontrol etmek için
            if (!_memoryCache.TryGetValue("zaman", out string zamancache))
            {

                MemoryCacheEntryOptions opt = new MemoryCacheEntryOptions();
                //cache ömrü ayarlamaları


                //absolute ile son süre verilir ne olursa olsun 30 saniye sonra ramden data silinir.
                // opt.AbsoluteExpiration = DateTime.Now.AddSeconds(30);

                //10 saniye içerisinde erişilmezse RAM'den silinir eğer erişilirse sürekli 10 saniye kendini artırır yeniler.
                opt.SlidingExpiration = TimeSpan.FromSeconds(10);


                //Priority ayarı : Ram dolunca bu ozellıge gore ya siler ya da hiç silmez aslında cache önem değeri veriliyor.
                opt.Priority = CacheItemPriority.NeverRemove;

                //Silindikten sonra calısan fonk.Bu metod delege alır.c#da delegeler metodları işaret ederler ben burda yeni bir metod yazmak yerıne uzun uzun parametre olarak verdım ve ıcıne metod yazdım.
                opt.RegisterPostEvictionCallback((key, value, reason, state) =>
                {
                    _memoryCache.Set("callback", $"{key}->{value} => sebep : {reason}");
                });


                _memoryCache.Set<string>("zaman", DateTime.Now.ToString(), opt);
            }

            return View();
        }

        public IActionResult Show()
        {
            /*_memoryCache.GetOrCreate<string>("zaman", entry => { return DateTime.Now.ToString(); }); bu metodta zaman keyi ile ilgili bir key yoksa olusturur.ikinci parametre olarak func alır olusturacagım datayı basarım.*/
            //  _memoryCache.GetOrCreate<string>("zaman", entry => { return DateTime.Now.ToString(); });


            /*=> zaman keyli bir cache var mı bak varsa zamancache degıskenıne doldur ve view a dön*/
            _memoryCache.TryGetValue("zaman", out string zamancache);
            _memoryCache.TryGetValue("callback", out string callback);
            ViewBag.zaman = zamancache;
            ViewBag.callback = callback;

            //  _memoryCache.Remove("zaman");//bu keye sahip olan datayı memoryden siliyor.
            // ViewBag.zaman = _memoryCache.Get<string>("zaman");
            return View();
        }
    }
}
