using SecureCodingWebshop.Data;
using SecureCodingWebshop.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromDays(730);
    options.IncludeSubDomains = true;
});
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<CartService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddSingleton<LogService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.Use(async (context, next) =>
{
    var allowedMethods = new HashSet<string> { "GET", "POST", "HEAD" };

    if (!allowedMethods.Contains(context.Request.Method))
    {
        context.Response.StatusCode = 405;
        await context.Response.WriteAsync("Method Not Allowed");
        return;
    }

    await next();
});
app.Use(async (context, next) =>
{
    if (context.Request.Method == "POST")
    {
        string contentType = context.Request.ContentType ?? "";

        if (!contentType.StartsWith(
            "application/x-www-form-urlencoded",
            StringComparison.OrdinalIgnoreCase) &&
            !contentType.StartsWith(
            "multipart/form-data",
            StringComparison.OrdinalIgnoreCase))
        {
            context.Response.StatusCode = 415;
            await context.Response.WriteAsync("Unsupported Media Type");
            return;
        }
    }

    await next();
});
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";

    await next();
});
app.Use(async (context, next) =>
{
    context.Response.Headers["Content-Security-Policy"] =
        "default-src 'self'; " +
        "script-src 'self'; " +
        "object-src 'none'; " +
        "frame-ancestors 'none'; " +
        "base-uri 'self';";

    await next();
});

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
