using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence
{
    public class BdContext : DbContext
    {
        public BdContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Seguimientos> Seguimiento { get; set; }

    }
}
