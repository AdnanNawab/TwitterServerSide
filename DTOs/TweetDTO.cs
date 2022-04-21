using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Twitter.DTOs;



public record TweetItemDTO
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("user_id")]
    public long UserId { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }




}

public record TweetCreateDTO
{

    [Required]
    [MinLength(3)]
    [MaxLength(255)]
    public string Title { get; set; }

    [Required]
    public int UserId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}

public record TweetUpdateDTO
{

    [Required]

    [MinLength(3)]
    [MaxLength(255)]
    public string Title { get; set; }

    // [Required]
    public DateTimeOffset UpdatedAt { get; set; }

    // [Required]
    // public int TweetId { get; set; }


}