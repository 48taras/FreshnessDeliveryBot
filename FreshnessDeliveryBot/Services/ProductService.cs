using FreshnessDeliveryBot.DbContexts;
using FreshnessDeliveryBot.Models;
using Microsoft.EntityFrameworkCore;

public class ProductService
{
    private readonly FreshnessBotDbContext _dbContext;

    public ProductService(FreshnessBotDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _dbContext.Products.ToListAsync();
    }

    public async Task CreateAsync(Product product)
    {
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _dbContext.Products.Update(product);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Product> GetByNameAsync(string productName)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(p => p.Name == productName);
    }

    public async Task DeleteAsync(int productId)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
        if (product != null)
        {
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
        }
    }
}
