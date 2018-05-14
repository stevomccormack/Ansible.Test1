using System;
using System.Collections.Generic;
using System.Text;
using Ansible.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Ansible.Data.Entity
{
    public class AnsibleDbContext : DbContext
    {
        public AnsibleDbContext(DbContextOptions<AnsibleDbContext> options) : base(options)
        {
        }

        public DbSet<Vote> Votes { get; set; }
    }
}
