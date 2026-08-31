using System;
using System.Globalization;
using Azure;
using Azure.Data.Tables;

namespace Finance.Data.TableStorage;

public abstract class PayrollTableEntity : ITableEntity
{
    public string PartitionKey { get; set; } = string.Empty;
    public string RowKey { get; set; } = string.Empty;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
    public int Id { get; set; }

    public void SetKeys(string partitionKey, int id)
    {
        PartitionKey = partitionKey;
        Id = id;
        RowKey = id.ToString("D10", CultureInfo.InvariantCulture);
    }

    internal static string Money(decimal value) => value.ToString("F2", CultureInfo.InvariantCulture);

    internal static decimal Money(string? value) =>
        decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var d) ? d : 0m;

    internal static DateTime ToUtc(DateTime value) => value.Kind switch
    {
        DateTimeKind.Utc => value,
        DateTimeKind.Local => value.ToUniversalTime(),
        _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
    };
}
