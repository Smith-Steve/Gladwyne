using Gladwyne.API.Data;
using Gladwyne.Models;
using Gladwyne.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gladwyne.API.Interfaces;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;

namespace Gladwyne.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    //Controller "Organization"
    public class OrganizationController : ControllerBase
    {
        // DataContextDapper _dapper;
        IOrganizationService _organizationService;
        public OrganizationController(IOrganizationService organizationService)
        {
            //Class Initializer
            // _dapper = new DataContextDapper(configuration);
            _organizationService = organizationService;
        }

        //Organization Delete Controller
        [HttpDelete("OrganizationDelete/{orgId}")]
        public IActionResult DeleteOrganization(int orgId)
        {
                        //This Delete Statement Won't Work Right Now Because If An Organization Is Deleted,
            //But there is something in one of the supporting tables supporting it, it won't delete.
            int responseCode = 200;
            BaseResponse respone = null;
            string sqlDeleteOrganization = $"[GladwyneSchema].[Organization_DELETE_Procedure] @OrgId = {orgId}";
            try
            {
                // _dapper.ExecuteSql(sqlDeleteOrganization);
                respone = new SuccessResponse();
            }
            catch (Exception exception)
            {
                responseCode = 500;
                respone = new ErrorResponse("Application Resource Not Found");
            }
            return StatusCode(responseCode, responseCode);
        }
        
        //Organization Post Controller
        [HttpPost("OrganizationPost")]
        public ActionResult<ItemResponse<int>> AddOrganization(OrganizationDTO organization)
        {
            int responseCode = 200;
            BaseResponse response = null;
            string sqlAddOrganization = $"[GladwyneSchema].[Organization_INSERT_Procedure] @OrgName='{organization.OrgName}', @OrgDescription='{organization.OrgDescription}', @OrgIndustry='{organization.OrgIndustry}', @OrgWebsite='{organization.OrgWebsite}', @OrgActive='{organization.OrgActive}'";
            try
            {
                // _dapper.ExecuteSql(sqlAddOrganization);
                response = new SuccessResponse();
            }
            catch(Exception exception)
            {
                response = new ErrorResponse("Unable To Add Organization");
            }
            return StatusCode(responseCode, response);
        }

        //Organization Put Controller
        [HttpPut("OrganizationPut/{orgId}")]
        public IActionResult EditOrganization(Organization organization, int orgId)
        {
            int statusCode = 200;
            BaseResponse response = null;
            ActionResult<ItemResponse<Organization>> responseFromGet = null;
            string sqlUpdateOrganization = $"[GladwyneSchema].[Organization_UPDATE_Procedure] @OrgDescription = '{organization.OrgDescription}', @OrgIndustry = '{organization.OrgIndustry}', @OrgWebsite = '{organization.OrgWebsite}', @OrgActive = {organization.OrgActive}, @OrgId = {orgId}";
            try
            {
                responseFromGet = GetSingleOrganization(orgId);
                if(responseFromGet.Value != null)
                {
                    statusCode = 500;
                    response = new ErrorResponse("Resource Does Not Exist");
                }
                else
                {
                    // _dapper.ExecuteSql(sqlUpdateOrganization);
                    response = new SuccessResponse();
                }
            }
            catch(Exception exception)
            {
                statusCode = 500;
                response = new ErrorResponse(exception.Message);
            }
            return StatusCode(statusCode,response);
        }

        //Get Organization By ID
        [HttpGet("OrganizationGet/GetOne/{orgId}")]
        public ActionResult<ItemResponse<Organization>> GetSingleOrganization(int orgId)
        {
            int responseCode = 200;
            BaseResponse response = null;
            string sqlGetOrganization = $"[GladwyneSchema].[Organization_GETONE_Procedure] @OrgId = {orgId}";
            try
            {
                // Organization organization = _dapper.LoadDataSingle<Organization>(sqlGetOrganization);
                // if(organization == null)
                // {
                //     throw new Exception("Organization Is Equal To Null");
                // }
                // else
                // {
                //     response = new ItemResponse<Organization> {Item = organization};
                // }
            }
            catch (Exception exception)
            {
                responseCode = 500;
                response = new ErrorResponse(exception.Message);
            }
            return StatusCode(responseCode, response);
        }

        [HttpGet("OrganizationGet/GetOne2/{orgId}")]
        public ActionResult<ItemResponse<Organization>> GetOne(int orgId)
        {
            int responseCode = 200;
            BaseResponse response = null;
            string sqlGetOrganization = $"[GladwyneSchema].[Organization_GETONE_Procedure] @OrgId = {orgId}";
            try
            {
                // Organization organization = _dapper.LoadDataSingle<Organization>(sqlGetOrganization);
                Organization organization = _organizationService.GetById(orgId);
                if(organization == null)
                {
                    throw new Exception("Organization Is Equal To Null");
                }
                else
                {
                    response = new ItemResponse<Organization> {Item = organization};
                }
            }
            catch (Exception exception)
            {
                responseCode = 500;
                response = new ErrorResponse(exception.Message);
            }
            return StatusCode(responseCode, response);
        }

        //Get All Organizations
        [HttpGet("OrganizationGetAll")]
        public ActionResult<ItemsResponse<Organization>> GetAllOrganizations()
        {
            string sqlGetAllOrganizations = "Select OrgName, OrgDescription, OrgIndustry, OrgWebsite, OrgUpdateDate, OrgCreateDate, OrgActive from  [GladwyneSchema].Organizations";
            int responseCode = 200;
            BaseResponse response = null;
            try
            {
                // IEnumerable<Organization> organizations = _dapper.LoadData<Organization>(sqlGetAllOrganizations);
                // if(organizations == null)
                // {
                //     responseCode = 404;
                //     response = new ErrorResponse("Application Resouce Not Found");
                // }
                // else
                // {
                //     response = new ItemsResponse<Organization> {Items = organizations};
                // }
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