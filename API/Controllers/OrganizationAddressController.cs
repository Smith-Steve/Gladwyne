//Gladwyne Spaces
using Gladwyne.API.Data;
using Gladwyne.Models;
using Gladwyne.Models.Responses;

//Exteranl Spaces
// - Microsoft Spaces
using Microsoft.AspNetCore.Mvc;

//Organization Address Controller.
namespace Gladwyne.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrganizationAddressController : ControllerBase
    {
        DataContextDapper _dapper;
        
        //Class Initializer
        public OrganizationAddressController(IConfiguration configuration)
        {
            _dapper = new DataContextDapper(configuration);
        }

        //Get One Organization (No Names of Organizations Attached to Returned Address Presently.)
        [HttpGet("GetOne/{orgId}")]
        public ActionResult<ItemResponse<OrganizationAddress>> GetOneOrganizationAddress(int orgId)
        {
            int responseCode = 200;
            BaseResponse response = null;
            string sqlGetOneOrgAddress = $"[GladwyneSchema].[OrganizationAddress_GETONE_Procedure] @OrgId = {orgId}";
            try
            {
                OrganizationAddress orgAddress = _dapper.LoadDataSingle<OrganizationAddress>(sqlGetOneOrgAddress);
                if(orgAddress != null)
                {
                    response = new ItemResponse<OrganizationAddress> { Item = orgAddress };
                }
                else
                {
                    responseCode = 400;
                    response = new ErrorResponse("Application Resource Not Found.");
                }
            }
            catch (Exception exception)
            {
                responseCode = 500;
                response = new ErrorResponse(exception.Message);
            }
            return StatusCode(responseCode, response);
        }

        //Delete One Organization Address
        [HttpDelete("DeleteOne/{orgId}")]
        public IActionResult DeleteOneOrganizationAddress(int orgId)
        {
            //Organization Addresses are stored in the OrganizationAddresses table by OrganizationID.
            //This leads to the posibility of deleting multiple addresses if they are tied
            //to the same OrgId.

            //Note for the future - 
            //Set up Addresses so there can be more than one address, but set some special
            //feature for a primary address.
            int responseCode = 200;
            BaseResponse response = null;
            string sqlDeleteOneAddress = $"[GladwyneSchema].[OrganizationAddress_DELETE_Procedure] @OrgId = {orgId}";
            try
            {
                _dapper.ExecuteSql(sqlDeleteOneAddress);
                response = new SuccessResponse();
            }
            catch (Exception exception)
            {
                responseCode = 500;
                response = new ErrorResponse("Application Resource Is Not Found.");
            }
            return StatusCode(responseCode, response);
        }

        //Post Organization Address
        [HttpPost("Add")]
        public ActionResult<ItemResponse<OrganizationAddress>> PostOrganizationAddress(OrganizationAddress orgAddress)
        {
            int responseCode = 200;
            BaseResponse response = null;
            string sqlPostAddress = $"[GladwyneSchema].[OrganizationAddress_INSERT_Procedure] @OrgId='{orgAddress.OrgId}', @OrgCountry='{orgAddress.OrgCountry}', @OrgStreetAddress='{orgAddress.OrgStreetAddress}', @OrgStreetAddress2='{orgAddress.OrgStreetAddress2}', @OrgCity='{orgAddress.OrgCity}', @OrgState='{orgAddress.OrgState}', @OrgZip='{orgAddress.OrgZip}'";
            try
            {
                _dapper.ExecuteSql(sqlPostAddress);
                response = new SuccessResponse();
            }
            catch (Exception exception)
            {
                response = new ErrorResponse(exception.Message);
            }
            return StatusCode(responseCode, response);
        }

        [HttpPut("UpdateAddress")]
        public IActionResult UpdateOrganizationAddress(OrganizationAddress orgAddress)
        {
            int responseCode = 200;
            BaseResponse response = null;
            string sqlPutAddress = $"[GladwyneSchema].[OrganizationAddress_UPDATE_Procedure] @OrgId='{orgAddress.OrgId}', @OrgCountry='{orgAddress.OrgCountry}', @OrgStreetAddress='{orgAddress.OrgStreetAddress}', @OrgStreetAddress2='{orgAddress.OrgStreetAddress2}', @OrgCity='{orgAddress.OrgCity}', @OrgState='{orgAddress.OrgState}', @OrgZip='{orgAddress.OrgZip}'";
            ActionResult<ItemResponse<OrganizationAddress>> responseFromGet = null;
            try
            {
                responseFromGet = GetOneOrganizationAddress(orgAddress.OrgId);
                if(responseFromGet != null)
                {
                    _dapper.ExecuteSql(sqlPutAddress);
                    response = new SuccessResponse();
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
                response = new ErrorResponse(exception.Message);
            }
            return StatusCode(responseCode, response);
        }
    }
}
