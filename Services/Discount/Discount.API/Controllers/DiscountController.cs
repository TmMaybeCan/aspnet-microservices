using Discount.gRPC.Entities;
using Discount.gRPC.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Discount.gRPC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _discountRepository;
        public DiscountController(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository ?? throw new ArgumentNullException(nameof(discountRepository));
        }

        [HttpGet("{productName}", Name = "GetDiscount")]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Coupon>> Get(string productName)
        {
            var result = await _discountRepository.GetDiscount(productName);

            return Ok(result);
        }

        [HttpPost(Name = "AddDiscount")]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> Add([FromBody] Coupon coupon)
        {
            await _discountRepository.AddDiscount(coupon);

            return CreatedAtRoute("GetDiscount", new { productName = coupon.ProductName }, coupon);
        }
        [HttpPut]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> Update([FromBody] Coupon coupon)
        {
            var result = await _discountRepository.UpdeteDiscount(coupon);

            return Ok(result);

            //await _discountRepository.UpdeteDiscount(coupon);

            //return NoContent();
        }

        [HttpDelete("{productName}", Name = "DeletDiscount")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> Delete(string productName)
        {
            var result = await _discountRepository.DeleteDiscount(productName);

            return Ok(result);
        }


    }
}
