using Twitter.Models;
using Twitter.Repositories;
using Microsoft.AspNetCore.Mvc;
using Twitter.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Twitter.Utilities;

namespace Twitter.Controllers;

[ApiController]
[Authorize]
[Route("api/comment")]
public class CommentController : ControllerBase
{
    private readonly ILogger<CommentController> _logger;
    private readonly ICommentRepository _comment;

    public CommentController(ILogger<CommentController> logger,
    ICommentRepository comment)
    {
        _logger = logger;
        _comment = comment;
    }

    private int GetUserIdFromClaims(IEnumerable<Claim> claims)
    {
        return Convert.ToInt32(claims.Where(x => x.Type == UserConstants.Id).First().Value);
    }

    [HttpPost("{tweet_id}")]
    public async Task<ActionResult<Comment>> CreateComment([FromRoute] int tweet_id, [FromBody] CommentCreateDTO Data)
    {
        var userId = GetUserIdFromClaims(User.Claims);

        var toCreateItem = new Comment
        {
            Text = Data.Text.Trim(),
            UserId = userId,
            TweetId = tweet_id

        };


        var createdItem = await _comment.Create(toCreateItem);


        return StatusCode(201, createdItem);
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteComment([FromRoute] int id)
    {
        var userId = GetUserIdFromClaims(User.Claims);

        var existingItem = await _comment.GetById(id);

        if (existingItem is null)
            return NotFound();

        if (existingItem.UserId != userId)
            return StatusCode(403, "You cannot delete other's Comment");

        await _comment.Delete(id);

        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<List<Comment>>> GetAllComments([FromQuery] int id)
    {
        var allComment = await _comment.GetAll(id);
        return Ok(allComment);
    }
}