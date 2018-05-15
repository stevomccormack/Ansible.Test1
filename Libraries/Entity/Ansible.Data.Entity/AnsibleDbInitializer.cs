using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ansible.Data.Model;

namespace Ansible.Data.Entity
{
    public class AnsibleDbInitializer
    {
        private AnsibleDbContext _context;

        public AnsibleDbInitializer(AnsibleDbContext context)
        {
            _context = context;
        }

        public async Task Seed()
        {
            if (!_context.Votes.Any())
            {
                _context.AddRange(new List<Vote>
                    {
                        new Vote { VoteId = 1, GivenName = "Steve", Surname = "McCormack", Gender = "Male", Age = 42, IsValid = true },
                        new Vote { VoteId = 2, GivenName = "Krishna", Surname = "Ghandi", Gender = "Male", Age = 38, IsValid = true  },
                        new Vote { VoteId = 3, GivenName = "Dan", Surname = "Whitmarsh", Gender = "Male", Age = 38, IsValid = true  },
                        new Vote { VoteId = 4, GivenName = "Elle", Surname = "McPherson", Gender = "Female", Age = 50, IsValid = false  }
                    });
                await _context.SaveChangesAsync();
            }
        }
    }
}
