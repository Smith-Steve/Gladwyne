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
    }
}