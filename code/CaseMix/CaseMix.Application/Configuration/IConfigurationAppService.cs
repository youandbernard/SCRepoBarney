using System.Threading.Tasks;
using CaseMix.Configuration.Dto;

namespace CaseMix.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
