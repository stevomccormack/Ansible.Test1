using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Ansible.Data.Entity;
using Ansible.Data.Model;
using URF.Core.Abstractions.Services;

namespace Ansible.Services.Interfaces
{
    public interface IVoteService : IService<Vote>, IVoteManager
    {

    }
}
