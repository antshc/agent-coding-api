namespace Api.SharedKernel;

public interface IDateTimeProvider
{
    DateTime Now { get; }
}