using IData;
using IRepository;
using Repository;
using StudentModels;
using System;
using System.Linq;

namespace StudentRepository
{
    public class UserRepository : IUserRepository
    {
        //This method is used to check and validate the user credentials
        private IStudentContext _context;
        public UserRepository(IStudentContext context)
        {
            _context = context;
        }
        public User ValidateUser(string username, string password)
        {
            return _context.Users.FirstOrDefault(user =>
                       user.UserName.Equals(username, StringComparison.OrdinalIgnoreCase)
                       && user.UserPassword == password);
        }
    }
}
