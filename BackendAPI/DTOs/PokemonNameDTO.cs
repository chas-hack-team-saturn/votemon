namespace BackendAPI.DTOs
{
    /// <summary>
    ///     This class is used when getting Pokemon names from PokeAPI.
    /// </summary>
    public class PokemonNameDTO
    {
        [System.Text.Json.Serialization.JsonPropertyName("name")]
        public string Name { get; set; } = null!;
    }
}
