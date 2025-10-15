using System.Text.Json.Serialization;

public class ApiResponse<T>
{
    [JsonPropertyName("code")]
    public int Code { get; set; } = 1000;

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("result")]
    public T? Result { get; set; }

}
