using CusjoAPI.Data;
using CusjoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CusjoAPI.Interfaces
{
    public interface IPermissionRepository
    {
        public Task<IEnumerable<PermissionDto>> GetPermissions();

        public Task<bool> CheckAdminPermission(string email);

        public IEnumerable<ChartDataSet> Getdata();

        public Task<IEnumerable<UserPermissions>> GetUserPermissions(string email);

        public Task<bool> SavePermissions(IEnumerable<PermissionDto> dto);
    }
}
