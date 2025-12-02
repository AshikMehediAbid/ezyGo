using AutoMapper;
using ezyGo.Admin.Domain.Managers;
using ezyGo.Admin.Domain.Models;
using ezyGo.Admin.Storage.Entities;
using ezyGo.Admin.Storage.Repositories;
using ezyGo.Core.Exceptions;
using Moq;

namespace ezyGo.Admin.Test.UnitTests;

public class BusStationServiceTests
{
    private readonly Mock<IBusStationRepository> _mockStationRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly BusStationService _service;

    public BusStationServiceTests()
    {
        _mockStationRepo = new Mock<IBusStationRepository>();
        _mockMapper = new Mock<IMapper>();
        _service = new BusStationService(_mockStationRepo.Object, _mockMapper.Object);
    }


    [Fact]
    public async Task CreateBusStation_ValidStation_CreateStation()
    {
        // Arrange
        var station = new Station
        {
            StationName = "Test Station",
            StationDescription = "Test Description",
            Geo = new Domain.Models.Geo { Latitude = 12.34m, Longitude = 56.78m }
        };

        var stationEntity = new BusStationEntity
        {
            Id = 1,
            StationName = "Test Station",
            StationDescription = "Test Description"
        };

        _mockMapper.Setup(m => m.Map<BusStationEntity>(station))
            .Returns(stationEntity);

        // Act
        await _service.CreateBusStationAsync(station);

        // Assert
        Assert.NotEqual(default, station.CreatedAt);
        Assert.NotEqual(default, station.UpdatedAt);
        _mockStationRepo.Verify(r => r.AddAsync(stationEntity), Times.Once);
        _mockMapper.Verify(m => m.Map<BusStationEntity>(station), Times.Once);
    }


    [Fact]
    public async Task CreateBusStation_NullStation_ThrowsNullReferenceException()
    {
        // Arrange
        Station? station = null;

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => _service.CreateBusStationAsync(station!));

        _mockStationRepo.Verify(r => r.AddAsync(It.IsAny<BusStationEntity>()), Times.Never);
    }


    [Fact]
    public async Task DeleteBusStation_StationExists_DeletesStationWithDependencies()
    {
        // Arrange
        var stationId = 1;
        var stationEntity = new BusStationEntity { Id = stationId };

        _mockStationRepo.Setup(r => r.GetByIdAsync(stationId))
            .ReturnsAsync(stationEntity);

        // Act
        await _service.DeleteBusStationAsync(stationId);

        // Assert
        _mockStationRepo.Verify(r => r.DeleteBusStationWithDependenciesAsync(stationEntity), Times.Once);
    }

    [Fact]
    public async Task DeleteBusStation_StationNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var stationId = 1;

        _mockStationRepo.Setup(r => r.GetByIdAsync(stationId))
            .ReturnsAsync((BusStationEntity?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteBusStationAsync(stationId));

        _mockStationRepo.Verify(r => r.DeleteBusStationWithDependenciesAsync(It.IsAny<BusStationEntity>()), Times.Never);
    }

    [Fact]
    public async Task DeleteBusStation_InvalidId_ThrowsNotFoundException()
    {
        // Arrange
        var stationId = -1;

        _mockStationRepo.Setup(r => r.GetByIdAsync(stationId))
            .ReturnsAsync((BusStationEntity?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteBusStationAsync(stationId));
    }


    [Fact]
    public async Task GetBusStationById_StationExists_ReturnsStation()
    {
        // Arrange
        var stationId = 1;
        var stationEntity = new BusStationEntity
        {
            Id = stationId,
            StationName = "Test Station",
            StationDescription = "Test Description",
            Geo = new Storage.Entities.Geo { Latitude = 12.34m, Longitude = 56.78m }
        };

        var expectedStation = new Station
        {
            Id = stationId,
            StationName = "Test Station",
            StationDescription = "Test Description",
            Geo = new Domain.Models.Geo { Latitude = 12.34m, Longitude = 56.78m }
        };

        _mockStationRepo.Setup(r => r.GetByIdAsync(stationId))
            .ReturnsAsync(stationEntity);

        _mockMapper.Setup(m => m.Map<Station>(stationEntity))
            .Returns(expectedStation);

        // Act
        var result = await _service.GetBusStationByIdAsync(stationId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(stationId, result.Id);
        Assert.Equal("Test Station", result.StationName);
        Assert.NotNull(result.Geo);
        Assert.Equal(12.34m, result.Geo.Latitude);

        _mockStationRepo.Verify(r => r.GetByIdAsync(stationId), Times.Once);
        _mockMapper.Verify(m => m.Map<Station>(stationEntity), Times.Once);
    }

    [Fact]
    public async Task GetBusStationById_StationNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var stationId = 1;

        _mockStationRepo.Setup(r => r.GetByIdAsync(stationId))
                       .ReturnsAsync((BusStationEntity?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetBusStationByIdAsync(stationId));

        _mockMapper.Verify(m => m.Map<Station>(It.IsAny<BusStationEntity>()), Times.Never);
    }


    [Fact]
    public async Task GetBusStations_WithEntities_ReturnsStations()
    {
        // Arrange
        var stationEntities = new List<BusStationEntity>
        {
            new() { Id = 1, StationName = "Station 1" },
            new() { Id = 2, StationName = "Station 2" },
            new() { Id = 3, StationName = "Station 3" }
        };

        var expectedStations = new List<Station>
        {
            new() { Id = 1, StationName = "Station 1" },
            new() { Id = 2, StationName = "Station 2" },
            new() { Id = 3, StationName = "Station 3" }
        };

        _mockStationRepo.Setup(r => r.GetAllAsync())
                       .ReturnsAsync(stationEntities);
        _mockMapper.Setup(m => m.Map<IEnumerable<Station>>(stationEntities))
                  .Returns(expectedStations);

        // Act
        var result = await _service.GetBusStationsAsync(null);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
        _mockStationRepo.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetBusStationsAsync_NoStations_ReturnsEmptyList()
    {
        // Arrange
        var emptyList = new List<BusStationEntity>();

        _mockStationRepo.Setup(r => r.GetAllAsync())
            .ReturnsAsync(emptyList);
        _mockMapper.Setup(m => m.Map<IEnumerable<Station>>(emptyList))
            .Returns([]);

        // Act
        var result = await _service.GetBusStationsAsync(null);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }


    [Fact]
    public async Task UpdateBusStation_ValidStation_UpdatesStation()
    {
        // Arrange
        var station = new Station
        {
            Id = 1,
            StationName = "Updated Station",
            StationDescription = "Updated Description",
            Geo = new Domain.Models.Geo { Latitude = 23.45m, Longitude = 67.89m },
            UpdatedAt = DateTime.UtcNow
        };

        var stationEntity = new BusStationEntity
        {
            Id = 1,
            StationName = "Updated Station",
            StationDescription = "Updated Description"
        };

        _mockMapper.Setup(m => m.Map<BusStationEntity>(station))
            .Returns(stationEntity);

        // Act
        await _service.UpdateBusStationAsync(station);

        // Assert
        Assert.True(station.UpdatedAt > DateTime.UtcNow.AddSeconds(-2));
        _mockStationRepo.Verify(r => r.UpdateAsync(stationEntity), Times.Once);
        _mockMapper.Verify(m => m.Map<BusStationEntity>(station), Times.Once);
    }

    [Fact]
    public async Task UpdateBusStation_NullStation_ThrowsNullReferenceException()
    {
        // Arrange
        Station? station = null;

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => _service.UpdateBusStationAsync(station!));

        _mockStationRepo.Verify(r => r.UpdateAsync(It.IsAny<BusStationEntity>()), Times.Never);
    }


}
