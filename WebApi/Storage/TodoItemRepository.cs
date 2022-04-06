using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using WebApi.Storage.Contracts;
using WebApi.Storage.Contracts.Entities;
using WebApi.Storage.Contracts.Repositories;

namespace WebApi.Storage
{
    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly IDbConnectionAdapter _dbConnectionAdapter;

        public TodoItemRepository(IDbConnectionAdapter dbConnectionAdapter)
        {
            _dbConnectionAdapter = dbConnectionAdapter;
        }

        public async Task<TodoItemEntity?> GetAsync(Guid id)
        {
            using var dbConnection = _dbConnectionAdapter.GetDbConnection();

            var item = await dbConnection.GetAsync<TodoItemEntity>(id);

            return item;
        }

        public async Task<List<TodoItemEntity>?> GetAllAsync()
        {
            using var dbConnection = _dbConnectionAdapter.GetDbConnection();

            var items = await dbConnection.GetListAsync<TodoItemEntity>();

            return items.ToList();
        }

        public async Task<TodoItemEntity> AddOrUpdateAsync(TodoItemEntity entity)
        {
            using var dbConnection = _dbConnectionAdapter.GetDbConnection();

            if(entity.Id == default)
                entity.Id = await dbConnection.InsertAsync<Guid, TodoItemEntity>(entity);
            else
                await dbConnection.UpdateAsync(entity);

            return entity;
        }
    }
}