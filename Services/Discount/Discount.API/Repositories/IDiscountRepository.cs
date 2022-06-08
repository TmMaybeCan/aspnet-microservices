using Discount.gRPC.Entities;
using System.Threading.Tasks;

namespace Discount.gRPC.Repositories
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscount(string productName);
        Task<bool> AddDiscount(Coupon coupon);
        Task<bool> UpdeteDiscount(Coupon coupon);
        Task<bool> DeleteDiscount(string productName);
    }
}
