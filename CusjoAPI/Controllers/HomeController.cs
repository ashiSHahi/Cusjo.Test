using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using CusjoAPI.Interfaces;
using CusjoAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CusjoAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class HomeController : ControllerBase
    {

        private IPermissionRepository _permissionRepository;
        public HomeController(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetPermissions()
        {
            return Ok(await _permissionRepository.GetPermissions());
        }

        public async Task<bool> CheckAdminPermission()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                return await _permissionRepository.CheckAdminPermission(identity.FindFirst(ClaimTypes.Email).Value);
            }
            return false;
        }

        [HttpGet]
        public  IActionResult ReturnData()
        {
            var data = _permissionRepository.Getdata();

            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserPermissions(string email)
        {
            var data = _permissionRepository.GetUserPermissions(email);

            return Ok(await data);
        }
        [HttpPost]
        public async Task<IActionResult> SavePermissions([FromBody] IEnumerable<PermissionDto> dto)
        {
            var result = await _permissionRepository.SavePermissions(dto);

            return Ok(result);
        }

    }
}