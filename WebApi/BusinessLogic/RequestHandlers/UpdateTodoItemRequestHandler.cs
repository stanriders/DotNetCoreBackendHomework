using System;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Configuration;
using WebApi.BusinessLogic.Contracts.UpdateTodoItem;
using WebApi.Queue.Contracts;

namespace WebApi.BusinessLogic.RequestHandlers
{
    public class UpdateTodoItemRequestHandler
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IMapper _mapper;
        private readonly IConfiguration _confguration;

        public UpdateTodoItemRequestHandler(ISendEndpointProvider sendEndpointProvider, IMapper mapper,
            IConfiguration confguration)
        {
            _sendEndpointProvider = sendEndpointProvider;
            _mapper = mapper;
            _confguration = confguration;
        }

        public async Task HandleAsync(Guid id, UpdateTodoItemRequest request)
        {
            var entity = _mapper.Map<UpdateTodoItemMessage>(request);
            entity.Id = id;

            var address = new Uri($"queue:{_confguration["QueueName"]}");
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(address);

            await endpoint.Send(entity);
        }
    }
}