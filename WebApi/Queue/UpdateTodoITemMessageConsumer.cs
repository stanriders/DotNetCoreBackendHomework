using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using WebApi.Queue.Contracts;
using WebApi.Storage.Contracts.Entities;
using WebApi.Storage.Contracts.Repositories;

namespace WebApi.Queue
{
    public class UpdateTodoItemMessageConsumer : IConsumer<UpdateTodoItemMessage>
    {
        private readonly ITodoItemRepository _todoItemRepository;
        private readonly IMapper _mapper;

        public UpdateTodoItemMessageConsumer(ITodoItemRepository todoItemRepository, IMapper mapper)
        {
            _todoItemRepository = todoItemRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<UpdateTodoItemMessage> context)
        {
            var entity = _mapper.Map<TodoItemEntity>(context.Message);

            await _todoItemRepository.AddOrUpdateAsync(entity);
        }
    }
}