using System.Collections.Generic;
using Abp.Configuration;
using Microsoft.Extensions.Configuration;

namespace CaseMix.Configuration
{
    public class AppSettingProvider : SettingProvider
    {
        protected IConfigurationRoot _appConfiguration;

        public AppSettingProvider(IAppConfigurationAccessor configurationAccessor)
        {
            _appConfiguration = configurationAccessor.Configuration;
        }

        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition(AppSettingNames.UiTheme, "red", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true),
                new SettingDefinition(AppSettingNames.App_ClientRootAddress, GetFromSettings("App:ClientRootAddress")),
                new SettingDefinition(AppSettingNames.Aws_OpenSearchDomain, GetFromSettings("Aws:OpenSearchDomain")),
                new SettingDefinition(AppSettingNames.Aws_MasterUser, GetFromSettings("Aws:MasterUser")),
                new SettingDefinition(AppSettingNames.Aws_MasterPassword, GetFromSettings("Aws:MasterPassword"))
            };
        }

        private string GetFromSettings(string name, string defaultValue = null)
        {
            return _appConfiguration[name] ?? defaultValue;
        }
    }
}
