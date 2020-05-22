using CusjoAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CusjoAPI.SeedData
{
    public static class SeedData
    {
        public static void SeedEnitties(ApplicationDbContext context)
        {
            if (!context.Entities_lu.Any())
            {
                
                var editor = new Entities()
                {
                    Name = "Editor"
                };
                var chart = new Entities()
                {
                    Name = "Charts"
                };
                var permission = new Entities()
                {
                    Name = "Permissions"
                };
                

                context.AddAsync(editor);
                context.AddAsync(chart);
                context.AddAsync(permission);
                context.SaveChanges();
            }
        }
    }
}
