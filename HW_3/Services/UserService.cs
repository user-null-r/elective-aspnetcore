using HW_3.Contracts;
using HW_3.Exceptions;
using HW_3.Models;
using HW_3.Repositories;

namespace HW_3.Services;

public sealed class UserService(IUserRepository users, IPasswordHasher passwordHasher) : IUserService
{
    public async Task<UserResponse> CreateAsync(
        CreateUserRequest request, CancellationToken cancellationToken)
    {
        var userName = NormalizeUserName(request.UserName);
        var email = NormalizeEmail(request.Email);
        await EnsureUniqueAsync(userName, email, null, cancellationToken);
        var now = DateTimeOffset.UtcNow;
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = userName,
            Email = email,
            PasswordHash = passwordHasher.Hash(request.Password),
            CreatedDate = now,
            UpdatedDate = now
        };
        await users.AddAsync(user, cancellationToken);
        await users.SaveChangesAsync(cancellationToken);
        return Map(user);
    }

    public async Task<UserResponse?> AuthorizeAsync(
        LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await users.GetByUserNameAsync(NormalizeUserName(request.UserName), cancellationToken);
        return user is not null && passwordHasher.Verify(request.Password, user.PasswordHash)
            ? Map(user)
            : null;
    }

    public async Task<UserResponse> UpdateAsync(
        Guid id, UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await users.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Пользователь не найден.");
        var userName = NormalizeUserName(request.UserName);
        var email = NormalizeEmail(request.Email);
        await EnsureUniqueAsync(userName, email, id, cancellationToken);
        user.UserName = userName;
        user.Email = email;
        if (!string.IsNullOrWhiteSpace(request.Password))
            user.PasswordHash = passwordHasher.Hash(request.Password);
        user.UpdatedDate = DateTimeOffset.UtcNow;
        await users.SaveChangesAsync(cancellationToken);
        return Map(user);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await users.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Пользователь не найден.");
        users.Remove(user);
        await users.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<UserResponse>> GetAllAsync(
        UserDateFilter filter, CancellationToken cancellationToken)
    {
        var result = await users.GetAllAsync(filter, cancellationToken);
        return result.Select(Map).ToList();
    }

    private async Task EnsureUniqueAsync(
        string userName, string email, Guid? excludedId, CancellationToken cancellationToken)
    {
        if (await users.UserNameExistsAsync(userName, excludedId, cancellationToken))
            throw new ConflictException("Имя пользователя уже занято.");
        if (await users.EmailExistsAsync(email, excludedId, cancellationToken))
            throw new ConflictException("Email уже используется.");
    }

    private static string NormalizeUserName(string userName)
    {
        return userName.Trim().ToLowerInvariant();
    }

    private static string NormalizeEmail(string email)
    {
        return email.Trim().ToLowerInvariant();
    }

    private static UserResponse Map(User user)
    {
        return new UserResponse(user.Id, user.UserName, user.Email, user.CreatedDate, user.UpdatedDate);
    }
}

