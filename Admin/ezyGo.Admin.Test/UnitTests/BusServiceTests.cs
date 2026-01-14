using AutoMapper;
using ezyGo.Admin.Domain.Managers;
using ezyGo.Admin.Domain.Models;
using ezyGo.Admin.Storage.Entities;
using ezyGo.Admin.Storage.Repositories;
using ezyGo.Core.Exceptions;
using Moq;

namespace ezyGo.Admin.Test.UnitTests;

public class BusServiceTests
{
    private readonly Mock<IBusRepository> _mockBusRepo;
    private readonly Mock<IBusCompanyRepository> _mockCompanyRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly BusService _service;

    public BusServiceTests()
    {
        _mockBusRepo = new Mock<IBusRepository>();
        _mockCompanyRepo = new Mock<IBusCompanyRepository>();
        _mockMapper = new Mock<IMapper>();
        _service = new BusService(_mockCompanyRepo.Object, _mockBusRepo.Object, _mockMapper.Object);
    }

    #region BusCompany Tests

    [Fact]
    public async Task BusCompany_CreateAsync_ValidCompany_ReturnCreatedCompany()
    {
        // Arrange 
        var company = new BusCompany { CompanyName = "Test Company" };
        var companyEntity = new BusCompanyEntity { CompanyName = "Test Company" };
        var createdCompany = new BusCompany { Id = 1, CompanyName = "Test Company" };


        _mockMapper.Setup(m => m.Map<BusCompanyEntity>(company)).Returns(companyEntity);
        _mockCompanyRepo.Setup(r => r.AddAsync(companyEntity))
            .ReturnsAsync(companyEntity);
        _mockMapper.Setup(m => m.Map<BusCompany>(companyEntity)).Returns(createdCompany);


        // Act
        var result = await _service.CreateBusCompanyAsync(company);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Company", result.CompanyName);
        _mockCompanyRepo.Verify(r => r.AddAsync(companyEntity), Times.Once);

    }

    [Fact]
    public async Task BusCompany_CreateAsync_EmptyCompany_ThrowException()
    {
        // Arrange 
        var company = new BusCompany { CompanyName = string.Empty };

        // Act
        var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _service.CreateBusCompanyAsync(company)
        );

        // Assert
        Assert.Contains("Company name can not be null or empty", ex.Message);
        Assert.Contains("CompanyName", ex.Message);
        _mockCompanyRepo.Verify(r => r.AddAsync(It.IsAny<BusCompanyEntity>()), Times.Never);

    }


    [Fact]
    public async Task BusCompany_DeleteAsync_CompanyExists_DeletesCompany()
    {
        // Arrange
        var companyId = 1;
        var companyEntity = new BusCompanyEntity { Id = companyId };
        _mockCompanyRepo.Setup(r => r.GetByIdAsync(companyId)).ReturnsAsync(companyEntity);

        // Act
        await _service.DeleteBusCompanyAsync(companyId);

        // Assert
        _mockCompanyRepo.Verify(r => r.GetByIdAsync(companyId), Times.Once);
        _mockCompanyRepo.Verify(r => r.DeleteAsync(companyEntity), Times.Once);
    }

    [Fact]
    public async Task BusCompany_DeleteAsync_CompanyNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var companyId = 123;
        _mockCompanyRepo.Setup(r => r.GetByIdAsync(companyId)).ReturnsAsync((BusCompanyEntity?)null);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteBusCompanyAsync(companyId));

        Assert.Equal("Bus Company Not Found!", ex.Message);
        _mockCompanyRepo.Verify(r => r.GetByIdAsync(companyId), Times.Once);
        _mockCompanyRepo.Verify(r => r.DeleteAsync(It.IsAny<BusCompanyEntity>()), Times.Never);
    }


    [Fact]
    public async Task BusCompanies_GetAllAsync_WithFilter_ReturnsCompanies()
    {
        // Arrange
        var filter = "test";
        var companyEntities = new List<BusCompanyEntity>
        {
            new() { Id = 1, CompanyName = "Test Company 1" },
            new() { Id = 2, CompanyName = "Test Company 2" }
        };
        var companies = new List<BusCompany>
        {
            new() { Id = 1, CompanyName = "Test Company 1" },
            new() { Id = 2, CompanyName = "Test Company 2" }
        };

        _mockCompanyRepo.Setup(r => r.GetAllBusCompanyAsync(filter)).ReturnsAsync(companyEntities);
        _mockMapper.Setup(m => m.Map<IEnumerable<BusCompany>>(companyEntities)).Returns(companies);

        // Act
        var result = await _service.GetAllBusCompaniesAsync(filter);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }


    [Fact]
    public async Task BusCompany_GetByIdAsync_CompanyExists_ReturnsCompany()
    {
        // Arrange
        var companyId = 1;
        var companyEntity = new BusCompanyEntity { Id = companyId, CompanyName = "Test Company" };
        var company = new BusCompany { Id = companyId, CompanyName = "Test Company" };

        _mockCompanyRepo.Setup(r => r.GetBusCompanyByIdAsync(companyId)).ReturnsAsync(companyEntity);
        _mockMapper.Setup(m => m.Map<BusCompany>(companyEntity)).Returns(company);

        // Act
        var result = await _service.GetBusCompanyByIdAsync(companyId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(companyId, result.Id);
    }

    [Fact]
    public async Task BusCompany_GetByIdAsync_CompanyNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var companyId = 1;
        _mockCompanyRepo.Setup(r => r.GetBusCompanyByIdAsync(companyId)).ReturnsAsync((BusCompanyEntity?)null);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _service.GetBusCompanyByIdAsync(companyId));

        Assert.Equal("Bus Company with id 1 Not Found!", ex.Message);
    }


    [Fact]
    public async Task BusCompany_UpdateAsync_CompanyExists_UpdatesCompany()
    {
        // Arrange
        var companyId = 1;
        var existingCompany = new BusCompanyEntity
        {
            Id = companyId,
            CompanyName = "Old Name",
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            UpdatedAt = DateTime.UtcNow.AddDays(-1)
        };
        var updatedCompany = new BusCompany
        {
            CompanyName = "New Name",
            OwnerName = "New Owner",
            RegistrationNo = "NEW123"
        };

        _mockCompanyRepo.Setup(r => r.GetByIdAsync(companyId)).ReturnsAsync(existingCompany);

        // Act
        await _service.UpdateBusCompanyAsync(companyId, updatedCompany);

        // Assert
        Assert.Equal("New Name", existingCompany.CompanyName);
        Assert.Equal("New Owner", existingCompany.OwnerName);
        Assert.Equal("NEW123", existingCompany.RegistrationNo);
        Assert.True(existingCompany.UpdatedAt > DateTime.UtcNow.AddSeconds(-1));
        _mockCompanyRepo.Verify(r => r.UpdateAsync(existingCompany), Times.Once);
    }

    [Fact]
    public async Task BusCompany_UpdateAsync_CompanyNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var companyId = 1;
        var updatedCompany = new BusCompany { CompanyName = "New Name" };
        _mockCompanyRepo.Setup(r => r.GetByIdAsync(companyId)).ReturnsAsync((BusCompanyEntity?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateBusCompanyAsync(companyId, updatedCompany));
        _mockCompanyRepo.Verify(r => r.UpdateAsync(It.IsAny<BusCompanyEntity>()), Times.Never);
    }

    #endregion



    #region Bus Tests
    [Fact]
    public async Task Bus_CreateAsync_ValidBus_ReturnsCreatedBus()
    {
        // Arrange
        var bus = new Bus
        {
            BusName = "Test Bus",
            BusCompanyId = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var busEntity = new BusEntity { Id = 1, BusName = "Test Bus" };
        var createdBus = new Bus { Id = 1, BusName = "Test Bus" };

        _mockCompanyRepo.Setup(r => r.IsExistsAsync(bus.BusCompanyId)).ReturnsAsync(true);
        _mockMapper.Setup(m => m.Map<BusEntity>(bus)).Returns(busEntity);
        _mockBusRepo.Setup(r => r.AddAsync(busEntity)).ReturnsAsync(busEntity);
        _mockMapper.Setup(m => m.Map<Bus>(busEntity)).Returns(createdBus);

        // Act
        var result = await _service.CreateBusAsync(bus);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Bus", result.BusName);

        _mockCompanyRepo.Verify(r => r.IsExistsAsync(1), Times.Once);
        _mockBusRepo.Verify(r => r.AddAsync(busEntity), Times.Once);

    }


    [Fact]
    public async Task Bus_CreateAsync_BusCompanyNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var bus = new Bus { BusCompanyId = 1 };
        _mockCompanyRepo.Setup(r => r.IsExistsAsync(bus.BusCompanyId)).ReturnsAsync(false);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _service.CreateBusAsync(bus));

        Assert.Equal("The selected company for Creating a new Bus Not Found!", ex.Message);
        _mockCompanyRepo.Verify(r => r.IsExistsAsync(1), Times.Once);
        _mockBusRepo.Verify(r => r.AddAsync(It.IsAny<BusEntity>()), Times.Never);
    }


    [Fact]
    public async Task Bus_CreateAsync_NullInput_NullReferenceException()
    {
        // Arrange
        Bus? bus = null;

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => _service.CreateBusAsync(bus!));
    }


    [Fact]
    public async Task Bus_DeleteAsync_BusExists_DeletesBus()
    {
        // Arrange
        var busId = 1;
        var busEntity = new BusEntity { Id = busId };

        _mockBusRepo.Setup(r => r.GetByIdAsync(busId)).ReturnsAsync(busEntity);

        // Act
        await _service.DeleteBusAsync(busId);

        // Assert
        _mockBusRepo.Verify(r => r.GetByIdAsync(busId), Times.Once);
        _mockBusRepo.Verify(r => r.DeleteAsync(busEntity), Times.Once);
    }

    [Fact]
    public async Task Bus_DeleteAsync_BusNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var busId = 1;
        _mockBusRepo.Setup(r => r.GetByIdAsync(busId)).ReturnsAsync((BusEntity?)null);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteBusAsync(busId));

        Assert.Equal("Bus with id: 1 Not Found!", ex.Message);
        _mockBusRepo.Verify(r => r.GetByIdAsync(busId), Times.Once);
        _mockBusRepo.Verify(r => r.DeleteAsync(It.IsAny<BusEntity>()), Times.Never);
    }


    [Fact]
    public async Task Bus_GetAllAsync_WithFilter_ReturnsBuses()
    {
        // Arrange
        var filter = "test";
        var busEntities = new List<BusEntity>
        {
            new() { Id = 1, BusName = "Test Bus 1" },
            new() { Id = 2, BusName = "Test Bus 2" }
        };
        var buses = new List<Bus>
        {
            new() { Id = 1, BusName = "Test Bus 1" },
            new() { Id = 2, BusName = "Test Bus 2" }
        };

        _mockBusRepo.Setup(r => r.GetAllBusesAsync(filter)).ReturnsAsync(busEntities);
        _mockMapper.Setup(m => m.Map<IEnumerable<Bus>>(busEntities)).Returns(buses);

        // Act
        var result = await _service.GetAllBusesAsync(filter);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }


    [Fact]
    public async Task Bus_GetByIdAsync_BusExists_ReturnsBus()
    {
        // Arrange
        var busId = 1;
        var busEntity = new BusEntity { Id = busId, BusName = "Test Bus" };
        var bus = new Bus { Id = busId, BusName = "Test Bus" };

        _mockBusRepo.Setup(r => r.GetByIdAsync(busId)).ReturnsAsync(busEntity);
        _mockMapper.Setup(m => m.Map<Bus>(busEntity)).Returns(bus);

        // Act
        var result = await _service.GetBusByIdAsync(busId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(busId, result.Id);
    }

    [Fact]
    public async Task Bus_GetByIdAsync_BusNotFound_ReturnsNull()
    {
        // Arrange
        var busId = 1;
        _mockBusRepo.Setup(r => r.GetByIdAsync(busId)).ReturnsAsync((BusEntity?)null);

        // Act
        var result = await _service.GetBusByIdAsync(busId);

        // Assert
        Assert.Null(result);
    }


    [Fact]
    public async Task Bus_UpdateAsync_BusExists_UpdatesBus()
    {
        // Arrange
        var busId = 1;
        var existingBus = new BusEntity
        {
            Id = busId,
            BusName = "Old Bus",
            BusRegNo = "OLD123",
            UpdatedAt = DateTime.UtcNow.AddDays(-1)
        };
        var updatedBus = new Bus
        {
            BusName = "New Bus",
            DriverName = "New Driver",
            BusRegNo = "NEW123",
            BusType = BusType.AC,
            TotalCapacity = "50"
        };

        _mockBusRepo.Setup(r => r.GetByIdAsync(busId)).ReturnsAsync(existingBus);

        // Act
        await _service.UpdateBusAsync(busId, updatedBus);

        // Assert
        Assert.Equal("New Bus", existingBus.BusName);
        Assert.Equal("New Driver", existingBus.DriverName);
        Assert.Equal("NEW123", existingBus.BusRegNo);
        Assert.Equal(BusType.AC, existingBus.BusType);
        Assert.Equal("50", existingBus.TotalCapacity);
        Assert.True(existingBus.UpdatedAt > DateTime.UtcNow.AddSeconds(-1));
        _mockBusRepo.Verify(r => r.UpdateAsync(existingBus), Times.Once);
    }

    [Fact]
    public async Task Bus_UpdateAsync_BusNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var busId = 1;
        var updatedBus = new Bus { BusName = "New Bus" };
        _mockBusRepo.Setup(r => r.GetByIdAsync(busId)).ReturnsAsync((BusEntity?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateBusAsync(busId, updatedBus));
        _mockBusRepo.Verify(r => r.UpdateAsync(It.IsAny<BusEntity>()), Times.Never);
    }

    #endregion
}
