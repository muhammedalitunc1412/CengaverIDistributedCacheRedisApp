using CengaverIDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace CengaverIDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IDistributedCache _distributedCache;
        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(10);
            #region  CreateCache
            //1
            //await _distributedCache.SetStringAsync("name", "Muhammed Ali Tunç", cacheEntryOptions);

            //2
            //  await _distributedCache.SetStringAsync("product:3", jsonProduct, cacheEntryOptions);
            #endregion

            Product product = new Product { Id = 3, Name = "Pen3", Price = 124 };
            var jsonProduct=JsonConvert.SerializeObject(product);
            var byteProduct=Encoding.UTF8.GetBytes(jsonProduct);
            _distributedCache.Set("product:3", byteProduct);
            return View();
        }
        public IActionResult Show()
        {
            var jsonProduct = _distributedCache.GetString("product:3") ;
            Product product= JsonConvert.DeserializeObject<Product>(jsonProduct);
            ViewBag.Cache = _distributedCache.GetString("product:2");
            return View();
        }
        public IActionResult Delete()
        {
            _distributedCache.Remove("name");
            return RedirectToAction("Show");
        }

        public IActionResult ImageCache()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/images/car.png");
            var imageByte=System.IO.File.ReadAllBytes(path);

            _distributedCache.Set("image", imageByte);
            return View();
        }
        public IActionResult ShowImage()
        {
           var imageByte=_distributedCache.Get("image");

            return File(imageByte,"image/png");
        }
    }
}
