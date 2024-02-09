using FreshnessDeliveryBot.DbContexts;
using FreshnessDeliveryBot.Models;
using Microsoft.EntityFrameworkCore;

public class OrderItemService
{
    private readonly FreshnessBotDbContext _dbContext;

    public OrderItemService(FreshnessBotDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<OrderItem>> GetAllAsync()
    {
        return await _dbContext.OrderItems.ToListAsync();
    }

    public async Task CreateAsync(OrderItem orderItem)
    {
        _dbContext.OrderItems.Add(orderItem);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(OrderItem orderItem)
    {
        _dbContext.OrderItems.Update(orderItem);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int orderItemId)
    {
        var orderItem = await _dbContext.OrderItems
            .Where(x => x.OrderItemID == orderItemId)
            .FirstOrDefaultAsync();

        if (orderItem != null)
        {
            _dbContext.OrderItems.Remove(orderItem);
            await _dbContext.SaveChangesAsync();
        }
    }
}
