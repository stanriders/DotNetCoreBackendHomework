using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Moq;
using WebApi.BusinessLogic.Contracts.UpdateTodoItem;
using WebApi.BusinessLogic.RequestHandlers;
using WebApi.Mappings;
using WebApi.Queue.Contracts;
using Xunit;

namespace WebApi.Tests.BusinessLogic.RequestHandlers
{
    public class UpdateTodoItemRequestHandlerTests
    {
        private readonly UpdateTodoItemRequestHandler _handler;
        private readonly Mock<ISendEndpointProvider> _endpointProviderMock;
        private readonly Mock<ISendEndpoint> _endpointMock;

        public UpdateTodoItemRequestHandlerTests()
        {
            _endpointProviderMock = new Mock<ISendEndpointProvider>();
            _endpointMock = new Mock<ISendEndpoint>();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());

            _handler = new UpdateTodoItemRequestHandler(_endpointProviderMock.Object, config.CreateMapper(), new ConfigurationManager());
        }

        [Fact]
        public async Task HandleAsyncRuns()
        {
            var id = Guid.NewGuid();
            var request = new UpdateTodoItemRequest
            {
                Title = "Test",
                IsCompleted = true
            };

            _endpointProviderMock.Setup(x=> x.GetSendEndpoint(It.IsAny<Uri>())).ReturnsAsync(_endpointMock.Object);
            _endpointMock.Setup(x => x.Send(It.Is<UpdateTodoItemMessage>(o => o.Id == id && o.Title == request.Title && o.IsCompleted == request.IsCompleted), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _handler.HandleAsync(id, request);

            _endpointProviderMock.VerifyAll();
            _endpointMock.VerifyAll();
        }
    }
}
