using CleanArch.EntraApi.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.EntraApi.Application.Projects;

public record GetProjectsQuery : IRequest<List<Project>>;

public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, List<Project>>
{
    public Task<List<Project>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        // Mock data - in real implementation, this would come from database
        return Task.FromResult(new List<Project>
        {
            new() { Id = Guid.NewGuid(), Name = "Project Alpha" },
            new() { Id = Guid.NewGuid(), Name = "Project Beta" },
            new() { Id = Guid.NewGuid(), Name = "Project Gamma" }
        });
    }
}
