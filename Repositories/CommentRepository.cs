using Twitter.Models;
using Dapper;
using Twitter.Utilities;

namespace Twitter.Repositories;

public interface ICommentRepository
{
    Task<Comment> Create(Comment Item);

    Task Delete(int Id);
    Task<List<Comment>> GetAll(int id);
    Task<Comment> GetById(int Id);
}

public class CommentRepository : BaseRepository, ICommentRepository
{
    public CommentRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<Comment> Create(Comment Item)
    {
        var query = $@"INSERT INTO {TableNames.comment} (text, user_id,tweet_id) 
        VALUES (@Text, @UserId, @TweetId) RETURNING *";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Comment>(query, Item);
    }

    public async Task Delete(int Id)
    {
        var query = $@"DELETE FROM {TableNames.comment} WHERE id = @Id";

        using (var con = NewConnection)
            await con.ExecuteAsync(query, new { Id });
    }

    // public async Task<List<Comment>> GetAll()
    // {
    //     var query = $@"SELECT * FROM {TableNames.comment} ORDER BY created_at DESC";

    //     using (var con = NewConnection)
    //         return (await con.QueryAsync<Comment>(query)).AsList();
    // }

    public async Task<List<Comment>> GetAll(int id)
    {
        var query = $@"SELECT * FROM {TableNames.comment} ORDER BY created_at DESC";

        using (var con = NewConnection)
            return (await con.QueryAsync<Comment>(query)).AsList();
    }

    public async Task<Comment> GetById(int Id)
    {
        var query = $@"SELECT * FROM {TableNames.comment} WHERE id = @Id";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Comment>(query, new { Id });
    }


}