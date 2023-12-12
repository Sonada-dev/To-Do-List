using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Refit;
using System.Text;
using To_Do_List.Web;
using To_Do_List.Web.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddRefitClient<IToDoApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://172.27.107.88/api"));

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:KeyToken").Value!))
        };
    });

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "Account/Login";
});


builder.Services.AddControllersWithViews();

var app = builder.Build();

app.Use(async (context, next) =>
{
    if (context.Request.Cookies.TryGetValue("JwtToken", out string? token))
        context.Request.Headers.Authorization = $"Bearer {token}";
    await next();
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
