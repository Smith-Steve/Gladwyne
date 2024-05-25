using Gladwyne.API.Interfaces;


namespace Gladwyne.API.Data
{
    public class UserRepository : IUserRepository
    {
        DataContextEF _entityFramework;
        public UserRepository(IConfiguration configuration)
        {
            //User Constructor
            _entityFramework = new DataContextEF(configuration);
        }

        public bool SaveChanges()
        {
            return _entityFramework.SaveChanges() > 0;
        }

        public void AddEntity<T>(T entityToAdd)
        {
            if(entityToAdd != null)
            {
                _entityFramework.Add(entityToAdd);
            }
        }

        public void RemoveEntity<T>(T entityToAdd)
        {
            if(entityToAdd != null)
            {
                _entityFramework.Add(entityToAdd);
            }
        }
    }
}