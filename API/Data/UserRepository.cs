


namespace Gladwyne.API.Data
{
    public class UserRepository
    {
        DataContextEF _entityFramework;
        public UserRepository(IConfiguration configuration)
        {
            //User Constructor
            _entityFramework = new DataContextEF(configuration);
        }
    }
}