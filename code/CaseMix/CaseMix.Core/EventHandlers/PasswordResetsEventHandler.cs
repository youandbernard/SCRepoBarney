using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using CaseMix.BackgroundJobs;
using CaseMix.BackgroundJobs.JobArgs;
using CaseMix.Entities;
using System.Threading.Tasks;

namespace CaseMix.EventHandlers
{
    public class PasswordResetsEventHandler :
        IAsyncEventHandler<EntityCreatedEventData<PasswordReset>>,
        ITransientDependency
    {
        private readonly IBackgroundJobManager _backgroundJobManager;

        public PasswordResetsEventHandler(IBackgroundJobManager backgroundJobManager)
        {
            _backgroundJobManager = backgroundJobManager;
        }

        public async Task HandleEventAsync(EntityCreatedEventData<PasswordReset> eventData)
        {
            await _backgroundJobManager.EnqueueAsync<SendPasswordResetEmailJob, SendPasswordResetEmailJobArgs>(new SendPasswordResetEmailJobArgs()
            {
                PasswordResetId = eventData.Entity.Id,
                Email = eventData.Entity.Email,
            });
        }
    }
}
