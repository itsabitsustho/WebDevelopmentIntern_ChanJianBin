
global using WebAsm.Models;
global using WebAsm;
using reCAPTCHA.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRecaptcha(builder.Configuration.GetSection("reCAPTCHA"));

//reCAPTCHA configuration
builder.Services.Configure<ReCaptchaSettings>(builder.Configuration.GetSection("reCAPTCHA"));

//Register services
builder.Services.AddControllersWithViews();
builder.Services.AddSqlServer<DB>($@"
    Data Source=(LocalDB)\MSSQLLocalDB;
    AttachDbFilename={builder.Environment.ContentRootPath}\DB.mdf;
");
builder.Services.AddScoped<Helper>();
builder.Services.AddAuthentication().AddCookie();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddRecaptcha(builder.Configuration.GetSection("reCAPTCHA"));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapDefaultControllerRoute();
app.UseSession();
app.Run();
