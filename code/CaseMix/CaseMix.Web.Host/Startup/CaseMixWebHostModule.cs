using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using CaseMix.Configuration;
using CaseMix.Core.Shared.Services;
using CaseMix.Aws.Email;
using CaseMix.Aws.S3.Services;
using CaseMix.Aws.OpenSearch.Services;
using CaseMix.Services.Dashboard;

namespace CaseMix.Web.Host.Startup
{
    [DependsOn(
       typeof(CaseMixWebCoreModule))]
    public class CaseMixWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public CaseMixWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CaseMixWebHostModule).GetAssembly());
            IocManager.Register<IEmailService, SESService>(Abp.Dependency.DependencyLifeStyle.Singleton);
            IocManager.Register<IS3Service, S3Service>(Abp.Dependency.DependencyLifeStyle.Singleton);
            IocManager.Register<IOpenSearchService, OpenSearchService>(Abp.Dependency.DependencyLifeStyle.Singleton);
            IocManager.Register<IDashboardAppService, DashboardAppService>(Abp.Dependency.DependencyLifeStyle.Singleton);

        }
    }
}
