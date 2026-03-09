namespace Api.Domain.Employees;

public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Employee>> GetAllAsync(CancellationToken cancellationToken);
    Task AddAsync(Employee employee, CancellationToken cancellationToken);
}
