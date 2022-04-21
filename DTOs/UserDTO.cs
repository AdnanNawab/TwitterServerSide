using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Twitter.DTOs;

public record UserDTO
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }

}

public record UserCreateDTO
{
    [JsonPropertyName("name")]
    [Required]
    [MaxLength(255)]
    public string Name { get; set; }

    [JsonPropertyName("password")]
    [MaxLength(255)]
    [Required]
    public string Password { get; set; }

    [JsonPropertyName("email")]
    [MaxLength(255)]
    public string Email { get; set; }


}

public record UserUpdateDTO
{
    [JsonPropertyName("name")]
    [MaxLength(255)]
    public string Name { get; set; }


}
public record UserLoginDTO
{
    [Required]
    [JsonPropertyName("email")]
    [MinLength(3)]
    [MaxLength(255)]

    public string Email { get; set; }

    [Required]
    [JsonPropertyName("password")]

    [MaxLength(255)]

    public string Password { get; set; }
}

public record UserLoginResDTO
{
    [JsonPropertyName("token")]
    public string Token { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }
}