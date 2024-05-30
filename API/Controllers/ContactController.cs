using Gladwyne.API.Data;
using Gladwyne.Models;
using Gladwyne.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Gladwyne.Controllers.Contacts
{
    [ApiController]
    [Route("[controller]")]
    public class ContactController : ControllerBase
    {
        private DataContextDapper _dapper;
        public ContactController(IConfiguration configuration)
        {
            _dapper = new DataContextDapper(configuration);
        }

        //Get All Contacts From Organization
        [HttpGet("GetAllFromOrganization/{orgId}")]
        public ActionResult<ItemResponse<Contact>> GetAllContactsFromOrganization(int orgId)
        {
            int responseCode = 200;
            BaseResponse response = null;
            string sqlGetAllContactsOrg = $"EXECUTE [GladwyneSchema].[Contacts_GetAllFromOrganization_Procedure] @OrgId={orgId}";
            try
            {
                IEnumerable<Contact> contactsListFromOrg = _dapper.LoadData<Contact>(sqlGetAllContactsOrg);
                if(contactsListFromOrg != null)
                {
                    response = new ItemsResponse<Contact> {Items = contactsListFromOrg};
                }
                else
                {
                    responseCode = 404;
                    response = new ErrorResponse("Application Resource Could Not Be Found.");
                }
            }
            catch (Exception exception)
            {
                responseCode = 500;
                response = new ErrorResponse(exception.Message);
            }
            return StatusCode(responseCode, response);
        }

        //Get Contact By ID
        [HttpGet("GetContact/{contactId}")]
        public ActionResult<ItemResponse<Contact>> GetSingleContact(int contactId)
        {
            int responseCode = 200;
            BaseResponse response = null;
            string sqlGetContact = $"EXECUTE [GladwyneSchema].[Contact_GETONE_Procedure] @ContactId = {contactId}";
            try
            {
                Contact contact = _dapper.LoadDataSingle<Contact>(sqlGetContact);
                if(contact != null)
                {
                    response = new ItemResponse<Contact> {Item = contact};
                }
                else
                {
                    responseCode = 500;
                    response = new ErrorResponse("Resource Does Not Exist.");
                }
            }
            catch (Exception exception)
            {
                responseCode = 500;
                response = new ErrorResponse("Resource Does Not Exist.");
            }
            return StatusCode(responseCode, response);
        }

        //Delete Contact
        [HttpDelete("DeleteContact/{contactId}")]
        public IActionResult DeleteContact(int contactId)
        {
            int responseCode = 200;
            BaseResponse response = null;
            string sqlDeleteContact = $"[GladwyneSchema].[Contact_DELETE_Procedure] @ContactId={contactId}";
            try
            {
                _dapper.ExecuteSql(sqlDeleteContact);
            }
            catch(Exception exception)
            {
                responseCode = 500;
                response = new ErrorResponse("Application Resource Could Not Be Found.");
            }
            return StatusCode(responseCode, response);
        }

        //Create Contact
        [HttpPost("CreateContact")]
        public IActionResult AddContact(ContactDTO contact)
        {
            int responseCode = 200;
            BaseResponse response = null;
            string sqlAddContact = $"EXECUTE [GladwyneSchema].[Contact_INSERT_Procedure] @FirstName='{contact.FirstName}', @LastName='{contact.LastName}', @Email='{contact.Email}', @OrgId={contact.OrgId}";
            try
            {
                _dapper.ExecuteSql(sqlAddContact);
                response = new SuccessResponse();
            }
            catch (Exception exception)
            {
                responseCode = 500;
                response = new ErrorResponse($"Unable To Crease Resource: {exception.Message}");
            }
            return StatusCode(responseCode, response);
        }

        //Edit Contact
        [HttpPut("EditContact")]
        public IActionResult EditContactInfo(Contact contact)
        {
            try
            {
                GetSingleContact(contact.ContactId);
                string updateSqlContact = $"UPDATE [GladwyneSchema].Contacts SET FirstName = '{contact.FirstName}', LastName = '{contact.LastName}', Email = '{contact.Email}' WHERE ContactId = {contact.ContactId}";
                if(_dapper.ExecuteSql(updateSqlContact))
                {
                    return Ok();
                }
                else
                {
                    throw new Exception($"Failed To Update Contact: {contact.FirstName} {contact.LastName}");
                }
            }
            catch
            {
                throw new Exception("Please check the contact entered and try again.");
            }
        }
    }
}