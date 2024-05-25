using System.Data;
using Gladwyne.API.Data;
using Gladwyne.Models;
using Gladwyne.Controllers.Contacts;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace Gladwyne.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //The controller name is defined by the string "User" in "UserController" - the class.
    public class UserEFController : ControllerBase
    {
        DataContextEF _entityFramework;
        IMapper _mapper;
        public UserEFController(IConfiguration configuration)
        {
            //User Constructor
            _entityFramework = new DataContextEF(configuration);
            _mapper = new Mapper(new MapperConfiguration(config => {
                config.CreateMap<UserDTO, User>();
            }));

        }

        [HttpGet("GetUsers")]
        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> users = _entityFramework.Users.ToList<User>();
            return users;
        }

        [HttpGet("GetSingleUser/{userId}")]
        public User GetSingleUser(int userId)
        {

            User? user = _entityFramework.Users.Where(user => user.UserId == userId)
                .FirstOrDefault<User>();
            if(user != null)
            {
                return user;
            }
            throw new Exception("Failed To Get User");
        }

        [HttpPut("EditUser")]
        public IActionResult EditUser(User user)
        {
            User? userDB = _entityFramework.Users
                .Where(u => u.UserId == user.UserId)
                .FirstOrDefault<User>(); 
            if (userDB != null)
            {
                userDB.FirstName = user.FirstName;
                userDB.LastName = user.LastName;
                userDB.Email = user.Email;
                if (_entityFramework.SaveChanges() > 0)
                {
                    return Ok("Updated User");
                }
                throw new Exception("Failed To Update User");
            }
            throw new Exception("Unable to Retrieve User");
        }

        [HttpPost]
        public IActionResult AddUser(UserDTO user)
        {
            User userDB = _mapper.Map<User>(user);
            _entityFramework.Add(userDB);
            if(_entityFramework.SaveChanges() > 0)
            {
                return Ok("User Added");
            }
            throw new Exception("Failed To Add User");
        }

        [HttpDelete("DeleteUser/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            User? userDb = _entityFramework.Users.Where(u => u.UserId == userId).FirstOrDefault<User>();

            if(userDb != null)
            {
                _entityFramework.Users.Remove(userDb);
                if(_entityFramework.SaveChanges() > 0)
                {
                    return Ok("User Successfully Removed");
                }
                throw new Exception("Failed to Delete User");
            }
            throw new Exception("User Does Not Exist");
        }
    }
}