namespace Api.Application.Employees.Payload;

public class CreateEmployeeDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public decimal? Salary { get; set; }
    public DateOnly? DateOfBirth { get; set; }
}
