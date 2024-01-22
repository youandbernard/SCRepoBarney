using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace CaseMix.Localization
{
    public static class CaseMixLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(CaseMixConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(CaseMixLocalizationConfigurer).GetAssembly(),
                        "CaseMix.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
