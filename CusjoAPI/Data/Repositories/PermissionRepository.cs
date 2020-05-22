using AutoMapper;
using CsvHelper;
using CusjoAPI.Interfaces;
using CusjoAPI.Mappers;
using CusjoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using static CusjoAPI.Enumerators.Enumerators;

namespace CusjoAPI.Data.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private IMapper _mapper;

        public PermissionRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public ApplicationDbContext DbContext { get; }

        public async Task<bool> CheckAdminPermission(string email)
        {
            
            var task = Task.Run(() =>
            {
                var user = _dbContext.AppUsers
                    .Include(x =>x.Permissions)
                    .FirstOrDefault(x => x.Email == email);

                if (user == null)
                    return false;

                var result = user.Permissions
                    .Where(x => x.EntityId == (int)eEntityType.Admin);
                    
                return result.Any() ? result.Select(x =>x.HasAccess).First(): false;
            });

            return await task;
        }

        public IEnumerable<ChartDataSet> Getdata()
        {
            TextReader reader = new StreamReader("Datafiles/datatest2.txt");
            var csvReader = new CsvReader(reader, System.Globalization.CultureInfo.CreateSpecificCulture("US"));
            
            var records = csvReader.GetRecords<ChartDataSet>();

            return records;
        }

        public async Task<IEnumerable<PermissionDto>> GetPermissions()
        {
            var task = Task.Run(() =>
            {
                var query = _dbContext.AppUsers
                .Include(x => x.Permissions)
                .ThenInclude(x => x.Entities)
                .ToList();

                return UserPermissionMapper.GetUsersPermissions(query);
            });

            return await task;
        }

        public async Task<IEnumerable<UserPermissions>> GetUserPermissions(string email)
        {
            var task = Task.Run(() =>
            {
                var query = _dbContext.AppUsers
                    .Include(x => x.Permissions)
                    .ThenInclude(x => x.Entities)
                    .Where(x => x.Email == email)
                    .ToList()
                    .First().Permissions;

                return UserMapper.GetUserPermissions(query);
            });

            return await task;
        }

        public async Task<bool> SavePermissions(IEnumerable<PermissionDto> dto)
        {
            foreach(var data in dto)
            {
                foreach(var permission in data.Permissions)
                {
                    var x = new UserEntity_assoc
                    {
                        Id = permission.Id,
                        EntityId = permission.EntityId,
                        UserId = permission.UserId,
                        HasAccess = permission.HasAccess
                    };
                    _dbContext.Update(x);
                }
            }
            try
            {
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
