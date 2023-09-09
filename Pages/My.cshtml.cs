using System.Security.Claims;
using System.Text;
using LinkShortener.Database;
using LinkShortener.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LinkShortener.Pages;

[TypeFilter(typeof(AuthFilter))]
[IgnoreAntiforgeryToken]
public class Panel : PageModel
{
    public User _user;
    public LinkService _linkService;
    public UserService _userService;
    
    public Panel(IHttpContextAccessor contextAccessor, LinkService linkService, UserService userService)
    {
        _linkService = linkService;
        _userService = userService;
    }

    public IActionResult OnGet()
    {
        _user = (ViewData["User"] as User)!;
        return Page();
    }

    public IActionResult OnPost()
    {
        _user = (ViewData["User"] as User)!;
        var sourceLink = Request.Form["link"].ToString();
        if (string.IsNullOrEmpty(sourceLink))
        {
            return Page();
        }
        if (!sourceLink.StartsWith("http://") && !sourceLink.StartsWith("https://"))
        {
            sourceLink = "http://" + sourceLink;
        }
        var shortCode = CreateUniqueShortCode(9);
        var link = new Link
        {
            User = _user,
            SourceLink = sourceLink,
            ShortCode = shortCode,
            Created = DateTime.UtcNow
        };
        
        if (_linkService.Register(link))
            TempData["Message"] = $"Ваша сокращенная ссылка /{shortCode}";
        else 
            TempData["Message"] = $"Такая ссылка уже добавлена";
        
        return RedirectToPage("/my");
    }

    private string CreateUniqueShortCode(int length)
    {
        while (true)
        {
            var shortCode = CreateShortCode(9);
            var existedSourceLink = _linkService.GetSourceLink(shortCode);
            if (string.IsNullOrEmpty(existedSourceLink))
                return shortCode;
        }
    }

    private static string CreateShortCode(int length)
    {
        var random = new Random();
        var sb = new StringBuilder();

        for (var i = 0; i < length; i++)
        {
            var c = random.Next(0, 26); // случайное число от 0 до 25
            if (random.Next(0, 4) == 0) // случайное число от 0 до 1
            {
                c += 'A'; // преобразуем в большую букву [A-Z]
            }
            else
            {
                c += 'a'; // преобразуем в маленькую букву [a-z]
            }
            sb.Append((char)c); // добавляем символ к строке
        }

        return sb.ToString();
    }
}