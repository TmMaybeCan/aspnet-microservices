using Discount.gRPC.Repositories;
using Discount.gRPC.Protos;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using AutoMapper;
using Discount.gRPC.Entities;

namespace Discount.gRPC.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly ILogger<DiscountService> _logger;
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;
        public DiscountService(ILogger<DiscountService> logger, IDiscountRepository discountRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _discountRepository = discountRepository ?? throw new ArgumentNullException(nameof(discountRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _discountRepository.GetDiscount(request.ProductName);

            if (coupon is null)
                throw new RpcException(new Status(StatusCode.NotFound, $"----->Discount with product name - {request.ProductName} is not found!"));

            _logger.LogInformation($"----->Coupon for {coupon.ProductName} retrieved. Amount - {coupon.Amount}");

            var couponModel = _mapper.Map<CouponModel>(coupon);

            #region mapping

            /*
            //source
            var c = new Coupon
            {
                Amount = coupon.Amount,
                Description = coupon.Description,
                Id = coupon.Id,
                ProductName = coupon.ProductName
            };

            //destination
            var cm = new CouponModel
            {
                Amount = c.Amount,
                Description = c.Description,
                Id = c.Id,
                ProductName = c.ProductName
            };
            */

            #endregion

            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountResponse request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);

            #region mapping

            /*
            //source
            var cm = new CouponModel
            {
                Amount = request.Coupon.Amount,
                Description = request.Coupon.Description,
                Id = request.Coupon.Id,
                ProductName = request.Coupon.ProductName
            };

            //destination
            var c = new Coupon
            {
                Amount = cm.Amount,
                Description = cm.Description,
                Id = cm.Id,
                ProductName = cm.ProductName
            };
            */

            #endregion

            await _discountRepository.AddDiscount(coupon);

            _logger.LogInformation($"----->Discount successfully created for {coupon.ProductName}");

            var couponModel = _mapper.Map<CouponModel>(coupon);

            #region mapping

            /*
            //destination
            var cm = new CouponModel
            {
                Amount = c.Amount,
                Description= c.Description,
                Id = c.Id,
                ProductName = c.ProductName
            };
            */

            #endregion

            return couponModel;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountResponse request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);

            await _discountRepository.UpdeteDiscount(coupon);

            _logger.LogInformation($"----->{coupon.ProductName} successfully updated!");

            var couponModel = _mapper.Map<CouponModel>(coupon);

            return couponModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {

            var result = await _discountRepository.DeleteDiscount(request.ProductName);

            _logger.LogInformation($"----->Discount for {request.ProductName} successfully deleted!");

            var response = new DeleteDiscountResponse
            {
                Success = result
            };

            return response;
        }
    }
}
