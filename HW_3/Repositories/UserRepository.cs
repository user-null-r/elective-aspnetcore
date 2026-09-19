using HW_3.Contracts;
using HW_3.Data;
using HW_3.Models;
using Microsoft.EntityFrameworkCore;

namespace HW_3.Repositories;

public sealed class UserRepository(AppDbContext database) : IUserRepository
{
    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return database.Users.SingleOrDefaultAsync(item => item.Id == id, cancellationToken);
    }

    public Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken)
    {
        return database.Users.AsNoTracking().SingleOrDefaultAsync(
            item => item.UserName == userName, cancellationToken);
    }

    public Task<bool> UserNameExistsAsync(
        string userName, Guid? excludedId, CancellationToken cancellationToken)
    {
        return database.Users.AnyAsync(
            item => item.UserName == userName && item.Id != excludedId, cancellationToken);
    }

    public Task<bool> EmailExistsAsync(string email, Guid? excludedId, CancellationToken cancellationToken)
    {
        return database.Users.AnyAsync(
            item => item.Email == email && item.Id != excludedId, cancellationToken);
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(
        UserDateFilter filter, CancellationToken cancellationToken)
    {
        var query = database.Users.AsNoTracking();
        if (filter.CreatedFrom.HasValue)
            query = query.Where(item => item.CreatedDate >= filter.CreatedFrom.Value);
        if (filter.CreatedTo.HasValue)
            query = query.Where(item => item.CreatedDate <= filter.CreatedTo.Value);
        if (filter.UpdatedFrom.HasValue)
            query = query.Where(item => item.UpdatedDate >= filter.UpdatedFrom.Value);
        if (filter.UpdatedTo.HasValue)
            query = query.Where(item => item.UpdatedDate <= filter.UpdatedTo.Value);
        return await query.OrderBy(item => item.CreatedDate).ToListAsync(cancellationToken);
    }

    public Task AddAsync(User user, CancellationToken cancellationToken)
    {
        return database.Users.AddAsync(user, cancellationToken).AsTask();
    }

    public void Remove(User user)
    {
        database.Users.Remove(user);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return database.SaveChangesAsync(cancellationToken);
    }
}

