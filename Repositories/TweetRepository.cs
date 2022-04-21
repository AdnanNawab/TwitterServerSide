using Dapper;
using Twitter.Models;
using Twitter.Utilities;

namespace Twitter.Repositories;

public interface ITweetRepository
{
    Task<Tweet> Create(Tweet Item);

    Task Update(Tweet Item);

    Task Delete(int Id);

    Task<List<Tweet>> GetAll();

    Task<Tweet> GetById(int TweetId);
}
public class TweetRepository : BaseRepository, ITweetRepository
{
    public TweetRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<Tweet> Create(Tweet Item)
    {
        var query = $@"INSERT INTO ""{TableNames.tweet}"" (title, user_id)
        VALUES (@Title, @UserId) Returning *";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Tweet>(query, Item);
    }

    public async Task Delete(int Id)
    {
        var query = $@"DELETE FROM ""{TableNames.tweet}"" WHERE id = @Id";

        using (var con = NewConnection)
            await con.ExecuteAsync(query, new { Id });
    }

    public async Task<List<Tweet>> GetAll()
    {
        var query = $@"SELECT * FROM ""{TableNames.tweet}"" ORDER BY created_at DESC";

        using (var con = NewConnection)
            return (await con.QueryAsync<Tweet>(query)).AsList();
    }

    public async Task<Tweet> GetById(int Id)
    {
        var query = $@"SELECT * FROM ""{TableNames.tweet}"" WHERE id = @Id";

        using (var con = NewConnection)
            return await con.QueryFirstOrDefaultAsync<Tweet>(query, new { Id });
    }

    public async Task Update(Tweet Item)
    {
        var query = $@"UPDATE ""{TableNames.tweet}"" SET title = @Title WHERE id = @Id";

        using (var con = NewConnection)
            await con.ExecuteAsync(query, Item);
    }
}