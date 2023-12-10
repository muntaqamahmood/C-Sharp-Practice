// namespace [ProjectName].[Folder]
using System.Text.Json.Serialization;

namespace HelloWorld.Models
{
  public class Computer
  {
    [JsonPropertyName("computer_id")]
    public int ComputerId { get; set; }
    [JsonPropertyName("motherboard")]
    public string? Motherboard
    { get; set; } = "default"; // better non-nullable check
    [JsonPropertyName("cpu_cores")]
    public int? CPUCores { get; set; } = 0;
    [JsonPropertyName("has_wifi")]
    public bool? HasWifi { get; set; } = false;
    [JsonPropertyName("has_lte")]
    public bool? HasLTE { get; set; } = false;
    [JsonPropertyName("release_date")]
    public DateTime? ReleaseDate { get; set; } = new DateTime();
    [JsonPropertyName("video_card")]
    public string? VideoCard { get; set; } = "";
    [JsonPropertyName("price")]
    public decimal? Price { get; set; } = -1;

    // public Computer()
    // {   //non-nullable check
    //     if (Motherboard == null)
    //     {
    //         Motherboard = "";
    //     }
    //     RAM ??= "";
    // }

  }
}