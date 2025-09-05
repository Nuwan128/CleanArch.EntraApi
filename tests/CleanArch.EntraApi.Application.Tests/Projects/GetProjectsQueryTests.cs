using CleanArch.EntraApi.Application.Projects;
using CleanArch.EntraApi.Domain.Entities;

namespace CleanArch.EntraApi.Application.Tests.Projects;

public class GetProjectsQueryTests
{
    [Fact]
    public async Task Handle_ReturnsProjectsList()
    {
        // Arrange
        var handler = new GetProjectsQueryHandler();
        var query = new GetProjectsQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<Project>>(result);
        Assert.Equal(3, result.Count);
    }
}