using System.Data;
using Gladwyne.API.Data;
using Gladwyne.Models;
using Gladwyne.Controllers.Contacts;
using Microsoft.AspNetCore.Mvc;
using Gladwyne.Models.Responses;

namespace Gladwyne.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //The controller name is defined by the string "User" in "UserController" - the class.
    public class UserController : ControllerBase
    {
        DataContextDapper _dapper;
        public UserController(IConfiguration configuration)
        {
            //User Constructor
            _dapper = new DataContextDapper(configuration);

        }

        [HttpGet("GetUsers")]
        public ActionResult<ItemResponse<User>> GetUsers()
        {
            int responseCode = 200;
            BaseResponse response = null;
            string sqlGetUsersQuery = "EXECUTE GladwyneSchema.Users_GetAll_Procedure";
            try
            {
                IEnumerable<User> users = _dapper.LoadData<User>(sqlGetUsersQuery);
                if(users != null)
                {
                    response = new ItemsResponse<User> {Items = users};
                }
                else
                {
                    responseCode = 404;
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
        [HttpGet("SingleUser/{userId}")]
        public ActionResult<ItemResponse<User>> GetSingleUser(int userId)
        {
            int responseCode = 200;
            BaseResponse response = null;
            string sqlGetSingleUserProcedure = "EXECUTE [GladwyneSchema].[Users_GetOne_Procedure]" + "@UserId=" + userId.ToString();
            try
            {
                User users = _dapper.LoadDataSingle<User>(sqlGetSingleUserProcedure);
                if(users != null)
                {
                    response = new ItemResponse<User> {Item = users};
                }
                else
                {
                    responseCode = 404;
                    response = new ErrorResponse("Application Resource Not Found");
                }
            }
            catch (Exception exception)
            {
                responseCode = 500;
                response = new ErrorResponse(exception.Message);
            }
            return StatusCode(responseCode, response);
        }

        [HttpPut("UserEdit")]
        public IActionResult EditUser(User user)
        {
            int responseCode = 200;
            BaseResponse response = null;
            ActionResult<ItemResponse<User>> responseFromGet = null;
            string sqlUpdate = $"[GladwyneSchema].[Users_Update_Procedure] @UserId={user.UserId}, @FirstName='{user.FirstName}', @LastName='{user.LastName}', @Email='{user.Email}'";
            try
            {
                responseFromGet = GetSingleUser(user.UserId);
                if(responseFromGet != null)
                {
                    _dapper.ExecuteSql(sqlUpdate);
                    response = new SuccessResponse();
                }
                else
                {
                    responseCode = 404;
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


        [HttpPost]
        public ActionResult<ItemResponse<UserDTO>> PostUser(UserDTO user)
        {
            int responseCode = 200;
            BaseResponse response = null;
            string sqlAddUser = $"[GladwyneSchema].[Users_INSERT_Procedure] @FirstName='{user.FirstName}', @LastName='{user.LastName}', @Email='{user.Email}'";
            try
            {
                _dapper.ExecuteSql(sqlAddUser);
                response = new SuccessResponse();
            }
            catch (Exception exception)
            {
                response = new ErrorResponse(exception.Message);
            }
            return StatusCode(responseCode, response);
        }

        [HttpDelete]
        public IActionResult DeleteUser(int userId)
        {
            int responseCode = 200;
            BaseResponse response = null;
            string sqlDeleteUser = $"DELETE FROM [GladwyneSchema].Users Where UserId={userId}";
            try
            {
                _dapper.ExecuteSql(sqlDeleteUser);
                response = new SuccessResponse();
            }
            catch (Exception exception)
            {
                responseCode = 500;
                response = new ErrorResponse("Application Resource Not Found");
            }
            return StatusCode(responseCode, response);
        }
    }
}