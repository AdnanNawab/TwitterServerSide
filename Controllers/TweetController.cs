
using System.Security.Claims;
using Twitter.DTOs;
using Twitter.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Twitter.Models;
using Twitter.Utilities;

namespace Twitter.Controllers;

[ApiController]
[Authorize]
[Route("api/Tweet")]
public class TweetController : ControllerBase
{
    private readonly ILogger<TweetController> _logger;

    private readonly ITweetRepository _tweet;

    public TweetController(ILogger<TweetController> logger, ITweetRepository tweet)
    {
        _logger = logger;
        _tweet = tweet;

    }

    private int GetUserIdFromClaims(IEnumerable<Claim> claims)
    {
        return Convert.ToInt32(claims.Where(x => x.Type == UserConstants.Id).First().Value);

    }

    [HttpPost]

    public async Task<ActionResult<Tweet>> CreateTweet([FromBody] TweetCreateDTO Data)
    {
        var UserId = GetUserIdFromClaims(User.Claims);



        var ToCreateItem = new Tweet
        {

            Title = Data.Title.Trim(),
            UserId = UserId,


        };

        var CreatedItem = await _tweet.Create(ToCreateItem);

        return StatusCode(201, CreatedItem);


    }


    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTweet([FromRoute] int id, [FromBody] TweetUpdateDTO Data)
    {
        var Id = GetUserIdFromClaims(User.Claims);

        var existingItem = await _tweet.GetById(id);

        if (existingItem is null)
            return NotFound();

        if (existingItem.UserId != Id)
            return StatusCode(404, "You cannot update other's Tweet");

        var toUpdateItem = existingItem with
        {
            Title = Data.Title is null ? existingItem.Title : Data.Title.Trim(),

        };

        await _tweet.Update(toUpdateItem);

        return NoContent();
    }


    [HttpDelete("{id}")]


    public async Task<ActionResult> DeleteTweet([FromRoute] int id)

    {
        var Id = GetUserIdFromClaims(User.Claims);

        var existingItem = await _tweet.GetById(id);

        if (existingItem is null)
            return NotFound();


        if (existingItem.UserId != Id)

            return StatusCode(403, "You cannot delete other's Tweet");




        await _tweet.Delete(id);

        return NoContent();
    }

    [HttpGet("{id}")]

    public async Task<ActionResult<Tweet>> GetById([FromRoute] int id)
    {
        var tweet = await _tweet.GetById(id);
        return Ok(tweet);

    }

    [HttpGet]
    public async Task<ActionResult> GetAllTweets()
    {
        var allTweet = await _tweet.GetAll();
        return Ok(allTweet);

    }







}