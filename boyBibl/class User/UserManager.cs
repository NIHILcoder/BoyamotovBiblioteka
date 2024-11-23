using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boyBibl.class_User
{
    public class UserManager
    {
        private List<User> users = new List<User>();

        public UserManager()
        {
            // Предустановленные пользователи
            users.Add(new User("admin", "password", "admin"));
            users.Add(new User("user", "1234", "user"));
        }

        public User Authenticate(string username, string password)
        {
            return users.Find(u => u.Username == username && u.Password == password);
        }
    }
}
