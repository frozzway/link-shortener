using LinkShortener.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LinkShortener.Pages;

public class AuthFilter : IPageFilter
{
    private readonly UserService _userService;
    
    public AuthFilter(UserService userService)
    {
        _userService = userService;
    }
    
    public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        var userEmail = context.HttpContext.Session.GetString("UserEmail");
        
        if (string.IsNullOrEmpty(userEmail))
        {
            context.Result = new RedirectResult("/Login");
        }
        else
        {
            var pageModel = context.HandlerInstance as PageModel;
            var user = _userService.GetByEmail(userEmail)!;
            pageModel!.ViewData["User"] = user;
        }
    }
    
    public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
    {
    }
    
    public void OnPageHandlerSelected(PageHandlerSelectedContext context)
    {
    }
}