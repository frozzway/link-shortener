using LinkShortener.Database;

namespace LinkShortener.Services;

public class UserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public string HashPassword(string password)
    {
        var salt = BCrypt.Net.BCrypt.GenerateSalt(10);
        var hash = BCrypt.Net.BCrypt.HashPassword(password, salt);

        return hash;
    }

    public bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }

    public void Add(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }
    
    public User? GetByEmail(string email)
    {
        return _context.Users.FirstOrDefault(u => u.Email == email);
    }

    public Link[] GetUserLinks(User user)
    {
        return _context.Links.Where(l => l.User == user).OrderByDescending(l => l.Id).ToArray();
    }
}