using HW_2.Services;

namespace HW_2;

public static class Program
{
    private static readonly UserService Users = new(new PasswordHasher());

    public static void Main()
    {
        while (true)
        {
            Console.WriteLine("1 — создать пользователя");
            Console.WriteLine("2 — авторизоваться");
            Console.WriteLine("3 — редактировать пользователя");
            Console.WriteLine("4 — удалить пользователя");
            Console.WriteLine("5 — показать пользователей");
            Console.WriteLine("0 — выйти");

            try
            {
                switch (Console.ReadLine())
                {
                    case "1": CreateUser(); break;
                    case "2": Authorize(); break;
                    case "3": EditUser(); break;
                    case "4": DeleteUser(); break;
                    case "5": ShowUsers(); break;
                    case "0": return;
                    default: Console.WriteLine("Неизвестная команда."); break;
                }
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine(exception.Message);
            }

            Console.WriteLine();
        }
    }

    public static void CreateUser()
    {
        Console.Write("Имя пользователя: ");
        var userName = ReadRequired();
        Console.Write("Email: ");
        var email = ReadRequired();
        Console.Write("Пароль: ");
        var password = ReadRequired();
        var user = Users.Create(userName, email, password);
        Console.WriteLine($"Пользователь создан. Id: {user.Id}");
    }

    public static void Authorize()
    {
        Console.Write("Имя пользователя: ");
        var userName = ReadRequired();
        Console.Write("Пароль: ");
        var password = ReadRequired();
        Console.WriteLine(Users.Authorize(userName, password)
            ? "Авторизация успешна."
            : "Неверное имя пользователя или пароль.");
    }

    public static void EditUser()
    {
        Console.Write("Id: ");
        var id = ReadId();
        Console.Write("Новое имя пользователя: ");
        var userName = ReadRequired();
        Console.Write("Новый email: ");
        var email = ReadRequired();
        Console.Write("Новый пароль или Enter, чтобы не менять: ");
        var password = Console.ReadLine();
        var user = Users.Update(id, userName, email, string.IsNullOrWhiteSpace(password) ? null : password);
        Console.WriteLine($"Пользователь {user.UserName} обновлен.");
    }

    public static void DeleteUser()
    {
        Console.Write("Id: ");
        var id = ReadId();
        Console.WriteLine(Users.Delete(id) ? "Пользователь удален." : "Пользователь не найден.");
    }

    private static void ShowUsers()
    {
        foreach (var user in Users.GetAll())
            Console.WriteLine($"{user.Id}: {user.UserName}, {user.Email}, {user.CreatedDate:u}");
    }

    private static Guid ReadId()
    {
        return Guid.TryParse(Console.ReadLine(), out var id)
            ? id
            : throw new ArgumentException("Некорректный Id.");
    }

    private static string ReadRequired()
    {
        var value = Console.ReadLine();
        return !string.IsNullOrWhiteSpace(value)
            ? value
            : throw new ArgumentException("Значение не может быть пустым.");
    }
}

