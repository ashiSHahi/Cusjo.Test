using AutoMapper;
using CusjoAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CusjoAPI.Models
{
    public class PermissionDto 
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public IEnumerable<UserPermissions> Permissions { get; set; }
    }
}
