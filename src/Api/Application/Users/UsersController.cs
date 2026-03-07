using Api.Application.Users.Payload;
using Api.Application.Users.Queries;
using Api.SharedKernel.Payload;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Application.Users;

[ApiController]
[Route("[controller]")]
public class UsersController(IUserQuery userQuery) : ControllerBase
{
    [SwaggerOperation(Summary = "Create user")]
    [HttpPost]
    public ActionResult Create(CreateUserDto userDto, CancellationToken _) => Created("", userDto);

    [SwaggerOperation(Summary = "Get user by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetUserDto>>> Get(int id, CancellationToken cancellationToken)
    {
        try
        {
            var emp = await userQuery.GetById(id, cancellationToken);

            var result = new ApiResponse<GetUserDto>
            {
                Data = emp,
                Success = true
            };
            return result;
        }
        catch (ApplicationException e) when (e.Message == "User not found")
        {
            return NotFound(new ApiResponse<GetUserDto>
            {
                Success = false,
                Message = e.Message
            });
        }
    }

    [SwaggerOperation(Summary = "Get all users")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<GetUserDto>>>> GetAll(CancellationToken cancellationToken)
    {
        //task: use a more realistic production approach
        IReadOnlyCollection<GetUserDto> employees = await userQuery.GetAll(cancellationToken);

        var result = new ApiResponse<IReadOnlyCollection<GetUserDto>>
        {
            Data = employees,
            Success = true
        };

        return result;
    }
}
