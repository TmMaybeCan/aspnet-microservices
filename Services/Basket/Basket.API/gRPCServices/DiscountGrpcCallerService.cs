using Discount.gRPC.Protos;
using System;
using System.Threading.Tasks;

namespace Basket.API.gRPCServices
{
    public class DiscountGrpcCallerService
    {
        readonly private DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

        public DiscountGrpcCallerService(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
        {
            _discountProtoServiceClient = discountProtoServiceClient ?? throw new ArgumentNullException(nameof(discountProtoServiceClient));
        }

        public async Task<CouponModel> GetDiscount(string productName)
        {
            var discountRequest = new GetDiscountRequest { ProductName =  productName};

            var result = await _discountProtoServiceClient.GetDiscountAsync(discountRequest);

            return result;
        }
    }
}
