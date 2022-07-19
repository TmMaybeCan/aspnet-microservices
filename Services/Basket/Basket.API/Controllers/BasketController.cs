using Basket.API.Entities;
using Basket.API.gRPCServices;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        readonly private DiscountGrpcCallerService _discountGrpcCallerService;

        public BasketController(IBasketRepository basketRepository, DiscountGrpcCallerService discountGrpcCallerService)
        {
            this._basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
            _discountGrpcCallerService = discountGrpcCallerService ?? throw new ArgumentNullException(nameof(discountGrpcCallerService));
        }

        [HttpGet("{userName}", Name = "Get")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> Get(string userName)
        {
            var result = await _basketRepository.GetBasket(userName);  

            return Ok(result ?? new ShoppingCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> Update([FromBody]ShoppingCart basket)
        {
            foreach (var item in basket.Items)
            {
                var coupon = await _discountGrpcCallerService.GetDiscount(item.ProductName);

                item.Price -= coupon.Amount;
            }

            var result = await _basketRepository.UpdateBasket(basket);

            return Ok(result);  
        }
        
        [HttpDelete("{userName}")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(string userName)
        {
            await _basketRepository.DeleteBasket(userName);

            return Ok();
        }
    }
}
