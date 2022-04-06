
using System.Threading.Tasks;
using AutoMapper;
using WebApi.BusinessLogic.Contracts.GetTodoItemList;
using WebApi.Storage.Contracts.Repositories;

namespace WebApi.BusinessLogic.RequestHandlers
{
    public class GetAllTodoItemsRequestHandler
    {
        private readonly ITodoItemRepository _todoItemRepository;
        private readonly IMapper _mapper;

        public GetAllTodoItemsRequestHandler(ITodoItemRepository todoItemRepository, IMapper mapper)
        {
            _todoItemRepository = todoItemRepository;
            _mapper = mapper;
        }

        public async Task<GetTodoItemListResponse> HandleAsync()
        {
            var item = await _todoItemRepository.GetAllAsync();

            return _mapper.Map<GetTodoItemListResponse>(item);
        }
    }
}
