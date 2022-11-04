using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CM.Database.Helpers;

public class DateConverterService: IDateConverterService
{
    public ValueConverter<DateTime, DateTime> DateConverter()
    {
        var output = new ValueConverter<DateTime, DateTime>(
            v => v.Kind == DateTimeKind.Unspecified || v.Kind == DateTimeKind.Local
                ? v.ToUniversalTime()
                : v,
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        return output;
    }

    public ValueConverter<DateTime?, DateTime?> NullableDateConverter()
    {
        var output = new ValueConverter<DateTime?, DateTime?>(
            v => v.HasValue
                ? v.Value.Kind == DateTimeKind.Unspecified || v.Value.Kind == DateTimeKind.Local
                    ? v.Value.ToUniversalTime()
                    : v
                : v,
            v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

        return output;
    }
}