using Npgsql;
using System.Threading.Tasks;

namespace ContactsAPI.Models
{
    public class NpgsqlConnectionFactory : IConnectionFactory
    {
        private readonly string connectionString_;
        public NpgsqlConnectionFactory(string connectionString)
        {
            connectionString_ = connectionString;
        }

        public async Task<NpgsqlConnection> CreateNpgsqlConnection()
        {
            var connection = new NpgsqlConnection(connectionString_);
            await connection.OpenAsync();
            return connection;
        }
    }
}
