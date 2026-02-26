namespace BackendAPI.Models;

public partial class Pokemon
{
    public int DexId { get; set; }
    public string Name { get; set; } = null!;
    public int? Votes { get; set; }
    public int? EloRating { get; set; }
}
