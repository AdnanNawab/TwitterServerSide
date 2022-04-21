using Dapper;
using Twitter.Utilities;
using User.Models;

namespace Twitter.Repositories;

public interface IUserRepository
{
    Task<Users> GetById(long Id);

    Task<Users> Create(Users Item);

    Task Update(Users Item);


    Task<Users> GetByEmail(string Email);



}
public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<Users> Create(Users Item)
    {
        var query = $@"INSERT INTO ""{TableNames.user}"" (name, email, password)
	VALUES (@Name, @Email, @Password) 
        RETURNING *";


        using (var con = NewConnection)
        {
            var res = await con.QuerySingleOrDefaultAsync<Users>(query, Item);
            return res;



        }


    }

    public async Task<Users> GetById(long Id)
    {
        var query = $@"SELECT * FROM ""{TableNames.user}"" WHERE id = @Id";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Users>(query, new { Id });

    }

    public async Task Update(Users Item)
    {
        var query = $@"UPDATE ""{TableNames.user}"" SET name = @Name WHERE id = @Id";
        // Update "user" set name='nithin' where id
        using (var con = NewConnection)
            await con.ExecuteAsync(query, Item);
    }

    public async Task<Users> GetByEmail(string Email)
    {

        var query = $@"SELECT * FROM ""{TableNames.user}""
        WHERE email = @Email";

        using (var con = NewConnection)
        {

            return await con.QuerySingleOrDefaultAsync<Users>(query, new { Email });
        }


        // }

        // public async Task<bool> Update(Users Item)
        // {
        //     var query = $@"UPDATE ""{TableNames.user}"" SET name = @Name,
        //      = @Mobile WHERE user_id = @UserId";

        //     using (var con = NewConnection)
        //     {
        //         var rowCount = await con.ExecuteAsync(query, Item);
        //         return rowCount == 1;
        //     }



    }
}
