using System.Data;
using Gladwyne.API.Data;
using Gladwyne.Models;
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

        [HttpGet("TestConnection")]
        public DateTime TestConnection()
        {
            return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
        }

        [HttpGet("Test")]
        public string[] Test(string testValue)
        {
            string[] responseArray = new string[]
            {
                "Test",
                "Test2",
                testValue
            };
            return responseArray;
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
            string sqlEditUserQuery = $"";
            return Ok();
        }

        [HttpPost]
        public IActionResult PostUser(User user)
        {
            string sqlPostUser = $"";
            return Ok();
        }
    }
}