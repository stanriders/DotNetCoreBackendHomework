using System.ComponentModel.DataAnnotations;

namespace WebApi.BusinessLogic.Contracts.AddTodoItem
{
    public class AddTodoItemRequest
    {
        [Required]
        public string Title { get; set; } = null!;
    }
}