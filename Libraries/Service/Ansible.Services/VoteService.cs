using System;
using System.Threading;
using System.Threading.Tasks;
using Ansible.Data.Model;
using Ansible.Data.Repository;
using Ansible.Services.Interfaces;
using TrackableEntities.Common.Core;
using URF.Core.Abstractions.Services;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;

namespace Ansible.Services
{
    public class VoteService : Service<Vote>, IVoteService
    {
        #region Constructors

        public VoteService(IVoteRepository repository) : base(repository)
        {

        }

        #endregion

        #region Implements IVoteRepository

        #region Implements IVoteQuery

        public async Task<Vote> FindByUniqueVoteNumber(string voteNumber)
        {
            return await ((IVoteRepository)Repository).FindByUniqueVoteNumber(voteNumber);
        }

        #endregion

        #region Implements IVoteCommand

        public async Task<bool> SetIsValid(int id, bool isValid)
        {
            return await ((IVoteRepository)Repository).SetIsValid(id, isValid);
        }

        #endregion

        #endregion
    }
}
