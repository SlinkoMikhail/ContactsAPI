using System;
namespace ContactsAPI.Models
{
    public class Contact
    {
        public Guid Uuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Number { get; set; }
        public DateTimeOffset CreationTime { get; set; }

        public Contact()
        {
            Uuid = Guid.NewGuid();
            CreationTime = DateTimeOffset.UtcNow;
        }
    }
}
