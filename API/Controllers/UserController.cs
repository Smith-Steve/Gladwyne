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
            string sqlGetUsersQuery = "Select UserId, FirstName, LastName, Email from GladwyneSchema.Users";
            IEnumerable<User> users = _dapper.LoadData<User>(sqlGetUsersQuery);
            return users;
        }

        [HttpGet("GetSingleUser/{userId}")]
        public User GetSingleUser(int userId)
        {
            string sqlGetSingleUserQuery = $"Select UserId, FirstName, LastName, Email from GladwyneSchema.Users WHERE UserId={userId}";
            User users = _dapper.LoadDataSingle<User>(sqlGetSingleUserQuery);
            return users;
        }

        [HttpPut]
        public IActionResult EditUser(User user)
        {
            try
            {
                GetSingleUser(user.UserId);
                string sqlUpdate = @"
                UPDATE GladwyneSchema.Users
                SET [FirstName] = '" + user.FirstName + 
                    "', [LastName] = '" + user.LastName +
                    "', [Email] = '" + user.Email +
                "' WHERE UserId = " + user.UserId;
                if(_dapper.ExecuteSql(sqlUpdate))
                {
                    return Ok();
                }
                else
                {
                    throw new Exception($"Failed to User: {user.FirstName} {user.LastName}");
                }
            }
            catch
            {
                throw new Exception("Please check the user and try again.");
            }
        }

        [HttpPost]
        public IActionResult PostUser(UserDTO user)
        {
            string sqlAddUser = $"INSERT INTO GladwyneSchema.USERS(FirstName, Lastname, Email) VALUES ('{user.FirstName}', '{user.LastName}', '{user.Email}')";
            Console.WriteLine(sqlAddUser);
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