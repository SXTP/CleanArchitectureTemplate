using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Noname.Application.Common.Interfaces;
using Noname.Domain.Common;
using Noname.Domain.Entities;
using Noname.Infrastructure.Data.Configurations;
using Noname.Infrastructure.Identity;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Noname.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Member> Members { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //.IgnoreQueryFilters() kullanarak bu filtreleri geçersiz kılabilirsiniz.
            //Bu, belirli bir sorguda silinmiş öğeleri dahil etmek istediğiniz durumlarda yararlı olabilir.
            builder.AddSoftDeleteFilter();

            //IsDeleted sütununa global bir indeks ekleyerek, silinmiş olmayan kayıtların sorgulanmasını
            //hızlandırabilirsiniz. Bu, özellikle büyük veri kümeleriyle çalışırken performansı artırabilir.
            builder.AddGlobalIsDeletedIndexes();
        }
    }
}

