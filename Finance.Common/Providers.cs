namespace Finance.Common;

public interface IUsernameProvider
{
    string GetUsername();
}

public interface ITokenProvider
{
    string GetToken();
}

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}

public sealed class UtcDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
