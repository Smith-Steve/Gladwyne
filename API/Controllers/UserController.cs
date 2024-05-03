using Microsoft.AspNetCore.Mvc;

namespace Gladwyne.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //The controller name is defined by the string "User" in "UserController" - the class.
    public class UserController : ControllerBase
    {
        public UserController()
        {
            //User Constructor
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
    }
}