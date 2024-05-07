using Gladwyne.API.Data;
using Gladwyne.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gladwyne.Controllers
{
    public class ContactController : ControllerBase
    {
        private DataContextDapper _dapper;
        public ContactController(IConfiguration configuration)
        {
            _dapper = new DataContextDapper(configuration);
        }

        //Get All Contacts
        [HttpGet("GetAll")]
        public IEnumerable<Contact> GetAllContacts()
        {
            string sqlGetAllContacts = "Select ContactId, FirstName, LastName, Email, OrgId from dbo.Contacts";
            IEnumerable<Contact> contacts = _dapper.LoadData<Contact>(sqlGetAllContacts);
            return contacts;
        }

        //Get All Contacts From Organization

        [HttpGet("GetAllOrganization/{OrgId}")]
        public IEnumerable<Contact> GetAllContactsFromOrganization(int OrgId)
        {
            string sqlGetAllContactsFromOrganization = $"Select ContactId, FirstName, LastName, Email, OrgId from dbo.Contacts WHERE OrgId = {OrgId}";
            IEnumerable<Contact> contacts = _dapper.LoadData<Contact>(sqlGetAllContactsFromOrganization);
            return contacts;
        }

        //Get Contact By ID
        [HttpGet("GetContact/{contactId}")]
        public Contact GetSingleContact(int contactId)
        {
            string sqlGetContact = $"Select ContactId, FirstName, LastName, OrgId from dbo.Contacts where ContactId = {contactId}";
            Contact contact = _dapper.LoadDataSingle<Contact>(sqlGetContact);
            return contact;
        }

        [HttpDelete("DeleteContact/{contactId}")]
        public IActionResult DeleteContact(int contactId)
        {
            string sqlDeleteContact = $"DELETE FROM dbo.Contacts Where ContactId = {contactId}";
            if(_dapper.ExecuteSql(sqlDeleteContact))
            {
                return Ok();
            }
            throw new Exception("Failed to Delete Contact");
        }

        [HttpPost("CreateContact")]
        public IActionResult AddContact(ContactDTO contact)
        {
            string sqlAddContact = $"INSERT INTO dbo.Contacts (FirstName, LastName, Email, OrgId) VALUES ('{contact.FirstName}', '{contact.LastName}', '{contact.Email}', '{contact.OrgId}')";
            if(_dapper.ExecuteSql(sqlAddContact))
            {
                return Ok();
            }
            throw new Exception("Failed to create Contact");
        }

        [HttpPut("EditContact")]
        public IActionResult EditContactInfo(Contact contact)
        {
            string sqlUpdateContact = $"Update dbo.Contacts SET [FirstName] = {contact.FirstName}, [LastName] = {contact.LastName}, [Email] = {contact.Email} WHERE ContactId = {contact.ContactId}";
            if(_dapper.ExecuteSql(sqlUpdateContact))
            {
                return Ok();
            }
            throw new Exception("Failed To Update Contact");
        }
    }
}