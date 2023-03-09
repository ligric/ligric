using Autofac;
using Quartz;
using Quartz.Spi;

namespace Ligric.Server.Infrastructure.Quartz
{
    public class JobFactory : IJobFactory
    {
        private readonly IContainer _container;

        public JobFactory(IContainer container)
        {
            this._container = container;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var job = _container.Resolve(bundle.JobDetail.JobType);

#pragma warning disable CS8603 // Possible null reference return.
			return job as IJob;
#pragma warning restore CS8603 // Possible null reference return.
		}

        public void ReturnJob(IJob job)
        {
        }
    }
}
