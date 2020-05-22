using CusjoAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CusjoAPI.Data.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AuthRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<User> Login(string username, string password)
        {
            var user = await _dbContext.AppUsers
                .Include(_ => _.Permissions).ThenInclude(_ => _.Entities)
                .FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            

            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (var i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
                return true;
            }
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;

            CreatePaaswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _dbContext.AppUsers.AddAsync(user);

            await _dbContext.SaveChangesAsync();
            
            if(_dbContext.AppUsers.Count() == 1)
            {
                user = await _dbContext.AppUsers
                    .Include(_ => _.Permissions)
                .FirstOrDefaultAsync(x => x.Username == user.Username);
                for (var i = 1; i <= 3; i++)
                {
                    var permissions = new UserEntity_assoc
                    {
                        UserId = user.Id,
                        EntityId = i,
                        HasAccess = true
                    };
                    user.Permissions.Add(permissions);
                }
            }
            else if(_dbContext.AppUsers.Count() > 1)
            {
                user = await _dbContext.AppUsers
                    .Include(_ => _.Permissions)
                    .FirstOrDefaultAsync(x => x.Username == user.Username);
                for (var i = 1; i <= 3; i++)
                {
                    var permissions = new UserEntity_assoc
                    {
                        UserId = user.Id,
                        EntityId = i,
                        HasAccess = false
                    };
                    user.Permissions.Add(permissions);
                }
            }

            try
            {

                _dbContext.UserEntity_Assocs.AddRange(user.Permissions);
                _dbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                return user;

                throw;
            }


            user = await _dbContext.AppUsers
            .Include(_ => _.Permissions).ThenInclude(_ => _.Entities)
            .FirstOrDefaultAsync(x => x.Username == user.Username);

            return user;
        }

        private void CreatePaaswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _dbContext.Users.AnyAsync(x => x.UserName == username))
                return true;

            return false;
        }

        public async Task<bool> UserNameExists(string email)
        {
            if (await _dbContext.Users.AnyAsync(x => x.Email == email))
                return true;

            return false;
        }
    }
}
