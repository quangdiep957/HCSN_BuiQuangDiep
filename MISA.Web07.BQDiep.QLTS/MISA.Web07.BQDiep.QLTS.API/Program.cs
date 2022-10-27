
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MISA.QLSP.Common.Entities.Entities;
using MISA.QLTS.BL;
using MISA.QLTS.BL.BaseBL;
using MISA.QLTS.DL;
using MISA.QLTS.DL.FixedAssetCategoryDL;
using MISA.Web07.BQDiep.QLTS.API.Middleware;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMemoryCache();
// Add services to the container.
builder.Services.AddScoped(typeof(IBaseDL<>), typeof(BaseDL<>));
builder.Services.AddScoped(typeof(IBaseBL<>), typeof(BaseBL<>));
builder.Services.AddScoped<IAssetBL, AssetBL>();
builder.Services.AddScoped<IAssetDL, AssetDL>();
builder.Services.AddScoped<IUserBL, UserBL>();
builder.Services.AddScoped<IUserDL, UserDL>();

builder.Services.AddScoped<IDepartmentBL, DepartmentBL>();
builder.Services.AddScoped<IDepartmentDL, DepartmentDL>();
builder.Services.AddScoped<IFixedAssetIncrementBL, FixedAssetIncrementBL>();
builder.Services.AddScoped<IFixedAssetIncrementDL, FixedAssetIncrementDL>();
builder.Services.AddScoped<IFixedAssetCategoryBL, FixedAssetCategoryBL>();
builder.Services.AddScoped<IFixedAssetCategoryDL, FixedAssetCategoryDL>();
builder.Services.AddScoped<IBudgetBL, BudgetBL>();
builder.Services.AddScoped<IBudgetDL,BudgetDL>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
//services cors
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

//builder.Services.Configure<CookiePolicyOptions>(options =>
//{
//    options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
//    options.OnAppendCookie = cookieContext =>
//        CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
//    options.OnDeleteCookie = cookieContext =>
//        CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
//});

//void CheckSameSite(HttpContext httpContext, CookieOptions options)
//{
//    if (options.SameSite == SameSiteMode.None)
//    {
//        var name = "login";
//        var cookie = httpContext.Request.Cookies[name];

//        if (cookie != null)
//            if (!httpContext.Request.Headers.ContainsKey("Authorization"))
//            options.SameSite = SameSiteMode.Unspecified;
        
//    }
//}
//builder.Services.AddCors(options => options.AddPolicy(name: "NgOrigins",
//    policy =>
//    {
//        policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
//    }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

DatabaseContext.ConnectionString = builder.Configuration.GetConnectionString("MySqlConnection");




//app.UseMiddleware<SimpleMiddleware>();
//app cors
app.UseCors("corsapp");
app.UseCors("NgOrigins");

app.UseHttpsRedirection();

//app.UseMiddleware<SimpleMiddleware>();
//app.UseCookiePolicy();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
