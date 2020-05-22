using CusjoAPI.Data;
using CusjoAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CusjoAPI.Mappers
{
    public class UserPermissionMapper
    {
        public async static Task<IEnumerable<PermissionDto>> GetUsersPermissions(IEnumerable<User> dto)
        {
            var task = Task.Run(() =>
            {
                var viewModel = new List<PermissionDto>();
                foreach (var dt in dto)
                {
                    var permission = new PermissionDto();
                    permission.Id = dt.Id;
                    permission.Email = dt.Email;
                    permission.Username = dt.Username;
                    var per = new List<UserPermissions>();
                    foreach (var pe in dt.Permissions)
                    {
                        var permi = new UserPermissions();
                        permi.Id = pe.Id;
                        permi.EntityId = pe.EntityId;
                        permi.UserId = pe.UserId;
                        permi.HasAccess = pe.HasAccess ? true : false;
                        per.Add(permi);
                    }
                    permission.Permissions = per;
                    viewModel.Add(permission);
                }
                return viewModel;
            });

            return await task;
        }
    }
}
