using RoadReady.Data;
using RoadReady.Models;

namespace RoadReady.Repositories
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserById(int id)
        {
            return _context.Users.Find(id);
        }

        public int AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.UserId;
        }

        public string UpdateUser(User user)
        {
            var existingUser = _context.Users.Find(user.UserId);
            if (existingUser == null) return "User not found";

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.Role = user.Role;

            _context.SaveChanges();
            return "User updated successfully";
        }

        public string DeleteUser(int id)
        {
            var existingUser = _context.Users.Find(id);
            if (existingUser == null) return "User not found";

            _context.Users.Remove(existingUser);
            _context.SaveChanges();
            return "User deleted successfully";
        }
    }
}
