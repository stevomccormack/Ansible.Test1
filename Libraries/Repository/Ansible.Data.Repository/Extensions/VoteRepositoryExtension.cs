using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ansible.Data.Model;

namespace Ansible.Data.Repository.Extensions
{
    public static class VoteRepositoryExtension
    {
        public static IQueryable<Vote> DeepInclude(this IQueryable<Vote> queryObject)
        {
            return queryObject;
        }

    }
}
