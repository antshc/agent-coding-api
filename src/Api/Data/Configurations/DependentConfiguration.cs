using Api.Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Configurations;

public class DependentConfiguration : IEntityTypeConfiguration<Dependent>
{
    public void Configure(EntityTypeBuilder<Dependent> builder)
    {
        builder.ToTable("Dependents");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Relationship)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(d => d.Employee)
            .WithMany(e => e.Dependents)
            .HasForeignKey(d => d.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
