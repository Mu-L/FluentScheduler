namespace FluentScheduler;

using NCrontab;
using System;

internal class CronTimeCalculator : INextRunCalculator
{
    private readonly CrontabSchedule _calculator;

    public void UseUtc() => ((INextRunCalculator)this).Now = () => DateTime.UtcNow;

    Func<DateTime> INextRunCalculator.Now { get; set; } = () => DateTime.Now;

    internal CronTimeCalculator(string cronExpression)
    {
        var cronFields = cronExpression.Split(StringSeparatorStock.Space, StringSplitOptions.RemoveEmptyEntries).Length;
        var parseOptions = new CrontabSchedule.ParseOptions
        {
            IncludingSeconds = cronFields == 6
        };

        _calculator = CrontabSchedule.Parse(cronExpression, parseOptions);
    }

    DateTime? INextRunCalculator.Calculate(DateTime last) => _calculator.GetNextOccurrence(last);

    void INextRunCalculator.Reset() { }
}