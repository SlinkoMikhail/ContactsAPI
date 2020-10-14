using Npgsql;
using System.Threading.Tasks;

namespace ContactsAPI.Models
{
    public interface IConnectionFactory
    {
        Task<NpgsqlConnection> CreateNpgsqlConnection();
    }
}
