using EatTogether.Models.EfModels;
using EatTogether.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EatTogether.Models.Repositories
{
    public interface IReportRepository
    {
        Task<List<Order>> GetCompletedOrdersAsync(DateTime start, DateTime end);
        Task<List<PreOrder>> GetAllPreOrdersInRangeAsync(DateTime start, DateTime end);
        Task<List<ProductRankItemViewModel>> GetTopProductsAsync(DateTime start, DateTime end, int top = 10);
        Task<List<CouponRankItemViewModel>> GetCouponRankingAsync(DateTime start, DateTime end);
    }

    public class ReportRepository : IReportRepository
    {
        private readonly EatTogetherDBContext _context;
        public ReportRepository(EatTogetherDBContext db) => _context = db;

        // Orders 表裡全部都是已完成且付款成功的訂單，不需要再過濾狀態
        public async Task<List<Order>> GetCompletedOrdersAsync(DateTime start, DateTime end)
        {
            return await _context.Orders
                .Where(o => o.OrderAt.Date >= start.Date
                         && o.OrderAt.Date <= end.Date)
                .OrderBy(o => o.OrderAt)
                .ToListAsync();
        }

        // 取消率等仍需要從 PreOrders 撈（Orders 只有成交資料）
        public async Task<List<PreOrder>> GetAllPreOrdersInRangeAsync(DateTime start, DateTime end)
        {
            return await _context.PreOrders
                .Where(p => p.OrderAt.Date >= start.Date
                         && p.OrderAt.Date <= end.Date)
                .ToListAsync();
        }

        // 熱銷改從 OrderDetails 撈，確保只統計真正成交的品項
        public async Task<List<ProductRankItemViewModel>> GetTopProductsAsync(
            DateTime start, DateTime end, int top = 10)
        {
            return await _context.OrderDetails
                .Where(d => d.Order.OrderAt.Date >= start.Date
                         && d.Order.OrderAt.Date <= end.Date)
                .GroupBy(d => d.ProductName)
                .Select(g => new ProductRankItemViewModel
                {
                    ProductName = g.Key,
                    TotalQty = g.Sum(d => d.Qty),
                    TotalRevenue = g.Sum(d => d.SubTotal)
                })
                .OrderByDescending(x => x.TotalQty)
                .Take(top)
                .ToListAsync();
        }

        public async Task<List<CouponRankItemViewModel>> GetCouponRankingAsync(
            DateTime start, DateTime end)
        {
            return await _context.Orders
                .Where(o => o.CouponId.HasValue
                         && o.OrderAt.Date >= start.Date
                         && o.OrderAt.Date <= end.Date)
                .Include(o => o.Coupon)
                .GroupBy(o => o.Coupon.Name)
                .Select(g => new CouponRankItemViewModel
                {
                    CouponName = g.Key,
                    UsedCount = g.Count(),
                    TotalDiscount = g.Sum(o => o.DiscountAmount)
                })
                .OrderByDescending(x => x.UsedCount)
                .ToListAsync();
        }
    }
}
