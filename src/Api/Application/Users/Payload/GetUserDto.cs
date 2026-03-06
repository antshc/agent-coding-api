
namespace Api.Application.Users.Payload;

public class GetUserDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
