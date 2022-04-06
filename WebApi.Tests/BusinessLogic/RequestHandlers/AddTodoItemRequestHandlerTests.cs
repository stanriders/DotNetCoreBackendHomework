
using System;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using WebApi.BusinessLogic.Contracts.AddTodoItem;
using WebApi.BusinessLogic.RequestHandlers;
using WebApi.Mappings;
using WebApi.Storage.Contracts.Entities;
using WebApi.Storage.Contracts.Repositories;
using Xunit;

namespace WebApi.Tests.BusinessLogic.RequestHandlers
{
    public class AddTodoItemRequestHandlerTests
    {
        private readonly AddTodoItemRequestHandler _handler;
        private readonly Mock<ITodoItemRepository> _repositoryMock;

        public AddTodoItemRequestHandlerTests()
        {
            _repositoryMock = new Mock<ITodoItemRepository>();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());

            _handler = new AddTodoItemRequestHandler(_repositoryMock.Object, config.CreateMapper());
        }

        [Fact]
        public async Task HandleAsyncRuns()
        {
            const string title = "Test";
            var id = Guid.NewGuid();

            _repositoryMock.Setup(x => x.AddOrUpdateAsync(It.Is<TodoItemEntity>(o => o.Title == title)))
                .ReturnsAsync(new TodoItemEntity
                {
                    Id = id,
                    Title = title
                });

            var result = await _handler.HandleAsync(new AddTodoItemRequest { Title = title });

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);

            _repositoryMock.VerifyAll();
        }
    }
}
