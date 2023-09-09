using LinkShortener.Database;
using LinkShortener.Handlers;
using LinkShortener.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<LinkService>();

builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromSeconds(10);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});
 
var app = builder.Build();

app.MapRazorPages();
app.MapGet("/{shortCode?}", LinksHandler.OnGet);
 
app.UseSession();
app.UseAuthorization();
app.UseRouting();

app.Run();