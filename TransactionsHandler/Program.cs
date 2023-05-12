using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TransactionsHandler.Data;
using TransactionsHandler.Filters;
using TransactionsHandler.Services;

var builder = WebApplication.CreateBuilder(args);

var corsPolicy = "AllowAll";

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ICryptographyHelper, CryptographyHelper>();

#region Filters

builder.Services.AddScoped<HandleDecryptionAttribute>();
builder.Services.AddScoped<HandleEncryptionAttribute>();

#endregion

#region CORS

builder.Services.AddCors(options =>
    options.AddPolicy(corsPolicy, p =>
        p.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod())
    );

#endregion

#region Database

var connectionString = builder.Configuration.GetConnectionString("UsersDb");
builder.Services.AddDbContext<UsersContext>(options =>
    options.UseSqlServer(connectionString));

#endregion

#region Identity

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
})
    .AddEntityFrameworkStores<UsersContext>();

#endregion

#region Authentication

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Spectacular";
    options.DefaultChallengeScheme = "Spectacular";
}).AddJwtBearer("Spectacular", options =>
{
    string key = builder.Configuration.GetValue<string>("SecretKey") ?? string.Empty;
    var keyInBytes = Encoding.ASCII.GetBytes(key);
    var securityKey = new SymmetricSecurityKey(keyInBytes);

    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        IssuerSigningKey = securityKey,
        ValidateIssuer = false,
        ValidateAudience = false
    };

});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(corsPolicy);
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
