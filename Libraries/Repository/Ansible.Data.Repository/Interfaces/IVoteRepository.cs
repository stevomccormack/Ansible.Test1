using System;
using System.Collections.Generic;
using System.Text;
using Ansible.Data.Entity;
using Ansible.Data.Model;
using URF.Core.Abstractions.Trackable;

namespace Ansible.Data.Repository
{

    public interface IVoteRepository : ITrackableRepository<Vote>, IVoteManager
    {
    }
}
