using System;
using Dapper;

namespace WebApi.Storage.Contracts.Entities
{
    [Table("TodoItems")]
    public class TodoItemEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public bool IsCompleted { get; set; } = false;
    }
}