namespace LaXiS.MyLibrary.Jobs
{
    public class JobManager
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly List<Task> _jobs = new();

        public JobManager(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        // TODO maybe add an overload that accepts an IJobOptions to pass to the job
        public void Launch<T>()
            where T : IJob
        {
            var job = _serviceProvider.GetRequiredService<T>();
            _jobs.Add(Task.Run(job.ExecuteAsync));
        }

        [Obsolete]
        public TaskStatus? GetLastJobStatus()
        {
            return _jobs.FirstOrDefault()?.Status;
        }
    }
}
