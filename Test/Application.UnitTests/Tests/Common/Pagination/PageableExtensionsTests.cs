using Application.Common.Pagination;
using MockQueryable;

namespace Application.UnitTests.Tests.Common.Pagination;
public class TestEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public decimal Value { get; set; }
}

public class TestPageableRequest : PageableRequest
{
}

public class PageableExtensionsTests
{
    private readonly IQueryable<TestEntity> _entities;

    public PageableExtensionsTests()
    {
        // Create test data
        var testData = new List<TestEntity>
        {
            new() { Id = 1, Name = "Entity A", CreatedDate = new DateTime(2023, 1, 15), Value = 10.5m },
            new() { Id = 2, Name = "Entity C", CreatedDate = new DateTime(2023, 3, 20), Value = 20.1m },
            new() { Id = 3, Name = "Entity B", CreatedDate = new DateTime(2023, 2, 10), Value = 15.3m },
            new() { Id = 4, Name = "Entity E", CreatedDate = new DateTime(2023, 5, 5), Value = 30.7m },
            new() { Id = 5, Name = "Entity D", CreatedDate = new DateTime(2023, 4, 1), Value = 25.9m },
            new() { Id = 6, Name = "Entity F", CreatedDate = new DateTime(2023, 6, 12), Value = 35.2m },
            new() { Id = 7, Name = "Entity H", CreatedDate = new DateTime(2023, 8, 8), Value = 45.6m },
            new() { Id = 8, Name = "Entity G", CreatedDate = new DateTime(2023, 7, 22), Value = 40.3m },
            new() { Id = 9, Name = "Entity J", CreatedDate = new DateTime(2023, 10, 30), Value = 55.8m },
            new() { Id = 10, Name = "Entity I", CreatedDate = new DateTime(2023, 9, 18), Value = 50.4m }
        };

        // Set up a mock queryable source
        _entities = testData.AsQueryable().BuildMock();
    }

    [Fact]
    public async Task ToPageableListAsync_WhenPageNumberIsLessThanOne_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var request = new TestPageableRequest
        {
            PageNumber = 0,
            PageSize = 5,
            OrderBy = "Id",
            OrderDirection = OrderDirectionEnum.Ascending
        };
        IOrderedQueryable<TestEntity> orderedQuery = _entities.OrderBy(e => e.Id);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            orderedQuery.ToPageableListAsync(request, CancellationToken.None));
    }

    [Fact]
    public async Task ToPageableListAsync_WhenPageSizeIsLessThanOne_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var request = new TestPageableRequest
        {
            PageNumber = 1,
            PageSize = 0,
            OrderBy = "Id",
            OrderDirection = OrderDirectionEnum.Ascending
        };
        IOrderedQueryable<TestEntity> orderedQuery = _entities.OrderBy(e => e.Id);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            orderedQuery.ToPageableListAsync(request, CancellationToken.None));
    }

    [Fact]
    public async Task ToPageableListAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Arrange
        IOrderedQueryable<TestEntity> orderedQuery = _entities.OrderBy(e => e.Id);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            orderedQuery.ToPageableListAsync(null!, CancellationToken.None));
    }

    [Fact]
    public async Task ToPageableListAsync_WhenPageNumberIsOne_ReturnsFirstPage()
    {
        // Arrange
        var request = new TestPageableRequest
        {
            PageNumber = 1,
            PageSize = 3,
            OrderBy = "Id",
            OrderDirection = OrderDirectionEnum.Ascending
        };
        IOrderedQueryable<TestEntity> orderedQuery = _entities.OrderBy(e => e.Id);

        // Act
        PageableResponse<TestEntity> result = await orderedQuery.ToPageableListAsync(request, CancellationToken.None);

        // Assert
        Assert.Equal(10, result.TotalRecords);
        Assert.Equal(3, result.PageSize);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(4, result.PageCount);
        Assert.Equal(3, result.Data.Count());
        Assert.Equal("Id", result.OrderBy);
        Assert.Equal(OrderDirectionEnum.Ascending, result.OrderDirection);

        // Verify first page content
        var data = result.Data.ToList();
        Assert.Equal(1, data[0].Id);
        Assert.Equal(2, data[1].Id);
        Assert.Equal(3, data[2].Id);
    }

    [Fact]
    public async Task ToPageableListAsync_WhenPageNumberIsTwo_ReturnsSecondPage()
    {
        // Arrange
        var request = new TestPageableRequest
        {
            PageNumber = 2,
            PageSize = 3,
            OrderBy = "Id",
            OrderDirection = OrderDirectionEnum.Ascending
        };
        IOrderedQueryable<TestEntity> orderedQuery = _entities.OrderBy(e => e.Id);

        // Act
        PageableResponse<TestEntity> result = await orderedQuery.ToPageableListAsync(request, CancellationToken.None);

        // Assert
        Assert.Equal(3, result.Data.Count());

        // Verify second page content
        var data = result.Data.ToList();
        Assert.Equal(4, data[0].Id);
        Assert.Equal(5, data[1].Id);
        Assert.Equal(6, data[2].Id);
    }

    [Fact]
    public async Task ToPageableListAsync_WithLastPage_ReturnsRemainingItems()
    {
        // Arrange
        var request = new TestPageableRequest
        {
            PageNumber = 4,
            PageSize = 3,
            OrderBy = "Id",
            OrderDirection = OrderDirectionEnum.Ascending
        };
        IOrderedQueryable<TestEntity> orderedQuery = _entities.OrderBy(e => e.Id);

        // Act
        PageableResponse<TestEntity> result = await orderedQuery.ToPageableListAsync(request, CancellationToken.None);

        // Assert
        Assert.Single(result.Data);
        Assert.Equal(10, result.Data.First().Id);
    }

    [Fact]
    public async Task ToPageableListAsync_WithOrderByNullOrEmpty_ThrowsArgumentNullException()
    {
        // Arrange
        var request = new TestPageableRequest
        {
            PageNumber = 1,
            PageSize = 5,
            OrderBy = null!,
            OrderDirection = OrderDirectionEnum.Ascending
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _entities.ToPageableListAsync(request, CancellationToken.None));

        request.OrderBy = "";
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _entities.ToPageableListAsync(request, CancellationToken.None));

        request.OrderBy = "   ";
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _entities.ToPageableListAsync(request, CancellationToken.None));
    }

    [Fact]
    public async Task ToPageableListAsync_WithAscendingOrder_OrdersDataAscending()
    {
        // Arrange
        var request = new TestPageableRequest
        {
            PageNumber = 1,
            PageSize = 10,
            OrderBy = "Name",
            OrderDirection = OrderDirectionEnum.Ascending
        };

        // Act
        PageableResponse<TestEntity> result = await _entities.ToPageableListAsync(request, CancellationToken.None);

        // Assert
        var data = result.Data.ToList();
        for (int i = 1; i < data.Count; i++)
        {
            Assert.True(string.Compare(data[i - 1].Name, data[i].Name, StringComparison.Ordinal) <= 0);
        }
    }

    [Fact]
    public async Task ToPageableListAsync_WithDescendingOrder_OrdersDataDescending()
    {
        // Arrange
        var request = new TestPageableRequest
        {
            PageNumber = 1,
            PageSize = 10,
            OrderBy = "Value",
            OrderDirection = OrderDirectionEnum.Descending
        };

        // Act
        PageableResponse<TestEntity> result = await _entities.ToPageableListAsync(request, CancellationToken.None);

        // Assert
        var data = result.Data.ToList();
        for (int i = 1; i < data.Count; i++)
        {
            Assert.True(data[i - 1].Value >= data[i].Value);
        }
    }

    [Fact]
    public async Task ToPageableListAsync_WithEmptyResults_ReturnsEmptyPage()
    {
        // Arrange
        var request = new TestPageableRequest
        {
            PageNumber = 1,
            PageSize = 10,
            OrderBy = "Id",
            OrderDirection = OrderDirectionEnum.Ascending
        };
        IQueryable<TestEntity> emptyQuery = _entities.Where(e => e.Id > 100);

        // Act
        PageableResponse<TestEntity> result = await emptyQuery.OrderBy(e => e.Id).ToPageableListAsync(request, CancellationToken.None);

        // Assert
        Assert.Empty(result.Data);
        Assert.Equal(0, result.TotalRecords);
        Assert.Equal(0, result.PageCount);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(10, result.PageSize);
    }

    [Fact]
    public async Task ToPageableListAsync_WithKeySelectorAndNoOrderBy_UsesKeySelector()
    {
        // Arrange
        var request = new TestPageableRequest
        {
            PageNumber = 1,
            PageSize = 10,
            OrderBy = "",
            OrderDirection = OrderDirectionEnum.Ascending
        };

        // Act & Assert - first verify it throws when no OrderBy and no KeySelector
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _entities.ToPageableListAsync(request, CancellationToken.None));

        // Then test with KeySelector
        PageableResponse<TestEntity> result = await _entities.ToPageableListAsync(e => e.CreatedDate, OrderDirectionEnum.Ascending, request, CancellationToken.None);

        // Assert data is ordered by CreatedDate
        var data = result.Data.ToList();
        for (int i = 1; i < data.Count; i++)
        {
            Assert.True(data[i - 1].CreatedDate <= data[i].CreatedDate);
        }
    }

    [Fact]
    public async Task ToPageableListAsync_WithKeySelectorAndOrderBy_UsesOrderBy()
    {
        // Arrange
        var request = new TestPageableRequest
        {
            PageNumber = 1,
            PageSize = 10,
            OrderBy = "Name",
            OrderDirection = OrderDirectionEnum.Ascending
        };

        // Act - provide a different KeySelector than OrderBy
        PageableResponse<TestEntity> result = await _entities.ToPageableListAsync(e => e.Id, OrderDirectionEnum.Ascending, request, CancellationToken.None);

        // Assert data is ordered by Name (from OrderBy), not Id (from KeySelector)
        var data = result.Data.ToList();
        for (int i = 1; i < data.Count; i++)
        {
            Assert.True(string.Compare(data[i - 1].Name, data[i].Name, StringComparison.Ordinal) <= 0);
        }
    }

    [Fact]
    public async Task ToPageableListAsync_WithNullKeySelector_ThrowsArgumentNullException()
    {
        // Arrange
        var request = new TestPageableRequest
        {
            PageNumber = 1,
            PageSize = 10,
            OrderBy = "",
            OrderDirection = OrderDirectionEnum.Ascending
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _entities.ToPageableListAsync<TestEntity, int>(null!, OrderDirectionEnum.Ascending, request, CancellationToken.None));
    }

    [Fact]
    public void OrderBy_ShouldOrderBySpecifiedProperty()
    {
        // Act
        var result = _entities.OrderBy("Name").ToList();

        // Assert
        var expected = result.OrderBy(e => e.Name).ToList();
        Assert.Equal(expected, result);
    }

    [Fact]
    public void OrderByDescending_ShouldOrderBySpecifiedPropertyDescending()
    {
        // Act
        var result = _entities.OrderByDescending("Value").ToList();

        // Assert
        var expected = result.OrderByDescending(e => e.Value).ToList();
        Assert.Equal(expected, result);
    }

    [Fact]
    public void OrderBy_WhenPropertyDoesNotExist_ThrowsException()
    {
        // Act & Assert
        Exception exception = Assert.ThrowsAny<Exception>(() =>
            _entities.OrderBy("NonExistentProperty").ToList());

        Assert.Contains("NonExistentProperty", exception.Message);
    }

    [Fact]
    public void OrderByDescending_WhenPropertyDoesNotExist_ThrowsException()
    {
        // Act & Assert
        Exception exception = Assert.ThrowsAny<Exception>(() =>
            _entities.OrderByDescending("NonExistentProperty").ToList());

        Assert.Contains("NonExistentProperty", exception.Message);
    }
}
