using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace ContactsAPI.Models
{
    public class ContactDbContext
    {
        NpgsqlConnection connection_;
        public ContactDbContext(IConfiguration configuration)
        {
            connection_ = new NpgsqlConnection(configuration.GetConnectionString("ContactsDB"));
        }
        public async Task<Contact> GetContactById(Guid uuid)
        {
            await connection_.OpenAsync();
            using NpgsqlCommand command = new NpgsqlCommand($"SELECT uuid, first_name, " +
                $"last_name, number, creation_time FROM contacts WHERE uuid = '{uuid}'", connection_);
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
            await connection_.OpenAsync();
            using NpgsqlCommand command = new NpgsqlCommand(
                $"SELECT uuid, first_name, last_name, number, creation_time FROM contacts", connection_);
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
            await connection_.OpenAsync();
            new NpgsqlCommand($"DELETE FROM contacts WHERE uuid = '{uuid}'", connection_)
                .ExecuteNonQuery();
        }

        public async Task<bool> CreateContact(Contact contact)
        {
            await connection_.OpenAsync();
            try
            {
                new NpgsqlCommand($"INSERT INTO contacts " +
                    $"(uuid, first_name, last_name, number, creation_time) " +
                    $"VALUES ('{contact.Uuid}', " +
                    $"'{contact.FirstName}', " +
                    $"'{contact.LastName}', " +
                    $"'{contact.Number}', " +
                    $"'{contact.CreationTime}')", connection_)
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
                await connection_.OpenAsync();
                new NpgsqlCommand($"UPDATE contacts SET " +
                    $"first_name = '{contact.FirstName}', " +
                    $"last_name = '{contact.LastName}', " +
                    $"number = '{contact.Number}'" +
                    $"WHERE uuid = '{uuid}'", connection_)
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
