using System.Data;

namespace WebApi.Storage.Contracts
{
    public interface IDbConnectionAdapter
    {
        IDbConnection GetDbConnection();
    }
}
