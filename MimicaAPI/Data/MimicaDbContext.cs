using Microsoft.EntityFrameworkCore;
using MimicaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicaAPI.Data
{
    public class MimicaDbContext : DbContext
    {
        public MimicaDbContext(DbContextOptions<MimicaDbContext>options) : base(options)
        {

        }
        public DbSet<Palavra> Palavra { get; set; }
    }
}
