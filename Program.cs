using System.Text;
using AppBackEnd.Controllers;
using AppBackEnd.Data;
using AppBackEnd.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

string MyOrginsPolicy = "CORSPolicy";
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options=>
    options.AddPolicy(MyOrginsPolicy,
        builders=>
        {
            builders.AllowAnyMethod().AllowAnyHeader().AllowCredentials()
            // .AllowAnyOrigin();
            .WithOrigins(
            "http://localhost:3000","https://localhost:3000", "http://192.168.65.48:3000");
        }
));

builder.Services.AddDbContextPool<AppDbContext>(options =>
        {
            var connetionString = builder.Configuration.GetConnectionString("DefaultConnection");
            options.UseLazyLoadingProxies().UseMySql(connetionString, ServerVersion.AutoDetect(connetionString));
        });
builder.Services.AddIdentity<BiblioUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
    options.Password.RequiredUniqueChars = 0; //може всички знаци да се повтарят
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
// Add services to the container
builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swaggerGenOptions=>{
    swaggerGenOptions.SwaggerDoc("v1",new OpenApiInfo{Title = "Asp.Net React MySQl",Version="v1"});

    swaggerGenOptions.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    swaggerGenOptions.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.FromMinutes(0),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenSettings"]))
    };
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(swaggerUIOptions=>
{
    swaggerUIOptions.DocumentTitle = "ASP.NET React MySQL";
    swaggerUIOptions.SwaggerEndpoint("swagger/v1/swagger.json","Web Api for books");
    swaggerUIOptions.RoutePrefix= string.Empty;
});
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors(MyOrginsPolicy);
app.MapControllers();
app.Run();
