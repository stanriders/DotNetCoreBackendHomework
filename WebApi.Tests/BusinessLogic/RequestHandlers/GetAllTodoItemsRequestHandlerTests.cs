using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using WebApi.BusinessLogic.RequestHandlers;
using WebApi.Mappings;
using WebApi.Storage.Contracts.Entities;
using WebApi.Storage.Contracts.Repositories;
using Xunit;

namespace WebApi.Tests.BusinessLogic.RequestHandlers
{
    public class GetAllTodoItemsRequestHandlerTests
    {
        private readonly GetAllTodoItemsRequestHandler _handler;
        private readonly Mock<ITodoItemRepository> _repositoryMock;

        public GetAllTodoItemsRequestHandlerTests()
        {
            _repositoryMock = new Mock<ITodoItemRepository>();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());

            _handler = new GetAllTodoItemsRequestHandler(_repositoryMock.Object, config.CreateMapper());
        }

        [Fact]
        public async Task HandleAsyncRuns()
        {
            var entity = new TodoItemEntity
            {
                Id = Guid.NewGuid(),
                Title = "Test",
                IsCompleted = true
            };

            _repositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<TodoItemEntity> { entity });

            var result = await _handler.HandleAsync();

            Assert.NotNull(result);
            Assert.NotEmpty(result.Items);
            Assert.Single(result.Items);

            Assert.Equal(result.Items[0].Id, entity.Id);
            Assert.Equal(result.Items[0].Title, entity.Title);
            Assert.Equal(result.Items[0].IsCompleted, entity.IsCompleted);

            _repositoryMock.VerifyAll();
        }

        [Fact]
        public async Task HandleAsyncReturnsEmptyArray()
        {
            _repositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<TodoItemEntity>());

            var result = await _handler.HandleAsync();

            Assert.NotNull(result);
            Assert.Empty(result.Items);

            _repositoryMock.VerifyAll();
        }
    }
}
