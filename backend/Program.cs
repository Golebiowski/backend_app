using backend_app.Common.Behaviors;
using backend_app.Common.Interfaces;
using backend_app.Common.Mappings;
using backend_app.Common.Services;
using backend_app.Data;
using backend_app.Middleware;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.ConfigureApplicationCookie(options => {
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

//Password Configuration
builder.Services.Configure<IdentityOptions>(options =>
{ 
    options.Password.RequireDigit = false;        
    options.Password.RequiredLength = 4;              
    options.Password.RequireNonAlphanumeric = false;    
    options.Password.RequireUppercase = false;       
    options.Password.RequireLowercase = false;     
     
    options.User.RequireUniqueEmail = true;           
});

/*builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer();  */
 
builder.Services.AddHttpContextAccessor(); 

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 10, // Spróbuje 10 razy
            maxRetryDelay: TimeSpan.FromSeconds(5), // Co 5 sekund
            errorNumbersToAdd: null)
    ));

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddScoped(typeof(ICurrentUserService), typeof(CurrentUserService));




var app = builder.Build();

// 1. Obs³uga b³êdów na samym pocz¹tku
app.UseExceptionHandler();

// 2. Swagger - najlepiej mieæ go przed routinguem, ¿eby zawsze by³ dostêpny
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

// 3. Routing musi byæ przed Auth i Mapowaniem
app.UseRouting();

// 4. Autentykacja ZAWSZE przed Autoryzacj¹
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

// 5. Mapowanie tras
app.MapGroup("/identity").MapIdentityApi<IdentityUser>();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
     
    var context = services.GetRequiredService<AppDbContext>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
     
    await context.Database.EnsureCreatedAsync(); 
     
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }
     
    var adminEmail = "admin@todo.pl";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail };
          
        var result = await userManager.CreateAsync(adminUser, builder.Configuration["AdminConfig:Password"] ?? "Admin123!");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}

app.Run();
