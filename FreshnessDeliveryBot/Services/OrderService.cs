using FreshnessDeliveryBot.DbContexts;
using FreshnessDeliveryBot.Models;
using Microsoft.EntityFrameworkCore;

public class OrderService
{
    private readonly FreshnessBotDbContext _dbContext;

    public OrderService(FreshnessBotDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Order>> GetAllAsync()
    {
        return await _dbContext.Orders.ToListAsync();
    }

    public async Task CreateAsync(Order order)
    {
        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Order order)
    {
        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int orderId)
    {
        var order = await _dbContext.Orders
            .Where(x => x.OrderID == orderId)
            .FirstOrDefaultAsync();

        if (order != null)
        {
            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();
        }
    }
}
