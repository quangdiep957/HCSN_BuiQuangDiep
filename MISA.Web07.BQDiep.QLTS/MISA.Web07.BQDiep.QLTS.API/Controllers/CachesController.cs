using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace MISA.Web07.BQDiep.QLTS.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CachesController : ControllerBase
    {
        public readonly IMemoryCache _memoryCache;
        public CachesController(IMemoryCache memoryCache)
        {
            this._memoryCache = memoryCache;
        }
        [HttpGet]
        public IActionResult GetCache(string id)
        {
            //string value = string.Empty;
            //_memoryCache.TryGetValue(key, out value);
            var cacheValue = _memoryCache.Get(id);
            return Ok(cacheValue);
        }
        [HttpPost]
        public IActionResult SetCache(CacheRequest data)
         {
            
         
            var cacheValue = (List<Guid>)_memoryCache.Get(data.key);
            var cacheExpiryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                Priority = CacheItemPriority.High,
                SlidingExpiration = TimeSpan.FromDays(1),
                Size = 1024,
            };
            var dataValue = new List<Guid>();
         
            if (cacheValue != null)
            {
                _memoryCache.Remove(data.key);
                dataValue = cacheValue;
                dataValue.AddRange(data.value);
          
            }
            else
            {
                _memoryCache.Remove(data.key);
                dataValue = data.value;
            }
            _memoryCache.Set(data.key, dataValue, cacheExpiryOptions);
            return Ok(data);
        }
        public class CacheRequest
        {
            public string? key { get; set; }
            public List<Guid> value { get; set; }
        }
    }
}
