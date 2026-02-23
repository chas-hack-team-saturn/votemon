namespace BackendAPI.DTOs
{
    public class PokemonNameDTO
    {
        [System.Text.Json.Serialization.JsonPropertyName("name")]
        public string Name { get; set; } = null!;
    }
}
