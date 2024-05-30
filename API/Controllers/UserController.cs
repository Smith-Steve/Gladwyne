using System.Data;
using Gladwyne.API.Data;
using Gladwyne.Models;
using Gladwyne.Controllers.Contacts;
using Microsoft.AspNetCore.Mvc;

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
        public IEnumerable<User> GetUsers()
        {
            string sqlGetUsersQuery = "EXECUTE GladwyneSchema.Users_GetAll_Procedure";
            IEnumerable<User> users = _dapper.LoadData<User>(sqlGetUsersQuery);
            return users;
        }
        [HttpGet("SingleUser/{userId}")]
        public User GetSingleUser(int userId)
        {
            try
            {
                string sqlGetSingleUserProcedure = "EXECUTE [GladwyneSchema].[Users_GetOne_Procedure]" + "@UserId=" + userId.ToString();
                User users = _dapper.LoadDataSingle<User>(sqlGetSingleUserProcedure);
                return users;
            }
            catch (Exception exception)
            {
                throw new Exception("Application Resource Could Not Be Found: ", exception);
            }
        }

        [HttpPut]
        public IActionResult EditUser(User user)
        {
            try
            {
                User returnedUser = GetSingleUser(user.UserId);
                string sqlUpdate = $"[GladwyneSchema].[Users_Update_Procedure] @UserId={user.UserId}, @FirstName='{user.FirstName}', @LastName='{user.LastName}', @Email='{user.Email}'";
                if(_dapper.ExecuteSql(sqlUpdate))
                {
                    return Ok("User Updated");
                }
                else
                {
                    throw new Exception("Failed To Update User");
                };
            }
            catch
            {
                throw new Exception("Edit Check");
            }
        }


        [HttpPost]
        public IActionResult PostUser(UserDTO user)
        {
            string sqlAddUser = $"[GladwyneSchema].[Users_INSERT_Procedure] @FirstName='{user.FirstName}', @LastName='{user.LastName}', @Email='{user.Email}'";
            if(_dapper.ExecuteSql(sqlAddUser))
            {
                return Ok();
            }
            else
            {
                throw new Exception("Failed to Add User.");
            }
        }

        [HttpDelete]
        public IActionResult DeleteUser(int userId)
        {
            string sqlDeleteUser = $"DELETE FROM [GladwyneSchema].Users Where UserId={userId}";
            if(_dapper.ExecuteSql(sqlDeleteUser))
            {
                return Ok();
            }
            else
            {
                throw new Exception("Unable to create user.");
            }
        }
    }
}