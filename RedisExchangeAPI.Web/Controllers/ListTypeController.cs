using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;

namespace RedisExchangeAPI.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        private readonly string listKey = "names";
        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(1);
        }
        public IActionResult Index()
        {
            List<string> namesList = new List<string>();
            if (db.KeyExists(listKey))
            {
                db.ListRange(listKey).ToList().ForEach(x =>
                {
                    namesList.Add(x.ToString());
                });
            }

            return View(namesList);
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
            db.ListRightPush(listKey, name);//datayı sona ekler [left....right]
                                            //   db.ListLeftPush(listKey, name);//datayı başa ekler [left....right]

            return RedirectToAction("Index");
        }
        public IActionResult DeleteItem(string name)
        {
            db.ListRemove(listKey, name);
            return RedirectToAction("Index");
        }
        public IActionResult DeleteFirstItem()
        {
            db.ListLeftPop(listKey);//baştan siler
            return RedirectToAction("Index");
        }
        public IActionResult DeleteLastItem()
        {
            db.ListRightPop(listKey);//sondan siler
            return RedirectToAction("Index");
        }
    }
}
