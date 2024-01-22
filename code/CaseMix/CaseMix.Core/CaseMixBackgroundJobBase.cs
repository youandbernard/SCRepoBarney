using Abp.BackgroundJobs;
using Abp.Dependency;

namespace CaseMix
{
    public abstract class CaseMixBackgroundJobBase<TJobArgs> : BackgroundJob<TJobArgs>, ITransientDependency
    {
        public CaseMixBackgroundJobBase()
        {
            LocalizationSourceName = CaseMixConsts.LocalizationSourceName;
        }
    }
}
