using Microsoft.Extensions.Configuration;

namespace CaseMix.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
