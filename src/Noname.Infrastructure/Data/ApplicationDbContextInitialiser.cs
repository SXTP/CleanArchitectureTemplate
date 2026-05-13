using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Noname.Domain.Entities;
using Noname.Domain.Enums;
using Noname.Infrastructure.Identity;

namespace Noname.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger,
                                            ApplicationDbContext context,
                                            UserManager<AppUser> userManager,
                                            RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            //DİKKAT! ÖNEMLİ! ERROR! EYVAH! İMDAAATTT!
            //EnsureDeletedAsync ve EnsureCreatedAsync metotları, veritabanını sıfırlamak ve yeniden oluşturmak
            //için kullanılır. Bu işlemler, mevcut verilerin kaybolmasına neden olabilir.
            //Bu nedenle, bu metotları üretim ortamlarında kullanmaktan kaçınmalısınız. Sadece geliştirme veya
            //test ortamlarında kullanmanız önerilir.
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var administratorRole = new IdentityRole("Admin");

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        // Default users
        var administrator = new AppUser 
        { 
            UserName = "admin@local.com", 
            Email = "admin@local.com",
            Member = new Member
            {
                FirstName = "Admin",
                LastName = "User",
                Status = EntityStatus.Active
            }
        };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            var res = await _userManager.CreateAsync(administrator, "Admin123!");
            if (res.Succeeded && !string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }
        }

        await _context.SaveChangesAsync();
    }
}
