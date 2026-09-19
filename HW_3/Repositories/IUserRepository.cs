using HW_3.Contracts;
using HW_3.Models;

namespace HW_3.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken);
    Task<bool> UserNameExistsAsync(string userName, Guid? excludedId, CancellationToken cancellationToken);
    Task<bool> EmailExistsAsync(string email, Guid? excludedId, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetAllAsync(UserDateFilter filter, CancellationToken cancellationToken);
    Task AddAsync(User user, CancellationToken cancellationToken);
    void Remove(User user);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}

