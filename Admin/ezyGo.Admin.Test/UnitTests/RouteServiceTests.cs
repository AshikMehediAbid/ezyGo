using AutoMapper;
using ezyGo.Admin.Domain.Interfaces;
using ezyGo.Admin.Domain.Managers;
using ezyGo.Admin.Domain.Models;
using ezyGo.Admin.Storage.Entities;
using ezyGo.Admin.Storage.Repositories;
using ezyGo.Core.Exceptions;
using Moq;

namespace ezyGo.Admin.Test.UnitTests;

public class RouteServiceTests
{
    private readonly Mock<IRouteRepository> _mockRouteRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IRouteStoppageManager> _mockStoppageManager;
    private readonly Mock<IBusStationRepository> _mockStationRepo;
    private readonly RouteService _service;

    public RouteServiceTests()
    {
        _mockRouteRepo = new Mock<IRouteRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockStoppageManager = new Mock<IRouteStoppageManager>();
        _mockStationRepo = new Mock<IBusStationRepository>();
        _service = new RouteService(_mockRouteRepo.Object, _mockMapper.Object, _mockStoppageManager.Object, _mockStationRepo.Object);
    }


    [Fact]
    public async Task CreateRouteAsync_ValidRoute_ReturnsCreatedRoute()
    {
        // Arrange
        var startingStation = new Station { Id = 1, StationName = "Start Station" };
        var endingStation = new Station { Id = 2, StationName = "End Station" };

        var route = new Route
        {
            StartingPoint = startingStation,
            EndingPoint = endingStation
        };

        var routeEntity = new RouteEntity
        {
            Id = 1,
            StartingPointId = 1,
            EndingPointId = 2
        };

        var createdRouteEntity = new RouteEntity
        {
            Id = 1,
            StartingPointId = 1,
            EndingPointId = 2,
            StartingPoint = new BusStationEntity { Id = 1, StationName = "Start Station" },
            EndingPoint = new BusStationEntity { Id = 2, StationName = "End Station" }
        };

        var expectedRoute = new Route
        {
            Id = 1,
            StartingPoint = startingStation,
            EndingPoint = endingStation
        };

        _mockStationRepo.Setup(r => r.IsExistsAsync(1)).ReturnsAsync(true);
        _mockStationRepo.Setup(r => r.IsExistsAsync(2)).ReturnsAsync(true);

        _mockRouteRepo.Setup(r => r.IsRouteExist(1, 2)).ReturnsAsync(false);
        _mockMapper.Setup(m => m.Map<RouteEntity>(route)).Returns(routeEntity);
        _mockRouteRepo.Setup(r => r.CreateRouteAsync(routeEntity)).ReturnsAsync(createdRouteEntity);
        _mockRouteRepo.Setup(r => r.GetRouteByIdWithDetailsAsync(1)).ReturnsAsync(createdRouteEntity);
        _mockMapper.Setup(m => m.Map<Route>(createdRouteEntity)).Returns(expectedRoute);

        // Act
        var result = await _service.CreateRouteAsync(route);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);

        _mockRouteRepo.Verify(r => r.CreateRouteAsync(routeEntity), Times.Once);
        _mockStoppageManager.Verify(m => m.AddStoppageEndAsync(It.Is<RouteStoppage>(s =>
            s.RouteId == 1 && s.StationId == 1)), Times.Once);
        _mockStoppageManager.Verify(m => m.AddStoppageEndAsync(It.Is<RouteStoppage>(s =>
            s.RouteId == 1 && s.StationId == 2)), Times.Once);
        _mockStationRepo.Verify(s => s.IsExistsAsync(It.IsAny<int>()), Times.Exactly(2));
    }


    [Fact]
    public async Task CreateRouteAsync_NullRoute_ThrowsNullReferenceException()
    {
        // Arrange
        Route? route = null;

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => _service.CreateRouteAsync(route!));
    }

    [Fact]
    public async Task CreateRouteAsync_BothPointsNull_ThrowsArgumentException()
    {
        // Arrange
        var route = new Route
        {
            StartingPoint = null!,
            EndingPoint = null!
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateRouteAsync(route));

        Assert.Contains("Route must include both starting and ending stations", exception.Message);
    }

    [Fact]
    public async Task CreateRouteAsync_RouteAlreadyExists_ThrowsAlreadyExistException()
    {
        // Arrange
        var startingStation = new Station { Id = 1, StationName = "Start Station" };
        var endingStation = new Station { Id = 2, StationName = "End Station" };

        var route = new Route
        {
            StartingPoint = startingStation,
            EndingPoint = endingStation
        };

        _mockStationRepo.Setup(r => r.IsExistsAsync(1)).ReturnsAsync(true);
        _mockStationRepo.Setup(r => r.IsExistsAsync(2)).ReturnsAsync(true);

        _mockRouteRepo.Setup(r => r.IsRouteExist(1, 2)).ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<AlreadyExistException>(() => _service.CreateRouteAsync(route));

        Assert.Contains("The route", exception.Message);
        _mockRouteRepo.Verify(r => r.CreateRouteAsync(It.IsAny<RouteEntity>()), Times.Never);
        _mockStationRepo.Verify(s => s.IsExistsAsync(It.IsAny<int>()), Times.Exactly(2));
    }


    [Fact]
    public async Task CreateRouteAsync_StationNotExist_ThrowNotFoundException()
    {
        // Arrange
        var startingStation = new Station { Id = 0, StationName = "Start Station" };
        var endingStation = new Station { Id = 2, StationName = "End Station" };

        var route = new Route
        {
            StartingPoint = startingStation,
            EndingPoint = endingStation
        };

        _mockStationRepo.Setup(r => r.IsExistsAsync(0)).ReturnsAsync(false);
        _mockStationRepo.Setup(r => r.IsExistsAsync(2)).ReturnsAsync(true);

        // Act

        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _service.CreateRouteAsync(route));

        // Assert
        Assert.Equal("To create Route, Start Or End Station Not Found!", ex.Message);
        _mockStationRepo.Verify(s => s.IsExistsAsync(It.IsAny<int>()), Times.Exactly(2));
    }



    [Fact]
    public async Task DeleteRouteAsync_RouteExists_DeletesRoute()
    {
        // Arrange
        var routeId = 1;
        var routeEntity = new RouteEntity { Id = routeId };

        _mockRouteRepo.Setup(r => r.GetByIdAsync(routeId)).ReturnsAsync(routeEntity);

        // Act
        await _service.DeleteRouteAsync(routeId);

        // Assert
        _mockRouteRepo.Verify(r => r.DeleteAsync(routeEntity), Times.Once);
    }


    [Fact]
    public async Task DeleteRouteAsync_RouteNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var routeId = 1;

        _mockRouteRepo.Setup(r => r.GetByIdAsync(routeId)).ReturnsAsync((RouteEntity?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteRouteAsync(routeId));
        _mockRouteRepo.Verify(r => r.DeleteAsync(It.IsAny<RouteEntity>()), Times.Never);
    }


    [Fact]
    public async Task GetRouteByIdAsync_RouteExists_ReturnsRoute()
    {
        // Arrange
        var routeId = 1;
        var routeEntity = new RouteEntity
        {
            Id = routeId,
            StartingPoint = new BusStationEntity { Id = 1, StationName = "Start" },
            EndingPoint = new BusStationEntity { Id = 2, StationName = "End" },
            Stoppages = []
        };

        var expectedRoute = new Route
        {
            Id = routeId,
            StartingPoint = new Station { Id = 1, StationName = "Start" },
            EndingPoint = new Station { Id = 2, StationName = "End" }
        };

        _mockRouteRepo.Setup(r => r.GetRouteByIdWithDetailsAsync(routeId)).ReturnsAsync(routeEntity);
        _mockMapper.Setup(m => m.Map<Route>(routeEntity)).Returns(expectedRoute);

        // Act
        var result = await _service.GetRouteByIdAsync(routeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(routeId, result.Id);

        _mockRouteRepo.Verify(r => r.GetRouteByIdWithDetailsAsync(routeId), Times.Once);
        _mockMapper.Verify(m => m.Map<Route>(routeEntity), Times.Once);
    }


    [Fact]
    public async Task GetRouteByIdAsync_RouteNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var routeId = 1;

        _mockRouteRepo.Setup(r => r.GetRouteByIdWithDetailsAsync(routeId)).ReturnsAsync((RouteEntity?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetRouteByIdAsync(routeId));
        _mockMapper.Verify(m => m.Map<Route>(It.IsAny<RouteEntity>()), Times.Never);
    }


    [Fact]
    public async Task GetRoutesAsync_WithFilter_ReturnsRoutes()
    {
        // Arrange
        var filter = "test";
        var routeEntities = new List<RouteEntity>
        {
            new() { Id = 1 },
            new() { Id = 2 }
        };

        var expectedRoutes = new List<Route>
        {
            new() { Id = 1 },
            new() { Id = 2 }
        };

        _mockRouteRepo.Setup(r => r.GetRoutesAsync(filter)).ReturnsAsync(routeEntities);
        _mockMapper.Setup(m => m.Map<IEnumerable<Route>>(routeEntities)).Returns(expectedRoutes);

        // Act
        var result = await _service.GetRoutesAsync(filter);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRouteRepo.Verify(r => r.GetRoutesAsync(filter), Times.Once);
    }


    [Fact]
    public async Task GetRoutesAsync_EmptyResult_ReturnsEmptyList()
    {
        // Arrange
        var emptyList = new List<RouteEntity>();

        _mockRouteRepo.Setup(r => r.GetRoutesAsync(It.IsAny<string>())).ReturnsAsync(emptyList);
        _mockMapper.Setup(m => m.Map<IEnumerable<Route>>(emptyList)).Returns([]);

        // Act
        var result = await _service.GetRoutesAsync("filter");

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }


    [Fact]
    public async Task UpdateRouteAsync_ValidRoute_UpdatesRoute()
    {
        // Arrange
        var route = new Route
        {
            Id = 1,
            StartingPoint = new Station { Id = 1, StationName = "Start" },
            EndingPoint = new Station { Id = 2, StationName = "End" },
            UpdatedAt = DateTime.UtcNow
        };

        var routeEntity = new RouteEntity { Id = 1 };

        _mockStationRepo.Setup(r => r.IsExistsAsync(1)).ReturnsAsync(true);
        _mockStationRepo.Setup(r => r.IsExistsAsync(2)).ReturnsAsync(true);

        _mockMapper.Setup(m => m.Map<RouteEntity>(route)).Returns(routeEntity);

        // Act
        await _service.UpdateRouteAsync(route);

        // Assert
        Assert.True(route.UpdatedAt > DateTime.UtcNow.AddSeconds(-2));
        _mockRouteRepo.Verify(r => r.UpdateAsync(routeEntity), Times.Once);
        _mockMapper.Verify(m => m.Map<RouteEntity>(route), Times.Once);

        _mockStationRepo.Verify(s => s.IsExistsAsync(It.IsAny<int>()), Times.Exactly(2));
    }


    [Fact]
    public async Task UpdateRouteAsync_RouteWithoutStations_UpdatesSuccessfully()
    {
        // Arrange
        var route = new Route
        {
            Id = 1,
            StartingPoint = new Station { Id = 0, StationName = "" },
            EndingPoint = new Station { Id = 0, StationName = "" },
            UpdatedAt = DateTime.UtcNow
        };

        _mockStationRepo.Setup(r => r.IsExistsAsync(1)).ReturnsAsync(false);
        _mockStationRepo.Setup(r => r.IsExistsAsync(2)).ReturnsAsync(false);

        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _service.CreateRouteAsync(route));

        // Assert
        Assert.Equal("To create Route, Start Or End Station Not Found!", ex.Message);
        _mockStationRepo.Verify(s => s.IsExistsAsync(It.IsAny<int>()), Times.Exactly(2));
    }
}
