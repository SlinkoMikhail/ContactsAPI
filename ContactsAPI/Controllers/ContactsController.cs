using Microsoft.AspNetCore.Mvc;
using System;
using ContactsAPI.Models;
using System.Threading.Tasks;

namespace ContactsAPI.Controllers
{
    [ApiController]
    [Route("/contacts")]
    public class ContactsController : ControllerBase
    {
        private IContactRepository contactsdb_;
        public ContactsController(IContactRepository contactsdb)
        {
            contactsdb_ = contactsdb;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            return Ok(await contactsdb_.GetAllContacts());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContact(string id)
        {
            Contact contact = await contactsdb_.GetContactById(Guid.Parse(id));
            if(contact != null)
                return Ok(contact);
            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(string id)
        {
            await contactsdb_.DeleteContactById(Guid.Parse(id));
            return NoContent();
        }
        [HttpPost]
        public async Task<IActionResult> CreateContact(Contact contact)
        {
            if (await contactsdb_.CreateContact(contact))
            {
                return Created("/contacts/" + contact.Id, contact);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPut("{uuid}")]
        public async Task<IActionResult> UpdateContact(string uuid, Contact contact)
        {
            if(await contactsdb_.UpdateContact(uuid, contact))
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
