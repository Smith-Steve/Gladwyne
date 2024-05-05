using Gladwyne.API.Data;
using Gladwyne.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gladwyne.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //Controller "Organization"
    public class OrganizationController : ControllerBase
    {
        DataContextDapper _dapper;
        public OrganizationController(IConfiguration configuration)
        {
            //Class Initializer
            _dapper = new DataContextDapper(configuration);
        }

        //Organization Delete Controller
        [HttpDelete("OrganizationDelete/{orgId}")]
        public IActionResult DeleteOrganization(int orgId)
        {
            string sqlDeleteOrganization = $"DELETE FROM dbo.Organizations WHERE OrgId = {orgId}";
            if(_dapper.ExecuteSql(sqlDeleteOrganization))
            {
                return StatusCode(200, "Delete Successful");
            }
            throw new Exception("Failed to Delete Organization.");
        }

        [HttpPost("OrganizationPost")]
        public IActionResult AddOrganization(OrganizationDTO organization)
        {
            Console.WriteLine("=== Print Out Organization ===");
            Console.WriteLine(organization.OrgName);
            Console.WriteLine("=== Print Out Organization - End ===");
            string sqlAddOrganization = @"
            INSERT INTO dbo.Organizations(
                OrgName, OrgDescription, OrgIndustry,
                OrgWebsite, orgActive,OrgUpdateDate, OrgCreateDate) VALUES ('"
                +  organization.OrgName
                + "','" + organization.OrgDescription
                + "','" + organization.OrgIndustry
                + "','" + organization.OrgWebsite
                + "','" + organization.OrgActive
                + "', GETDATE(), GETDATE() )";
            Console.WriteLine("=== START ===");
            Console.WriteLine(sqlAddOrganization);
            Console.WriteLine("=== END ===");

            // string sqlAddOrganization2 = @"INSERT 
            // INTO dbo.Organizations(
            //     [OrgName], 
            //     [OrgDescription], 
            //     [OrgIndustry],
            //     [OrgWebsite], 
            //     [OrgUpdateDate], 
            //     [OrgCreateDate], 
            //     [OrgActive]) VALUES 
            //     ('{organization.OrgName}', 
            //     '{organization.OrgDescription}', 
            //     '{organization.OrgIndustry}', 
            //     '{organization.OrgWebsite}', 
            //     GETDATE(), GETDATE(), 
            //     {organization.OrgActive})";
            if(_dapper.ExecuteSql(sqlAddOrganization))
            {
                return Ok();
            }
            throw new Exception("Failed to create new Organization.");
        }

        [HttpGet("{orgId}")]
        public Organization GetSingleOrganization(int orgId)
        {
            string sqlGetOrganization = $"Select OrgName, OrgDescription, OrgIndustry, OrgWebsite, OrgUpdateDate, OrgCreateDate, OrgActive From dbo.Organizations WHERE OrgId = {orgId}";
            Organization organization = _dapper.LoadDataSingle<Organization>(sqlGetOrganization);
            return organization;
        }
    }
}