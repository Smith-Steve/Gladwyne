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
            Console.WriteLine(sqlGetOneOrgAddress);
            OrganizationAddress organizationAddress = _dapper.LoadDataSingle<OrganizationAddress>(sqlGetOneOrgAddress);
            return organizationAddress;
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
            string sqlDeleteOneAddress = $"DELETE FROM dbo.OrganizationsAddress WHERE OrgId = {orgId}";
            if(_dapper.ExecuteSql(sqlDeleteOneAddress))
            {
                return Ok();
            }
            throw new Exception("Failed to Delete User");
        }

        //Post Organization Address
        [HttpPost("Add")]
        public IActionResult PostOrganizationAddress(OrganizationAddress orgAddress)
        {
            string sqlPostAddress = $"INSERT INTO dbo.OrganizationsAddress(OrgId, OrgCountry, OrgStreetAddress, OrgStreetAddress2, OrgCity, OrgState, OrgZip) VALUES ('{orgAddress.OrgId}', '{orgAddress.OrgCountry}','{orgAddress.OrgStreetAddress}', '{orgAddress.OrgStreetAddress2}','{orgAddress.OrgCity}','{orgAddress.OrgState}', '{orgAddress.OrgZip}')";
            if(_dapper.ExecuteSql(sqlPostAddress))
            {
                return Ok();
            }
            throw new Exception("Failed to Post Organization Address");
        }

        [HttpPut("UpdateAddress")]
        public IActionResult UpdateOrganizationAddress(OrganizationAddress orgAdress)
        {
            string sqlUpdateOrgAddress = $"UPDATE dbo.OrganizationsAddress SET '{orgAdress.OrgCountry}','{orgAdress.OrgStreetAddress}', '{orgAdress.OrgStreetAddress2}','{orgAdress.OrgCity}','{orgAdress.OrgState}', '{orgAdress.OrgZip}' WHERE OrgId = '{orgAdress.OrgId}'";
            Console.WriteLine("=====");
            Console.WriteLine(sqlUpdateOrgAddress);
            Console.WriteLine("=====");
            if(_dapper.ExecuteSql(sqlUpdateOrgAddress))
            {
                return Ok();
            }
            throw new Exception("Failed to Update Organization Address");
        }
    }
}
