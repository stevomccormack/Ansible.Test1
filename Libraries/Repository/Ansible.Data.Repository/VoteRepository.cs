using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ansible.Data.Entity;
using Ansible.Data.Model;
using Microsoft.EntityFrameworkCore;
using TrackableEntities.Common.Core;
using URF.Core.EF.Trackable;
using Ansible.Data.Repository.Extensions;

namespace Ansible.Data.Repository
{
    public class VoteRepository: TrackableRepository<Vote>, IVoteRepository
    {
        #region Constructors

        public VoteRepository(DbContext context) : base(context)
        {

        }

        #endregion

        #region Methods

        #region Implements IVoteRepository

        #region Implements IVoteQuery

        public async Task<Vote> FindByUniqueVoteNumber( string voteNumber )
        {
            return await Context.Query<Vote>().FirstOrDefaultAsync( x => x.VoteNumber == voteNumber);
        }

        #endregion

        #region Implements IVoteCommand

        public async Task<bool> SetIsValid( int id, bool isValid )
        {
            var entity = await FindAsync(id);
            if (entity == null)
                return false;

            entity.IsValid = isValid;
            entity.TrackingState = TrackingState.Modified;

            Update(entity);
            return true;
        }

        #endregion

        #endregion

        #endregion
    }
}
