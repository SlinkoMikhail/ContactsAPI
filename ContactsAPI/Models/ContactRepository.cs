using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactsAPI.Models
{
    public class ContactRepository : IContactRepository
    {
        private readonly IConnectionFactory connectionFactory_;
        public ContactRepository(IConnectionFactory connectionFactory)
        {
            connectionFactory_ = connectionFactory;
        }
        public async Task<Contact> GetContactById(Guid uuid)
        {
            using NpgsqlCommand command = new NpgsqlCommand($"SELECT uuid, first_name, " +
                $"last_name, number, creation_time FROM contacts WHERE uuid = '{uuid}'", await connectionFactory_.CreateNpgsqlConnection());
            using NpgsqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Contact()
                {
                    Uuid = reader.GetGuid(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Number = reader.GetString(3),
                    CreationTime = reader.GetTimeStamp(4).ToDateTime()
                };
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Contact>> GetAllContacts()
        {
            using NpgsqlCommand command = new NpgsqlCommand(
                $"SELECT uuid, first_name, last_name, number, creation_time FROM contacts", await connectionFactory_.CreateNpgsqlConnection());
            using NpgsqlDataReader reader = command.ExecuteReader();
            List<Contact> contacts = new List<Contact>();
            while (reader.Read())
            {
                contacts.Add(new Contact()
                {
                    Uuid = reader.GetGuid(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Number = reader.GetString(3),
                    CreationTime = reader.GetTimeStamp(4).ToDateTime()
                });
            }
            return contacts;
        }

        public async Task DeleteContactById(Guid uuid)
        {
            new NpgsqlCommand($"DELETE FROM contacts WHERE uuid = '{uuid}'", await connectionFactory_.CreateNpgsqlConnection())
                .ExecuteNonQuery();
        }

        public async Task<bool> CreateContact(Contact contact)
        {
            try
            {
                new NpgsqlCommand($"INSERT INTO contacts " +
                    $"(uuid, first_name, last_name, number, creation_time) " +
                    $"VALUES ('{contact.Uuid}', " +
                    $"'{contact.FirstName}', " +
                    $"'{contact.LastName}', " +
                    $"'{contact.Number}', " +
                    $"'{contact.CreationTime}')", await connectionFactory_.CreateNpgsqlConnection())
                    .ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateContact(string uuid, Contact contact)
        {
            try
            {
                new NpgsqlCommand($"UPDATE contacts SET " +
                    $"first_name = '{contact.FirstName}', " +
                    $"last_name = '{contact.LastName}', " +
                    $"number = '{contact.Number}'" +
                    $"WHERE uuid = '{uuid}'", await connectionFactory_.CreateNpgsqlConnection())
                    .ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
