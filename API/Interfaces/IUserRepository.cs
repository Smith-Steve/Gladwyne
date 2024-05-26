using Gladwyne.Models;
namespace Gladwyne.API.Interfaces
{
    public interface IUserRepository
    {
        //Interfaces are not methods themselves, but they are rather invocations of the methods that are within the class invoking them.
        //In this case it is the 'UserRepository' class.
          public bool SaveChanges();
          public void AddEntity<T>(T entityToAdd);
          public void RemoveEntity<T>(T entityToAdd);
          public IEnumerable<User> GetUsers();
          public User GetSingleUser(int userId);
    }
}
