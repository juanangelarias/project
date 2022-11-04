using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CM.Database.Helpers;

public interface IDateConverterService
{
    ValueConverter<DateTime, DateTime> DateConverter();
    ValueConverter<DateTime?, DateTime?> NullableDateConverter();
}