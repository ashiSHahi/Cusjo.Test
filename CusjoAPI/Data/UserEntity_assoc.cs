using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CusjoAPI.Data
{
    [Table("UserEntity_assoc")]
    public class UserEntity_assoc
    {
        public int Id { get; set; }

        [ForeignKey("EntityId")]
        public virtual Entities Entities { get; set; }
        public int EntityId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public Guid UserId { get; set; }

        [DefaultValue(false)]
        public bool HasAccess { get; set; }
    }
}
