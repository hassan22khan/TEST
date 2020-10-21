using Data.Model;
using StudentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRepository
{
    public class UserRepository :IDisposable
    {
        //This method is used to check and validate the user credentials
        StudentContext context = new StudentContext();
        public User ValidateUser(string username, string password)
        {
            return context.Users.FirstOrDefault(user =>
                       user.UserName.Equals(username, StringComparison.OrdinalIgnoreCase)
                       && user.UserPassword == password);
        }
        public void Dispose()
        {
            context.Dispose();
        }
    }
}
