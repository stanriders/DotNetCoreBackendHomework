using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using WebApi.Storage.Contracts;

namespace WebApi.Storage
{
    public class NpgsqlConnectionAdapter : IDbConnectionAdapter
    {
        private readonly string _connectionString;

        public NpgsqlConnectionAdapter(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("postgres");
        }

        public IDbConnection GetDbConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}
