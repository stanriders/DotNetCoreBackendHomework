using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.BusinessLogic.Contracts.AddTodoItem;
using WebApi.BusinessLogic.Contracts.GetTodoItem;
using WebApi.BusinessLogic.Contracts.GetTodoItemList;
using WebApi.BusinessLogic.Contracts.UpdateTodoItem;
using WebApi.BusinessLogic.RequestHandlers;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("todoItems")]
    public class TodoItemsController : ControllerBase
    {
        private readonly GetTodoItemRequestHandler _getTodoItemRequestHandler;
        private readonly AddTodoItemRequestHandler _addTodoItemRequestHandler;
        private readonly GetAllTodoItemsRequestHandler _getAllTodoItemRequestHandler;
        private readonly UpdateTodoItemRequestHandler _updateTodoItemRequestHandler;

        public TodoItemsController(
            GetTodoItemRequestHandler getTodoItemRequestHandler,
            AddTodoItemRequestHandler addTodoItemRequestHandler,
            GetAllTodoItemsRequestHandler getAllTodoItemRequestHandler,
            UpdateTodoItemRequestHandler updateTodoItemRequestHandler
        )
        {
            _getTodoItemRequestHandler = getTodoItemRequestHandler;
            _addTodoItemRequestHandler = addTodoItemRequestHandler;
            _getAllTodoItemRequestHandler = getAllTodoItemRequestHandler;
            _updateTodoItemRequestHandler = updateTodoItemRequestHandler;
        }

        [HttpGet]
        public Task<GetTodoItemListResponse> GetAllTodoItemsAsync()
        {
            return _getAllTodoItemRequestHandler.HandleAsync();
        }

        [HttpGet("{id:guid}")]
        public Task<GetTodoItemResponse> GetTodoItemAsync(Guid id)
        {
            return _getTodoItemRequestHandler.HandleAsync(id);
        }

        [HttpPost]
        public Task<AddTodoItemResponse> AddTodoItemAsync([FromBody] AddTodoItemRequest request)
        {
            return _addTodoItemRequestHandler.HandleAsync(request);
        }

        [HttpPut("{id:guid}")]
        public Task UpdateTodoItemAsync(Guid id, [FromBody] UpdateTodoItemRequest request)
        {
            return _updateTodoItemRequestHandler.HandleAsync(id, request);
        }
    }
}