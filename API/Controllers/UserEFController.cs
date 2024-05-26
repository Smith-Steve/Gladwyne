using System.Data;
using Gladwyne.API.Data;
using Gladwyne.Models;
using Gladwyne.Controllers.Contacts;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Gladwyne.API.Interfaces;

namespace Gladwyne.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //The controller name is defined by the string "User" in "UserController" - the class.
    public class UserEFController : ControllerBase
    {
        IMapper _mapper;
        IUserRepository _userRepository;
        public UserEFController(IConfiguration configuration, IUserRepository userRepository)
        {
            //User Constructor
            _userRepository = userRepository;
            _mapper = new Mapper(new MapperConfiguration(config => {
                config.CreateMap<UserDTO, User>();
            }));

        }

        [HttpGet("GetUsers")]
        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> users = _userRepository.GetUsers();
            return users;
        }

        [HttpGet("GetSingleUser/{userId}")]
        public User GetSingleUser(int userId)
        {
            return _userRepository.GetSingleUser(userId);
        }
        
        [HttpPost]
        public IActionResult AddUser(UserDTO user)
        {
            User userDB = _mapper.Map<User>(user);
            _userRepository.AddEntity<User>(userDB);
            if(_userRepository.SaveChanges())
            {
                return Ok("User Added");
            }
            throw new Exception("Failed To Add User");
        }
        [HttpPut("EditUser")]
        public IActionResult EditUser(User user)
        {
            User? userDB = _userRepository.GetSingleUser(user.UserId);

            if (userDB != null)
            {
                userDB.FirstName = user.FirstName;
                userDB.LastName = user.LastName;
                userDB.Email = user.Email;
                if (_userRepository.SaveChanges())
                {
                    return Ok("Updated User");
                }
                throw new Exception("Failed To Update User");
            }
            throw new Exception("Unable to Retrieve User");
        }

        [HttpDelete("DeleteUser/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            User? userDb = _userRepository.GetSingleUser(userId);

            if(userDb != null)
            {
                _userRepository.RemoveEntity<User>(userDb); 
                //'userDb' is a variable of user type. So we want to make sure we're passing that information
                //in
                if(_userRepository.SaveChanges())
                {
                    return Ok("User Successfully Removed");
                }
                throw new Exception("Failed to Delete User");
            }
            throw new Exception("User Does Not Exist");
        }
    }
}