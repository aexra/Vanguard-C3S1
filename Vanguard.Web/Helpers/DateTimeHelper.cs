namespace Vanguard.Web.Helpers;
public static class DateTimeHelper
{
    public static DateTime Now => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));
}
