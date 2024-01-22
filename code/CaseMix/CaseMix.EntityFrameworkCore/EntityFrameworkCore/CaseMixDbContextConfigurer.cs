using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace CaseMix.EntityFrameworkCore
{
    public static class CaseMixDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<CaseMixDbContext> builder, string connectionString)
        {
            builder.UseMySql(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<CaseMixDbContext> builder, DbConnection connection)
        {
            builder.UseMySql(connection);
        }
    }
}
