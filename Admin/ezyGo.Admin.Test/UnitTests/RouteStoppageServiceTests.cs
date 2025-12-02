using AutoMapper;
using ezyGo.Admin.Domain.Managers;
using ezyGo.Admin.Domain.Models;
using ezyGo.Admin.Storage.Entities;
using ezyGo.Admin.Storage.Repositories;
using ezyGo.Core.Exceptions;
using Moq;

namespace ezyGo.Admin.Test.UnitTests;

public class RouteStoppageServiceTests
{
    private readonly Mock<IRouteStoppageRepository> _mockStoppageRepo;
    private readonly Mock<IRouteRepository> _mockRouteRepo;
    private readonly Mock<IBusStationRepository> _mockStationRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly RouteStoppageManager _manager;

    public RouteStoppageServiceTests()
    {
        _mockStoppageRepo = new Mock<IRouteStoppageRepository>();
        _mockRouteRepo = new Mock<IRouteRepository>();
        _mockStationRepo = new Mock<IBusStationRepository>();
        _mockMapper = new Mock<IMapper>();
        _manager = new RouteStoppageManager(_mockStoppageRepo.Object, _mockMapper.Object, _mockRouteRepo.Object, _mockStationRepo.Object);
    }


    [Fact]
    public async Task AddStoppageEndAsync_ValidStoppage_ReturnsCreatedStoppage()
    {
        // Arrange
        var model = new RouteStoppage
        {
            RouteId = 1,
            StationId = 2
        };

        var stoppageEntity = new RouteStoppageEntity
        {
            Id = 1,
            RouteEntityId = 1,
            BusStationEntityId = 2
        };

        var createdStoppage = new RouteStoppage
        {
            Id = 1,
            RouteId = 1,
            StationId = 2
        };

        _mockStoppageRepo.Setup(r => r.IsStoppageAlreadyExist(1, 2)).ReturnsAsync(false);
        _mockStationRepo.Setup(r => r.IsExistsAsync(2)).ReturnsAsync(true);
        _mockRouteRepo.Setup(r => r.IsExistsAsync(1)).ReturnsAsync(true);
        _mockMapper.Setup(m => m.Map<RouteStoppageEntity>(model)).Returns(stoppageEntity);
        _mockStoppageRepo.Setup(r => r.InsertAtEndAsync(stoppageEntity)).ReturnsAsync(stoppageEntity);
        _mockMapper.Setup(m => m.Map<RouteStoppage>(stoppageEntity)).Returns(createdStoppage);

        // Act
        var result = await _manager.AddStoppageEndAsync(model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(1, result.RouteId);
        Assert.Equal(2, result.StationId);
        _mockStoppageRepo.Verify(r => r.InsertAtEndAsync(stoppageEntity), Times.Once);
    }



    [Fact]
    public async Task AddStoppageEndAsync_NullModel_ThrowsNullReferenceException()
    {
        // Arrange
        RouteStoppage? model = null;

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => _manager.AddStoppageEndAsync(model!));
    }

    [Fact]
    public async Task AddStoppageEndAsync_StoppageAlreadyExists_ThrowsAlreadyExistException()
    {
        // Arrange
        var model = new RouteStoppage
        {
            RouteId = 1,
            StationId = 2
        };

        _mockStoppageRepo.Setup(r => r.IsStoppageAlreadyExist(1, 2)).ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<AlreadyExistException>(() => _manager.AddStoppageEndAsync(model));

        Assert.Contains("already assigned", exception.Message);
        _mockStationRepo.Verify(r => r.IsExistsAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task AddStoppageEndAsync_StationNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var model = new RouteStoppage
        {
            RouteId = 1,
            StationId = 2
        };

        _mockStoppageRepo.Setup(r => r.IsStoppageAlreadyExist(1, 2)).ReturnsAsync(false);
        _mockStationRepo.Setup(r => r.IsExistsAsync(2)).ReturnsAsync(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _manager.AddStoppageEndAsync(model));
        Assert.Contains("Station with id", exception.Message);
        _mockRouteRepo.Verify(r => r.IsExistsAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task AddStoppageEndAsync_RouteNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var model = new RouteStoppage
        {
            RouteId = 1,
            StationId = 2
        };

        _mockStoppageRepo.Setup(r => r.IsStoppageAlreadyExist(1, 2)).ReturnsAsync(false);
        _mockStationRepo.Setup(r => r.IsExistsAsync(2)).ReturnsAsync(true);
        _mockRouteRepo.Setup(r => r.IsExistsAsync(1)).ReturnsAsync(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _manager.AddStoppageEndAsync(model));
        Assert.Contains("Route with id", exception.Message);
    }


    [Fact]
    public async Task AddStoppageMiddleAsync_ValidInput_ReturnsCreatedStoppage()
    {
        // Arrange
        var routeId = 1;
        var stationId = 2;
        var insertAfterOrder = 3;

        var stoppageEntity = new RouteStoppageEntity
        {
            Id = 1,
            RouteEntityId = 1,
            BusStationEntityId = 2
        };

        var createdStoppage = new RouteStoppage
        {
            Id = 1,
            RouteId = 1,
            StationId = 2
        };

        _mockStoppageRepo.Setup(r => r.IsStoppageAlreadyExist(routeId, stationId)).ReturnsAsync(false);
        _mockStationRepo.Setup(r => r.IsExistsAsync(stationId)).ReturnsAsync(true);
        _mockRouteRepo.Setup(r => r.IsExistsAsync(routeId)).ReturnsAsync(true);
        _mockStoppageRepo.Setup(r => r.InsertInMiddleAsync(routeId, stationId, insertAfterOrder)).ReturnsAsync(stoppageEntity);
        _mockMapper.Setup(m => m.Map<RouteStoppage>(stoppageEntity)).Returns(createdStoppage);

        // Act
        var result = await _manager.AddStoppageMiddleAsync(routeId, stationId, insertAfterOrder);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(1, result.RouteId);
        Assert.Equal(2, result.StationId);
        _mockStoppageRepo.Verify(r => r.InsertInMiddleAsync(routeId, stationId, insertAfterOrder), Times.Once);
    }


    [Fact]
    public async Task GetStoppagesAsync_ValidRouteId_ReturnsStoppages()
    {
        // Arrange
        var routeId = 1;
        var stoppageEntities = new List<RouteStoppageEntity>
        {
            new() { Id = 1, RouteEntityId = 1, BusStationEntityId = 2 },
            new() { Id = 2, RouteEntityId = 1, BusStationEntityId = 3 }
        };

        var expectedStoppages = new List<RouteStoppage>
        {
            new() { Id = 1, RouteId = 1, StationId = 2 },
            new() { Id = 2, RouteId = 1, StationId = 3 }
        };

        _mockStoppageRepo.Setup(r => r.GetByRouteIdAsync(routeId)).ReturnsAsync(stoppageEntities);
        _mockMapper.Setup(m => m.Map<List<RouteStoppage>>(stoppageEntities)).Returns(expectedStoppages);

        // Act
        var result = await _manager.GetStoppagesAsync(routeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockStoppageRepo.Verify(r => r.GetByRouteIdAsync(routeId), Times.Once);
    }


    [Fact]
    public async Task GetStoppagesAsync_NoStoppages_ReturnsEmptyList()
    {
        // Arrange
        var routeId = 1;
        var emptyList = new List<RouteStoppageEntity>();

        _mockStoppageRepo.Setup(r => r.GetByRouteIdAsync(routeId)).ReturnsAsync(emptyList);
        _mockMapper.Setup(m => m.Map<List<RouteStoppage>>(emptyList)).Returns(new List<RouteStoppage>());

        // Act
        var result = await _manager.GetStoppagesAsync(routeId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

}
