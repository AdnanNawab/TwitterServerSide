using Twitter.DTOs;

namespace User.Models;

public record Users
{

    public int Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }


     public UserDTO asDto => new UserDTO
    {
          Id = Id,
        Name = Name,
        Password = Password,
        Email = Email,


    };


}