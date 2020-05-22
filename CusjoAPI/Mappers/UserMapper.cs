using CusjoAPI.Data;
using CusjoAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CusjoAPI.Mappers
{
    public static class UserMapper
    {
        public static IEnumerable<UserPermissions> GetUserPermissions(IEnumerable<UserEntity_assoc> users)
        {
            var viewModel = new List<UserPermissions>();

            foreach (var user in users)
            {
                var m = new UserPermissions();
                m.EntityId = user.EntityId;
                m.Id = user.Id;
                m.UserId = user.UserId;
                m.HasAccess = user.HasAccess;
                viewModel.Add(m);
            }

            return viewModel;
        }
    }
}
