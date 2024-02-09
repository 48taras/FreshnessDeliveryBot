using FreshnessDeliveryBot.DbContexts;
using FreshnessDeliveryBot.Models;
using Microsoft.EntityFrameworkCore;

public class PersonService
{
    private readonly FreshnessBotDbContext _dbContext;

    public PersonService(FreshnessBotDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Person>> GetAllAsync()
    {
        return await _dbContext.Persons.ToListAsync();
    }

    public async Task CreateAsync(Person user)
    {
        _dbContext.Persons.Add(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Person user)
    {
        _dbContext.Persons.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int userId)
    {
        var user = await _dbContext.Persons
            .Where(x => x.CustomerID == userId)
            .FirstOrDefaultAsync();

        if (user != null)
        {
            _dbContext.Persons.Remove(user);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<bool> UserExistsAsync(long userId)
    {
        return await _dbContext.Persons.AnyAsync(x => x.CustomerID == userId);
    }
}
