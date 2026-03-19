using Api.Application.Employees.Payload;
using Api.Domain.Employees;
using Api.SharedKernel.Payload;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Application.Employees;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController(IEmployeeRepository employeeRepository) : ControllerBase
{
    [SwaggerOperation(Summary = "Create employee")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Create(CreateEmployeeDto dto, CancellationToken cancellationToken)
    {
        if (dto.Salary is null)
            return BadRequest(new ApiResponse<GetEmployeeDto> { Success = false, Error = "Salary is required." });

        if (dto.DateOfBirth is null)
            return BadRequest(new ApiResponse<GetEmployeeDto> { Success = false, Error = "Date of birth is required." });

        try
        {
            var employee = new Employee(dto.FirstName, dto.LastName, dto.Salary.Value, dto.DateOfBirth.Value);
            var created = await employeeRepository.AddAsync(employee, cancellationToken);

            var result = new ApiResponse<GetEmployeeDto>
            {
                Success = true,
                Data = new GetEmployeeDto
                {
                    Id = created.Id,
                    FirstName = created.FirstName,
                    LastName = created.LastName,
                    Salary = created.Salary,
                    DateOfBirth = created.DateOfBirth
                }
            };

            return Created($"/api/v1/employees/{created.Id}", result);
        }
        catch (ArgumentException e)
        {
            return BadRequest(new ApiResponse<GetEmployeeDto> { Success = false, Error = e.Message });
        }
    }
}
