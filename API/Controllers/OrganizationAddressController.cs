//Gladwyne Spaces
using Gladwyne.API.Data;
using Gladwyne.Models;
//Exteranl Spaces
// - Microsoft Spaces
using Microsoft.AspNetCore.Mvc;

//Organization Address Controller.
namespace Gladwyne.Controllers
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

        //Get All Organization Addresses (No Names of Organizations Attached to this Query)
        [HttpGet("GetAll")]
        public IEnumerable<OrganizationAddress> GetAllOrganizationAddress()
        {
            string sqlGetAllOrgAddress = "Select OrgId, OrgCountry, OrgStreetAddress, OrgStreetAddress2, OrgCity, OrgState, OrgZip from Dbo.OrganizationsAddress";
            IEnumerable<OrganizationAddress> organizationAddresses = _dapper.LoadData<OrganizationAddress>(sqlGetAllOrgAddress);
            return organizationAddresses;
        }

        //Get One Organization (No Names of Organizations Attached to Returned Address Presently.)
        [HttpGet("GetOne/{orgId}")]
        public OrganizationAddress GetOneOrganizationAddress(int orgId)
        {
            string sqlGetOneOrgAddress = $"Select OrgId, OrgCountry, OrgStreetAddress, OrgStreetAddress2, OrgCity, OrgState, OrgZip from Dbo.OrganizationsAddress Where OrgId = {orgId}";
            OrganizationAddress organizationAddress = _dapper.LoadDataSingle<OrganizationAddress>(sqlGetOneOrgAddress);
            return organizationAddress;
        }

        [HttpDelete("DeleteOne/{orgId}")]
        public IActionResult DeleteOneOrganizationAddress(int orgId)
        {
            Console.WriteLine(orgId);
            string sqlDeleteOneAddress = $"DELETE FROM dbo.OrganizationsAddress WHERE OrgId = {orgId}";
            if(_dapper.ExecuteSql(sqlDeleteOneAddress))
            {
                return Ok();
            }
            throw new Exception("Failed to Delete User");
        }
    }
}
