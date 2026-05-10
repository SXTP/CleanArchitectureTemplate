using Microsoft.EntityFrameworkCore;
using Noname.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Noname.Infrastructure.Data.Configurations;

public static class SoftDeleteConfiguration
{
    public static void AddSoftDeleteFilter(this ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            // Eğer sınıf ISoftDelete arayüzünü implemente etmişse
            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            {
                // Arka planda: builder.Entity<SeninEntityn>().HasQueryFilter(m => !m.IsDeleted); işlemini yapar
                entityType.SetQueryFilter(GenerateSoftDeleteExpression(entityType.ClrType));
            }
        }
    }

    private static LambdaExpression GenerateSoftDeleteExpression(Type type)
    {
        var parameter = Expression.Parameter(type, "m");
        var property = Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
        var compare = Expression.Not(property);
        return Expression.Lambda(compare, parameter);
    }
}
