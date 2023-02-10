using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        private readonly string listKey = "hashnames";
        public SetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(2);
        }
        public IActionResult Index()
        {//normal listten farklı içeride tutacağı değerler uniqedir ve sırasız bir şekilde tutar
            HashSet<string> namesList = new HashSet<string>();

            if (db.KeyExists(listKey))
            {
                db.SetMembers(listKey).ToList().ForEach(x =>
                {
                    namesList.Add(x.ToString());
                });
            }

            return View(namesList);
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
            db.KeyExpire(listKey, System.DateTime.Now.AddMinutes(5));
            db.SetAdd(listKey, name);


            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteItem(string name)
        {
            await db.SetRemoveAsync(listKey, name);
            return RedirectToAction("Index");
        }

    }
}
