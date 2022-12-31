using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Passwordmanager.Data;
using Passwordmanager.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string cs = builder.Configuration.GetConnectionString("Str");
builder.Services.AddDbContext<ApplicationDbContext>
    (option => option.UseSqlServer(cs));
builder.Services.AddTransient<ITokenService, TokenService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AJ",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000/")
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
        });

});

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
//        options.SlidingExpiration = true;
//        options.AccessDeniedPath = "/Forbidden/";
//    });

builder.Services.AddControllers();





//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = "JWT_OR_COOKIE";
//    options.DefaultChallengeScheme = "JWT_OR_COOKIE";
//})
//.AddCookie(options =>
//{
//    options.LoginPath = "/login";
//    options.ExpireTimeSpan = TimeSpan.FromMinutes(2);
//})

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };


});

//.AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
//{
//    options.ForwardDefaultSelector = context =>
//    {
//        string authorization = context.Request.Headers[HeaderNames.Authorization];
//        if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
//            return JwtBearerDefaults.AuthenticationScheme;

//        return CookieAuthenticationDefaults.AuthenticationScheme;
//    };
//});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AJ");
app.UseHttpsRedirection();
app.UseAuthentication();
//app.UseCookiePolicy();

app.UseAuthorization();

//app.Use(async (context, next) =>
//{
//    var token = context.Request.Cookies["access_token"];
//    if (!string.IsNullOrEmpty(token)) context.Request.Headers.Add("Authorization", "Bearer " + token);
//    await next();
//});

app.MapControllers();

app.Run();
