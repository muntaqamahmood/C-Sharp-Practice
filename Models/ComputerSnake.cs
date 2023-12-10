// namespace [ProjectName].[Folder]
namespace HelloWorld.Models
{
  public class ComputerSnake
  {
#pragma warning disable IDE1006 // Naming Styles
    public int computer_id { get; set; }
    public string? motherboard
    { get; set; } = "default"; // better non-nullable check
    public int? cpu_cores { get; set; } = 0;
    public bool? has_wifi { get; set; } = false;
    public bool? has_lte { get; set; } = false;
    public DateTime? release_date { get; set; } = new DateTime();
    public string? video_card { get; set; } = "";
    public decimal? price { get; set; } = -1;
#pragma warning restore IDE1006 // Naming Styles

    // public ComputerSnake()
    // {   //non-nullable check
    //     if (Motherboard == null)
    //     {
    //         Motherboard = "";
    //     }
    //     RAM ??= "";
    // }

  }
}