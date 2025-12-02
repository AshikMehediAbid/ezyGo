using AutoMapper;
using ezyGo.Admin.Domain.Managers;
using ezyGo.Admin.Domain.Models;
using ezyGo.Admin.Storage.Entities;
using ezyGo.Admin.Storage.Repositories;
using ezyGo.Core.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;

namespace ezyGo.Admin.Test.UnitTests;

public class TripTemplateServiceTests
{
    private readonly Mock<ITripTemplateRepository> _mockTripRepo;
    private readonly Mock<IBusRepository> _mockBusRepo;
    private readonly Mock<IRouteRepository> _mockRouteRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<TripTemplateService>> _mockLogger;
    private readonly TripTemplateService _service;
    public TripTemplateServiceTests()
    {
        _mockTripRepo = new Mock<ITripTemplateRepository>();
        _mockBusRepo = new Mock<IBusRepository>();
        _mockRouteRepo = new Mock<IRouteRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<TripTemplateService>>();

        _service = new TripTemplateService(
            _mockTripRepo.Object,
            _mockBusRepo.Object,
            _mockRouteRepo.Object,
            _mockMapper.Object,
            _mockLogger.Object);
    }



    [Fact]
    public async Task CreateTripTemplateAsync_ValidTemplate_ReturnsCreatedTemplate()
    {
        // Arrange
        var tripTemplate = new TripTemplateModel
        {
            RouteId = 1,
            BusId = 1,
            BaseFare = 100,
            DepartureTime = TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(1)),
            ArrivalTime = TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(5)),
        };

        var routeEntity = new RouteEntity
        {
            Id = 1,
            Stoppages = new List<RouteStoppageEntity>
            {
                new()
                {
                    Order = 1,
                    BusStationEntity = new BusStationEntity { StationName = "Station A" }
                },
                new()
                {
                    Order = 2,
                    BusStationEntity = new BusStationEntity { StationName = "Station B" }
                }
            }
        };

        var tripTemplateEntity = new TripTemplate { Id = 1 };
        var createdTemplate = new TripTemplate { Id = 1 };
        var expectedResult = new TripTemplateModel { Id = 1 };

        _mockRouteRepo.Setup(r => r.GetRouteByIdWithDetailsAsync(1)).ReturnsAsync(routeEntity);
        _mockBusRepo.Setup(r => r.IsExistsAsync(1)).ReturnsAsync(true);
        _mockTripRepo.Setup(r => r.IsTripTemplateExistAsync(1, 1, 100,
            tripTemplate.DepartureTime, tripTemplate.ArrivalTime)).ReturnsAsync(false);
        _mockMapper.Setup(m => m.Map<TripTemplate>(tripTemplate)).Returns(tripTemplateEntity);
        _mockTripRepo.Setup(r => r.AddAsync(tripTemplateEntity)).ReturnsAsync(createdTemplate);
        _mockTripRepo.Setup(r => r.GetTripTemplateByIdWithDetailsAsync(1)).ReturnsAsync(createdTemplate);
        _mockMapper.Setup(m => m.Map<TripTemplateModel>(createdTemplate)).Returns(expectedResult);

        // Act
        var result = await _service.CreateTripTemplateAsync(tripTemplate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Station A - Station B", tripTemplate.Stoppages);
        _mockTripRepo.Verify(r => r.AddAsync(tripTemplateEntity), Times.Once);
    }



    [Fact]
    public async Task CreateTripTemplateAsync_RouteNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var tripTemplate = new TripTemplateModel
        {
            RouteId = 999,
            BusId = 1
        };

        _mockRouteRepo.Setup(r => r.GetRouteByIdWithDetailsAsync(999)).ReturnsAsync((RouteEntity?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.CreateTripTemplateAsync(tripTemplate));
        _mockTripRepo.Verify(r => r.AddAsync(It.IsAny<TripTemplate>()), Times.Never);
    }


    [Fact]
    public async Task CreateTripTemplateAsync_DuplicateTrip_ThrowsAlreadyExistException()
    {
        // Arrange
        var tripTemplate = new TripTemplateModel
        {
            RouteId = 1,
            BusId = 1,
            BaseFare = 100,
            DepartureTime = TimeOnly.FromDateTime(DateTime.UtcNow),
            ArrivalTime = TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(4))
        };

        var routeEntity = new RouteEntity { Id = 1 };

        _mockRouteRepo.Setup(r => r.GetRouteByIdWithDetailsAsync(1)).ReturnsAsync(routeEntity);
        _mockBusRepo.Setup(r => r.IsExistsAsync(1)).ReturnsAsync(true);
        _mockTripRepo.Setup(r => r.IsTripTemplateExistAsync(1, 1, 100,
            tripTemplate.DepartureTime, tripTemplate.ArrivalTime)).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<AlreadyExistException>(() => _service.CreateTripTemplateAsync(tripTemplate));
        _mockTripRepo.Verify(r => r.AddAsync(It.IsAny<TripTemplate>()), Times.Never);
    }



    [Fact]
    public async Task GetTripTemplatesAsync_WithFilter_ReturnsTemplates()
    {
        // Arrange
        var filter = "morning";
        var templateEntities = new List<TripTemplate>
        {
            new() { Id = 1, Description = "Morning Trip" },
            new() { Id = 2, Description = "Evening Trip" }
        };

        var expectedTemplates = new List<TripTemplateModel>
        {
            new() { Id = 1, Description = "Morning Trip" },
            new() { Id = 2, Description = "Evening Trip" }
        };

        _mockTripRepo.Setup(r => r.GetTripTemplatesWithDetailsAsync(filter)).ReturnsAsync(templateEntities);
        _mockMapper.Setup(m => m.Map<IEnumerable<TripTemplateModel>>(templateEntities)).Returns(expectedTemplates);

        // Act
        var result = await _service.GetTripTemplatesAsync(filter);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockTripRepo.Verify(r => r.GetTripTemplatesWithDetailsAsync(filter), Times.Once);
    }


    [Fact]
    public async Task GetTripTemplateByIdAsync_TemplateExists_ReturnsTemplate()
    {
        // Arrange
        var templateId = 1;
        var templateEntity = new TripTemplate
        {
            Id = templateId,
            Description = "Test Template"
        };

        var expectedTemplate = new TripTemplateModel
        {
            Id = templateId,
            Description = "Test Template"
        };

        _mockTripRepo.Setup(r => r.GetTripTemplateByIdWithDetailsAsync(templateId)).ReturnsAsync(templateEntity);
        _mockMapper.Setup(m => m.Map<TripTemplateModel>(templateEntity)).Returns(expectedTemplate);

        // Act
        var result = await _service.GetTripTemplateByIdAsync(templateId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(templateId, result.Id);
        _mockTripRepo.Verify(r => r.GetTripTemplateByIdWithDetailsAsync(templateId), Times.Once);
    }

    [Fact]
    public async Task GetTripTemplateByIdAsync_TemplateNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var templateId = 999;

        _mockTripRepo.Setup(r => r.GetTripTemplateByIdWithDetailsAsync(templateId)).ReturnsAsync((TripTemplate?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetTripTemplateByIdAsync(templateId));
        _mockMapper.Verify(m => m.Map<TripTemplateModel>(It.IsAny<TripTemplate>()), Times.Never);
    }


    [Fact]
    public async Task UpdateTripTemplateAsync_TemplateExists_UpdatesTemplate()
    {
        // Arrange
        var templateId = 1;
        var existingTemplate = new TripTemplate { Id = templateId };

        var tripTemplate = new TripTemplateModel
        {
            RouteId = 1,
            BusId = 1,
            BaseFare = 150,
            DepartureTime = TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(2)),
            ArrivalTime = TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(6))
        };

        var routeEntity = new RouteEntity
        {
            Id = 1,
            Stoppages = new List<RouteStoppageEntity>
            {
                new() { Order = 1, BusStationEntity = new BusStationEntity { StationName = "Updated Station" } }
            }
        };

        var tripTemplateEntity = new TripTemplate { Id = templateId };

        _mockTripRepo.Setup(r => r.GetByIdAsync(templateId)).ReturnsAsync(existingTemplate);
        _mockRouteRepo.Setup(r => r.GetRouteByIdWithDetailsAsync(1)).ReturnsAsync(routeEntity);
        _mockBusRepo.Setup(r => r.IsExistsAsync(1)).ReturnsAsync(true);
        _mockMapper.Setup(m => m.Map<TripTemplate>(tripTemplate)).Returns(tripTemplateEntity);

        // Act
        await _service.UpdateTripTemplateAsync(templateId, tripTemplate);

        // Assert
        Assert.True(tripTemplate.UpdatedAt > DateTime.UtcNow.AddSeconds(-2));
        Assert.Equal("Updated Station", tripTemplate.Stoppages);
        _mockTripRepo.Verify(r => r.UpdateAsync(tripTemplateEntity), Times.Once);
    }


    [Fact]
    public async Task DeleteTripTemplateAsync_TemplateExists_DeletesTemplate()
    {
        // Arrange
        var templateId = 1;
        var template = new TripTemplate { Id = templateId };

        _mockTripRepo.Setup(r => r.GetByIdAsync(templateId)).ReturnsAsync(template);

        // Act
        await _service.DeleteTripTemplateAsync(templateId);

        // Assert
        _mockTripRepo.Verify(r => r.DeleteAsync(template), Times.Once);
    }
}
