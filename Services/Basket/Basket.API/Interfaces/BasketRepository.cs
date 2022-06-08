﻿using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Basket.API.Interfaces
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache redisCache;
        public BasketRepository(IDistributedCache distributedCache)
        {
            this.redisCache = distributedCache ?? throw new ArgumentNullException(nameof(redisCache));
        }


        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket = await redisCache.GetStringAsync(userName);

            if (string.IsNullOrEmpty(basket))
                return null;

            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            await redisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));
            
            return await GetBasket(basket.UserName);
        }

        public async Task DeleteBasket(string userName)
        {
            //var basket = await redisCache.GetStringAsync(userName);
            await redisCache.RemoveAsync(userName);
        }

        
    }
}
