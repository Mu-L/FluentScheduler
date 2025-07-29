namespace FluentScheduler.UnitTests
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static System.Threading.Tasks.Task;
    using static Xunit.Assert;

    public class RemoveTests
    {
        [Fact]
        public void Should_Remove_Named_Job()
        {
            // Act
            var schedule = new Schedule(() => { }).WithName("remove named job");
            schedule.ToRunNow().AndEvery(1).Seconds();
            JobManager.RemoveJob("remove named job");

            // Assert
            Null(JobManager.GetSchedule("remove named job"));
        }

        [Fact]
        public void Should_Remove_All_Jobs()
        {
            // Act
            JobManager.AddJob(() => { }, s => s.ToRunNow());
            JobManager.AddJob(() => { }, s => s.ToRunNow());
            JobManager.AddJob(() => { }, s => s.ToRunNow());
            JobManager.AddJob(() => { }, s => s.ToRunNow());
            JobManager.AddJob(() => { }, s => s.ToRunNow());
            JobManager.AddJob(() => { }, s => s.ToRunNow());

            JobManager.RemoveAllJobs();

            // Assert
            True(JobManager.AllSchedules.Count() == 0);
        }

        [Fact]
        public async Task Should_Remove_LongRunning_Job_But_Keep_Running()
        {
            // Act
            var schedule = new Schedule(() => Thread.Sleep(100));
            schedule.WithName("remove long running job").ToRunNow().AndEvery(2).Seconds();
            schedule.Execute();

            // Assert
            Contains(JobManager.RunningSchedules, s => s.Name == "remove long running job");

            // Act
            JobManager.RemoveJob("remove long running job");

            // Assert
            Null(JobManager.GetSchedule("remove long running job"));
            Contains(JobManager.RunningSchedules, s => s.Name == "remove long running job");
            await Delay(2000);
            DoesNotContain(JobManager.RunningSchedules, s => s.Name == "remove long running job");
        }
    }
}
