// namespace [ProjectName].[Folder]
namespace HelloWorld.Models
{
  public class Computer
  {
    public int ComputerId { get; set; }
    public string? Motherboard
    { get; set; } = "default"; // better non-nullable check
    public int? CPUCores { get; set; } = 0;
    public bool? HasWifi { get; set; } = false;
    public bool? HasLTE { get; set; } = false;
    public DateTime? ReleaseDate { get; set; } = new DateTime();
    public string? VideoCard { get; set; } = "";
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