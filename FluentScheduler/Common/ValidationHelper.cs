using System;
using System.Runtime.CompilerServices;

namespace FluentScheduler;

internal class ValidationHelper
{
    internal static void ThrowIfNotDefinedInEnum<T>(
        T value, [CallerArgumentExpression(nameof(value))] string paramName = null) where T : struct, Enum
    {
        if (!Enum.IsDefined(value))
            throw new ArgumentOutOfRangeException(paramName, value, "Enumeration value out of range.");
    }
    internal static void ThrowIfNotDefinedInEnum<T>(
        T[] values, [CallerArgumentExpression(nameof(values))] string paramName = null) where T : struct, Enum
    {
        ArgumentNullException.ThrowIfNull(values);

        Array.ForEach(values, v => ThrowIfNotDefinedInEnum(v, paramName));
    }

    internal static void ThrowIfNegative(
        TimeSpan value, [CallerArgumentExpression(nameof(value))] string paramName = null)
    {
        if (value < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(paramName, value, "The given time must be a non-negative value.");
    }

    internal static void ThrowIfOutOfMilitaryTimeRange(
        int hour,
        int minute,
        [CallerArgumentExpression(nameof(hour))] string hourParamName = null,
        [CallerArgumentExpression(nameof(minute))] string minuteParamName = null)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(hour, hourParamName);
        ArgumentOutOfRangeException.ThrowIfNegative(minute, minuteParamName);

        ArgumentOutOfRangeException.ThrowIfGreaterThan(hour, 23, hourParamName);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(minute, 59, minuteParamName);
    }

    internal static void ThrowIfOutOfMilitaryTimeRange(
        TimeSpan value, [CallerArgumentExpression(nameof(value))] string paramName = null)
    {
        ThrowIfNegative(value);

        ArgumentOutOfRangeException.ThrowIfGreaterThan(value.Hours, 23, $"{paramName}.Hours");
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value.Minutes, 59, $"{paramName}.Minutes");
    }

    internal static void ThrowIfOutOfMilitaryTimeRange(
        TimeSpan[] values, [CallerArgumentExpression(nameof(values))] string paramName = null)
    {
        ArgumentNullException.ThrowIfNull(values);

        Array.ForEach(values, v => ThrowIfOutOfMilitaryTimeRange(v, paramName));
    }
}
