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
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Create(
        [FromBody] CreateEmployeeDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!dto.Salary.HasValue)
                throw new ArgumentException("Salary is required.", nameof(dto.Salary));

            if (!dto.DateOfBirth.HasValue)
                throw new ArgumentException("Date of birth is required.", nameof(dto.DateOfBirth));

            var employee = new Employee(dto.FirstName, dto.LastName, dto.Salary.Value, dto.DateOfBirth.Value);
            var created = await employeeRepository.AddAsync(employee, cancellationToken);

            var result = new GetEmployeeDto
            {
                Id = created.Id,
                FirstName = created.FirstName,
                LastName = created.LastName,
                Salary = created.Salary,
                DateOfBirth = created.DateOfBirth
            };

            return Created(
                $"api/v1/employees/{created.Id}",
                new ApiResponse<GetEmployeeDto> { Data = result, Success = true });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ApiResponse<GetEmployeeDto> { Success = false, Error = ex.Message });
        }
    }
}
