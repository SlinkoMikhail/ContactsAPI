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
        private ContactDbContext contactsdb_;
        public ContactsController(ContactDbContext contactsdb)
        {
            contactsdb_ = contactsdb;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            return Ok(await contactsdb_.GetAllContacts());
        }

        [HttpGet("{uuid}")]
        public async Task<IActionResult> GetContact(string uuid)
        {
            Contact contact = await contactsdb_.GetContactById(Guid.Parse(uuid));
            if(contact != null)
                return Ok(contact);
            return NotFound();
        }

        [HttpDelete("{uuid}")]
        public async Task<IActionResult> DeleteContact(string uuid)
        {
            await contactsdb_.DeleteContactById(Guid.Parse(uuid));
            return NoContent();
        }
        [HttpPost]
        public async Task<IActionResult> CreateContact(Contact contact)
        {
            if (await contactsdb_.CreateContact(contact))
            {
                return Created("/contacts/" + contact.Uuid, contact);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPut("{uuid}")]
        public async Task<IActionResult> UpdateContact(string uuid, Contact contact)
        {
            if(!await contactsdb_.UpdateContact(uuid, contact))
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}
