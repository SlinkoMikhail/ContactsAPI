using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ContactsAPI.Models
{
    public interface IContactRepository
    {
        Task<List<Contact>> GetAllContacts();
        Task<Contact> GetContactById(Guid id);
        Task<bool> CreateContact(Contact contact);
        Task<bool> UpdateContact(string id, Contact contact);
        Task DeleteContactById(Guid id);
    }
}