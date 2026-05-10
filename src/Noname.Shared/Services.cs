namespace Noname.Shared;

public static class Services
{
    public const string Database = "NonameDb";

    // DateTime -> long (Epoch)
    public static long ToEpochMilliseconds(this DateTime dateTime)
    {
        return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
    }

    // long (Epoch) -> DateTime
    public static DateTime FromEpochMilliseconds(this long milliseconds)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(milliseconds).UtcDateTime;
    }
}
