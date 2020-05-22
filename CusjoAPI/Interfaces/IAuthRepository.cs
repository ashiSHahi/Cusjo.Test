using CusjoAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CusjoAPI.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string passwod);

        Task<User> Login(string username, string password);

        Task<bool> UserExists(string username);
        Task<bool> UserNameExists(string email);
    }
}
