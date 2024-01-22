using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using CaseMix.Authorization;

namespace CaseMix
{
    [DependsOn(
        typeof(CaseMixCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class CaseMixApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<CaseMixAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(CaseMixApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
