using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using CaseMix.Configuration;
using CaseMix.Web;

namespace CaseMix.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class CaseMixDbContextFactory : IDesignTimeDbContextFactory<CaseMixDbContext>
    {
        public CaseMixDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<CaseMixDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            CaseMixDbContextConfigurer.Configure(builder, configuration.GetConnectionString(CaseMixConsts.ConnectionStringName));

            return new CaseMixDbContext(builder.Options);
        }
    }
}
