
using System.Threading.Tasks;
using AutoMapper;
using WebApi.BusinessLogic.Contracts.AddTodoItem;
using WebApi.Storage.Contracts.Entities;
using WebApi.Storage.Contracts.Repositories;

namespace WebApi.BusinessLogic.RequestHandlers
{
    public class AddTodoItemRequestHandler
    {
        private readonly ITodoItemRepository _todoItemRepository;
        private readonly IMapper _mapper;

        public AddTodoItemRequestHandler(ITodoItemRepository todoItemRepository, IMapper mapper)
        {
            _todoItemRepository = todoItemRepository;
            _mapper = mapper;
        }

        public async Task<AddTodoItemResponse> HandleAsync(AddTodoItemRequest request)
        {
            var entity = _mapper.Map<TodoItemEntity>(request);

            return _mapper.Map<AddTodoItemResponse>(await _todoItemRepository.AddOrUpdateAsync(entity));
        }
    }
}