using Dapper;
using Discount.gRPC.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace Discount.gRPC.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException();
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("select * from Coupon where ProductName = @ProductName", new { ProductName = productName});

            if (coupon is null)
                return new Coupon
                {
                    ProductName = "No discount",
                    Description = "No discount description",
                    Amount = 0,
                };
            return coupon;
        }
        public async Task<bool> AddDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync("insert into Coupon (ProductName, Description, Amount) values (@ProductName, @Description, @Amount)", 
                new 
                { 
                    ProductName = coupon.ProductName, 
                    Description = coupon.Description, 
                    Amount = coupon.Amount 
                });

            if (affected == 0)
                return false;
            return true;
        }
        public async Task<bool> UpdeteDiscount(Coupon coupon)
        {
            var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync("update Coupon set ProductName = @ProductName, Description = @Description, Amount = @Amount where Id = @Id",
                new
                {
                    Id = coupon.Id,
                    ProductName = coupon.ProductName,
                    Description = coupon.Description,
                    Amount = coupon.Amount
                });
             
            if (affected == 0)
                return false;
            return true;                    
        }
        public async Task<bool> DeleteDiscount(string productName)
        {
            var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync("delete from Coupon where ProductName = @ProductName", new { ProductName = productName});

            if (affected == 0)
                return false;
            return true;
        }
    }
}
