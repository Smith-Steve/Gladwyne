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
            return Ok();
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