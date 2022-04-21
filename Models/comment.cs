namespace Twitter.Models;

public record Comment
{

    public long Id { get; set; }

    public string Text { get; set; }

    public int UserId { get; set; }

    public int TweetId { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

}