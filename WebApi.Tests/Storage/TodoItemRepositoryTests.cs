using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Dapper;
using Moq;
using Moq.Dapper;
using WebApi.Storage;
using WebApi.Storage.Contracts;
using WebApi.Storage.Contracts.Entities;
using Xunit;

namespace WebApi.Tests.Storage
{
    public class TodoItemRepositoryTests
    {
        private readonly Mock<IDbConnectionAdapter> _dbAdapterMock;
        private readonly Mock<DbConnection> _dbMock;
        private readonly TodoItemRepository _repository;

        public TodoItemRepositoryTests()
        {
            _dbAdapterMock = new Mock<IDbConnectionAdapter>(MockBehavior.Loose);
            _dbMock = new Mock<DbConnection>();

            _repository = new TodoItemRepository(_dbAdapterMock.Object);

            _dbAdapterMock.Setup(x => x.GetDbConnection()).Returns(_dbMock.Object);
        }

        [Fact]
        public async Task GetAsyncQueriesDatabase()
        {
            var entity = new TodoItemEntity
            {
                Id = Guid.NewGuid(),
                Title = "Test",
                IsCompleted = true
            };

            _dbMock.SetupDapperAsync(x => x.QueryAsync<TodoItemEntity>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDbTransaction>(), It.IsAny<int?>(), It.IsAny<CommandType>()))
                .ReturnsAsync(new[] { entity });

            var result = await _repository.GetAsync(entity.Id);

            Assert.NotNull(result);
            Assert.Equal(entity.Id, result?.Id);
            Assert.Equal(entity.Title, result?.Title);
            Assert.Equal(entity.IsCompleted, result?.IsCompleted);

            _dbMock.Verify();
            _dbAdapterMock.Verify();
        }

        [Fact]
        public async Task GetAllAsyncQueriesDatabase()
        {
            var entity = new TodoItemEntity
            {
                Id = Guid.NewGuid(),
                Title = "Test",
                IsCompleted = true
            };

            _dbMock.SetupDapperAsync(x => x.QueryAsync<TodoItemEntity>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDbTransaction>(), It.IsAny<int?>(), It.IsAny<CommandType>()))
                .ReturnsAsync(new[] { entity });

            var result = await _repository.GetAllAsync();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Single(result);

            Assert.Equal(entity.Id, result?[0].Id);
            Assert.Equal(entity.Title, result?[0].Title);
            Assert.Equal(entity.IsCompleted, result?[0].IsCompleted);

            _dbMock.Verify();
            _dbAdapterMock.Verify();
        }

        [Fact]
        public async Task AddOrUpdateAsyncAddsNewEntity()
        {
            var entity = new TodoItemEntity
            {
                Title = "Test",
                IsCompleted = true
            };

            _dbMock.SetupDapperAsync(x => x.QueryAsync<TodoItemEntity>(It.IsAny<string>(), null, null, null, null))
                .ReturnsAsync(Array.Empty<TodoItemEntity>());

            var result = await _repository.AddOrUpdateAsync(entity);

            Assert.NotNull(result);
            Assert.NotEqual(default, result.Id);
            Assert.Equal(entity.Title, result.Title);
            Assert.Equal(entity.IsCompleted, result.IsCompleted);

            _dbMock.Verify();
            _dbAdapterMock.Verify();
        }

        [Fact]
        public async Task AddOrUpdateAsyncUpdatesExistingEntity()
        {
            var entity = new TodoItemEntity
            {
                Id = Guid.NewGuid(),
                Title = "Test",
                IsCompleted = true
            };
            
            _dbMock.SetupDapperAsync(x => x.QueryAsync<TodoItemEntity>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDbTransaction>(), It.IsAny<int?>(), It.IsAny<CommandType>()))
                .ReturnsAsync(new [] {entity});

            var result = await _repository.AddOrUpdateAsync(entity);

            Assert.NotNull(result);
            Assert.Equal(entity.Id, result.Id);
            Assert.Equal(entity.Title, result.Title);
            Assert.Equal(entity.IsCompleted, result.IsCompleted);

            _dbMock.Verify();
            _dbAdapterMock.Verify();
        }
    }
}