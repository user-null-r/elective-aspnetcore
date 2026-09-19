using HW_3.Contracts;

namespace HW_3.Services;

public interface IUserService
{
    Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken);
    Task<UserResponse?> AuthorizeAsync(LoginRequest request, CancellationToken cancellationToken);
    Task<UserResponse> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<UserResponse>> GetAllAsync(UserDateFilter filter, CancellationToken cancellationToken);
}

