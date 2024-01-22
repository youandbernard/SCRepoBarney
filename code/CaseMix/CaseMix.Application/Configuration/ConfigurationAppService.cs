using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using CaseMix.Configuration.Dto;

namespace CaseMix.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : CaseMixAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
