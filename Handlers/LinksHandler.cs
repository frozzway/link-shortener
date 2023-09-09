using LinkShortener.Services;

namespace LinkShortener.Handlers;

public class LinksHandler
{

    public static IResult OnGet(string? shortCode, LinkService linkService)
    {
        if (string.IsNullOrEmpty(shortCode))
            return Results.Redirect("/login");
        
        var sourceLink = linkService.GetSourceLink(shortCode);
        
        if (sourceLink is null)
            return Results.NotFound();

        while (true)
        {
            try
            {
                linkService.IncrementCounter(shortCode);
                break;
            }
            catch (Npgsql.PostgresException ex) when (ex.SqlState == "40001")
            {
                var milliseconds =  new Random().Next(100, 200);
                Thread.Sleep(milliseconds);
            }
        }
        
        
        return Results.Redirect(sourceLink);
    }
    
}