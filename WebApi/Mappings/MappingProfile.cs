using System.Collections.Generic;
using AutoMapper;
using WebApi.BusinessLogic.Contracts.AddTodoItem;
using WebApi.BusinessLogic.Contracts.GetTodoItem;
using WebApi.BusinessLogic.Contracts.GetTodoItemList;
using WebApi.BusinessLogic.Contracts.UpdateTodoItem;
using WebApi.Queue.Contracts;
using WebApi.Storage.Contracts.Entities;

namespace WebApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TodoItemEntity, GetTodoItemResponse>().ReverseMap();

            CreateMap<TodoItemEntity, GetTodoItemListElement>().ReverseMap();

            CreateMap<List<TodoItemEntity>, GetTodoItemListResponse>()
                .ForMember(x=> x.Items, c => c.MapFrom(m => m));

            CreateMap<TodoItemEntity, UpdateTodoItemRequest>().ReverseMap();

            CreateMap<AddTodoItemRequest, TodoItemEntity>();
            CreateMap<TodoItemEntity, AddTodoItemResponse>();

            CreateMap<UpdateTodoItemRequest, UpdateTodoItemMessage>();
            CreateMap<UpdateTodoItemMessage, TodoItemEntity>();
        }
    }
}
