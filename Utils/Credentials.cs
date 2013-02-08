using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task.Utils
{
    public struct Credentials
    {
        public string Login;
        public string Password;

        public Credentials(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
