using System.Data;
using LinkShortener.Database;
using Microsoft.EntityFrameworkCore;


namespace LinkShortener.Services;

public class LinkService
{
    private readonly AppDbContext _context;

    public LinkService(AppDbContext context)
    {
        _context = context;
    }

    public bool Register(Link link)
    {
        var existingLink = _context.Links.FirstOrDefault((l) => l.SourceLink == link.SourceLink && l.User == link.User);
        
        if (existingLink is not null)
            return false;
        
        _context.Links.Add(link);
        _context.SaveChanges();
        return true;
    }

    public string? GetSourceLink(string shortCode)
    {
        var link = _context.Links.FirstOrDefault(l => l.ShortCode == shortCode);
        return link?.SourceLink;
    }

    public void IncrementCounter(string shortCode)
    {
        var link = _context.Links.First(l => l.ShortCode == shortCode);
        using (var transaction = _context.Database.BeginTransaction(IsolationLevel.RepeatableRead))
        {
            _context.Database.ExecuteSqlRaw("UPDATE public.\"Links\" SET \"Counter\" = \"Counter\" + 1 WHERE \"Id\" = {0}", link.Id);
            _context.SaveChanges();
            transaction.Commit();
        }
    }
}