using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CusjoAPI.Enumerators.Enumerators;

namespace CusjoAPI.Models
{
    public class UserPermissions
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public Guid UserId { get; set; }
        public bool HasAccess { get; set; }
    }
}
