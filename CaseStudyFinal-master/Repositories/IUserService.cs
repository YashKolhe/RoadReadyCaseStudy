using RoadReady.Models;

namespace RoadReady.Repositories
{
    public interface IUserService
    {
        List<User> GetAllUsers();
        User GetUserById(int id);
        int AddUser(User user);
        string UpdateUser(User user);
        string DeleteUser(int id);
    }
}
