namespace FluentScheduler.UnitTests
{
    using Mocks;
    using System.Threading.Tasks;
    using Xunit;
    using static JobManager;
    using static System.Threading.Tasks.Task;
    using static Xunit.Assert;

    public class JobConstructionTests
    {
        [Fact]
        public async Task Should_Call_Ctor()
        {
            AddJob<CtorJob>(s => s.ToRunNow());
            await Delay(200);
            Equal(1, CtorJob.Calls);

            AddJob<CtorJob>(s => s.ToRunNow());
            await Delay(200);
            Equal(2, CtorJob.Calls);
        }

        [Fact]
        public async Task Should_Call_Dispose()
        {
            AddJob<DisposableJob>(s => s.ToRunNow());
            await Delay(200);
            True(DisposableJob.Disposed);
        }
    }
}
