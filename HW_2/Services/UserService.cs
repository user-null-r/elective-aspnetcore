using System.ComponentModel.DataAnnotations;
using HW_2.Models;

namespace HW_2.Services;

public sealed class UserService(PasswordHasher passwordHasher)
{
    private readonly List<User> _users = [];

    public User Create(string userName, string email, string password)
    {
        Validate(userName, email, password);
        EnsureUnique(userName, email);
        var now = DateTimeOffset.UtcNow;
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = userName.Trim(),
            Email = email.Trim().ToLowerInvariant(),
            PasswordHash = passwordHasher.Hash(password),
            CreatedDate = now,
            UpdatedDate = now
        };
        _users.Add(user);
        return user;
    }

    public bool Authorize(string userName, string password)
    {
        var user = _users.SingleOrDefault(item =>
            string.Equals(item.UserName, userName.Trim(), StringComparison.OrdinalIgnoreCase));
        return user is not null && passwordHasher.Verify(password, user.PasswordHash);
    }

    public User Update(Guid id, string userName, string email, string? password)
    {
        Validate(userName, email, password, passwordRequired: false);
        var user = _users.SingleOrDefault(item => item.Id == id)
            ?? throw new ArgumentException("Пользователь не найден.");
        EnsureUnique(userName, email, id);
        user.UserName = userName.Trim();
        user.Email = email.Trim().ToLowerInvariant();
        if (!string.IsNullOrWhiteSpace(password))
            user.PasswordHash = passwordHasher.Hash(password);
        user.UpdatedDate = DateTimeOffset.UtcNow;
        return user;
    }

    public bool Delete(Guid id)
    {
        var user = _users.SingleOrDefault(item => item.Id == id);
        return user is not null && _users.Remove(user);
    }

    public IReadOnlyCollection<User> GetAll()
    {
        return _users.AsReadOnly();
    }

    private void EnsureUnique(string userName, string email, Guid? excludedId = null)
    {
        if (_users.Any(item => item.Id != excludedId &&
            string.Equals(item.UserName, userName.Trim(), StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException("Имя пользователя уже занято.");

        if (_users.Any(item => item.Id != excludedId &&
            string.Equals(item.Email, email.Trim(), StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException("Email уже используется.");
    }

    private static void Validate(string userName, string email, string? password, bool passwordRequired = true)
    {
        if (string.IsNullOrWhiteSpace(userName) || userName.Trim().Length is < 3 or > 50)
            throw new ArgumentException("Имя пользователя должно содержать от 3 до 50 символов.");
        if (string.IsNullOrWhiteSpace(email) || !new EmailAddressAttribute().IsValid(email))
            throw new ArgumentException("Некорректный email.");
        if ((passwordRequired || password is not null) && (password?.Length ?? 0) < 8)
            throw new ArgumentException("Пароль должен содержать не менее 8 символов.");
    }
}
