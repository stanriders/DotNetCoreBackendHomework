using System;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using WebApi.BusinessLogic.Contracts.Exceptions;
using WebApi.BusinessLogic.RequestHandlers;
using WebApi.Mappings;
using WebApi.Storage.Contracts.Entities;
using WebApi.Storage.Contracts.Repositories;
using Xunit;

namespace WebApi.Tests.BusinessLogic.RequestHandlers
{
    public class GetTodoItemRequestHandlerTests
    {
        private readonly GetTodoItemRequestHandler _handler;
        private readonly Mock<ITodoItemRepository> _repositoryMock;

        public GetTodoItemRequestHandlerTests()
        {
            _repositoryMock = new Mock<ITodoItemRepository>();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());

            _handler = new GetTodoItemRequestHandler(_repositoryMock.Object, config.CreateMapper());
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

            _repositoryMock.Setup(x => x.GetAsync(It.Is<Guid>(x => x == entity.Id)))
                .ReturnsAsync(entity);

            var result = await _handler.HandleAsync(entity.Id);

            Assert.NotNull(result);
            Assert.Equal(result.Id, entity.Id);
            Assert.Equal(result.Title, entity.Title);
            Assert.Equal(result.IsCompleted, entity.IsCompleted);

            _repositoryMock.VerifyAll();
        }

        [Fact]
        public async Task HandleAsyncReturnsNotFoundOnIncorrectId()
        {
            TodoItemEntity? returnEntity = null;

            _repositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(returnEntity);

            await Assert.ThrowsAsync<NotFoundException>(async () => await _handler.HandleAsync(new Guid()));
            
            _repositoryMock.VerifyAll();
        }
    }
}
