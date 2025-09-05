using CleanArch.EntraApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArch.EntraApi.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Project> Projects { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}