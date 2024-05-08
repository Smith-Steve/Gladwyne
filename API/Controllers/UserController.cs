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
            string sqlGetUsersQuery = "Select UserId, FirstName, LastName, Email, Password from dbo.Users";
            IEnumerable<User> users = _dapper.LoadData<User>(sqlGetUsersQuery);
            return users;
        }

        [HttpGet("GetSingleUser/{userId}")]
        public User GetSingleUser(int userId)
        {
            string sqlGetSingleUserQuery = $"Select UserId, FirstName, LastName, Email from dbo.Users WHERE UserId={userId}";
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
                UPDATE TutorialAppSchema.Users
                SET [FirstName] = '" + user.FirstName + 
                    "', [LastName] = '" + user.LastName +
                    "', [Email] = '" + user.Email + 
                    "', [Password] = '" + user.Password + 
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
            return Ok();
        }

        [HttpPost]
        public IActionResult PostUser(User user)
        {
            try
            {
                GetSingleUser
            }
            catch
            {

            }
        }
    }
}