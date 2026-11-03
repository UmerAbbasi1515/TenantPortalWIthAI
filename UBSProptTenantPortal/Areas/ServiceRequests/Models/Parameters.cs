using System.Text.Json;

public static class SessionExtensions
{
    public static void SetObjectAsJson(this ISession session, string key, object value)
    {
        session.SetString(key, JsonSerializer.Serialize(value));
    }

    public static T? GetObjectFromJson<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default : JsonSerializer.Deserialize<T>(value);
    }
}

public class GetServiceRequestParameterN
{
    public string? type { get; set; }
    public string? fromDate { get; set; }
    public string? toDate { get; set; }
    public string? search { get; set; }
}

public class ServiceReqDetailparameterCaseNo
{
    public string? CaseNo { get; set; }
}