using LinkShortener.Database;
using LinkShortener.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortener.Pages;


[IgnoreAntiforgeryToken]
public class Register : PageModel
{
    private readonly UserService _userService;
    
    public Register(UserService userService)
    {
        _userService = userService;
    }
    
    public void OnGet() {}

    public IActionResult OnPost()
    {
        var name = Request.Form["name"].ToString();
        var password = Request.Form["password"].ToString();
        var email = Request.Form["email"].ToString();
        
        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(email))
        {
            var user = new User
            {
                Email = email,
                Name = name,
                Password = _userService.HashPassword(password)
            };

            if (_userService.GetByEmail(email) != null)
            {
                TempData["RegistrationMessage"] = "Email занят";
                return Page();
            }
            
            _userService.Add(user);
            HttpContext.Session.SetString("UserEmail", user.Email);
            return RedirectToPage("/My");
        }
        
        ModelState.AddModelError("", "Все поля обязательны для заполнения");
        return Page();
    }
}