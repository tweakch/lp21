using System.Text.Json.Serialization;

namespace CMI.Crawler.Lehrplan21.Models;

public class ApiResponse
{
    [JsonPropertyName("lp21")]
    public List<Lp21> Lp21 { get; set; }
}