using Api.Domain.Employees;

namespace Api.Data;

public class EmployeeRepository(DataContext context) : IEmployeeRepository
{
    public async Task<Employee> AddAsync(Employee employee, CancellationToken cancellationToken)
    {
        await context.Employees.AddAsync(employee, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return employee;
    }
}
