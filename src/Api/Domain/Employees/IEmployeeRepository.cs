namespace Api.Domain.Employees;

public interface IEmployeeRepository
{
    Task<Employee> AddAsync(Employee employee, CancellationToken cancellationToken);
}
