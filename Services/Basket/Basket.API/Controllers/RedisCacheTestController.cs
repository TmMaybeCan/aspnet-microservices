using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisCacheTestController : ControllerBase
    {
		private readonly IDistributedCache _distributedCache;

		public RedisCacheTestController(IDistributedCache distributedCache)
		{
			_distributedCache = distributedCache;
		}

		[HttpGet]
		public async Task<string> Get()
		{
			var cacheKey = "TheTime";
			var existingTime = _distributedCache.GetString(cacheKey);
		    await _distributedCache.RemoveAsync(cacheKey);
			if (!string.IsNullOrEmpty(existingTime))
			{
				return "Fetched from cache : " + existingTime;
			}
			else
			{
				existingTime = DateTime.UtcNow.ToString();
				await _distributedCache.SetStringAsync(cacheKey, existingTime);
				return "Added to cache : " + existingTime;
			}
		}
	}
}
