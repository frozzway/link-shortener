using LinkShortener.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortener.Pages;


[IgnoreAntiforgeryToken]
public class Login : PageModel
{
    private readonly UserService _userService;
    
    public Login(UserService userService)
    {
        _userService = userService;
    }
    
    public void OnGet() {}
    
    public IActionResult OnPost()
    {
        var email = Request.Form["email"].ToString();
        var password = Request.Form["password"].ToString();
        
        if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
        {
            var user = _userService.GetByEmail(email);
            
            if (user != null && _userService.VerifyPassword(password, user.Password))
            {
                HttpContext.Session.SetString("UserEmail", user.Email);
                
                return RedirectToPage("/My");
            }
        }
        
        ModelState.AddModelError("", "Неверное имя пользователя или пароль");
        return Page();
    }
}